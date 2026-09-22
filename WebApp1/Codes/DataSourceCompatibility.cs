using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Kendo.Mvc
{
    public enum FilterCompositionLogicalOperator
    {
        And,
        Or
    }

    public enum FilterOperator
    {
        IsEqualTo,
        IsNotEqualTo,
        IsGreaterThan,
        IsGreaterThanOrEqualTo,
        IsLessThan,
        IsLessThanOrEqualTo,
        Contains,
        StartsWith,
        EndsWith,
        IsNull,
        IsNotNull,
        IsEmpty,
        IsNotEmpty
    }

    public enum ListSortDirection
    {
        Ascending,
        Descending
    }

    public interface IFilterDescriptor
    {
        bool IsMatch(object item);
    }

    public sealed class FilterDescriptor : IFilterDescriptor
    {
        public string Member { get; set; }
        public FilterOperator Operator { get; set; }
        public object Value { get; set; }

        public FilterDescriptor() { }

        public FilterDescriptor(string member, FilterOperator @operator, object value)
        {
            Member = member;
            Operator = @operator;
            Value = value;
        }

        public bool IsMatch(object item)
        {
            object actual = GetValue(item, Member);
            string expectedText = Value?.ToString();
            if (Operator == FilterOperator.IsNull) return actual == null;
            if (Operator == FilterOperator.IsNotNull) return actual != null;
            if (Operator == FilterOperator.IsEmpty) return actual is string empty && empty.Length == 0;
            if (Operator == FilterOperator.IsNotEmpty) return actual is string nonEmpty && nonEmpty.Length > 0;
            if (actual == null) return false;

            if (actual is string actualText)
            {
                var comparison = StringComparison.CurrentCultureIgnoreCase;
                return Operator switch
                {
                    FilterOperator.Contains => actualText.Contains(expectedText ?? string.Empty, comparison),
                    FilterOperator.StartsWith => actualText.StartsWith(expectedText ?? string.Empty, comparison),
                    FilterOperator.EndsWith => actualText.EndsWith(expectedText ?? string.Empty, comparison),
                    _ => Compare(actualText, expectedText, Operator)
                };
            }

            object expected = ConvertValue(expectedText, actual.GetType());
            int comparisonResult = Comparer.DefaultInvariant.Compare(actual, expected);
            return Operator switch
            {
                FilterOperator.IsEqualTo => Equals(actual, expected),
                FilterOperator.IsNotEqualTo => !Equals(actual, expected),
                FilterOperator.IsGreaterThan => comparisonResult > 0,
                FilterOperator.IsGreaterThanOrEqualTo => comparisonResult >= 0,
                FilterOperator.IsLessThan => comparisonResult < 0,
                FilterOperator.IsLessThanOrEqualTo => comparisonResult <= 0,
                _ => true
            };
        }

        private static bool Compare(string actual, string expected, FilterOperator @operator)
        {
            int result = string.Compare(actual, expected, StringComparison.CurrentCultureIgnoreCase);
            return @operator switch
            {
                FilterOperator.IsEqualTo => result == 0,
                FilterOperator.IsNotEqualTo => result != 0,
                FilterOperator.IsGreaterThan => result > 0,
                FilterOperator.IsGreaterThanOrEqualTo => result >= 0,
                FilterOperator.IsLessThan => result < 0,
                FilterOperator.IsLessThanOrEqualTo => result <= 0,
                _ => true
            };
        }

        internal static object GetValue(object item, string member)
        {
            object current = item;
            foreach (string part in (member ?? string.Empty).Split('.'))
            {
                if (current == null) return null;
                PropertyInfo property = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                current = property?.GetValue(current);
            }
            return current;
        }

        private static object ConvertValue(string value, Type targetType)
        {
            Type type = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (type.IsEnum) return Enum.Parse(type, value, true);
            if (type == typeof(Guid)) return Guid.Parse(value);
            return Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
        }
    }

    public sealed class CompositeFilterDescriptor : IFilterDescriptor
    {
        public FilterCompositionLogicalOperator LogicalOperator { get; set; }
        public IList<IFilterDescriptor> FilterDescriptors { get; } = new List<IFilterDescriptor>();

        public bool IsMatch(object item)
        {
            return LogicalOperator == FilterCompositionLogicalOperator.Or
                ? FilterDescriptors.Any(filter => filter.IsMatch(item))
                : FilterDescriptors.All(filter => filter.IsMatch(item));
        }
    }

    public sealed class SortDescriptor
    {
        public string Member { get; set; }
        public ListSortDirection SortDirection { get; set; }
    }
}

namespace Kendo.Mvc.UI
{
    public class DataSourceRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IList<Kendo.Mvc.IFilterDescriptor> Filters { get; set; } = new List<Kendo.Mvc.IFilterDescriptor>();
        public IList<Kendo.Mvc.SortDescriptor> Sorts { get; set; } = new List<Kendo.Mvc.SortDescriptor>();
    }

    public class DataSourceResult
    {
        public object Data { get; set; }
        public int Total { get; set; }
        public object Errors { get; set; }
    }

    public class TreeDataSourceResult : DataSourceResult
    {
    }

    public sealed class DataSourceRequestAttribute : ModelBinderAttribute
    {
        public DataSourceRequestAttribute() : base(typeof(DataSourceRequestModelBinder)) { }
    }

    public sealed class DataSourceRequestModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = new DataSourceRequest
            {
                Page = ReadInt(bindingContext, "page", 1),
                PageSize = ReadInt(bindingContext, "pageSize", 10)
            };

            for (int index = 0; ; index++)
            {
                string field = Read(bindingContext, $"sort[{index}][field]");
                if (field == null) break;
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor
                {
                    Member = field,
                    SortDirection = string.Equals(Read(bindingContext, $"sort[{index}][dir]"), "desc", StringComparison.OrdinalIgnoreCase)
                        ? Kendo.Mvc.ListSortDirection.Descending
                        : Kendo.Mvc.ListSortDirection.Ascending
                });
            }

            for (int index = 0; ; index++)
            {
                string field = Read(bindingContext, $"filter[filters][{index}][field]");
                if (field == null) break;
                Enum.TryParse(Read(bindingContext, $"filter[filters][{index}][operator]"), true, out Kendo.Mvc.FilterOperator @operator);
                request.Filters.Add(new Kendo.Mvc.FilterDescriptor(field, @operator, Read(bindingContext, $"filter[filters][{index}][value]")));
            }

            bindingContext.Result = ModelBindingResult.Success(request);
            return Task.CompletedTask;
        }

        private static string Read(ModelBindingContext context, string key)
        {
            return context.HttpContext.Request.Query.TryGetValue(key, out var value) ? value.FirstOrDefault() : null;
        }

        private static int ReadInt(ModelBindingContext context, string key, int fallback)
        {
            return int.TryParse(Read(context, key), out int value) && value > 0 ? value : fallback;
        }
    }
}

namespace Kendo.Mvc.Extensions
{
    public static class QueryableExtensions
    {
        public static Kendo.Mvc.UI.DataSourceResult ToDataSourceResult<T>(this IQueryable<T> source, Kendo.Mvc.UI.DataSourceRequest request)
        {
            return CreateResult(source.AsEnumerable(), request, false);
        }

        public static Kendo.Mvc.UI.TreeDataSourceResult ToTreeDataSourceResult<T>(this IQueryable<T> source, Kendo.Mvc.UI.DataSourceRequest request)
        {
            var result = CreateResult(source.AsEnumerable(), request, true);
            return new Kendo.Mvc.UI.TreeDataSourceResult { Data = result.Data, Total = result.Total, Errors = result.Errors };
        }

        private static Kendo.Mvc.UI.DataSourceResult CreateResult<T>(IEnumerable<T> source, Kendo.Mvc.UI.DataSourceRequest request, bool tree)
        {
            IEnumerable<object> items = source.Cast<object>();
            foreach (var filter in request.Filters ?? Array.Empty<Kendo.Mvc.IFilterDescriptor>())
            {
                items = items.Where(filter.IsMatch);
            }

            int total = items.Count();
            IOrderedEnumerable<object> ordered = null;
            foreach (var sort in request.Sorts ?? Array.Empty<Kendo.Mvc.SortDescriptor>())
            {
                Func<object, object> key = item => Kendo.Mvc.FilterDescriptor.GetValue(item, sort.Member);
                ordered = ordered == null
                    ? (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending ? items.OrderBy(key) : items.OrderByDescending(key))
                    : (sort.SortDirection == Kendo.Mvc.ListSortDirection.Ascending ? ordered.ThenBy(key) : ordered.ThenByDescending(key));
            }

            items = ordered ?? items;
            if (request.PageSize > 0)
            {
                items = items.Skip(Math.Max(request.Page - 1, 0) * request.PageSize).Take(request.PageSize);
            }

            return new Kendo.Mvc.UI.DataSourceResult { Data = items.ToList(), Total = total };
        }
    }
}

namespace System.Linq
{
    internal static class DbSetLinqExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(this DbSet<TEntity> source, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Queryable.Where(source, predicate);
        }

        public static IQueryable<TEntity> DefaultIfEmpty<TEntity>(this DbSet<TEntity> source)
            where TEntity : class
        {
            return Queryable.DefaultIfEmpty(source);
        }
    }
}
