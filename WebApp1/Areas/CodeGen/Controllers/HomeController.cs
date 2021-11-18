using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using WebApp1.Areas.CodeGen.Codes;
using WebApp1.Areas.CodeGen.Models;
using WebApp1.Codes;

namespace WebApp1.Areas.CodeGen.Controllers
{
    [Area("CodeGen")]
    public class HomeController : WebApp1.Controllers._Controller
    {
        public HomeController(IServiceProvider _serviceProvider)
            : base(_serviceProvider) { }

        public IActionResult Index()
        {
#if DEBUG
            var ignoreTables = new List<string>() {
                "TemSemaVersiyon"
            }.Select(s => s.ToLower());

            var MyCodeGenInfoList = new List<MyCodeGenInfo>();
            foreach (var table in this.dataContext.GetDBSchema().Tables.Where(c => !ignoreTables.Contains(c.Name.ToLower())))
            {
                var tableOptions = MyCodeGen.GetTableOptions(table.Name);

                MyCodeGenInfoList.Add(
                    new MyCodeGenInfo
                    {
                        TableName = table.Name,
                        TableOptions = tableOptions,
                        DataTransferObjects = MyCodeGen.GetDataTransferObjects(table.Name),
                        DataManipulationObjects = MyCodeGen.GetDataManipulationObjects(table.Name),
                        Controllers = MyCodeGen.GetControllers(table.Name),
                        Dictionaries = MyCodeGen.GetDictionaries(table.Name),
                        FormViews = MyCodeGen.GetFormViews(table.Name),
                        GridViews = MyCodeGen.GetGridViews(table.Name),
                        TreeLists = MyCodeGen.GetTreeLists(table.Name),
                        SearchViews = MyCodeGen.GetSearchViews(table.Name)
                    }
                );
            }

            ViewBag.MyCodeGenInfoList = MyCodeGenInfoList;
#endif
            return View();
        }

        #region ReadLocks
        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult ReadLocks(string _TableName, string _TableOptionName)
        {
            Boolean rError = false;
            string rMessage = "";
            var rLockList = new List<string>();
            try
            {
                var props = new[] {
                    "DataTransferObjectLock",
                    "DataManipulationObjectLock",
                    "ControllerLock",
                    "DictionaryLock",
                    "FormViewLock",
                    "GridViewLock",
                    "TreeListLock",
                    "SearchViewLock"
                };


                var myCodeGen = new MyCodeGen(this.dataContext);
                var tableOption = myCodeGen.FnTableOptionRead(_TableName, _TableOptionName);

                foreach (var prop in props)
                {
                    object value = typeof(MyTableOption).GetProperty(prop).GetValue(tableOption);
                    if ((Boolean)value == true)
                    {
                        rLockList.Add(prop);
                    }
                }
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage, oLockList = rLockList });
        }
        #endregion


        #region translator
        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TranslateWithApi(string _text, string _target, string _source, Boolean _titleCase)
        {
            Boolean rError = false;
            string rMessage = "";
            string rTranslatedText = "";
            try
            {
                //rTranslatedText = MyApp.TranslateWithGoogleApi(_text, _target, _source, _titleCase);
                rTranslatedText = MyApp.TranslateWithBingApiV3(_text, _target, _source, _titleCase);
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }
            return Json(new { bError = rError, sMessage = rMessage, sTranslatedText = rTranslatedText });
        }
        #endregion

        #region lookup
        public IActionResult GetColumns(string _TableName)
        {
            var rList = new List<string>();
            if (_TableName != null)
            {
                var data = this.dataContext.GetDBSchema().Tables
                    .Where(c => c.Name == _TableName).FirstOrDefault();

                if (data != null)
                {
                    rList = data.Columns.Select(s => s.Name).ToList();
                }
            }

            return Json(new { data = rList });
        }
        #endregion

        #region Table Options

        public IActionResult TableOption(string _TableName, string _TableOptionName)
        {
#if DEBUG
            ViewBag.TableName = _TableName;
            ViewBag.TableOptionName = _TableOptionName;
            if (_TableName == _TableOptionName)
            {
                ViewBag.TableOptionName = _TableOptionName + "_";
            }

            ViewBag.tableList = this.dataContext.GetDBSchema().Tables.Select(s => s.Name).ToList();
            ViewBag.areaList = MyApp.Areas;

            ViewBag.DataTransferObjectCount = MyCodeGen.GetDataTransferObjects(_TableName).Count;
            ViewBag.DataManipulationObjectCount = MyCodeGen.GetDataManipulationObjects(_TableName).Count;
            ViewBag.ControllerCount = MyCodeGen.GetControllers(_TableName).Count;
            ViewBag.DictionaryCount = MyCodeGen.GetDictionaries(_TableName).Count;
            ViewBag.FormViewCount = MyCodeGen.GetFormViews(_TableName).Count;
            ViewBag.GridViewCount = MyCodeGen.GetGridViews(_TableName).Count;
            ViewBag.TreeListCount = MyCodeGen.GetTreeLists(_TableName).Count;
            ViewBag.SearchViewCount = MyCodeGen.GetSearchViews(_TableName).Count;
#endif
            return View();
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TableOptionRead(string _TableName, string _TableOptionName)
        {
            Boolean rError = false;
            string rMessage = "";
            bool rExists = false;
            var rTableOption = new MyTableOption();
            try
            {
                //dosya varmı?
                if (System.IO.File.Exists(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory + "\\" + _TableOptionName + ".dat"))
                {
                    rExists = true;
                }

                //option read
                var myCodeGen = new MyCodeGen(this.dataContext);
                rTableOption = myCodeGen.FnTableOptionRead(_TableName, _TableOptionName);
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage = ex.MyLastInner().Message;
            }

            return Json(new { bError = rError, sMessage = rMessage, bExists = rExists, oTableOption = rTableOption });
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult TableOptionSave(string _TableOptionName, string _TableOptionText)
        {

            Boolean rError = false;
            string rMessage = string.Empty;
#if DEBUG
            try
            {
                //option read
                var myCodeGen = new MyCodeGen(this.dataContext);
                var result = myCodeGen.FnTableOptionSave(_TableOptionName, _TableOptionText);
                rError = result.Error;
                rMessage = result.Message;
            }
            catch (Exception ex)
            {
                rError = true;
                rMessage += ex.MyLastInner().Message;
            }
#endif
            return Json(new { bError = rError, sMessage = rMessage });
        }

        #endregion

        #region CodeWrite

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult CodeWrite(string _TableOptionNames, string _CodeName)
        {
            var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(this.dataContext);
#if DEBUG
            foreach (var tableOptionName in _TableOptionNames.Split(","))
            {
                MyCustomResult rV = myCodeGen.CodeWriteAll(tableOptionName, _CodeName);
                result.Error = rV.Error;
                result.Message += rV.Message;
            }
#endif
            return Json(new { bError = result.Error, sMessage = result.Message });
        }

        [HttpPost]
        [ResponseCache(Duration = 0)]
        public IActionResult ReTableOptionSave(string _TableOptionNames)
        {
            var result = new MyCustomResult();
            var myCodeGen = new MyCodeGen(this.dataContext);
#if DEBUG
            foreach (var tableOptionName in _TableOptionNames.Split(","))
            {
                //tümü geldiğinde, tüm options dosyalarının defaultlarıyla read ve write yapılması
                string optionsFileName = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory + "\\" + tableOptionName + ".dat";
                if (System.IO.File.Exists(optionsFileName))
                {
                    var tableOption = myCodeGen.FnTableOptionRead("", tableOptionName);
                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(tableOption);
                    myCodeGen.FnTableOptionSave(tableOptionName, jsonText);
                    result.Message += tableOptionName + " kaydedildi." + Environment.NewLine;
                }
            }
#endif
            return Json(new { bError = result.Error, sMessage = result.Message });
        }

        #endregion

    }
}