using System;

namespace WebApp1.Models
{
    //public interface IDto
    //{
    //    DataContext DataContext { get; set; }
    //}

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class _Dmo
    {
        protected readonly DataContext dataContext;

        public _Dmo(DataContext context)
        {
            this.dataContext = context;
        }
    }

    public class BaseDmo
    {
        protected readonly DataContext dataContext;

        public BaseDmo(DataContext context)
        {
            this.dataContext = context;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public partial class _Rep
    {
        public readonly DataContext dataContext;

        public _Rep(DataContext context)
        {
            this.dataContext = context;

            this.Init_Tem();   //burasıda yok ise yazılsın...
            this.Init_Nft(); //generatore öğret bence....
            /*code_generator_rep_init_end*/
        }

        public int SaveChanges()
        {
            return this.dataContext.SaveChanges();
        }
    }

}
