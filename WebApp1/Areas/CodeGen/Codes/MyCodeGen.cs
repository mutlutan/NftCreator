using System;
using System.Collections.Generic;
using System.Linq;
using WebApp1.Codes;
using WebApp1.Areas.CodeGen.Models;
using System.Text;
using System.IO;

namespace WebApp1.Areas.CodeGen.Codes
{


    public enum EnmComponentType
    {
        TextBox, PasswordTextBox, TextArea, TextAreaForJson, NumericTextBox, DropDownList, DatePicker, TimePicker,
        DateTimePicker, MaskedTextBox, MultiSelect, TextEditor, ColorPicker, Checkbox, Radio, AutoComplete,
        ComboBox, Slider, ExternalSearchEdit, DownloadFileLink, ImageBox, ImageSearchEdit
    }

    public enum EnmSearchViewType
    {
        Grid, TreeList
    }

#pragma warning disable IDE0057

    public class MyCodeGen
    {
        readonly WebApp1.Models.DataContext dataContext;

        #region directories 

        public static string CodeGenRootNameSpace { get; set; } = "WebApp1";
        public static string CodeGenAreasDirectory { get; set; } = MyApp.AppAreasDirectory;
        public static string CodeGenDataDirectory { get; set; } = MyCodeGen.CodeGenAreasDirectory + "\\CodeGen\\Data";
        public static string CodeGenDataFileExtension { get; set; } = ".dat";
        public static string CodeGenDataTransferObjectDirectory { get; set; } = "Dto";
        public static string CodeGenDataManipulationObjectDirectory { get; set; } = "Dmo";
        public static string CodeGenRepositoryDirectory { get; set; } = "Models";
        public static string CodeGenControllerDirectory { get; set; } = "Controllers";
        public static string CodeGenViewDirectory { get; set; } = "Views";
        public static string CodeGenDictionaryDirectory { get; set; } = MyApp.AppDictionaryDirectory;

        #endregion

        #region prefix

        public static string CodeGen_SequencePrefix { get; set; } = "sq";
        public static string CodeGen_DtoPrefix { get; set; } = "Dto";
        public static string CodeGen_DmoPrefix { get; set; } = "Dmo";
        public static string CodeGen_RepPrefix { get; set; } = "Rep";
        public static string CodeGen_ControllerPrefix { get; set; } = "Dwa"; // controller prefix olmayabilir, ancak suffix olacak
        public static string CodeGen_ControllerSuffix { get; set; } = "Controller";// controller prefix olmayabilir, ancak suffix olacak, başka birşey yazabilir ancak boş olmayacak

        #endregion

        #region constractor
        public MyCodeGen(WebApp1.Models.DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        #endregion

        #region functions I static 

        public static List<string> GetTableOptions(string _TableName)
        {
            List<string> SL = new();

            DirectoryInfo diList = new(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory);
            if (diList.Exists)
            {
                IEnumerable<FileInfo> files = diList.EnumerateFiles("*_" + _TableName + "_" + MyCodeGen.CodeGenDataFileExtension, SearchOption.TopDirectoryOnly);
                SL.AddRange(files.Select(s => s.Name));
            }
            return SL;
        }

        public static List<string> GetDataTransferObjects(string _TableName)
        {
            List<string> SL = new();

            DirectoryInfo diList = new(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataTransferObjectDirectory);
            if (diList.Exists)
            {
                var files = diList.EnumerateFiles(MyCodeGen.CodeGen_DtoPrefix + _TableName + ".cs", SearchOption.TopDirectoryOnly);
                SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
            }

            foreach (string areaName in MyApp.Areas)
            {
                diList = new DirectoryInfo(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenDataTransferObjectDirectory);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(MyCodeGen.CodeGen_DtoPrefix + _TableName + ".cs", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }

            return SL;
        }

        public static List<string> GetDataManipulationObjects(string _TableName)
        {
            List<string> SL = new();
            DirectoryInfo diList = new(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataManipulationObjectDirectory);
            if (diList.Exists)
            {
                var files = diList.EnumerateFiles(MyCodeGen.CodeGen_DmoPrefix + _TableName + ".cs", SearchOption.TopDirectoryOnly);
                SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
            }

            foreach (string areaName in MyApp.Areas)
            {
                diList = new DirectoryInfo(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenDataManipulationObjectDirectory);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(MyCodeGen.CodeGen_DmoPrefix + _TableName + ".cs", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }

            return SL;
        }

        public static List<string> GetControllers(string _TableName)
        {
            List<string> SL = new();

            DirectoryInfo diList = new(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenControllerDirectory);
            if (diList.Exists)
            {
                var files = diList.EnumerateFiles(MyCodeGen.CodeGen_ControllerPrefix + _TableName + MyCodeGen.CodeGen_ControllerSuffix + ".cs", SearchOption.TopDirectoryOnly);
                SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
            }

            foreach (string areaName in MyApp.Areas)
            {
                diList = new DirectoryInfo(MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenControllerDirectory);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(MyCodeGen.CodeGen_ControllerPrefix + _TableName + MyCodeGen.CodeGen_ControllerSuffix + ".cs", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }

            return SL;
        }

        public static List<string> GetDictionaries(string _TableName)
        {
            List<string> SL = new();
            foreach (string areaName in MyApp.Areas)
            {
                string dir = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenDictionaryDirectory;
                DirectoryInfo diList = new(dir);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(_TableName + ".json", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }
            return SL;
        }

        public static List<string> GetFormViews(string _TableName)
        {
            List<string> SL = new();
            foreach (string areaName in MyApp.Areas)
            {
                string dir = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                DirectoryInfo diList = new(dir);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(_TableName + "ForForm" + ".html", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }
            return SL;
        }

        public static List<string> GetGridViews(string _TableName)
        {
            List<string> SL = new();
            foreach (string areaName in MyApp.Areas)
            {
                string dir = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                DirectoryInfo diList = new(dir);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(_TableName + "ForGrid" + ".html", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }
            return SL;
        }

        public static List<string> GetTreeLists(string _TableName)
        {
            List<string> SL = new();
            foreach (string areaName in MyApp.Areas)
            {
                string dir = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                DirectoryInfo diList = new(dir);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(_TableName + "ForTreeList" + ".html", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }
            return SL;
        }

        public static List<string> GetSearchViews(string _TableName)
        {
            List<string> SL = new();
            foreach (string areaName in MyApp.Areas)
            {
                string dir = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + areaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                DirectoryInfo diList = new(dir);
                if (diList.Exists)
                {
                    var files = diList.EnumerateFiles(_TableName + "ForSearch" + ".html", SearchOption.TopDirectoryOnly);
                    SL.AddRange(files.Select(s => s.Name.Substring(0, s.Name.Length - 3)));
                }
            }
            return SL;
        }

        #endregion

        #region helper functions
        public string FnNetTypeToJsonType(string columnNetType)
        {
            string columnJsonType = "string";
            if (columnNetType == "System.String")
            {
                columnJsonType = "string";
            }
            else if (columnNetType == "System.Boolean")
            {
                columnJsonType = "boolean";
            }
            else if (columnNetType == "System.Int32" || columnNetType == "System.Decimal")
            {
                columnJsonType = "number";
            }
            else if (columnNetType == "System.DateTime")
            {
                columnJsonType = "date";
            }
            else if (columnNetType == "System.Date")
            {
                columnJsonType = "date";
            }
            else if (columnNetType == "System.TimeSpan")
            {
                columnJsonType = "time";
            }

            return columnJsonType;
        }
        #endregion

        #region TableOptionRead, TableOptionSave

        public MyTableOption FnTableOptionRead(string _TableName, string _TableOptionName)
        {
            MyTableOption tableOption = new(); // mevcut table opt
            MyTableOption rTableOption = new();


            string optionsFileName = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory + "\\" + _TableOptionName + ".dat";
            if (File.Exists(optionsFileName))
            {
                rTableOption = Newtonsoft.Json.JsonConvert.DeserializeObject<MyTableOption>(File.ReadAllText(optionsFileName));
            }
            else
            {
                rTableOption.TableName = _TableName;
                rTableOption.TableDictionaryTitleTr = rTableOption.TableName;
                rTableOption.TableDictionaryTitleEn = rTableOption.TableName;

                rTableOption.TableDictionaryShortTitleTr = rTableOption.TableName;
                rTableOption.TableDictionaryShortTitleEn = rTableOption.TableName;

                string sequenceName = MyCodeGen.CodeGen_SequencePrefix + rTableOption.TableName;
                if (!this.dataContext.GetDBSchema().Sequences.Where(c => c.Name == sequenceName).Any())
                {
                    sequenceName = "";
                }
                rTableOption.SequenceName = sequenceName;

                rTableOption.FormViewTabDictionaryTr = "I||II||III||IV||V||VI||VII||VIII";
                rTableOption.FormViewTabDictionaryEn = "I||II||III||IV||V||VI||VII||VIII";

                rTableOption.GridViewTabDictionaryTr = "I||II||III||IV||V||VI||VII||VIII";
                rTableOption.GridViewTabDictionaryEn = "I||II||III||IV||V||VI||VII||VIII";

                rTableOption.SearchViewTabDictionaryTr = "I||II||III||IV||V||VI||VII||VIII";
                rTableOption.SearchViewTabDictionaryEn = "I||II||III||IV||V||VI||VII||VIII";

                rTableOption.GridViewCrudEditorType = "Inline";
                rTableOption.GridViewDataSourcePageSize = 10;
            }

            var tableInfo = this.dataContext.GetDBSchema().Tables.Where(c => c.Name == rTableOption.TableName).FirstOrDefault();

            rTableOption.DataTransferObjectName = MyCodeGen.CodeGen_DtoPrefix + rTableOption.TableName;
            rTableOption.DataManipulationObjectName = MyCodeGen.CodeGen_DmoPrefix + rTableOption.TableName;
            rTableOption.RepositoryName = MyCodeGen.CodeGen_RepPrefix + rTableOption.TableName;
            rTableOption.ControllerName = MyCodeGen.CodeGen_ControllerPrefix + rTableOption.TableName;

            //primary key olmayan tablonun taaa...a.q.
            rTableOption.PrimaryKey = tableInfo.PrimaryKeyColumn.Name;

            //Kaldırılan Columnları bulur işaretler, ColumnStatus = "Sil" ile
            foreach (var x in rTableOption.Fields)
            {
                if (!tableInfo.Columns.Where(c => c.Name == x.ColumnName).Any())
                {
                    x.ColumnStatus = "Sil";
                    x.ColumnUse = false;
                    x.FormUse = false;
                    x.GridUse = false;
                    x.SearchUse = false;
                }
            }

            foreach (var column in tableInfo.Columns)
            {
                // tabloda var, tanımlamada column yok ise, yani yeni column ise
                if (rTableOption.Fields.Where(c => c.ColumnName == column.Name).FirstOrDefault() == null)
                {
                    string sComponentType = "";
                    string sFormComponentFormat = "";
                    string sGridComponentFormat = "";
                    int nGridComponentWidth = 100;
                    string sReferansTableName = "";
                    string sReferansValueColumnName = "";



                    if (column.IsForeignKey)
                    {
                        sComponentType = "DropDownList";
                        sReferansTableName = column.ForeignKeyTableName;
                        sReferansValueColumnName = column.ForeignKeyTable.PrimaryKeyColumn.Name;
                    }
                    else if (!column.IsPrimaryKey)
                    {
                        if (column.DataType.NetDataType == "System.TimeSpan")
                        {
                            sComponentType = "TimePicker";
                            sGridComponentFormat = "";
                            nGridComponentWidth = 80;
                        }
                        else if (column.DataType.NetDataType == "System.DateTime")
                        {
                            if (column.DbDataType == "datetime")
                            {
                                sComponentType = "DateTimePicker";
                                sFormComponentFormat = "";
                                sGridComponentFormat = "{0:g}";
                                nGridComponentWidth = 145;
                            }
                            else if (column.DbDataType == "date")
                            {
                                sComponentType = "DatePicker";
                                sFormComponentFormat = "";
                                sGridComponentFormat = "{0:d}";
                                nGridComponentWidth = 110;
                            }
                        }
                        else if (column.DataType.NetDataType == "System.Int32" || column.DataType.NetDataType == "System.Int64")
                        {
                            sComponentType = "NumericTextBox";
                            sFormComponentFormat = "n0";
                            sGridComponentFormat = "{0:n0}";
                            nGridComponentWidth = 90;
                        }
                        else if (column.DataType.NetDataType == "System.Double" || column.DataType.NetDataType == "System.Single")
                        {
                            sComponentType = "NumericTextBox";
                            sFormComponentFormat = "n0";
                            sGridComponentFormat = "{0:n0}";
                            nGridComponentWidth = 90;
                        }
                        else if (column.DataType.NetDataType == "System.Decimal")
                        {
                            sComponentType = "NumericTextBox";
                            sFormComponentFormat = "n" + column.Scale;
                            sGridComponentFormat = "{0:n" + column.Scale + "}";
                            nGridComponentWidth = 100;
                        }
                        else if (column.DataType.NetDataType == "System.String")
                        {
                            sComponentType = "TextBox";
                            nGridComponentWidth = 130;
                        }
                        else if (column.DataType.NetDataType == "System.Boolean")
                        {
                            sComponentType = "Checkbox";
                            nGridComponentWidth = 80;
                        }
                        else if (column.DataType.NetDataType == "System.Guid")
                        {
                            sComponentType = "TextBox";
                            nGridComponentWidth = 150;
                        }
                    }
                    else
                    {
                        sComponentType = "TextBox";
                    }

                    MyField newItem = new();
                    newItem.ColumnStatus = "Yeni";
                    newItem.ColumnOrder = column.Ordinal;
                    newItem.ColumnName = column.Name;
                    newItem.ColumnDictionaryTr = column.Name;
                    newItem.ColumnDictionaryEn = column.Name;

                    newItem.ColumnDbType = column.DbDataType;
                    newItem.ColumnNetType = column.DataType.NetDataType; // karşılaştıtıken Type.GetType("System.Int32");
                    newItem.ColumnJsonType = this.FnNetTypeToJsonType(newItem.ColumnNetType);

                    newItem.ColumnUse = true;
                    newItem.ColumnRequired = !column.Nullable & !column.IsPrimaryKey;



                    newItem.ColumnDefault = "";
                    if (column.DataType.IsFloat || column.DataType.IsInt || column.DataType.IsNumeric)
                    {
                        newItem.ColumnDefault = "0";
                    }

                    if (column.DataType.NetDataType == "System.Boolean")
                    {
                        newItem.ColumnDefault = "true";
                    }


                    newItem.ColumnReadConvertValue = "";
                    newItem.ColumnWriteConvertValue = "";

                    newItem.ColumnReferansTableName = sReferansTableName;
                    newItem.ColumnReferansValueColumnName = sReferansValueColumnName;

                    newItem.ColumnReferansDisplayColumnNames = "";
                    newItem.ColumnReferansSortColumnNames = "";
                    if (column.ForeignKeyTable?.Columns.Where(c => c.Name == "Ad")?.Count() > 0)
                    {
                        newItem.ColumnReferansDisplayColumnNames = "Ad";
                        newItem.ColumnReferansSortColumnNames = "Ad";
                    }

                    if (!column.IsForeignKey && column.DataType.NetDataType == "System.Boolean")
                    {
                        newItem.ColumnReferansJsonDataSource = "dsAktifPasif";
                    }

                    newItem.FormUse = !column.IsPrimaryKey;
                    newItem.FormOrder = column.Ordinal - 1;
                    newItem.FormEditable = true;
                    newItem.FormViewTab = "I";
                    newItem.FormViewLocation = "left";
                    newItem.FormComponentType = sComponentType;
                    newItem.FormComponentFormat = sFormComponentFormat;
                    newItem.FormColumnShowRoleGroupIds = "11,21,31,41";

                    newItem.GridUse = !column.IsPrimaryKey;
                    newItem.GridOrder = column.Ordinal - 1;
                    newItem.GridEditable = true;
                    newItem.GridLocked = column.IsPrimaryKey;
                    newItem.GridEncoded = true;
                    newItem.GridTextNowrap = true;

                    newItem.GridFindTab = "";
                    newItem.GridComponentType = sComponentType;
                    newItem.GridComponentFormat = sGridComponentFormat;
                    newItem.GridComponentWidth = nGridComponentWidth;
                    newItem.GridColumnShowRoleGroupIds = "11,21,31,41";

                    newItem.SearchUse = !column.IsPrimaryKey;
                    newItem.SearchOrder = column.Ordinal - 1;
                    newItem.SearchHidden = false;
                    newItem.SearchLocked = column.IsPrimaryKey;
                    newItem.SearchEncoded = true;

                    #region User field defaults

                    if (column.Name == MyApp.InsertDateColumnName)
                    {
                        newItem.FormEditable = false;
                        newItem.GridEditable = false;
                        newItem.FormUse = false;
                        newItem.GridUse = false;
                        newItem.SearchUse = false;
                        newItem.ColumnDictionaryTr = "Eklenme Zamanı";
                        newItem.ColumnDictionaryEn = "Time To Add";
                        newItem.ColumnDefault = "ActionDefault:DateTime.Now";
                    }
                    if (column.Name == MyApp.UpdateDateColumnName)
                    {
                        newItem.FormEditable = false;
                        newItem.GridEditable = false;
                        newItem.FormUse = false;
                        newItem.GridUse = false;
                        newItem.SearchUse = false;
                        newItem.ColumnDictionaryTr = "Değiştirme Zamanı";
                        newItem.ColumnDictionaryEn = "Update Time";
                        newItem.ColumnWriteConvertValue = "ActionWrite:DateTime.Now";
                    }
                    if (column.Name == MyApp.InsertUserColumnName)
                    {
                        newItem.FormEditable = false;
                        newItem.GridEditable = false;
                        newItem.FormUse = false;
                        newItem.GridUse = false;
                        newItem.SearchUse = false;
                        newItem.ColumnDictionaryTr = "Ekleyen Kullanıcı";
                        newItem.ColumnDictionaryEn = "User Who Added";
                        newItem.FormComponentType = "DropDownList";
                        newItem.GridComponentType = "DropDownList";
                        newItem.ColumnReferansJsonDataSource = "dsKullaniciSahip";
                        newItem.ColumnDefault = "ActionDefault:UserId";
                    }
                    if (column.Name == MyApp.UpdateUserColumnName)
                    {
                        newItem.FormEditable = false;
                        newItem.GridEditable = false;
                        newItem.FormUse = false;
                        newItem.GridUse = false;
                        newItem.SearchUse = false;

                        newItem.ColumnDictionaryTr = "Değiştiren Kullanıcı";
                        newItem.ColumnDictionaryEn = "User Who Changed";
                        newItem.FormComponentType = "DropDownList";
                        newItem.GridComponentType = "DropDownList";
                        newItem.ColumnReferansJsonDataSource = "dsKullaniciSahip";
                        newItem.ColumnWriteConvertValue = "ActionWrite:UserId";
                    }
                    #endregion

                    rTableOption.Fields.Add(newItem);
                }
                else
                {
                    var currentItem = rTableOption.Fields.Where(c => c.ColumnName == column.Name).FirstOrDefault();
                    currentItem.ColumnStatus = "Mevcut";
                    currentItem.ColumnOrder = column.Ordinal;
                    currentItem.ColumnDbType = column.DbDataType;
                    currentItem.ColumnNetType = column.DataType.NetDataType; // karşılaştıtıken Type.GetType("System.Int32");
                    currentItem.ColumnJsonType = this.FnNetTypeToJsonType(currentItem.ColumnNetType);
                }
            }

            rTableOption.Fields = rTableOption.Fields.OrderBy(o => o.ColumnOrder).ToList();

            return rTableOption;
        }

        public MyCustomResult FnTableOptionSave(string _TableOptionName, string _TableOptionText)
        {
            MyCustomResult rV = new();
            MyTableOption _TableOption = Newtonsoft.Json.JsonConvert.DeserializeObject<MyTableOption>(_TableOptionText);

            if (_TableOption.AreaName.MyToTrim().Length > 0)
            {

                _TableOption.Fields = _TableOption.Fields.Where(c => c.ColumnStatus != "Sil").OrderBy(o => o.ColumnOrder).ToList();

                foreach (var f in _TableOption.Fields)
                {
                    f.ColumnStatus = "Mevcut";
                }

                string TableOptionFileName = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory + "\\" + _TableOptionName + ".dat";

                _TableOption.LangKeyRoot = "x" + _TableOption.AreaName + "." + _TableOption.TableName;

                _TableOption.DataTransferObjectNameName = MyCodeGen.CodeGenAreasDirectory + "_" + _TableOption.AreaName + "_" + MyCodeGen.CodeGenDataTransferObjectDirectory + "_" + _TableOption.DataTransferObjectName;
                _TableOption.DataManipulationObjectNameName = MyCodeGen.CodeGenAreasDirectory + "_" + _TableOption.AreaName + "_" + MyCodeGen.CodeGenDataManipulationObjectDirectory + "_" + _TableOption.DataManipulationObjectName;
                _TableOption.RepositoryNameName = MyCodeGen.CodeGenAreasDirectory + "_" + _TableOption.AreaName + "_" + _TableOption.RepositoryName;
                _TableOption.ControllerNameName = MyCodeGen.CodeGenAreasDirectory + "_" + _TableOption.AreaName + "_" + MyCodeGen.CodeGenControllerDirectory + "_" + _TableOption.ControllerName;

                _TableOption.DataTransferObjectNameSpace = MyCodeGen.CodeGenAreasDirectory + "." + _TableOption.AreaName + "." + MyCodeGen.CodeGenDataTransferObjectDirectory;
                _TableOption.DataManipulationObjectNameSpace = MyCodeGen.CodeGenAreasDirectory + "." + _TableOption.AreaName + "." + MyCodeGen.CodeGenDataManipulationObjectDirectory;
                _TableOption.RepositoryNameSpace = MyCodeGen.CodeGenAreasDirectory + "." + _TableOption.AreaName + "." + MyCodeGen.CodeGenRepositoryDirectory;
                _TableOption.ControllerNameSpace = MyCodeGen.CodeGenAreasDirectory + "." + _TableOption.AreaName + "." + MyCodeGen.CodeGenControllerDirectory;

                _TableOption.FormViewName = _TableOption.TableName + "ForForm";
                _TableOption.GridViewName = _TableOption.TableName + "ForGrid";
                _TableOption.TreeListName = _TableOption.TableName + "ForTreeList";
                _TableOption.SearchViewName = _TableOption.TableName + "ForSearch";

                File.WriteAllText(TableOptionFileName, Newtonsoft.Json.JsonConvert.SerializeObject(_TableOption));

                rV.Message = "Table Option Kayıt edildi.";
            }
            else
            {
                rV.Error = true;
                rV.Message = "AreaName Seçiniz !";
            }

            return rV;
        }

        #endregion

        #region functions II
        public MyTableOption TableOptionLoad(string _TableOptionName)
        {
            string optionsFileName = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenDataDirectory + "\\" + _TableOptionName + ".dat";
            MyTableOption tableOptions = Newtonsoft.Json.JsonConvert.DeserializeObject<MyTableOption>(File.ReadAllText(optionsFileName));
            return tableOptions;
        }

        public MyCustomResult DataTransferObjectWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();
            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);
                if (!oTableOptions.DataTransferObjectLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.DataTransferObjectGenerate(oTableOptions);
                    string codeDirectory = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenDataTransferObjectDirectory;
                    string codeFileName = oTableOptions.DataTransferObjectName + ".cs";
                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message = "Data Transfer Object eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message = "Data Transfer Object kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }

        public MyCustomResult DataManipulationObjectWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);

                if (!oTableOptions.DataManipulationObjectLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.DataManipulationObjectGenerate(oTableOptions);
                    string codeDirectory = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenDataManipulationObjectDirectory;
                    string codeFileName = oTableOptions.DataManipulationObjectName + ".cs";
                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message = "Data Manipulation Object eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message = "Data Manipulation Object kilitli.";
                }

                // --------Repository yok ise--------------------------------------------------------
                string code_generator_property_line = "public " + oTableOptions.DataManipulationObjectNameSpace + "." + oTableOptions.DataManipulationObjectName + " " + oTableOptions.RepositoryNameName + " { get; private set; }  /*auto generated*/";
                string code_generator_constructor_line = "this." + oTableOptions.RepositoryNameName + " = new " + oTableOptions.DataManipulationObjectNameSpace + "." + oTableOptions.DataManipulationObjectName + "(this.dataContext);  /*auto generated*/";

                string directoryNameRepository = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenRepositoryDirectory;
                string fileNameRepository = "_Rep.cs";

                if (!Directory.Exists(directoryNameRepository))
                {
                    Directory.CreateDirectory(directoryNameRepository);
                }

                if (!File.Exists(directoryNameRepository + "/" + fileNameRepository))
                {
                    StringBuilder sb = new();
                    sb.AppendLine("using System;");

                    sb.AppendLine("");
                    sb.AppendLine("//<!-- Auto Generated -->");
                    sb.AppendLine("");
                    sb.AppendLine("namespace " + MyCodeGen.CodeGenRootNameSpace + ".Models");
                    sb.AppendLine("{");
                    sb.AppendLine(" public partial class _Rep");
                    sb.AppendLine(" {");
                    sb.AppendLine("     /*code_generator_property_start*/");
                    sb.AppendLine("     /*code_generator_property_end*/");
                    sb.AppendLine("     ");
                    sb.AppendLine("     public void Init_" + oTableOptions.AreaName + "()");
                    sb.AppendLine("     {");
                    sb.AppendLine("         /*code_generator_constructor_start*/");
                    sb.AppendLine("         /*code_generator_constructor_end*/");
                    sb.AppendLine("     }");
                    sb.AppendLine(" }");
                    sb.AppendLine("}");
                    File.WriteAllText(directoryNameRepository + "/" + fileNameRepository, sb.ToString());
                }
                //--Repository init eklenmesi--------------------------------------------------------------------
                ///*code_generator_rep_init_end*/
                //this.rep.Init_Tem();
                //--------------------------------------------------------------------------

                List<string> newlines = new();
                string[] lines = File.ReadAllLines(directoryNameRepository + "\\" + fileNameRepository);

                foreach (string line in lines)
                {
                    if (lines.Where(c => c.Contains(code_generator_property_line)).FirstOrDefault() == null)
                    {
                        if (line.Trim() == "/*code_generator_property_end*/")
                        {
                            newlines.Add("        " + code_generator_property_line + "/*" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "*/");
                            rV.Message += "Repositories property eklendi." + Environment.NewLine;
                        }
                    }

                    if (lines.Where(c => c.Contains(code_generator_constructor_line)).FirstOrDefault() == null)
                    {
                        if (line.Trim() == "/*code_generator_constructor_end*/")
                        {
                            newlines.Add("            " + code_generator_constructor_line + "/*" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "*/");
                            rV.Message += "Repositories constructor eklendi." + Environment.NewLine;
                        }
                    }

                    newlines.Add(line);
                }

                File.WriteAllLines(directoryNameRepository + "\\" + fileNameRepository, newlines);
                //------------------------------------------------------------------------------------

            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }
            return rV;
        }

        public MyCustomResult ControllerWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);

                if (!oTableOptions.ControllerLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.ControllerGenerate(oTableOptions);
                    string codeDirectory = MyApp.EnvContentRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenControllerDirectory;
                    string codeFileName = oTableOptions.ControllerName + MyCodeGen.CodeGen_ControllerSuffix + ".cs";
                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message += "Controller eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Controller kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }
            return rV;
        }

        public MyCustomResult DictionaryWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);

                if (!oTableOptions.DictionaryLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.DictionaryGenerate(oTableOptions);
                    string codeDirectory = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenDictionaryDirectory;
                    string codeFileName = oTableOptions.TableName + ".json";

                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message += "Dictionary eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Dictionary kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }

        public MyCustomResult FormViewWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);
                if (!oTableOptions.FormViewLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.FormViewGenerate(oTableOptions);
                    string codeDirectory = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                    string codeFileName = oTableOptions.FormViewName + ".html";

                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message += "Form View eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Form View kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }
            return rV;
        }

        public MyCustomResult GridViewWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);
                if (!oTableOptions.GridViewLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.GridViewGenerate(oTableOptions, false);
                    string codeDirectory = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                    string codeFileName = oTableOptions.GridViewName + ".html";

                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message += "Grid View eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Grid View kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }

        public MyCustomResult TreeListWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);

                if (!oTableOptions.TreeListLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = this.TreeListGenerate(oTableOptions, false);
                    string codeDirectory = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                    string codeFileName = oTableOptions.TreeListName + ".html";

                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }
                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());
                    //----- diskeyazım bit ------------------------------------------
                    rV.Message += "Tree List eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Tree List kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }

        public MyCustomResult SearchViewWrite(string _TableOptionName)
        {
            MyCustomResult rV = new();

            try
            {
                MyTableOption oTableOptions = TableOptionLoad(_TableOptionName);

                if (!oTableOptions.SearchViewLock)
                {
                    //----- diskeyazım baş ------------------------------------------
                    StringBuilder sbCodes = new();

                    if (oTableOptions.SearchViewType == EnmSearchViewType.TreeList.ToString())
                    {
                        sbCodes = this.TreeListGenerate(oTableOptions, true);
                    }
                    else
                    {
                        sbCodes = this.GridViewGenerate(oTableOptions, true);
                    }

                    string codeDirectory = MyApp.EnvWebRootPath + "\\" + MyCodeGen.CodeGenAreasDirectory + "\\" + oTableOptions.AreaName + "\\" + MyCodeGen.CodeGenViewDirectory;
                    string codeFileName = oTableOptions.SearchViewName + ".html";

                    if (!Directory.Exists(codeDirectory))
                    {
                        Directory.CreateDirectory(codeDirectory);
                    }

                    File.WriteAllText(codeDirectory + "\\" + codeFileName, sbCodes.ToString());

                    //----- diske yazım bit ------------------------------------------
                    rV.Message += "Search View eklendi." + Environment.NewLine;
                }
                else
                {
                    rV.Error = true;
                    rV.Message += "Search View kilitli." + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }
        #endregion

        #region functions III
        public MyCustomResult CodeWriteAll(string _TableOptionName, string _CodeName)
        {
            MyCustomResult rV = new();

            try
            {
                MyCodeGen myCodeGen = new(this.dataContext);
                MyTableOption oTableOptions = myCodeGen.TableOptionLoad(_TableOptionName);

                if (oTableOptions.AreaName.MyToTrim().Length > 0)
                {
                    //
                    if (_CodeName == "DataTransferObjectWrite" || (_CodeName == "*" && oTableOptions.DataTransferObjectAutoCreate))
                    {

                        MyCustomResult result = myCodeGen.DataTransferObjectWrite(_TableOptionName);
                        if (rV.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "DataManipulationObjectWrite" || (_CodeName == "*" && oTableOptions.DataManipulationObjectAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.DataManipulationObjectWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "ControllerWrite" || (_CodeName == "*" && oTableOptions.ControllerAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.ControllerWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "DictionaryWrite" || (_CodeName == "*" && oTableOptions.DictionaryAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.DictionaryWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "FormViewWrite" || (_CodeName == "*" && oTableOptions.FormViewAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.FormViewWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "GridViewWrite" || (_CodeName == "*" && oTableOptions.GridViewAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.GridViewWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "TreeListWrite" || (_CodeName == "*" && oTableOptions.TreeListAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.TreeListWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }

                    //
                    if (_CodeName == "SearchViewWrite" || (_CodeName == "*" && oTableOptions.SearchViewAutoCreate))
                    {
                        MyCustomResult result = myCodeGen.SearchViewWrite(_TableOptionName);
                        if (result.Error) { rV.Error = result.Error; }
                        rV.Message += "\n" + result.Message;
                    }
                }
                else
                {
                    rV.Error = true;
                    rV.Message = "AreaName Seçiniz !";
                }
            }
            catch (Exception ex)
            {
                rV.Error = true;
                rV.Message = ex.MyLastInner().Message;
            }

            return rV;
        }
        #endregion

        #region Backend Generate code

        public StringBuilder DataTransferObjectGenerate(MyTableOption _oTableOptions)
        {
            StringBuilder sbCodes = new();
            sbCodes.AppendLine("using System;");
            sbCodes.AppendLine("using System.Collections.Generic;");
            sbCodes.AppendLine("using System.Linq;");
            sbCodes.AppendLine("using Microsoft.EntityFrameworkCore;");
            sbCodes.AppendLine("using WebApp1.Codes;");
            sbCodes.AppendLine("using WebApp1.Models;");

            sbCodes.AppendLine("");

            sbCodes.AppendLine("//<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("namespace " + MyCodeGen.CodeGenRootNameSpace + "." + _oTableOptions.DataTransferObjectNameSpace);

            sbCodes.AppendLine("{");

            sbCodes.AppendLine("    public partial class " + _oTableOptions.DataTransferObjectName + " : " + _oTableOptions.TableName);
            sbCodes.AppendLine("    {");
            sbCodes.AppendLine("        protected readonly DataContext dataContext;");
            sbCodes.AppendLine("");

            if (!string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("        public int? Cc" + _oTableOptions.ParentColumnName + "{");
                sbCodes.AppendLine("            get");
                sbCodes.AppendLine("            {");
                sbCodes.AppendLine("                int? rV = null;");
                sbCodes.AppendLine("                if (this." + _oTableOptions.ParentColumnName + " > 0)");
                sbCodes.AppendLine("                {");
                sbCodes.AppendLine("                    rV = this." + _oTableOptions.ParentColumnName + ";");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("                return rV;");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("        public Boolean HasChildren {");
                sbCodes.AppendLine("            get");
                sbCodes.AppendLine("            {");
                sbCodes.AppendLine("                Boolean rV = false;");
                sbCodes.AppendLine("                try");
                sbCodes.AppendLine("                {");
                sbCodes.AppendLine("                    rV = this.dataContext.TemAdres.Where(c => c." + _oTableOptions.ParentColumnName + " == this." + _oTableOptions.PrimaryKey + ").Any();");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("                catch { }");
                sbCodes.AppendLine("                return rV;");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");

            }

            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse))
            {
                if (col.FormComponentType == EnmComponentType.DownloadFileLink.ToString() || col.GridComponentType == EnmComponentType.DownloadFileLink.ToString())
                {
                    sbCodes.AppendLine("        public string Cc" + col.ColumnName + "Link");
                    sbCodes.AppendLine("        {");
                    sbCodes.AppendLine("           get");
                    sbCodes.AppendLine("           {");
                    sbCodes.AppendLine("               string rV = string.Empty;");
                    sbCodes.AppendLine("               try");
                    sbCodes.AppendLine("               {");
                    sbCodes.AppendLine("                   if (this." + col.ColumnName + ".MyToTrim().Length > 0)");
                    sbCodes.AppendLine("                   {");
                    sbCodes.AppendLine("                       string fileName = System.IO.Path.GetFileName(this.DownloadFileUrl.Trim());");
                    sbCodes.AppendLine("                       rV = \"<a href='\" + this.DownloadFileUrl.Trim() + \"' download>\" + fileName + \"</a>\";");
                    sbCodes.AppendLine("                   }");
                    sbCodes.AppendLine("               }");
                    sbCodes.AppendLine("               catch { }");
                    sbCodes.AppendLine("               return rV;");
                    sbCodes.AppendLine("           }");
                    sbCodes.AppendLine("        }");
                }

                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    if (col.ColumnReferansJsonDataSource == "dsEvetHayir")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("            get { return (this." + col.ColumnName + " ? MyApp.TranslateTo(\"xLng.Evet\", this.dataContext.Language) : MyApp.TranslateTo(\"xLng.Hayir\", this.dataContext.Language)); }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsAktifPasif")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("            get { return (this." + col.ColumnName + " ? MyApp.TranslateTo(\"xLng.Aktif\", this.dataContext.Language) : MyApp.TranslateTo(\"xLng.Pasif\", this.dataContext.Language)); }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsDogruYanlis")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("            get { return (this." + col.ColumnName + " ? MyApp.TranslateTo(\"xLng.Dogru\", this.dataContext.Language) : MyApp.TranslateTo(\"xLng.Yanlis\", this.dataContext.Language)); }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsCRUD")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   switch (this." + col.ColumnName + ")");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       case \"C\":");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Create\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                       case \"R\":");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Read\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                       case \"U\":");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Update\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                       case \"D\":");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Delete\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsZamanTur")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   switch (this." + col.ColumnName + ")");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       case 0:");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Gunluk\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                       case 1:");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Haftalik\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                       case 2:");
                        sbCodes.AppendLine("                           rV = MyApp.TranslateTo(\"xLng.Aylik\", this.dataContext.Language);");
                        sbCodes.AppendLine("                           break;");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsUlkeSehirIlce")
                    {
                        if (col.FormComponentType == EnmComponentType.MultiSelect.ToString() || col.GridComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                            sbCodes.AppendLine("        {");
                            sbCodes.AppendLine("            get");
                            sbCodes.AppendLine("            {");
                            sbCodes.AppendLine("                string rV = string.Empty;");
                            sbCodes.AppendLine("                try");
                            sbCodes.AppendLine("                {");
                            sbCodes.AppendLine("                    if (this." + col.ColumnName + ".MyToTrim().Length > 0)");
                            sbCodes.AppendLine("                    {");
                            sbCodes.AppendLine("                        foreach (string colValue in this." + col.ColumnName + ".Split(','))");
                            sbCodes.AppendLine("                        {");
                            sbCodes.AppendLine("                            int id = Convert.ToInt32(colValue.MyToInt());");
                            sbCodes.AppendLine("");
                            sbCodes.AppendLine("                            var queryResult = this.dataContext.TemIlce.Where(c=> c.Id == id)");
                            sbCodes.AppendLine("                            .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + \" / \" + s.Sehir.Ad.MyToTrim() + \" / \" + s.Sehir.Ulke.Ad.MyToTrim() })");
                            sbCodes.AppendLine("                            .FirstOrDefault();");
                            sbCodes.AppendLine("");
                            sbCodes.AppendLine("                            if (queryResult != null)");
                            sbCodes.AppendLine("                            {");
                            sbCodes.AppendLine("                                rV += queryResult.text + \" \";");
                            sbCodes.AppendLine("                            }");
                            sbCodes.AppendLine("                        }");
                            sbCodes.AppendLine("                    }");
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("                catch { }");
                            sbCodes.AppendLine("                return rV;");
                            sbCodes.AppendLine("            }");
                            sbCodes.AppendLine("        }");
                        }
                        else
                        {
                            sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                            sbCodes.AppendLine("        {");
                            sbCodes.AppendLine("           get");
                            sbCodes.AppendLine("           {");
                            sbCodes.AppendLine("               string rV = string.Empty;");
                            sbCodes.AppendLine("               try");
                            sbCodes.AppendLine("               {");
                            sbCodes.AppendLine("                   var queryResult = this.dataContext.TemIlce.Where(c => c.Id == this." + col.ColumnName + ")");
                            sbCodes.AppendLine("                       .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + \" / \" + s.Sehir.Ad.MyToTrim() + \" / \" + s.Sehir.Ulke.Ad.MyToTrim() })");
                            sbCodes.AppendLine("                       .FirstOrDefault();");
                            sbCodes.AppendLine("");
                            sbCodes.AppendLine("                   if (queryResult != null)");
                            sbCodes.AppendLine("                   {");
                            sbCodes.AppendLine("                       rV += queryResult.text;");
                            sbCodes.AppendLine("                   }");
                            sbCodes.AppendLine("               }");
                            sbCodes.AppendLine("               catch { }");
                            sbCodes.AppendLine("               return rV;");
                            sbCodes.AppendLine("           }");
                            sbCodes.AppendLine("        }");
                        }
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsEnmSahipTurForDescription")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   rV = ((EnmSahipTur)this." + col.ColumnName + ").MyGetDescription();");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsKullaniciSahip")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   Areas.Tem.Codes.TemBusiness temBusiness = new(this.dataContext);");
                        sbCodes.AppendLine("                   var queryResult = this.dataContext.TemKullanici.Where(c => c.Id == this." + col.ColumnName + ")");
                        sbCodes.AppendLine("                   .Select(s => new { value = s.Id, text = s.Ad + \" <\" + temBusiness.GetKullaniciSahipAdSahipTur(s.Id) + \">\" })");
                        sbCodes.AppendLine("                   .FirstOrDefault();");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                   if (queryResult != null)");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV = queryResult.text;");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsProjeDers")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   var queryResult = this.dataContext.RobProje.Where(c => c.Id == this." + col.ColumnName + ")");
                        sbCodes.AppendLine("                       .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + \" <\" + s.Ders.Ad.MyToTrim() + \"> \" })");
                        sbCodes.AppendLine("                       .FirstOrDefault();");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                   if (queryResult != null)");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV += queryResult.text;");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsSubeOkul")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   var queryResult = this.dataContext.RobSube.Where(c => c.Id == this." + col.ColumnName + ")");
                        sbCodes.AppendLine("                       .Select(s => new { value = s.Id, text = s.Ad.MyToTrim() + \" <\" + s.Okul.Ad.MyToTrim() + \"> \" })");
                        sbCodes.AppendLine("                       .FirstOrDefault();");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                   if (queryResult != null)");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV += queryResult.text;");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else if (col.ColumnReferansJsonDataSource == "dsHaftaNoList")
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   if (this." + col.ColumnName + " == 0)");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV = MyApp.TranslateTo(\"xLng.UyumHaftasi\", this.dataContext.Language);");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("                   else if (this." + col.ColumnName + " == 99)");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV = MyApp.TranslateTo(\"xLng.Havuz\", this.dataContext.Language);");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("                   else");
                        sbCodes.AppendLine("                   {");
                        sbCodes.AppendLine("                       rV = this." + col.ColumnName + ".MyToStr() +\".\"+ MyApp.TranslateTo(\"xLng.Hafta\", this.dataContext.Language);");
                        sbCodes.AppendLine("                   }");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                    else
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName);
                        sbCodes.AppendLine("        {");
                        sbCodes.AppendLine("           get");
                        sbCodes.AppendLine("           {");
                        sbCodes.AppendLine("               string rV = string.Empty;");
                        sbCodes.AppendLine("               try");
                        sbCodes.AppendLine("               {");
                        sbCodes.AppendLine("                   rV = this." + col.ColumnName + ".MyToTrim();");
                        sbCodes.AppendLine("               }");
                        sbCodes.AppendLine("               catch { }");
                        sbCodes.AppendLine("               return rV;");
                        sbCodes.AppendLine("           }");
                        sbCodes.AppendLine("        }");
                    }
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    //foren key yok ise olmalı, datayı bağlantıdan getiriyor
                    if (col.ColumnReferansDisplayColumnRead)
                    {
                        if (col.FormComponentType == EnmComponentType.MultiSelect.ToString() || col.GridComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            sbCodes.AppendLine("        public string Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "{");
                            sbCodes.AppendLine("            get {");
                            sbCodes.AppendLine("                string rV = string.Empty;");
                            sbCodes.AppendLine("                try");
                            sbCodes.AppendLine("                {");
                            sbCodes.AppendLine("                    if (!string.IsNullOrEmpty(this." + col.ColumnName + ") && this." + col.ColumnName + ".Trim().Length > 0)");
                            sbCodes.AppendLine("                    {");
                            sbCodes.AppendLine("                        foreach (string s in this." + col.ColumnName + ".Split(','))");
                            sbCodes.AppendLine("                        {");
                            sbCodes.AppendLine("                            int id = Convert.ToInt32(s.MyToInt());");
                            foreach (string displayColumnName in col.ColumnReferansDisplayColumnNames.Split(','))
                            {
                                sbCodes.AppendLine("                            if (rV != string.Empty) { rV += MyApp.Ayrac; }");
                                sbCodes.AppendLine("                            rV += this.dataContext." + col.ColumnReferansTableName + ".Where(c => c." + col.ColumnReferansValueColumnName + " == id).FirstOrDefault()." + displayColumnName + ".MyToTrim();");
                            }
                            sbCodes.AppendLine("                        }");
                            sbCodes.AppendLine("                    }");
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("                catch { }");
                            sbCodes.AppendLine("                return rV;");
                            sbCodes.AppendLine("            }");
                            sbCodes.AppendLine("        }");
                        }
                        else
                        {
                            sbCodes.AppendLine("        public string Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "{");
                            sbCodes.AppendLine("            get {");
                            sbCodes.AppendLine("                string rV = string.Empty;");
                            sbCodes.AppendLine("                try");
                            sbCodes.AppendLine("                {");
                            foreach (string displayColumnName in col.ColumnReferansDisplayColumnNames.Split(','))
                            {
                                sbCodes.AppendLine("                    if (rV != string.Empty) { rV += MyApp.Ayrac; }");
                                sbCodes.AppendLine("                    rV += this.dataContext." + col.ColumnReferansTableName + ".Where(c => c." + col.ColumnReferansValueColumnName + " == this." + col.ColumnName + ").FirstOrDefault()." + displayColumnName + ".MyToTrim();");
                            }
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("                catch { }");
                            sbCodes.AppendLine("                    return rV;");
                            sbCodes.AppendLine("            }");
                            sbCodes.AppendLine("        }");
                        }
                    }
                    else
                    {
                        sbCodes.AppendLine("        public string Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "{ get; set; }");
                    }
                }
            }

            sbCodes.AppendLine("");
            sbCodes.AppendLine("        //Constructor");
            sbCodes.AppendLine("        public " + _oTableOptions.DataTransferObjectName + "(DataContext dataContext)");
            sbCodes.AppendLine("        {");
            sbCodes.AppendLine("            this.dataContext = dataContext;");
            //sbCodes.AppendLine("            this.dataContext = new DataContext();");
            //sbCodes.AppendLine("            this.dataContext.Database.GetDbConnection().ConnectionString = MyApp.Configs.DefaultConnectionString;");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("    }");

            sbCodes.AppendLine("}");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("");
            return sbCodes;
        }

        public StringBuilder DataManipulationObjectGenerate(MyTableOption _oTableOptions)
        {
            StringBuilder sbCodes = new();
            var tableInfo = this.dataContext.GetTableInfo(_oTableOptions.TableName);

            sbCodes.AppendLine("using System;");
            sbCodes.AppendLine("using System.Collections.Generic;");
            sbCodes.AppendLine("using System.Linq;");
            sbCodes.AppendLine("using Microsoft.EntityFrameworkCore;");
            sbCodes.AppendLine("using WebApp1.Codes;");
            sbCodes.AppendLine("using WebApp1.Models;");
            sbCodes.AppendLine("using " + MyCodeGen.CodeGenRootNameSpace + "." + _oTableOptions.DataTransferObjectNameSpace + ";");

            sbCodes.AppendLine("");
            sbCodes.AppendLine("//<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("namespace " + MyCodeGen.CodeGenRootNameSpace + "." + _oTableOptions.DataManipulationObjectNameSpace);
            sbCodes.AppendLine("{");
            sbCodes.AppendLine(" public class " + _oTableOptions.DataManipulationObjectName + " : BaseDmo");
            sbCodes.AppendLine(" {");
            sbCodes.AppendLine("     public " + _oTableOptions.DataManipulationObjectName + "(DataContext dataContext) : base(dataContext) { }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("     public IQueryable<" + _oTableOptions.DataTransferObjectName + "> Get()");
            sbCodes.AppendLine("     {");
            sbCodes.AppendLine("         return this.dataContext." + _oTableOptions.TableName + ".AsNoTracking()");
            sbCodes.AppendLine("             .Select(s => new " + _oTableOptions.DataTransferObjectName + "(this.dataContext)");
            sbCodes.AppendLine("             {");

            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse))
            {
                if (!string.IsNullOrEmpty(col.ColumnReadConvertValue))
                {
                    if (col.ColumnReadConvertValue == "ActionRead:NumericZero")
                    {
                        sbCodes.AppendLine("                 " + col.ColumnName + " = " + 0 + ",");
                    }
                    else if (col.ColumnReadConvertValue == "ActionRead:StringEmpty")
                    {
                        sbCodes.AppendLine("                 " + col.ColumnName + " = " + "string.Empty" + ",");
                    }
                    else if (col.ColumnReadConvertValue == "ActionRead:DecryptPassword")
                    {
                        sbCodes.AppendLine("                 " + col.ColumnName + " = " + "string.Empty" + ",");
                        sbCodes.AppendLine("                 " + col.ColumnName + " = s." + col.ColumnName + ".MyToDecryptPassword(),");
                    }
                    else if (col.ColumnReadConvertValue == "ActionRead:MyFromBase64Str")
                    {
                        sbCodes.AppendLine("                 " + col.ColumnName + " = s." + col.ColumnName + ".MyFromBase64Str(),");
                    }
                    else
                    {
                        sbCodes.AppendLine("                 " + col.ColumnName + " = s." + col.ColumnName + "." + col.ColumnReadConvertValue.Split(":")[1] + "(),");
                    }
                }
                else
                {
                    sbCodes.AppendLine("                 " + col.ColumnName + " = s." + col.ColumnName + ",");
                }
            }

            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse))
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    if (!col.ColumnReferansDisplayColumnRead)
                    {
                        //boş
                    }
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) & !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) & !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    if (!col.ColumnReferansDisplayColumnRead)
                    {
                        var colNameList = col.ColumnReferansDisplayColumnNames.Split(",");
                        if (colNameList.Length == 1)
                        {
                            sbCodes.AppendLine("                 " + "Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + " = " + "s." + col.ColumnName.Substring(0, col.ColumnName.Length - 2) + "." + col.ColumnReferansDisplayColumnNames + ".MyToTrim(),");
                        }
                        else
                        {
                            string sLine = string.Empty;
                            foreach (var colName in colNameList)
                            {
                                if (!string.IsNullOrEmpty(sLine))
                                {
                                    sLine += " + \" \" + ";
                                }
                                sLine += "s." + col.ColumnName.Substring(0, col.ColumnName.Length - 2) + "." + colName + ".MyToTrim()";
                            }

                            sbCodes.AppendLine("                 " + "Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + " = " + sLine + ",");
                        }

                    }
                }
            }

            sbCodes.Remove(sbCodes.Length - 3, 1);// son virgülü atmak için (sonundaki entera dokunmadan)

            sbCodes.AppendLine("             });");
            sbCodes.AppendLine("     }");

            sbCodes.AppendLine("");

            sbCodes.AppendLine("     public " + _oTableOptions.DataTransferObjectName + " GetByNew()");
            sbCodes.AppendLine("     {");
            sbCodes.AppendLine("         //Default değerler ile bir row döner");
            sbCodes.AppendLine("         " + _oTableOptions.DataTransferObjectName + " row = new(this.dataContext) { };");
            sbCodes.AppendLine("         // Burada field default değerleri veriliyor...");

            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse))
            {
                if (!string.IsNullOrEmpty(col.ColumnDefault))
                {
                    if (col.ColumnDefault.StartsWith("ActionDefault:"))
                    {
                        if (col.ColumnDefault == "ActionDefault:DateTime.Now")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = DateTime.Now;");
                        }
                        else if (col.ColumnDefault == "ActionDefault:OrderMaxPlusOne")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = " + "this.dataContext." + _oTableOptions.TableName + ".DefaultIfEmpty().Max(m => m == null ? 0 : m." + col.ColumnName + ") + 1;");
                        }
                        else if (col.ColumnDefault == "ActionDefault:NewGuid")
                        {
                            if (col.ColumnDbType == "uniqueidentifier")
                            {
                                sbCodes.AppendLine("         row." + col.ColumnName + " = Guid.NewGuid();");
                            }
                            else
                            {
                                sbCodes.AppendLine("         row." + col.ColumnName + " = Guid.NewGuid().ToString();");
                            }
                        }
                        else if (col.ColumnDefault == "ActionDefault:NewGuidToUpper")
                        {
                            if (col.ColumnDbType == "uniqueidentifier")
                            {
                                sbCodes.AppendLine("         row." + col.ColumnName + " = Guid.NewGuid();");
                            }
                            else
                            {
                                sbCodes.AppendLine("         row." + col.ColumnName + " = Guid.NewGuid().ToString().MyToUpper();");
                            }
                        }
                        else if (col.ColumnDefault == "ActionDefault:UniqueRandomNumber16")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = new Random().Next(10,99) + DateTime.Now.ToString(\"yyMMddHHmmssff\");");
                        }
                        else if (col.ColumnDefault == "ActionDefault:UserId")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = this.dataContext.UserId;");
                        }
                        else if (col.ColumnDefault == "ActionDefault:UserName")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = this.dataContext.UserName;");
                        }
                        else if (col.ColumnDefault == "ActionDefault:Star")
                        {
                            sbCodes.AppendLine("         row." + col.ColumnName + " = \"*\";");
                        }
                    }
                    else
                    {
                        sbCodes.AppendLine("         row." + col.ColumnName + " = " + col.ColumnDefault + ";");
                    }
                }
            }

            sbCodes.AppendLine("");
            sbCodes.AppendLine("         return row;");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("     public IEnumerable<" + _oTableOptions.DataTransferObjectName + "> GetById(int _id)");
            sbCodes.AppendLine("     {");
            sbCodes.AppendLine("         return this.Get().Where(c => c." + tableInfo.PrimaryKeyColumn.Name + " == _id);");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");


            sbCodes.AppendLine("     public int CreateOrUpdate(" + _oTableOptions.DataTransferObjectName + " _model)");
            sbCodes.AppendLine("     {");
            sbCodes.AppendLine("         " + _oTableOptions.TableName + " row;");
            sbCodes.AppendLine("         Boolean isNew = _model." + tableInfo.PrimaryKeyColumn.Name + " == 0;");
            sbCodes.AppendLine("         ");
            sbCodes.AppendLine("         if (isNew)");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             //sadece insertte eklenip, update de değişmeyecek alanlar buraya");
            sbCodes.AppendLine("             row = new " + _oTableOptions.TableName + "() { };");

            if (string.IsNullOrEmpty(_oTableOptions.SequenceName))
            {
                //auto inc olarak işaretli değil ise kontrolüde yapmak lazım, ilerde
                sbCodes.AppendLine("             row." + tableInfo.PrimaryKeyColumn.Name + " = this.dataContext." + _oTableOptions.TableName + ".DefaultIfEmpty().Max(m => m == null ? 0 : m.Id) + 1;");
            }
            else
            {
                sbCodes.AppendLine("             row." + tableInfo.PrimaryKeyColumn.Name + " = (int)this.dataContext.GetNextSequenceValue(\"" + _oTableOptions.SequenceName + "\");");
            }

            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("         else");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             row = this.dataContext." + _oTableOptions.TableName + ".Where(c => c." + tableInfo.PrimaryKeyColumn.Name + " == _model." + tableInfo.PrimaryKeyColumn.Name + ").FirstOrDefault();");
            sbCodes.AppendLine("             if (row == null)");
            sbCodes.AppendLine("             {");
            sbCodes.AppendLine("                 throw new Exception(MyApp.TranslateTo(\"xLng.IslemYapilacakKayitBulunamadi\", this.dataContext.Language));");
            sbCodes.AppendLine("             }");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("         ");


            foreach (var column in tableInfo.Columns)
            {
                if (!column.IsPrimaryKey)
                {
                    var colOpt = _oTableOptions.Fields.Where(c => c.ColumnUse && c.ColumnName == column.Name).FirstOrDefault();
                    if (colOpt != null)
                    {
                        if (string.IsNullOrEmpty(colOpt.ColumnWriteConvertValue))
                        {
                            sbCodes.AppendLine("         row." + column.Name + " = _model." + column.Name + ";");
                        }
                        else
                        {
                            if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:EncryptPassword"))
                            {
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("         if (!string.IsNullOrEmpty(_model." + column.Name + "))");
                                sbCodes.AppendLine("         {");
                                sbCodes.AppendLine("             row." + column.Name + " = _model." + column.Name + ".MyToEncryptPassword();");
                                sbCodes.AppendLine("         }");
                                sbCodes.AppendLine("");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:MyToUpper"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = _model." + column.Name + ".MyToUpper();");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:MyToTitleCase"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = _model." + column.Name + ".MyToTitleCase();");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:MyToMD5"))
                            {
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("         if (!string.IsNullOrEmpty(_model." + column.Name + "))");
                                sbCodes.AppendLine("         {");
                                sbCodes.AppendLine("             row." + column.Name + " = _model." + column.Name + ".MyToMD5();");
                                sbCodes.AppendLine("         }");
                                sbCodes.AppendLine("");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:MyToLatinString"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = _model." + column.Name + ".MyToLatinString();");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:UserId"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = this.dataContext.UserId;");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:UserName"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = this.dataContext.UserName;");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:DateTime.Now"))
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = DateTime.Now;");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:IfZeroThenOrderMaxPlusOne"))
                            {
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("         if (_model." + column.Name + " != 0 )");
                                sbCodes.AppendLine("         {");
                                sbCodes.AppendLine("             row." + column.Name + " = _model." + column.Name + ";");
                                sbCodes.AppendLine("         } ");
                                sbCodes.AppendLine("         else {");
                                sbCodes.AppendLine("            row." + column.Name + " = " + "this.dataContext." + _oTableOptions.TableName);
                                if (!string.IsNullOrEmpty(_oTableOptions.GridViewMasterColumnName))
                                {
                                    sbCodes.AppendLine("                .Where(c => c." + _oTableOptions.GridViewMasterColumnName + " == _model." + _oTableOptions.GridViewMasterColumnName + ")");
                                }
                                sbCodes.AppendLine("                .DefaultIfEmpty().Max(m => m == null ? 0 : m." + column.Name + ") + 1;");
                                sbCodes.AppendLine("         }");
                                sbCodes.AppendLine("");
                            }
                            else if (colOpt.ColumnWriteConvertValue.StartsWith("ActionWrite:IfStarThenCodePlusOne"))
                            {
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("         if (_model." + column.Name + ".MyToTrim() == \"*\" )");
                                sbCodes.AppendLine("         {");
                                sbCodes.AppendLine("            string yilAy = DateTime.Now.ToString(\"yy\") + DateTime.Now.ToString(\"MM\").MyMoonToStr();");
                                sbCodes.AppendLine("            int inc = 0;");
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("            var lastDataRow = this.dataContext." + _oTableOptions.TableName + ".AsNoTracking().Where(c => c." + column.Name + ".StartsWith(yilAy)).OrderBy(o => o." + column.Name + ").LastOrDefault();");
                                sbCodes.AppendLine("            if (lastDataRow != null)");
                                sbCodes.AppendLine("            {");
                                sbCodes.AppendLine("                inc = lastDataRow." + column.Name + ".Replace(yilAy, \"\").MyToInt();");
                                sbCodes.AppendLine("            }");
                                sbCodes.AppendLine("");
                                sbCodes.AppendLine("            inc++;");
                                sbCodes.AppendLine("            row." + column.Name + " = yilAy + inc.MyToStr().PadLeft(3, '0');");
                                sbCodes.AppendLine("         }");
                                sbCodes.AppendLine("         else");
                                sbCodes.AppendLine("         {");
                                sbCodes.AppendLine("             row." + column.Name + " = _model." + column.Name + ";");
                                sbCodes.AppendLine("         }");
                                sbCodes.AppendLine("");
                            }
                            else
                            {
                                sbCodes.AppendLine("         row." + column.Name + " = _model." + column.Name + "." + colOpt.ColumnWriteConvertValue.Split(":")[1] + "();");
                            }
                        }
                    }

                }
            }
            sbCodes.AppendLine("");

            sbCodes.AppendLine("         if (!isNew)");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             //sadece update eklenip, insertte de değişmeyecek alanlar buraya");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("         ");
            sbCodes.AppendLine("         if (isNew)");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             this.dataContext." + _oTableOptions.TableName + ".Add(row);");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("         ");
            sbCodes.AppendLine("         return row." + tableInfo.PrimaryKeyColumn.Name + ";");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("     public bool Delete(int _id)");
            sbCodes.AppendLine("     {");
            sbCodes.AppendLine("         Boolean rV = false;");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         var row = this.dataContext." + _oTableOptions.TableName + ".Where(c => c." + tableInfo.PrimaryKeyColumn.Name + " == _id).FirstOrDefault();");
            sbCodes.AppendLine("         if (row != null)");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             this.dataContext." + _oTableOptions.TableName + ".Remove(row);");
            sbCodes.AppendLine("             rV = true;");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("         else");
            sbCodes.AppendLine("         {");
            sbCodes.AppendLine("             throw new Exception(MyApp.TranslateTo(\"xLng.IslemYapilacakKayitBulunamadi\", this.dataContext.Language));");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         return rV;");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");


            sbCodes.AppendLine("     #region Ek fonksiyonlar");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("     #endregion");
            sbCodes.AppendLine("");

            sbCodes.AppendLine(" }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("}");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("");
            return sbCodes;
        }

        public StringBuilder ControllerGenerate(MyTableOption _oTableOptions)
        {
            StringBuilder sbCodes = new();
            sbCodes.AppendLine("");
            sbCodes.AppendLine("//<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");

            #region using

            sbCodes.AppendLine("using System;");
            sbCodes.AppendLine("using System.Linq;");
            sbCodes.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sbCodes.AppendLine("using Microsoft.Extensions.Options;");
            sbCodes.AppendLine("using Microsoft.AspNetCore.Http;");
            sbCodes.AppendLine("using Kendo.Mvc.UI;");
            sbCodes.AppendLine("using Kendo.Mvc.Extensions;");
            sbCodes.AppendLine("using WebApp1.Codes;");
            sbCodes.AppendLine("using " + MyCodeGen.CodeGenRootNameSpace + "." + MyCodeGen.CodeGenControllerDirectory + ";");
            sbCodes.AppendLine("");

            #endregion

            sbCodes.AppendLine("namespace " + MyCodeGen.CodeGenRootNameSpace + "." + _oTableOptions.ControllerNameSpace);
            sbCodes.AppendLine("{");

            sbCodes.AppendLine("    [Area(\"" + _oTableOptions.AreaName + "\")]");
            sbCodes.AppendLine("    public class " + _oTableOptions.ControllerName + MyCodeGen.CodeGen_ControllerSuffix + " : _Controller");
            sbCodes.AppendLine("    {");
            sbCodes.AppendLine("        public " + _oTableOptions.ControllerName + MyCodeGen.CodeGen_ControllerSuffix + "(IServiceProvider _serviceProvider)");
            sbCodes.AppendLine("            : base(_serviceProvider) { }");
            sbCodes.AppendLine("");

            #region read

            sbCodes.AppendLine("        [HttpGet]");
            sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
            sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".D_R.\")]");
            if (string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("        public ActionResult Read([DataSourceRequest]DataSourceRequest request)");
            }
            else
            {
                sbCodes.AppendLine("        public ActionResult Read([DataSourceRequest]DataSourceRequest request, int? id)");
            }

            sbCodes.AppendLine("        {");
            if (string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("            DataSourceResult dsr = new();");
            }
            else
            {
                sbCodes.AppendLine("            TreeDataSourceResult dsr = new();");
            }

            sbCodes.AppendLine("            try");
            sbCodes.AppendLine("            {");

            sbCodes.AppendLine("                var query = this.rep." + _oTableOptions.RepositoryNameName + ".Get().Where(c => c." + _oTableOptions.PrimaryKey + " > 0);");

            if (!string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("                if (id == null || id == 0)");
                sbCodes.AppendLine("                {");
                sbCodes.AppendLine("                   query = query.Where(c => c." + _oTableOptions.ParentColumnName + " == 0);");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("                else");
                sbCodes.AppendLine("                {");
                sbCodes.AppendLine("                   query = query.Where(c => c." + _oTableOptions.ParentColumnName + " == id);");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("                dsr = query.ToTreeDataSourceResult(request);");
            }
            else
            {
                sbCodes.AppendLine("                dsr = query.ToDataSourceResult(request);");
            }

            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            catch (Exception ex)");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                dsr.Errors = ex.MyLastInner().Message;");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            return Json(dsr);");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("        [HttpGet]");
            sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
            sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".D_C.\")]");
            sbCodes.AppendLine("        public ActionResult GetByNew()");
            sbCodes.AppendLine("        {");
            sbCodes.AppendLine("             return Json(this.rep." + _oTableOptions.RepositoryNameName + ".GetByNew());");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");

            #endregion

            #region Create
            sbCodes.AppendLine("        [HttpPost]");
            sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
            sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".D_C.\")]");
            sbCodes.AppendLine("        public ActionResult Create([FromBody]" + _oTableOptions.DataTransferObjectNameSpace + "." + _oTableOptions.DataTransferObjectName + " dto)");
            sbCodes.AppendLine("        {");

            if (string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("            DataSourceResult dsr = new();");
            }
            else
            {
                sbCodes.AppendLine("            TreeDataSourceResult dsr = new();");
            }

            sbCodes.AppendLine("            try");
            sbCodes.AppendLine("            {");

            sbCodes.AppendLine("                int id = this.rep." + _oTableOptions.RepositoryNameName + ".CreateOrUpdate(dto);");
            sbCodes.AppendLine("                this.rep.SaveChanges();");
            sbCodes.AppendLine("                dsr.Data = this.rep." + _oTableOptions.RepositoryNameName + ".GetById(id);");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            catch (Exception ex)");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                dsr.Errors = ex.MyLastInner().Message;");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            ");

            sbCodes.AppendLine("            return Json(dsr);");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");
            #endregion

            #region Update
            sbCodes.AppendLine("        [HttpPost]");
            sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
            sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".D_U.\")]");
            sbCodes.AppendLine("        public ActionResult Update([FromBody]" + _oTableOptions.DataTransferObjectNameSpace + "." + _oTableOptions.DataTransferObjectName + " dto)");
            sbCodes.AppendLine("        {");

            if (string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("            DataSourceResult dsr = new();");
            }
            else
            {
                sbCodes.AppendLine("            TreeDataSourceResult dsr = new();");
            }

            sbCodes.AppendLine("            try");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                int id = this.rep." + _oTableOptions.RepositoryNameName + ".CreateOrUpdate(dto);");
            sbCodes.AppendLine("                this.rep.SaveChanges();");
            sbCodes.AppendLine("                dsr.Data = this.rep." + _oTableOptions.RepositoryNameName + ".GetById(id);");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            catch (Exception ex)");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                dsr.Errors = ex.MyLastInner().Message;");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            ");

            sbCodes.AppendLine("            return Json(dsr);");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");
            #endregion

            #region delete

            sbCodes.AppendLine("        [HttpPost]");
            sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
            sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".D_D.\")]");
            sbCodes.AppendLine("        public ActionResult Delete(int _id)");
            sbCodes.AppendLine("        {");
            if (string.IsNullOrEmpty(_oTableOptions.ParentColumnName))
            {
                sbCodes.AppendLine("            DataSourceResult dsr = new();");
            }
            else
            {
                sbCodes.AppendLine("            TreeDataSourceResult dsr = new();");
            }
            sbCodes.AppendLine("            try");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                this.rep." + _oTableOptions.RepositoryNameName + ".Delete(_id);");
            sbCodes.AppendLine("                this.rep.SaveChanges();");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            catch (Exception ex)");
            sbCodes.AppendLine("            {");
            sbCodes.AppendLine("                dsr.Errors = ex.MyLastInner().Message;");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("            return Json(dsr);");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");

            #endregion

            #region Reset Password
            foreach (var column in _oTableOptions.Fields.Where(c => c.FormComponentType == EnmComponentType.PasswordTextBox.ToString() && c.FormResetPasswordMailColumnName.MyToTrim().Length > 0))
            {
                sbCodes.AppendLine("        [HttpPost]");
                sbCodes.AppendLine("        [ResponseCache(Duration = 0)]");
                sbCodes.AppendLine("        [AuthenticateRequired(AuthorityKeys = \"" + _oTableOptions.TableName + ".A_ResetPassword.\")]");
                sbCodes.AppendLine("        public ActionResult ResetPassword" + column.ColumnName + "(int _id)");
                sbCodes.AppendLine("        {");
                sbCodes.AppendLine("            string rMessage = string.Empty;");
                sbCodes.AppendLine("            Boolean rOk = false;");
                sbCodes.AppendLine("            try");
                sbCodes.AppendLine("            {");
                sbCodes.AppendLine("                var dto = this.rep.Areas_Tem_Rep" + _oTableOptions.TableName + ".GetById(_id).FirstOrDefault();");
                sbCodes.AppendLine("                if (dto != null)");
                sbCodes.AppendLine("                {");
                sbCodes.AppendLine("                    dto.Sifre = Guid.NewGuid().ToString().MyToUpper().Replace(\"-\", \"\").Substring(0, 8);");
                sbCodes.AppendLine("                    using(var mailHelper = new MyMailHelper(this.rep))");
                sbCodes.AppendLine("                    {");
                sbCodes.AppendLine("                        mailHelper.SendMail_Sifre_Reset_Bildirim(dto." + column.ColumnName + ", dto.Sifre);");
                sbCodes.AppendLine("                    }");
                sbCodes.AppendLine("                    ");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("                    int id = this.rep.Areas_Tem_Rep" + _oTableOptions.TableName + ".Update(dto);");
                sbCodes.AppendLine("                    this.rep.SaveChanges();");
                sbCodes.AppendLine("                    rMessage += MyApp.TranslateTo(\"xLng.YeniSifreKayitliMailAdresineGonderildi\", this.dataContext.Language);");
                sbCodes.AppendLine("                    rOk = true;");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("                else {");
                sbCodes.AppendLine("                    rMessage += MyApp.TranslateTo(\"xLng.IslemYapilacakKayitBulunamadi\", this.dataContext.Language);");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("            catch (Exception ex)");
                sbCodes.AppendLine("            {");
                sbCodes.AppendLine("                rMessage = ex.MyLastInner().Message;");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("            return Json(new { Message = rMessage, Ok = rOk });");
                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");
            }
            #endregion

            sbCodes.AppendLine("");
            sbCodes.AppendLine("    }");
            sbCodes.AppendLine("}");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("");
            return sbCodes;
        }

        #endregion

        #region Dictionary Generate json
        public StringBuilder DictionaryGenerate(MyTableOption _oTableOptions)
        {
            StringBuilder sbCodes = new();
            sbCodes.AppendLine("[");
            //--------------------title-------------------------------
            sbCodes.AppendLine(" {");
            sbCodes.AppendLine("     \"key\" : \"x" + _oTableOptions.AreaName + "." + _oTableOptions.TableName + ".Title" + "\",");
            sbCodes.AppendLine("     \"value\": {");
            sbCodes.AppendLine("         \"tr\": \"" + _oTableOptions.TableDictionaryTitleTr + "\"");
            sbCodes.AppendLine(",");
            sbCodes.AppendLine("         \"en\": \"" + _oTableOptions.TableDictionaryTitleEn + "\"");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine(" },");
            //------------------shor title------------------------------
            sbCodes.AppendLine(" {");
            sbCodes.AppendLine("     \"key\" : \"x" + _oTableOptions.AreaName + "." + _oTableOptions.TableName + ".ShortTitle" + "\",");
            sbCodes.AppendLine("     \"value\": {");
            sbCodes.AppendLine("         \"tr\": \"" + _oTableOptions.TableDictionaryShortTitleTr + "\"");
            sbCodes.AppendLine(",");
            sbCodes.AppendLine("         \"en\": \"" + _oTableOptions.TableDictionaryShortTitleEn + "\"");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine(" }");


            //form tabs

            foreach (var formViewTab in _oTableOptions.Fields.Where(c => c.ColumnUse).GroupBy(g => g.FormViewTab).OrderBy(o => o.Key))
            {
                if (!string.IsNullOrEmpty(formViewTab.Key))
                {
                    string key = _oTableOptions.AreaName + "." + _oTableOptions.TableName + ".FormViewTab." + formViewTab.Key;
                    string textTr = _oTableOptions.FormViewTabDictionaryTr.Split("||")[formViewTab.Key.MyRomanToInt() - 1];

                    sbCodes.AppendLine(",");

                    sbCodes.AppendLine(" {");
                    sbCodes.AppendLine("     \"key\" : \"x" + key + "\",");
                    sbCodes.AppendLine("     \"value\": {");
                    sbCodes.AppendLine("         \"tr\": \"" + textTr + "\"");
                    string textEn = _oTableOptions.FormViewTabDictionaryEn.Split("||")[formViewTab.Key.MyRomanToInt() - 1];
                    sbCodes.AppendLine(",");
                    sbCodes.AppendLine("         \"en\": \"" + textEn + "\"");
                    sbCodes.AppendLine("     }");
                    sbCodes.AppendLine(" }");
                }
            }

            foreach (var gridFindTab in _oTableOptions.Fields.Where(c => c.ColumnUse).GroupBy(g => g.GridFindTab).OrderBy(o => o.Key))
            {
                if (!string.IsNullOrEmpty(gridFindTab.Key))
                {
                    string key = _oTableOptions.AreaName + "." + _oTableOptions.TableName + ".GridFindTab." + gridFindTab.Key;
                    string textTr = _oTableOptions.GridViewTabDictionaryTr.Split("||")[gridFindTab.Key.MyRomanToInt() - 1];

                    sbCodes.AppendLine(",");

                    sbCodes.AppendLine(" {");
                    sbCodes.AppendLine("     \"key\" : \"x" + key + "\",");
                    sbCodes.AppendLine("     \"value\": {");
                    sbCodes.AppendLine("         \"tr\": \"" + textTr + "\"");
                    string textEn = _oTableOptions.GridViewTabDictionaryEn.Split("||")[gridFindTab.Key.MyRomanToInt() - 1];
                    sbCodes.AppendLine(",");
                    sbCodes.AppendLine("         \"en\": \"" + textEn + "\"");
                    sbCodes.AppendLine("     }");
                    sbCodes.AppendLine(" }");
                }
            }

            foreach (var searchFindTab in _oTableOptions.Fields.Where(c => c.ColumnUse).GroupBy(g => g.SearchFindTab).OrderBy(o => o.Key))
            {
                if (!string.IsNullOrEmpty(searchFindTab.Key))
                {
                    string key = _oTableOptions.AreaName + "." + _oTableOptions.TableName + ".SearchFindTab." + searchFindTab.Key;
                    string textTr = _oTableOptions.SearchViewTabDictionaryTr.Split("||")[searchFindTab.Key.MyRomanToInt() - 1];

                    sbCodes.AppendLine(",");

                    sbCodes.AppendLine(" {");
                    sbCodes.AppendLine("     \"key\" : \"x" + key + "\",");
                    sbCodes.AppendLine("     \"value\": {");
                    sbCodes.AppendLine("         \"tr\": \"" + textTr + "\"");
                    string textEn = _oTableOptions.SearchViewTabDictionaryEn.Split("||")[searchFindTab.Key.MyRomanToInt() - 1];
                    sbCodes.AppendLine(",");
                    sbCodes.AppendLine("         \"en\": \"" + textEn + "\"");
                    sbCodes.AppendLine("     }");
                    sbCodes.AppendLine(" }");
                }
            }



            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse).OrderBy(o => o.FormOrder))
            {
                string key = _oTableOptions.AreaName + "." + _oTableOptions.TableName + "." + col.ColumnName;
                sbCodes.AppendLine(",");
                sbCodes.AppendLine(" {");
                sbCodes.AppendLine("     \"key\" : \"x" + key + "\",");
                sbCodes.AppendLine("     \"value\": {");
                sbCodes.AppendLine("         \"tr\": \"" + col.ColumnDictionaryTr + "\"");
                sbCodes.AppendLine(",");
                sbCodes.AppendLine("         \"en\": \"" + col.ColumnDictionaryEn + "\"");
                sbCodes.AppendLine("     }");
                sbCodes.AppendLine(" }");
            }

            sbCodes.AppendLine("]");
            return sbCodes;
        }
        #endregion

        #region Frontend Generate code  Form, Grid, TreeList

        public StringBuilder FormViewGenerate(MyTableOption _oTableOptions)
        {
            StringBuilder sbCodes = new();
            var viewName = _oTableOptions.FormViewName;
            var dataSourceFileds = _oTableOptions.Fields.Where(c => c.ColumnUse).OrderBy(o => o.ColumnOrder);
            var formFileds = _oTableOptions.Fields.Where(c => c.ColumnUse && c.FormUse).OrderBy(o => o.ColumnOrder);
            var dataSourceDetails = _oTableOptions.Details.Where(c => c.FormViewUse);
            var visibilityFileds = formFileds.Where(c => c.FormVisibilityColumnName.MyToTrim().Length > 0);

            /*html */
            sbCodes.AppendLine("");
            sbCodes.AppendLine("<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");

            #region html codes

            sbCodes.AppendLine("<div id='" + _oTableOptions.FormViewName + "'>");
            sbCodes.AppendLine("    <div id='divButtonGroup' class='mnButtonGroup float-right'>");

            if (dataSourceDetails.Any())
            {
                sbCodes.AppendLine("        <div class='dropdown' style='position:absolute; right:40px;'>");
                sbCodes.AppendLine("            <button type='button' class='btn btn-outline-primary fa fa-ellipsis-h' data-toggle='dropdown' style='padding:2px 9px;'></button>");
                sbCodes.AppendLine("            <div class='dropdown-menu rounded-0 mn-default-border-color mn-default-bg-color mn-default-text-color shadow ' style='padding:5px 0px;'>");
                foreach (var item in dataSourceDetails)
                {
                    sbCodes.AppendLine("                <button id='btnDetay" + item.TableName + "' class='dropdown-item mn-hover-color-blue'> <span data-langkey-text='x" + item.AreaName + "." + item.TableName + ".Title'></span> </button>");
                    sbCodes.AppendLine("");
                }
                sbCodes.AppendLine("            </div>");
                sbCodes.AppendLine("        </div>");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("        <button id='btnKaydet' type='button' class='btn btn-outline-success text-nowrap' data-langkey-title='xLng.Kaydet' style='position:absolute; right:0px;'> <i class='fa fa-save'></i> </button>");
            sbCodes.AppendLine("    </div>");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("    <div style='padding-bottom:15px;'>");
            sbCodes.AppendLine("        <div id='tabstrip'>");
            sbCodes.AppendLine("            <ul>");
            foreach (var tabs in formFileds.Where(c => c.FormViewTab != null).GroupBy(g => g.FormViewTab).OrderBy(o => o.Key))
            {
                sbCodes.AppendLine("                <li> <span data-langkey-text='" + _oTableOptions.LangKeyRoot + ".FormViewTab." + tabs.Key + "'></span> </li>");
            }
            sbCodes.AppendLine("            </ul>");

            foreach (var tabs in formFileds.Where(c => c.FormViewTab != null).GroupBy(g => g.FormViewTab).OrderBy(o => o.Key))
            {
                sbCodes.AppendLine("        <div>");
                sbCodes.AppendLine("            <div class='mnFormElementContainer row'>");

                List<string> formViewLocationList = new() { "justify", "left", "right" };
                foreach (var formViewLocation in formViewLocationList)
                {
                    string labelContainerSizex = "col-sm-4";
                    string inputContainerSizex = "col-sm-8";
                    if (formViewLocation == "justify")
                    {
                        sbCodes.AppendLine("                     <div class='col-md-12'>");
                        labelContainerSizex = "col-sm-4 col-md-2";
                        inputContainerSizex = "col-sm-8 col-md-10";
                    }
                    else
                    {
                        sbCodes.AppendLine("                     <div class='col-lg-6 col-md-12'>");
                    }

                    foreach (var col in formFileds.Where(c => c.FormViewTab == tabs.Key && c.FormViewLocation == formViewLocation).OrderBy(o => o.FormOrder))
                    {
                        string labelContainerSize = labelContainerSizex;
                        string inputContainerSize = inputContainerSizex;
                        if (col.FormLabelLocationTop)
                        {
                            labelContainerSize = "col-sm-12";
                            inputContainerSize = "col-sm-12";
                        }

                        sbCodes.AppendLine("                     <div name='div" + col.ColumnName + "' class='mnFormElementDiv form-group row'>");


                        string colAttribute = "";
                        if (!col.FormEditable)
                        {
                            colAttribute = " disabled ";
                        }

                        if (col.FormReadonly)
                        {
                            colAttribute = " readonly ";
                        }

                        if (col.FormComponentType == EnmComponentType.TextBox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' type='text' class='k-textbox' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + " autocomplete='off' />");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.PasswordTextBox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <span name='ExternalResetPasswordButtonWrapper" + col.ColumnName + "' class='mnResetPasswordButton k-textbox k-space-right w-100'>");
                            sbCodes.AppendLine("                                <input name='" + col.ColumnName + "' type='password' class='font-weight-light' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + " autocomplete='off' />");
                            if (col.FormResetPasswordMailColumnName.MyToTrim().Length > 0)
                            {
                                sbCodes.AppendLine("                                <button name='btnResetPassword' class='btn btn-light fa fa-refresh' data-langkey-title='xLng.SifreSifirla'></button>");
                            }
                            sbCodes.AppendLine("                             </span>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.TextArea.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <textarea name='" + col.ColumnName + "' class='k-textbox' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + " style='height:" + col.FormComponentHeight + "px;' ></textarea>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.TextAreaForJson.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <textarea name='" + col.ColumnName + "' class='k-textbox' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + " style='height:" + col.FormComponentHeight + "px;' ></textarea>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.DropDownList.ToString() || col.FormComponentType == EnmComponentType.ComboBox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' data-bind='value:" + col.ColumnName + "' " + colAttribute + "/>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.DatePicker.ToString() || col.FormComponentType == EnmComponentType.TimePicker.ToString() || col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' data-bind='value:" + col.ColumnName + "' " + colAttribute + "/>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.NumericTextBox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' type='number' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + "/>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.Checkbox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                            <label style='font-weight:normal;'>");
                            sbCodes.AppendLine("                                <input name='" + col.ColumnName + "' type='checkbox' data-bind='checked:" + col.ColumnName + "' style='width: 24px; height: 24px; vertical-align: middle;' " + colAttribute + " />");
                            sbCodes.AppendLine("                                <span data-langkey-Text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                            </label>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' type='text' " + colAttribute + "/>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.TextEditor.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <textarea name='" + col.ColumnName + "' type='text' class='k-textbox' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' style='height:" + col.FormComponentHeight + "px;'" + "></textarea>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.ExternalSearchEdit.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <span name='ExternalSearchButtonWrapper" + col.ColumnName + "' class='mnSearchEdit k-textbox k-space-right w-100'>");
                            sbCodes.AppendLine("                                <input name='" + col.ColumnName + "' type='hidden' data-bind='value:" + col.ColumnName + "' />");
                            sbCodes.AppendLine("                                <input class='font-weight-light' type='text' data-bind='value:Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' disabled />");
                            sbCodes.AppendLine("                                <button name='btnClear' class='btn btn-light fa fa-close'></button>");
                            sbCodes.AppendLine("                                <button name='btnSearch' class='btn btn-light fa fa-search'></button>");
                            sbCodes.AppendLine("                             </span>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.DownloadFileLink.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "' >");
                            sbCodes.AppendLine("                            <span class='font-weight-light' data-bind='html:Cc" + col.ColumnName + "Link' ></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.ImageBox.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "' >");
                            sbCodes.AppendLine("                            <img name='" + col.ColumnName + "' data-bind='attr:{src:" + col.ColumnName + "}' style='border:1px solid silver; cursor:pointer; height:" + col.FormComponentHeight + "px; width:" + (col.FormComponentHeight.MyToInt() / 10) * 9 + "px; ' />");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else if (col.FormComponentType == EnmComponentType.ImageSearchEdit.ToString())
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <span name='ImageSearchEditWrapper" + col.ColumnName + "' class='mnSearchEdit k-textbox k-space-right w-100'>");
                            sbCodes.AppendLine("                                <input name='" + col.ColumnName + "' type='hidden' data-bind='value:" + col.ColumnName + "' />");
                            sbCodes.AppendLine("                                <input class='font-weight-light' type='text' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' />");
                            sbCodes.AppendLine("                                <a name='btnPreview' class='btn btn-link fa fa-picture-o'  onclick='(function () { this.event.stopPropagation(); })();' href='' target='_blank' data-bind='attr:{href:" + col.ColumnName + "}'> </a>");
                            sbCodes.AppendLine("                                <button name='btnClear' class='btn btn-light fa fa-close'></button>");
                            sbCodes.AppendLine("                                <button name='btnSearch' class='btn btn-light fa fa-search'></button>");
                            sbCodes.AppendLine("                             </span>");
                            sbCodes.AppendLine("                             <span class='k-invalid-msg' data-for='" + col.ColumnName + "'></span>");
                            sbCodes.AppendLine("                         </div>");
                        }
                        else
                        {
                            sbCodes.AppendLine("                         <label class='" + labelContainerSize + " col-form-label' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                         <div class='" + inputContainerSize + "'>");
                            sbCodes.AppendLine("                             <input name='" + col.ColumnName + "' type='text' class='k-textbox' data-bind='value:" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' " + colAttribute + " autocomplete='off' />");
                            sbCodes.AppendLine("                         </div>");
                        }

                        sbCodes.AppendLine("                     </div>");
                        sbCodes.AppendLine("");
                    }
                    sbCodes.AppendLine("");
                    sbCodes.AppendLine("                    </div>");
                    sbCodes.AppendLine("");
                }

                sbCodes.AppendLine("                </div>");
                sbCodes.AppendLine("            </div>");
            }

            sbCodes.AppendLine("        </div>");
            sbCodes.AppendLine("    </div>");
            sbCodes.AppendLine("</div>");

            #endregion

            #region script codes                     

            sbCodes.AppendLine("");
            sbCodes.AppendLine("<script>");
            sbCodes.AppendLine(" window." + _oTableOptions.FormViewName + " = function () {");
            sbCodes.AppendLine("     var self = {};");
            sbCodes.AppendLine("     self.opt = null;");
            sbCodes.AppendLine("     self.area = '" + _oTableOptions.AreaName + "';");
            sbCodes.AppendLine("     self.title = '" + _oTableOptions.LangKeyRoot + "." + "Title" + "';");
            sbCodes.AppendLine("     self.selector = '#" + _oTableOptions.FormViewName + "';");
            sbCodes.AppendLine("     self.primaryKey = '" + _oTableOptions.PrimaryKey + "';");
            sbCodes.AppendLine("     self.tableName = '" + _oTableOptions.TableName + "';");
            sbCodes.AppendLine("     self.apiUrlPrefix = '/' + self.area + '/" + MyCodeGen.CodeGen_ControllerPrefix + "' + self.tableName;");
            sbCodes.AppendLine("");

            #region Create DataSource

            sbCodes.AppendLine("     function fCreateDataSource() {");
            sbCodes.AppendLine("         self.dataSource = new kendo.data.DataSource({");
            sbCodes.AppendLine("             transport: {");
            sbCodes.AppendLine("                 read: { type: 'GET', url: self.apiUrlPrefix + '/Read', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 create: { type: 'POST', url: self.apiUrlPrefix + '/Create', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 update: { type: 'POST', url: self.apiUrlPrefix + '/Update', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 destroy: { type: 'POST', url: self.apiUrlPrefix + '/Delete', dataType: 'json' },");
            sbCodes.AppendLine("                 parameterMap: function (data, operation) {");
            sbCodes.AppendLine("                     if (operation === 'read') {");
            sbCodes.AppendLine("                         return (new kendo.data.transports['aspnetmvc-ajax']({})).options.parameterMap(data);");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     else if (operation === 'create' || operation === 'update') {");

            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnDbType == "datetime")
                {
                    sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                }
                if (col.ColumnDbType == "date")
                {
                    sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                }
                if (col.ColumnDbType == "time")
                {
                    sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 't');");
                }
            }

            sbCodes.AppendLine("                         return kendo.stringify(data);");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     else if (operation === 'destroy') {");
            sbCodes.AppendLine("                         return { _id: data[self.primaryKey] };");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             pageSize: 10, serverPaging: true, serverSorting: true, serverFiltering: true, serverGrouping: true, serverAggregates: true,");
            sbCodes.AppendLine("             schema: {");
            sbCodes.AppendLine("                 errors: 'Errors', data: 'Data', total: 'Total', aggregates: 'AggregateResults',");
            sbCodes.AppendLine("                 model: {");
            sbCodes.AppendLine("                     id: self.primaryKey,");
            sbCodes.AppendLine("                     fields: {");

            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnName == _oTableOptions.PrimaryKey)
                {
                    sbCodes.AppendLine("                         " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "', defaultValue: null },");
                }
                else
                {
                    string editable = "";
                    if (!col.FormEditable)
                    {
                        editable = ", editable:false";
                    }
                    sbCodes.AppendLine("                         " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "'" + editable + " },");
                }
            }

            // computed alanların tanımlanması
            foreach (var col in dataSourceFileds)
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + ": { type: 'string' },");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ": { type: 'string' },");
                }
                else if (col.FormComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString() || col.GridComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString())
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + "Link: { type: 'string' },");
                }

            }

            sbCodes.Remove(sbCodes.Length - 3, 1);// son virgülü atmak için (sonundaki entera dokunmadan)

            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             error: function (e) {");
            sbCodes.AppendLine("                 if (e.xhr === null) { mnNotification.warning(e.errors); } else { mnErrorHandler.Handle(e.xhr); }");
            sbCodes.AppendLine("                 //this.cancelChanges();");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             requestStart: function (e) {");
            sbCodes.AppendLine("                 setTimeout(function () {");
            sbCodes.AppendLine("                     kendo.ui.progress($(self.selector), true); //progress On");
            sbCodes.AppendLine("                 });");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             requestEnd: function (e) {");
            sbCodes.AppendLine("                 setTimeout(function () {");
            sbCodes.AppendLine("                     kendo.ui.progress($(self.selector), false); //progress Off");
            sbCodes.AppendLine("                 });");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                 if (e.response !== undefined && e.response.Errors === null) {");
            sbCodes.AppendLine("                     if (e.type === 'create' || e.type === 'update' || e.type === 'destroy') {");
            //sbCodes.AppendLine("                         self.isDirty = true;");
            sbCodes.AppendLine("                         mnLookup.listRead(self.tableName);");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     if (e.type === 'create') {");
            sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitEklendi'));");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     if (e.type === 'update') {");
            sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitDuzeltildi'));");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     if (e.type === 'destroy') {");
            sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitSilindi'));");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("");
            foreach (var col in formFileds)
            {
                if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                {
                    sbCodes.AppendLine("                     self.ms" + col.ColumnName + ".value([]);");
                }
            }

            sbCodes.AppendLine("                     if (e.response.Data.length > 0) {");
            sbCodes.AppendLine("                         var row = e.response.Data[0];");

            foreach (var col in formFileds)
            {
                if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                {
                    sbCodes.AppendLine("                         if (row." + col.ColumnName + " !== null || undefined || '') {");
                    sbCodes.AppendLine("                             self.ms" + col.ColumnName + ".value(row." + col.ColumnName + ".split(','));");
                    sbCodes.AppendLine("                         }");
                }
            }
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             change: function (e) {");
            sbCodes.AppendLine("                 if (e.items[0] !== undefined) {");
            sbCodes.AppendLine("                     if (e.items[0].get(self.primaryKey) === null) {");
            sbCodes.AppendLine("                         $.ajax({");
            sbCodes.AppendLine("                             url: self.apiUrlPrefix + '/GetByNew', type: 'GET', dataType: 'json', async: false,");
            sbCodes.AppendLine("                             success: function (result) {");

            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnNetType == "System.DateTime")
                {
                    sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = kendo.parseDate(result." + col.ColumnName + ");");
                }
                else
                {
                    sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = result." + col.ColumnName + ";");
                }
            }

            sbCodes.AppendLine("                                 //computed alanların tanımlanması");
            foreach (var col in dataSourceFileds)
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + " = result.Cc" + col.ColumnName + ";");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + " = result.Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ";");
                }
            }

            sbCodes.AppendLine("                                 //filterdan gelen default value set için");
            sbCodes.AppendLine("                                 $(self.opt.filters).each(function (index, row) {");
            sbCodes.AppendLine("                                     if (row.filterColumnName !== 'Id') {");
            sbCodes.AppendLine("                                        e.items[0].set(row.filterColumnName, row.filterValue);");
            sbCodes.AppendLine("                                     }");
            sbCodes.AppendLine("                                 });");
            sbCodes.AppendLine("");

            foreach (var col in formFileds)
            {
                if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                {
                    sbCodes.AppendLine("                                 //(multi select için)");
                    sbCodes.AppendLine("                                 self.ms" + col.ColumnName + ".value([]);");
                    sbCodes.AppendLine("                                 if (e.items[0]." + col.ColumnName + " !== null || undefined || '') {");
                    sbCodes.AppendLine("                                     self.ms" + col.ColumnName + ".value(e.items[0]." + col.ColumnName + ".split(','));");
                    sbCodes.AppendLine("                                 }");
                }
            }

            sbCodes.AppendLine("                             },");
            sbCodes.AppendLine("                             error: function (xhr, status) {");
            sbCodes.AppendLine("                                 mnErrorHandler.Handle(xhr);");
            sbCodes.AppendLine("                             }");
            sbCodes.AppendLine("                         });");
            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                     // Bind");
            sbCodes.AppendLine("                     kendo.bind($(self.selector), e.items[0]);");
            sbCodes.AppendLine("                     self.validator.validate();");

            foreach (var col in formFileds)
            {
                if (col.FormComponentType == EnmComponentType.TextAreaForJson.ToString())
                {
                    sbCodes.AppendLine("                     //(TextAreaForJson için)");
                    sbCodes.AppendLine("                     $(self.selector).find('[name=" + col.ColumnName + "]').get(0).value = JSON.stringify(JSON.parse(e.items[0]." + col.ColumnName + "), null, 2);");
                    sbCodes.AppendLine("");
                }
            }

            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                 // Yetki");
            sbCodes.AppendLine("                 fYetkiUygula();");
            sbCodes.AppendLine("");

            if (visibilityFileds.Any())
            {
                sbCodes.AppendLine("                 // Görünürlük");
                sbCodes.AppendLine("                 fGorunurlukUygula(e.items[0]);");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("                 // ve gerekebilecek diğer işlemler");
            sbCodes.AppendLine("                 // ...");
            sbCodes.AppendLine("             }");
            sbCodes.AppendLine("         });");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            #endregion

            #region Create FormElements

            sbCodes.AppendLine("     function fCreateFormElements() {");
            sbCodes.AppendLine("         // tabstrip");
            sbCodes.AppendLine("         self.tabstrip = $(self.selector).find('#tabstrip').kendoTabStrip({");
            sbCodes.AppendLine("             animation: false, tabPosition: 'top'");
            sbCodes.AppendLine("         }).getKendoTabStrip();");
            sbCodes.AppendLine("         self.tabstrip.select(0);");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         //validator");
            sbCodes.AppendLine("         let formElm = $(self.selector).find('.mnFormElementContainer');");
            sbCodes.AppendLine("         self.validator = mnApp.createValidator(formElm, mnApp.validatorErrorTemplateIconMsg);");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         //butonlar");
            sbCodes.AppendLine("         $(self.selector).find('#btnKaydet').click(function (e) {");
            sbCodes.AppendLine("             mnApi.controlDisableWait($(e.target));");
            sbCodes.AppendLine("             if (self.validator.validate()) {");
            sbCodes.AppendLine("                 if (self.dataSource.at(0).dirty) {");
            sbCodes.AppendLine("                     self.dataSource.sync();");
            sbCodes.AppendLine("                 } else {");
            sbCodes.AppendLine("                     mnAlert.warning(mnLang.TranslateWithWord('xLng.KayyittaDegisiklikYapmadiniz'));");
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             } else {");
            sbCodes.AppendLine("                 mnAlert.warning(mnLang.TranslateWithWord('xLng.DoldurulmasiGerekenAlanlarVar'));");
            sbCodes.AppendLine("             }");
            sbCodes.AppendLine("         });");
            sbCodes.AppendLine("");

            foreach (var item in dataSourceDetails)
            {
                sbCodes.AppendLine("         $(self.selector).find('#btnDetay" + item.TableName + "').click(function (e) {");
                sbCodes.AppendLine("            var dataItem = self.dataSource.at(0);");
                sbCodes.AppendLine("            mn" + item.ShowType + "View.create({");
                sbCodes.AppendLine("                areaName: '" + item.AreaName + "',");
                sbCodes.AppendLine("                viewFileName: '" + item.TableName + "For" + item.ViewType + "',");
                sbCodes.AppendLine("                viewName: '" + item.TableName + "For" + item.ViewType + "',");
                sbCodes.AppendLine("                subTitle: mnLang.TranslateWithWord('xLng.DuzenlemeIslemleri'),");
                sbCodes.AppendLine("                onShow: function (e) {");
                sbCodes.AppendLine("                    e.beforeShow({");
                sbCodes.AppendLine("                        'ownerViewName': '" + viewName + "',");
                sbCodes.AppendLine("                        'filters': [");
                if (!string.IsNullOrEmpty(item.FilterColumnName) && !string.IsNullOrEmpty(item.FilterOperator) && !string.IsNullOrEmpty(item.FilterValue))
                {
                    sbCodes.AppendLine("                            { 'filterColumnName': '" + item.FilterColumnName + "', 'filterOperator': '" + item.FilterOperator + "', 'filterValue': " + item.FilterValue + " },");
                }
                sbCodes.AppendLine("                            { 'filterColumnName': '" + item.ColumnName + "', 'filterOperator': 'eq', 'filterValue': dataItem.Id }");
                sbCodes.AppendLine("                        ]");
                sbCodes.AppendLine("                    });");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("            });");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("         //Diğer Form elementleri");
            foreach (var col in formFileds.OrderBy(o => o.FormOrder))
            {
                if (col.FormComponentType == EnmComponentType.TextBox.ToString())
                {
                    //text box için kendo da gerek yok, k-textbox ile hallediliyor
                }
                else if (col.FormComponentType == EnmComponentType.PasswordTextBox.ToString() && col.FormResetPasswordMailColumnName.MyToTrim().Length > 0)
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=ExternalResetPasswordButtonWrapper" + col.ColumnName + "] [name=btnResetPassword]').click(function (e) {");
                    sbCodes.AppendLine("            e.stopPropagation();");
                    sbCodes.AppendLine("            if (confirm(mnLang.TranslateWithWord('xLng.SifreyiSifirlamakIstediginizdenEminmisiniz'))) {");
                    sbCodes.AppendLine("                var dataItem = self.dataSource.at(0);");
                    sbCodes.AppendLine("                var _data = {");
                    sbCodes.AppendLine("                    _id : dataItem.get(self.primaryKey)");
                    sbCodes.AppendLine("                };");
                    sbCodes.AppendLine("");
                    sbCodes.AppendLine("                $.ajax({");
                    sbCodes.AppendLine("                    url: self.apiUrlPrefix + '/ResetPassword" + col.ColumnName + "',");
                    sbCodes.AppendLine("                    data: _data,");
                    sbCodes.AppendLine("                    type: 'POST', dataType: 'json',");
                    sbCodes.AppendLine("                    beforeSend: function (jqXHR, settings) {");
                    sbCodes.AppendLine("                        kendo.ui.progress($(self.selector), true); //progress On");
                    sbCodes.AppendLine("                    },");
                    sbCodes.AppendLine("                    success: function (result, textStatus, jqXHR) {");
                    sbCodes.AppendLine("                        kendo.ui.progress($(self.selector), false); //progress Off");
                    sbCodes.AppendLine("                        mnAlert.warning(result.Message);");
                    sbCodes.AppendLine("                    },");
                    sbCodes.AppendLine("                    error: function (jqXHR, textStatus, errorThrown) {");
                    sbCodes.AppendLine("                        kendo.ui.progress($(self.selector), false); //progress Off");
                    sbCodes.AppendLine("                        alert('(' + jqXHR.status + ') ' + jqXHR.statusText + ' ' + this.url);");
                    sbCodes.AppendLine("                    }");
                    sbCodes.AppendLine("                });");
                    sbCodes.AppendLine("            }");
                    sbCodes.AppendLine("");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.NumericTextBox.ToString())
                {
                    string decimals = "0";
                    if (!string.IsNullOrEmpty(col.FormComponentFormat))
                    {
                        decimals = col.FormComponentFormat.Substring(1);
                    }

                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoNumericTextBox({");
                    sbCodes.AppendLine("             format: '" + col.FormComponentFormat + "', decimals: " + decimals + ", min: 0, spinners: false, step: 0");
                    sbCodes.AppendLine("         }).getKendoNumericTextBox();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.DropDownList.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoDropDownList({");
                    if (!string.IsNullOrEmpty(col.FormComponentFilterType))
                    {
                        sbCodes.AppendLine("             filter: '" + col.FormComponentFilterType + "',");
                    }
                    sbCodes.AppendLine("             valuePrimitive: true,");
                    sbCodes.AppendLine("             optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                    sbCodes.AppendLine("             dataValueField: 'value',");
                    sbCodes.AppendLine("             dataTextField: 'text',");

                    if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                    }
                    else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                        sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                        sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                        sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                        sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                        sbCodes.AppendLine("                 Filters: [");
                        sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                        sbCodes.AppendLine("                 ],");
                        sbCodes.AppendLine("                 Sorts: [");
                        if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                        {
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                        }
                        sbCodes.AppendLine("                 ]");
                        sbCodes.AppendLine("             }),");
                    }

                    sbCodes.AppendLine("             open: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: self.dataSource.at(0)." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                    }
                    sbCodes.AppendLine("             },");

                    sbCodes.AppendLine("             close: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter([]);");
                    }
                    sbCodes.AppendLine("             }");

                    sbCodes.AppendLine("         }).getKendoDropDownList();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.DatePicker.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoDatePicker({");
                    sbCodes.AppendLine("            componentType: mnApp.kendoDatePiker_ComponentType,");
                    sbCodes.AppendLine("            dateInput: mnApp.kendoDatePiker_DateInput");
                    sbCodes.AppendLine("         }).getKendoDatePicker();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.TimePicker.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoTimePicker({");
                    sbCodes.AppendLine("             componentType: mnApp.kendoTimePiker_ComponentType,");
                    sbCodes.AppendLine("             dateInput: mnApp.kendoTimePiker_DateInput");
                    sbCodes.AppendLine("         }).getKendoTimePicker();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoDateTimePicker({");
                    sbCodes.AppendLine("             componentType: mnApp.kendoDateTimePiker_ComponentType,");
                    sbCodes.AppendLine("             dateInput: mnApp.kendoDateTimePiker_DateInput");
                    sbCodes.AppendLine("         }).getKendoDateTimePicker();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.MaskedTextBox.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoMaskedTextBox({");
                    sbCodes.AppendLine("             mask: '" + col.FormComponentFormat + "' ");
                    sbCodes.AppendLine("         }).getKendoMaskedTextBox();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                {
                    sbCodes.AppendLine("         self.ms" + col.ColumnName + " = $(self.selector).find('[name=" + col.ColumnName + "]').kendoMultiSelect({");
                    sbCodes.AppendLine("             valuePrimitive: true,");
                    sbCodes.AppendLine("             autoClose: false,");
                    sbCodes.AppendLine("             optionLabel: { value: '', text: mnLang.TranslateWithWord('xLng.Seciniz') },");
                    sbCodes.AppendLine("             dataValueField: 'value',");
                    sbCodes.AppendLine("             dataTextField: 'text',");

                    if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                    }
                    else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                        sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                        sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                        sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                        sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                        sbCodes.AppendLine("                 Filters: [");
                        sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                        sbCodes.AppendLine("                 ],");
                        sbCodes.AppendLine("                 Sorts: [");
                        if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                        {
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                        }
                        sbCodes.AppendLine("                 ]");
                        sbCodes.AppendLine("             }),");
                    }

                    sbCodes.AppendLine("             change: function (e) {");
                    sbCodes.AppendLine("                 if (self.dataSource.at(0) !== undefined) {");
                    sbCodes.AppendLine("                     self.dataSource.at(0).set('" + col.ColumnName + "', this.value().join());");
                    sbCodes.AppendLine("                 }");
                    sbCodes.AppendLine("             },");

                    sbCodes.AppendLine("             open: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: self.dataSource.at(0)." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                    }
                    sbCodes.AppendLine("             },");

                    sbCodes.AppendLine("             close: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter([]);");
                    }
                    sbCodes.AppendLine("             }");

                    sbCodes.AppendLine("         }).getKendoMultiSelect();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.TextEditor.ToString())
                {
                    sbCodes.AppendLine("         self.ke" + col.ColumnName + " = $(self.selector).find('[name=" + col.ColumnName + "]').kendoEditor({");
                    sbCodes.AppendLine("             serialization: { entities: false },");
                    if (col.FormEditable)
                    {
                        sbCodes.AppendLine("             tools: ['bold','italic','underline','strikethrough','justifyLeft','justifyCenter','justifyRight','justifyFull','insertUnorderedList','insertOrderedList','indent','outdent','createLink','unlink','insertImage','insertFile','subscript','superscript','tableWizard','createTable','addRowAbove','addRowBelow','addColumnLeft','addColumnRight','deleteRow','deleteColumn','viewHtml','formatting','cleanFormatting','fontName','fontSize','foreColor','backColor','print'],");
                    }
                    else
                    {
                        sbCodes.AppendLine("             tools: [],");
                    }
                    sbCodes.AppendLine("             resizable: { content: true }");
                    sbCodes.AppendLine("         }).getKendoEditor();");
                    if (!col.FormEditable)
                    {
                        sbCodes.AppendLine("         $(self.ke" + col.ColumnName + ".body).attr('contenteditable', false);");
                    }

                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.ColorPicker.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoColorPicker({");
                    sbCodes.AppendLine("             buttons: true");
                    sbCodes.AppendLine("         }).getKendoColorPicker();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.AutoComplete.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoAutoComplete({");
                    sbCodes.AppendLine("             valuePrimitive: true,");
                    sbCodes.AppendLine("             dataTextField: 'text',");

                    if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                    }
                    else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                        sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                        sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                        sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                        sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                        sbCodes.AppendLine("                 Filters: [");
                        sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                        sbCodes.AppendLine("                 ],");
                        sbCodes.AppendLine("                 Sorts: [");
                        if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                        {
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                        }
                        sbCodes.AppendLine("                 ]");
                        sbCodes.AppendLine("             }),");
                    }

                    sbCodes.AppendLine("             open: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: self.dataSource.at(0)." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                    }
                    sbCodes.AppendLine("             },");

                    sbCodes.AppendLine("             close: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter([]);");
                    }
                    sbCodes.AppendLine("             }");

                    sbCodes.AppendLine("         }).getKendoAutoComplete();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.ComboBox.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoComboBox({");
                    sbCodes.AppendLine("             valuePrimitive: true,");
                    sbCodes.AppendLine("             dataValueField: 'value',");
                    sbCodes.AppendLine("             dataTextField: 'text',");

                    if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                    }
                    else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                    {
                        sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                        sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                        sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                        sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                        sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                        sbCodes.AppendLine("                 Filters: [");
                        sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                        sbCodes.AppendLine("                 ],");
                        sbCodes.AppendLine("                 Sorts: [");
                        if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                        {
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                        }
                        sbCodes.AppendLine("                 ]");
                        sbCodes.AppendLine("             }),");
                    }

                    sbCodes.AppendLine("             open: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: self.dataSource.at(0)." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                    }
                    sbCodes.AppendLine("             },");

                    sbCodes.AppendLine("             close: function (e) {");
                    if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                    {
                        sbCodes.AppendLine("                this.dataSource.filter([]);");
                    }
                    sbCodes.AppendLine("             }");

                    sbCodes.AppendLine("         }).getKendoComboBox();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.Slider.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').kendoSlider({");
                    sbCodes.AppendLine("             min: 0,");
                    sbCodes.AppendLine("             max: 100,");
                    sbCodes.AppendLine("             smallStep: 10,");
                    sbCodes.AppendLine("             largeStep: 25");
                    sbCodes.AppendLine("         }).getKendoSlider();");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.ExternalSearchEdit.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=ExternalSearchButtonWrapper" + col.ColumnName + "]').click(function (e) {");
                    sbCodes.AppendLine("             var dataItem = self.dataSource.at(0);");
                    sbCodes.AppendLine("             mn" + col.SearchShowType + "View.create({");
                    sbCodes.AppendLine("                areaName: '" + col.SearchAreaName + "',");
                    sbCodes.AppendLine("                viewFileName: '" + col.ColumnReferansTableName + "ForSearch',");
                    sbCodes.AppendLine("                viewName: '" + col.ColumnReferansTableName + "ForSearch',");
                    sbCodes.AppendLine("                subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),");
                    sbCodes.AppendLine("                onShow: function (e) {");
                    sbCodes.AppendLine("                    e.beforeShow({");
                    sbCodes.AppendLine("                        'ownerViewName':'" + viewName + "',");
                    if (!string.IsNullOrEmpty(col.SearchFilterColumnName) && !string.IsNullOrEmpty(col.SearchFilterOperator) && !string.IsNullOrEmpty(col.SearchFilterValue))
                    {
                        sbCodes.AppendLine("                        'filters': [");
                        sbCodes.AppendLine("                            { 'filterColumnName': '" + col.SearchFilterColumnName + "', 'filterOperator': '" + col.SearchFilterOperator + "', 'filterValue': " + col.SearchFilterValue + " }");
                        sbCodes.AppendLine("                        ]");
                    }
                    sbCodes.AppendLine("                    });");
                    sbCodes.AppendLine("                },");
                    sbCodes.AppendLine("                onClose: function (e) {");
                    sbCodes.AppendLine("                    if (e.opt.isSelected) {");
                    sbCodes.AppendLine("                        dataItem.set('" + col.ColumnName + "', e.opt.selectedDataItem." + col.ColumnReferansValueColumnName + ");");

                    string displayItemGetter = "";
                    foreach (var referansDisplayColumnName in col.ColumnReferansDisplayColumnNames.Split(","))
                    {
                        if (displayItemGetter.Length > 0)
                        {
                            displayItemGetter += "+ ' ' +";
                        }
                        displayItemGetter += "e.opt.selectedDataItem." + referansDisplayColumnName;
                    }

                    sbCodes.AppendLine("                        dataItem.set('Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "', " + displayItemGetter + ");");
                    sbCodes.AppendLine("                    }");
                    sbCodes.AppendLine("                    self.validator.validate();");
                    sbCodes.AppendLine("                }");
                    sbCodes.AppendLine("             });");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");

                    sbCodes.AppendLine("         $(self.selector).find('[name=ExternalSearchButtonWrapper" + col.ColumnName + "] [name=btnClear]').click(function (e) {");
                    sbCodes.AppendLine("            e.stopPropagation();");
                    sbCodes.AppendLine("            var dataItem = self.dataSource.at(0);");
                    if (col.ColumnDefault.MyToStr() == "0")
                    {
                        sbCodes.AppendLine("            dataItem.set('" + col.ColumnName + "', 0);");
                    }
                    else
                    {
                        sbCodes.AppendLine("            dataItem.set('" + col.ColumnName + "', '');");
                    }
                    sbCodes.AppendLine("            dataItem.set('Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "', '');");
                    sbCodes.AppendLine("            self.validator.validate();");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.ImageBox.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').click(function (e) {");
                    sbCodes.AppendLine("             $elmImg = $(\"<input id='imageFile' type='file' accept='image/*' style='display:none;' />\"); ");
                    sbCodes.AppendLine("             $elmImg.change(function () {");
                    sbCodes.AppendLine("                if (this.files && this.files[0]) {");
                    sbCodes.AppendLine("                    var reader = new FileReader();");
                    sbCodes.AppendLine("                    reader.onload = function (e) {");
                    sbCodes.AppendLine("                        if (self.dataSource.at(0) !== undefined) {");
                    sbCodes.AppendLine("                            self.dataSource.at(0).set('" + col.ColumnName + "', e.target.result);");
                    sbCodes.AppendLine("                        }");
                    sbCodes.AppendLine("                    }");
                    sbCodes.AppendLine("                    reader.readAsDataURL(this.files[0]);");
                    sbCodes.AppendLine("                    this.value = null; //aynı resmi tekrar seçtiğinde event çalışabilmesi için son seçileni siliyoruz value dan");
                    sbCodes.AppendLine("                }");
                    sbCodes.AppendLine("             });");
                    sbCodes.AppendLine("             $elmImg.click();");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");
                }
                else if (col.FormComponentType == EnmComponentType.ImageSearchEdit.ToString())
                {
                    sbCodes.AppendLine("         $(self.selector).find('[name=ImageSearchEditWrapper" + col.ColumnName + "] [name=btnSearch]').click(function (e) {");
                    sbCodes.AppendLine("             var dataItem = self.dataSource.at(0);");
                    sbCodes.AppendLine("             mn" + col.SearchShowType + "View.create({");
                    sbCodes.AppendLine("                areaName: '_',");
                    sbCodes.AppendLine("                viewFileName: 'viewGorseller',");
                    sbCodes.AppendLine("                viewName: 'viewGorseller',");
                    sbCodes.AppendLine("                subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),");
                    sbCodes.AppendLine("                onShow: function (e) {");
                    sbCodes.AppendLine("                    e.beforeShow({ 'ownerViewName':'" + viewName + "' });");
                    sbCodes.AppendLine("                },");
                    sbCodes.AppendLine("                onClose: function (e) {");
                    sbCodes.AppendLine("                    if (e.opt.isSelected) {");
                    sbCodes.AppendLine("                        dataItem.set('" + col.ColumnName + "', e.opt.selectedDataItem.FileUrl);");
                    sbCodes.AppendLine("                    }");
                    sbCodes.AppendLine("                    self.validator.validate();");
                    sbCodes.AppendLine("                }");
                    sbCodes.AppendLine("             });");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");

                    sbCodes.AppendLine("         $(self.selector).find('[name=ImageSearchEditWrapper" + col.ColumnName + "] [name=btnClear]').click(function (e) {");
                    sbCodes.AppendLine("            e.stopPropagation();");
                    sbCodes.AppendLine("            var dataItem = self.dataSource.at(0);");
                    sbCodes.AppendLine("            dataItem.set('" + col.ColumnName + "', '');");
                    sbCodes.AppendLine("            self.validator.validate();");
                    sbCodes.AppendLine("         });");
                    sbCodes.AppendLine("");
                }

            }
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            #endregion

            #region Set Atribute FormElements

            sbCodes.AppendLine("     function fSetAtributeFormElements() {");
            foreach (var col in formFileds.OrderBy(o => o.FormOrder))
            {
                if (col.ColumnRequired && col.FormComponentType != EnmComponentType.Checkbox.ToString())
                {
                    if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                    {
                        sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').attr('min', '1');");
                    }
                    else
                    {
                        sbCodes.AppendLine("         $(self.selector).find('[name=" + col.ColumnName + "]').attr('required', 'required');");
                    }
                }
            }
            sbCodes.AppendLine("     }");

            #endregion

            #region Yetki Uygula

            sbCodes.AppendLine("");
            sbCodes.AppendLine("     function fYetkiUygula() {");
            sbCodes.AppendLine("         //Standart Yetkiler");
            sbCodes.AppendLine("         var _C = mnUser.isYetkili(self.tableName + '.D_C.');");
            sbCodes.AppendLine("         var _U = mnUser.isYetkili(self.tableName + '.D_U.');");
            sbCodes.AppendLine("         var _D = mnUser.isYetkili(self.tableName + '.D_D.');");
            sbCodes.AppendLine("");

            if (dataSourceDetails.Any())
            {
                sbCodes.AppendLine("             // menu buttons yetkiler için");
                foreach (var item in dataSourceDetails)
                {
                    string btnName = "btnDetay" + item.TableName;
                    string btnName_R = btnName + "_R";
                    string btnName_Prefix_Yetki = item.TableName + ".D_R.";

                    sbCodes.AppendLine("             var " + btnName_R + " = mnUser.isYetkili('" + btnName_Prefix_Yetki + "');");
                    sbCodes.AppendLine("             mnApi.controlEnable($(self.selector).find('#" + btnName + "'), " + btnName_R + ");");
                    sbCodes.AppendLine("");
                }
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("         //ek yetkiler için)");
            sbCodes.AppendLine("         //...");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         //Form İçin");
            sbCodes.AppendLine("         if (_C || _U) {");
            sbCodes.AppendLine("             $(self.selector).find('#btnKaydet').show();");
            sbCodes.AppendLine("         } else {");
            sbCodes.AppendLine("             $(self.selector).find('#btnKaydet').hide();");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("        //form element görünümleri");
            sbCodes.AppendLine("        var fieldList = [];");
            foreach (var col in formFileds)
            {
                sbCodes.AppendLine("        fieldList.push({ 'Name': '" + col.ColumnName + "', 'Visible': '" + col.FormColumnShowRoleGroupIds + "'.indexOf(mnUser.Info.nYetkiGrup) >= 0 });");
            }
            sbCodes.AppendLine("");
            sbCodes.AppendLine("        for (var i in fieldList) {");
            sbCodes.AppendLine("            var $elm = $(self.selector).find('.mnFormElementContainer .mnFormElementDiv[name=div' + fieldList[i].Name + ']');");
            sbCodes.AppendLine("            if (fieldList[i].Visible) {");
            sbCodes.AppendLine("                $elm.show();");
            sbCodes.AppendLine("            } else {");
            sbCodes.AppendLine("                $elm.hide();");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");


            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            #endregion

            #region Görünürlük uygula
            sbCodes.AppendLine("");
            sbCodes.AppendLine("     function fGorunurlukUygula(dataItem) {");

            foreach (var col in visibilityFileds)
            {
                sbCodes.AppendLine("        $(self.selector).find('.mnFormElementDiv[name=div" + col.ColumnName + "]').hide();");
            }
            sbCodes.AppendLine("");

            sbCodes.AppendLine("        if (dataItem != null && dataItem != undefined) {");

            foreach (var col in visibilityFileds)
            {
                sbCodes.AppendLine("            if (dataItem." + col.FormVisibilityColumnName + " == '" + col.FormVisibilityColumnValue + "') {");
                sbCodes.AppendLine("                $(self.selector).find('.mnFormElementDiv[name=div" + col.ColumnName + "]').show();");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("        }");

            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");
            #endregion

            #region prepare

            sbCodes.AppendLine("     self.prepare = function () {");
            sbCodes.AppendLine("         // form element atribute set");
            sbCodes.AppendLine("         fSetAtributeFormElements();");
            sbCodes.AppendLine("         // DataSource");
            sbCodes.AppendLine("         fCreateDataSource();");
            sbCodes.AppendLine("         // form Elementler");
            sbCodes.AppendLine("         fCreateFormElements();");
            sbCodes.AppendLine("         // Language");
            sbCodes.AppendLine("         mnLang.TranslateWithSelector(self.selector);");
            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion

            #region beforeShow

            sbCodes.AppendLine("     self.beforeShow = function (_opt) {");
            sbCodes.AppendLine("         self.opt = $.extend({}, _opt);");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("         if (self.opt.qprms." + _oTableOptions.PrimaryKey + " !== null && self.opt.qprms." + _oTableOptions.PrimaryKey + " > 0) {");
            sbCodes.AppendLine("             self.dataSource.filter({ field: self.primaryKey, operator: 'eq', value: self.opt.qprms." + _oTableOptions.PrimaryKey + " });");
            sbCodes.AppendLine("         } else {");
            sbCodes.AppendLine("             self.dataSource.data([]); // eski data varsa işleme girmemesi için");
            sbCodes.AppendLine("             self.dataSource.add();");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         self.tabstrip.select(0);");

            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion

            sbCodes.AppendLine("     return self;");
            sbCodes.AppendLine(" }();");
            sbCodes.AppendLine("</script>");

            #endregion
            return sbCodes;
        }

        public StringBuilder GridViewGenerate(MyTableOption _oTableOptions, Boolean _SearchView)
        {
            StringBuilder sbCodes = new();
            var viewName = _oTableOptions.GridViewName;
            var findTabs = _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridFindTab.MyToTrim().Length > 0).GroupBy(g => g.GridFindTab).OrderBy(o => o.Key);
            var findField = _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridFindTab.MyToTrim().Length > 0).OrderBy(o => o.GridOrder);
            var dataSourceFileds = _oTableOptions.Fields.Where(c => c.ColumnUse).OrderBy(o => o.ColumnOrder);
            var gridFileds = _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridUse).OrderBy(o => o.GridOrder);
            var dataSourceDetails = _oTableOptions.Details.Where(c => c.GridViewUse);
            var SearchRequiredFileds = _oTableOptions.Fields.Where(c => c.SearchRequired).OrderBy(o => o.ColumnOrder);
            var dataSourceSortFileds = _oTableOptions.Fields.OrderBy(o => o.ColumnOrder).Where(c => c.GridDataSourceSortDir != null && c.GridDataSourceSortDir.Length > 0);

            if (_SearchView)
            {
                viewName = _oTableOptions.SearchViewName;
                gridFileds = _oTableOptions.Fields.Where(c => c.ColumnUse && c.SearchUse).OrderBy(o => o.SearchOrder);
                findTabs = _oTableOptions.Fields.Where(c => c.ColumnUse && c.SearchFindTab.MyToTrim().Length > 0).GroupBy(g => g.SearchFindTab).OrderBy(o => o.Key);
                findField = _oTableOptions.Fields.Where(c => c.ColumnUse && c.SearchFindTab.MyToTrim().Length > 0).OrderBy(o => o.SearchOrder);
            }

            #region html codes

            sbCodes.AppendLine("");
            sbCodes.AppendLine("<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("<div id='" + viewName + "'>");

            if (findTabs.Any())
            {
                sbCodes.AppendLine("    <div>");
                sbCodes.AppendLine("        <div id='divButtonGroup' class='mnButtonGroup float-right' >");
                sbCodes.AppendLine("            <button id='btnAra' type='button' class='btn btn-outline-warning text-nowrap mn-hover-color-white' data-langkey-title='xLng.Ara' style='position:absolute; right:0px;' > <i class='fa fa-binoculars'></i> <small data-langkey-text='xLng.Ara'></small> </button>");
                sbCodes.AppendLine("        </div>");
                sbCodes.AppendLine("    </div>");
            }

            sbCodes.AppendLine("    <div>");
            if (findTabs.Any())
            {
                sbCodes.AppendLine("        <div id='tabstrip' class='mnFindTab'>");
                sbCodes.AppendLine("            <ul>");
                foreach (var findTab in findTabs)
                {
                    if (!_SearchView)
                    {
                        sbCodes.AppendLine("                <li> <span data-langkey-text='" + _oTableOptions.LangKeyRoot + ".GridFindTab." + findTab.Key + "'></span> </li>");
                    }
                    else
                    {
                        sbCodes.AppendLine("                <li> <span data-langkey-text='" + _oTableOptions.LangKeyRoot + ".SearchFindTab." + findTab.Key + "'></span> </li>");
                    }
                }
                sbCodes.AppendLine("            </ul>");
                sbCodes.AppendLine("");

                foreach (var findTab in findTabs)
                {
                    sbCodes.AppendLine("            <div style='padding:5px 0px;'>");
                    sbCodes.AppendLine("                <div class='form-inline mnFindElementContainer'>");

                    var tabFindField = findField.Where(c => c.GridFindTab == findTab.Key);
                    if (_SearchView)
                    {
                        tabFindField = findField.Where(c => c.SearchFindTab == findTab.Key);
                    }

                    foreach (var col in tabFindField)
                    {
                        int gercekGridComponentWidth = col.GridComponentWidth;
                        string find_operator = "eq";
                        if (col.ColumnNetType == "System.String")
                        {
                            find_operator = "contains";
                        }

                        sbCodes.AppendLine("                    <span name='div" + col.ColumnName + "' class='mnFindElementDiv form-inline'>");

                        if (col.FormComponentType == EnmComponentType.TextBox.ToString() || col.FormComponentType == EnmComponentType.PasswordTextBox.ToString() || col.FormComponentType == EnmComponentType.TextEditor.ToString() || col.FormComponentType == EnmComponentType.TextArea.ToString() || col.FormComponentType == EnmComponentType.TextAreaForJson.ToString() || col.FormComponentType == EnmComponentType.ImageSearchEdit.ToString())
                        {
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' class='k-textbox' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' autocomplete='off' />");
                        }
                        else if (col.FormComponentType == EnmComponentType.DatePicker.ToString() || col.FormComponentType == EnmComponentType.TimePicker.ToString() || col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                        {
                            gercekGridComponentWidth = Convert.ToInt32(col.GridComponentWidth * 1.5); // boyutu % olarak artırıyoruz
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' class='mr-2' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='gte' />");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='lte' />");
                        }
                        else if (col.FormComponentType == EnmComponentType.Checkbox.ToString())
                        {
                            gercekGridComponentWidth = Convert.ToInt32(col.GridComponentWidth * 1.25); // boyutu % olarak artırıyoruz
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' />");
                        }
                        else if (col.FormComponentType == EnmComponentType.ExternalSearchEdit.ToString())
                        {
                            gercekGridComponentWidth = Convert.ToInt32(col.GridComponentWidth * 1.25); // boyutu % olarak artırıyoruz
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <span name='FindExternalSearchButtonWrapper" + col.ColumnName + "' class='mnSearchEdit k-textbox k-space-right' style='width:" + gercekGridComponentWidth.ToString() + "px' >");
                            sbCodes.AppendLine("                            <input class='d-none' name='" + col.ColumnName + "' type='hidden' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' />");
                            sbCodes.AppendLine("                            <input class='font-weight-light' type='text' data-find_display_field='" + col.ColumnName + "' data-langkey-placeholder='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "' disabled>");
                            sbCodes.AppendLine("                            <button name='btnClear' class='btn btn-light fa fa-close' ></button>");
                            sbCodes.AppendLine("                            <button name='btnSearch' class='btn btn-light fa fa-search'></button>");
                            sbCodes.AppendLine("                        </span>");
                        }
                        else if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            gercekGridComponentWidth = Convert.ToInt32(col.GridComponentWidth * 1.25); // boyutu % olarak artırıyoruz
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + "StringArray" + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' />");
                        }
                        else
                        {
                            gercekGridComponentWidth = Convert.ToInt32(col.GridComponentWidth * 1.25); // boyutu % olarak artırıyoruz
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + gercekGridComponentWidth.ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' autocomplete='off' />");
                        }
                        sbCodes.AppendLine("                    </span>");

                        sbCodes.AppendLine("");
                    }
                    sbCodes.AppendLine("                </div>");
                    sbCodes.AppendLine("            </div>");
                    sbCodes.AppendLine("");
                }
                sbCodes.AppendLine("        </div>");
                sbCodes.AppendLine("");
            }
            sbCodes.AppendLine("        <div id='divGrid'></div>");
            sbCodes.AppendLine("    </div>");

            sbCodes.AppendLine("</div>");
            sbCodes.AppendLine("");
            #endregion

            #region script codes
            sbCodes.AppendLine("<script>");
            sbCodes.AppendLine(" window." + viewName + " = function () {");
            sbCodes.AppendLine("     var self = {};");
            sbCodes.AppendLine("     self.opt = null;");
            sbCodes.AppendLine("     self.area = '" + _oTableOptions.AreaName + "';");
            sbCodes.AppendLine("     self.title = '" + _oTableOptions.LangKeyRoot + ".Title" + "';");
            sbCodes.AppendLine("     self.selector = '#" + viewName + "';");
            sbCodes.AppendLine("     self.primaryKey = '" + _oTableOptions.PrimaryKey + "';");
            sbCodes.AppendLine("     self.tableName = '" + _oTableOptions.TableName + "';");
            sbCodes.AppendLine("     self.apiUrlPrefix = '/' + self.area + '/" + MyCodeGen.CodeGen_ControllerPrefix + "' + self.tableName;");
            sbCodes.AppendLine("");

            #region Create Data Source

            sbCodes.AppendLine("     function fCreateDataSource() {");
            sbCodes.AppendLine("         self.dataSource = new kendo.data.DataSource({");
            sbCodes.AppendLine("             transport: {");
            sbCodes.AppendLine("                 read: { type: 'GET', url: self.apiUrlPrefix + '/Read', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 create: { type: 'POST', url: self.apiUrlPrefix + '/Create', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 update: { type: 'POST', url: self.apiUrlPrefix + '/Update', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                 destroy: { type: 'POST', url: self.apiUrlPrefix + '/Delete', dataType: 'json' },");
            sbCodes.AppendLine("                 parameterMap: function (data, operation) {");
            sbCodes.AppendLine("                     if (operation === 'read') {");
            sbCodes.AppendLine("                         return (new kendo.data.transports['aspnetmvc-ajax']({})).options.parameterMap(data);");
            sbCodes.AppendLine("                     }");
            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("                     else if (operation === 'create' || operation === 'update') {");
                foreach (var col in dataSourceFileds)
                {
                    if (col.ColumnDbType == "datetime")
                    {
                        sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                    }
                    if (col.ColumnDbType == "date")
                    {
                        sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                    }
                    if (col.ColumnDbType == "time")
                    {
                        sbCodes.AppendLine("                         data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 't');");
                    }
                }

                sbCodes.AppendLine("                         return kendo.stringify(data);");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                     else if (operation === 'destroy') {");
                sbCodes.AppendLine("                         return { _id: data[self.primaryKey] };");
                sbCodes.AppendLine("                     }");
            }
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             },");

            sbCodes.AppendLine($"             pageSize: {_oTableOptions.GridViewDataSourcePageSize}, serverPaging: true, serverSorting: true, serverFiltering: true, serverGrouping: true, serverAggregates: true,");

            sbCodes.AppendLine("             sort: [");


            foreach (var col in dataSourceSortFileds)
            {
                string virgul = ",";
                if (dataSourceSortFileds.Last().Equals(col))
                {
                    virgul = "";
                }

                sbCodes.AppendLine("                 { field: '" + col.ColumnName + "', dir: '" + col.GridDataSourceSortDir + "' }" + virgul);
            }

            sbCodes.AppendLine("             ],");

            sbCodes.AppendLine("             schema: {");
            sbCodes.AppendLine("                 errors: 'Errors', data: 'Data', total: 'Total', aggregates: 'AggregateResults',");
            sbCodes.AppendLine("                 model: {");
            sbCodes.AppendLine("                     id: self.primaryKey,");
            sbCodes.AppendLine("                     fields: {");

            foreach (var col in dataSourceFileds)
            {
                string editable = "";
                if (!col.GridEditable)
                {
                    editable = ", editable:false";
                }

                if (col.ColumnName == _oTableOptions.PrimaryKey)
                {
                    sbCodes.AppendLine("                         " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "', defaultValue: null " + editable + " },");
                }
                else
                {
                    sbCodes.AppendLine("                         " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "'" + editable + " },");
                }
            }

            // computed alanların tanımlanması
            foreach (var col in dataSourceFileds)
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + ": { type: 'string' },");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ": { type: 'string' },");
                }
                else if (col.FormComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString() || col.GridComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString())
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + "Link: { type: 'string' },");
                }
            }

            sbCodes.Remove(sbCodes.Length - 3, 1);// son virgülü atmak için (sonundaki entera dokunmadan)

            sbCodes.AppendLine("                     }");
            sbCodes.AppendLine("                 }");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             error: function (e) {");
            sbCodes.AppendLine("                 if (e.xhr === null) { mnNotification.warning(e.errors); } else { mnErrorHandler.Handle(e.xhr); }");
            sbCodes.AppendLine("                 this.cancelChanges();");
            sbCodes.AppendLine("             },");

            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("             requestStart: function (e) {");
                sbCodes.AppendLine("             },");

                sbCodes.AppendLine("             requestEnd: function (e) {");
                sbCodes.AppendLine("                 if (e.response !== undefined && e.response.Errors === null) {");
                sbCodes.AppendLine("                     if (e.type === 'create' || e.type === 'update' || e.type === 'destroy') {");
                //sbCodes.AppendLine("                         self.isDirty = true;");
                sbCodes.AppendLine("                         mnLookup.listRead(self.tableName);");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                     if (e.type === 'create') {");
                sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitEklendi'));");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                     if (e.type === 'update') {");
                sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitDuzeltildi'));");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                     if (e.type === 'destroy') {");
                sbCodes.AppendLine("                         mnNotification.success(mnLang.TranslateWithWord('xLng.KayitSilindi'));");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                 }");
                sbCodes.AppendLine("             },");

                sbCodes.AppendLine("             change: function (e) {");
                sbCodes.AppendLine("                 if (e.items[0] !== undefined) {");
                sbCodes.AppendLine("                     if (e.items[0].get(self.primaryKey) === null) {");
                sbCodes.AppendLine("                         $.ajax({");
                sbCodes.AppendLine("                             url: self.apiUrlPrefix + '/GetByNew', type: 'GET', dataType: 'json', async: false,");
                sbCodes.AppendLine("                             success: function (result) {");

                foreach (var col in dataSourceFileds)
                {
                    if (col.ColumnNetType == "System.DateTime")
                    {
                        sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = kendo.parseDate(result." + col.ColumnName + ");");
                    }
                    else
                    {
                        sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = result." + col.ColumnName + ";");
                    }
                }

                sbCodes.AppendLine("                                 //computed alanların tanımlanması");
                foreach (var col in dataSourceFileds)
                {
                    if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                    {
                        sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + " = result.Cc" + col.ColumnName + ";");
                    }
                    else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                    {
                        sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + " = result.Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ";");
                    }
                }

                sbCodes.AppendLine("                                 //filterdan gelen default value set için");
                sbCodes.AppendLine("                                 $(self.opt.filters).each(function (index, row) {");
                sbCodes.AppendLine("                                     if (row.filterColumnName !== 'Id') {");
                sbCodes.AppendLine("                                        e.items[0].set(row.filterColumnName, row.filterValue);");
                sbCodes.AppendLine("                                     }");
                sbCodes.AppendLine("                                 });");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("                             },");
                sbCodes.AppendLine("                             error: function (xhr, status) {");
                sbCodes.AppendLine("                                 mnErrorHandler.Handle(xhr);");
                sbCodes.AppendLine("                             }");
                sbCodes.AppendLine("                         });");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                 }");
                sbCodes.AppendLine("                 // ve gerekebilecek diğer işlemler");
                sbCodes.AppendLine("                 // ...");
                sbCodes.AppendLine("             }");
            }
            sbCodes.AppendLine("         });");
            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            #endregion

            #region Create Grid

            sbCodes.AppendLine("     function fCreateGrid() {");
            sbCodes.AppendLine("         self.grid = $(self.selector).find('#divGrid').kendoGrid({");
            sbCodes.AppendLine("             excel: { allPages: true },");
            sbCodes.AppendLine("             excelExport: mnApp.exportGridWithTemplatesContentForKendo,");
            sbCodes.AppendLine("             autoBind: false, resizable: true, reorderable: true,");
            sbCodes.AppendLine("             sortable: { mode: 'multiple', allowUnsort: true, showIndexes: true },");
            sbCodes.AppendLine("             pageable: {");
            sbCodes.AppendLine("                 refresh: true, pageSizes: mnApp.gridPageSizes_default, buttonCount: 5, input: true,");
            sbCodes.AppendLine("                 messages: { itemsPerPage: '' }");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             editable: {");
            sbCodes.AppendLine("                 confirmation: true, mode: 'inline', createAt: 'bottom'");
            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             columns: [");

            if (_SearchView)
            {
                sbCodes.AppendLine("             {");
                sbCodes.AppendLine("                 locked: true,");
                sbCodes.AppendLine("                 title: '',");
                sbCodes.AppendLine("                 width: '35px',");
                sbCodes.AppendLine("                 command: [");
                sbCodes.AppendLine("                     { template: '<button id=\"btnSecDon\" class=\"btn btn-link btn-xs fa fa-download\" style=\"padding:0px;\" title=\"' + mnLang.TranslateWithWord(\"xLng.SecDon\") + '\"> </button>' }");
                sbCodes.AppendLine("                 ]");
                sbCodes.AppendLine("             },");
            }

            //grid crud var ise
            if ((_SearchView == false && _oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0) || (_SearchView == true && _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0))
            {
                sbCodes.AppendLine("             {");
                sbCodes.AppendLine("                 locked: true,");
                string headerTemplate = "";
                if (_oTableOptions.TableCuds.Contains("C", StringComparison.CurrentCulture))
                {
                    headerTemplate = "'<span id=\"btnEkle\" style=\" \" title=\"' + mnLang.TranslateWithWord(\"xLng.Ekle\") + '\" ></span>'";
                }
                if (headerTemplate.MyToTrim().Length > 0)
                {
                    headerTemplate += "+";
                }
                headerTemplate += "'<span id=\"btnSaveAsExcel\" style=\" \" title=\"' + mnLang.TranslateWithWord(\"xLng.SaveAsExcel\") + '\" ></span>'";

                sbCodes.AppendLine("                 headerTemplate: " + headerTemplate + ",");

                sbCodes.AppendLine("                 width: '95px',");
                sbCodes.AppendLine("                 command: [");
                if (_oTableOptions.TableCuds.Contains("U", StringComparison.CurrentCulture))
                {
                    sbCodes.AppendLine("                     { name: 'edit', buttonType: 'ImageAndText', text: { cancel: '', update: '', edit: '' }, attr: 'style=\" \" data-langkey-title=\"xLng.Duzelt\" ' },");
                }
                if (_oTableOptions.TableCuds.Contains("D", StringComparison.CurrentCulture))
                {
                    sbCodes.AppendLine("                     { name: 'destroy', buttonType: 'ImageAndText', text: '', attr: 'style=\" \" data-langkey-title=\"xLng.Sil\"' }");
                }
                sbCodes.AppendLine("                 ]");
                sbCodes.AppendLine("             },");
            }

            //grid detay geçiş var ise
            if (!_SearchView)
            {
                if (dataSourceDetails.Any())
                {
                    sbCodes.AppendLine("             {");
                    sbCodes.AppendLine("                 locked: true,");
                    sbCodes.AppendLine("                 title: mnLang.TranslateWithWord('xLng.Detay'),");

                    sbCodes.AppendLine("                 width: '" + (35 + (60 * dataSourceDetails.Count())) + "px',"); // sabiti 35 her ek buton için 60 ekler

                    sbCodes.AppendLine("                 command: [");
                    foreach (var item in dataSourceDetails)
                    {
                        sbCodes.AppendLine("                     { name: 'btnDetay_" + item.TableName + "', buttonType: 'ImageAndText', text: mnLang.TranslateWithWord('x" + item.AreaName + "." + item.TableName + ".ShortTitle'), attr: 'class=\"btnDetay\" data-langkey-title=\"x" + item.AreaName + "." + item.TableName + ".Title\" ' },");
                    }
                    sbCodes.AppendLine("                 ]");
                    sbCodes.AppendLine("             },");
                }
            }


            foreach (var col in gridFileds)
            {
                sbCodes.AppendLine("             {");
                sbCodes.AppendLine("                 hidden: ![" + col.GridColumnShowRoleGroupIds + "].includes(mnUser.Info.nYetkiGrup),");

                if (col.GridLocked)
                {
                    sbCodes.AppendLine("                 locked: true,");
                }

                if (!col.GridEncoded)
                {
                    sbCodes.AppendLine("                 encoded: false,");  //code gene bu yok ekleyelim
                }

                sbCodes.AppendLine("                 title: mnLang.TranslateWithWord('" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'),");
                sbCodes.AppendLine("                 field: '" + col.ColumnName + "',");


                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                 template: '#:Cc" + col.ColumnName + "#',");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                 template: '#:Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "#',");
                }

                if (col.GridComponentType == EnmComponentType.ImageBox.ToString())
                {
                    sbCodes.AppendLine("                 template:\"<img name='" + col.ColumnName + "' src='#:" + col.ColumnName + " || ''#' class='' style='width:28px; height:30px;' />\", ");
                }


                // grid crud var ise ve format belirlenmiş ise
                if (!string.IsNullOrEmpty(_oTableOptions.GridViewCrudEditorType) && !string.IsNullOrEmpty(col.GridComponentFormat))
                {
                    sbCodes.AppendLine("                 format: '" + col.GridComponentFormat + "',");
                }

                //attributes //  grid colonun içine sığmayan textleri kırpar
                if (col.GridTextNowrap)
                {
                    sbCodes.AppendLine("                 attributes: { 'class': 'text-nowrap' },");
                }

                // grid crud var ise ve inline editör ise
                if ((_SearchView && _oTableOptions.SearchViewCrudEditorType == "Inline") || (!_SearchView && _oTableOptions.GridViewCrudEditorType == "Inline"))
                {
                    string decimals = "0";
                    if (!string.IsNullOrEmpty(col.FormComponentFormat))
                    {
                        decimals = col.FormComponentFormat.Substring(1);
                    }

                    sbCodes.AppendLine("                 editor: function(container, options) { ");

                    if (col.GridComponentType == EnmComponentType.TextArea.ToString())
                    {
                        sbCodes.AppendLine("                     var input = $('<textarea class=\"k-textbox\" />');");
                    }
                    else
                    {
                        sbCodes.AppendLine("                     var input = $('<input type=\"text\"/>');");
                    }

                    sbCodes.AppendLine("                     input.attr('name', options.field);");
                    if (col.ColumnRequired && col.GridComponentType != EnmComponentType.Checkbox.ToString())
                    {
                        sbCodes.AppendLine("                     input.attr('required', 'required');");
                        sbCodes.AppendLine("                     $('<span class=\"k-invalid-msg\" data-for=\"' + options.field + '\"></span>').appendTo(container);");
                    }

                    sbCodes.AppendLine("                     input.appendTo(container);");

                    if (col.GridComponentType == EnmComponentType.TextBox.ToString())
                    {
                        sbCodes.AppendLine("                     input.addClass('k-textbox');");
                    }
                    else if (col.GridComponentType == EnmComponentType.PasswordTextBox.ToString())
                    {
                        sbCodes.AppendLine("                     input.addClass('k-textbox');");
                        sbCodes.AppendLine("                     input.attr('type', 'password');");
                    }
                    else if (col.GridComponentType == EnmComponentType.NumericTextBox.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoNumericTextBox({");
                        sbCodes.AppendLine("                         format: '" + col.FormComponentFormat + "', decimals: " + decimals + ", min: 0, spinners: false, step: 0");
                        sbCodes.AppendLine("                     });");
                    }
                    else if (col.GridComponentType == EnmComponentType.DropDownList.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoDropDownList({");
                        if (!string.IsNullOrEmpty(col.GridComponentFilterType))
                        {
                            sbCodes.AppendLine("                         filter: '" + col.GridComponentFilterType + "',");
                        }

                        sbCodes.AppendLine("                         valuePrimitive: true,");
                        sbCodes.AppendLine("                         optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                        sbCodes.AppendLine("                         dataValueField: 'value',");
                        sbCodes.AppendLine("                         dataTextField: 'text',");

                        if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                        }
                        else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.listLoad({");
                            sbCodes.AppendLine("                             TableName: '" + col.ColumnReferansTableName + "',");
                            sbCodes.AppendLine("                             ValueField: '" + col.ColumnReferansValueColumnName + "',");
                            sbCodes.AppendLine("                             TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                            sbCodes.AppendLine("                             OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                            sbCodes.AppendLine("                             Filters: [");
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                            sbCodes.AppendLine("                             ],");
                            sbCodes.AppendLine("                             Sorts: [");
                            if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                            {
                                sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                            }
                            sbCodes.AppendLine("                             ]");
                            sbCodes.AppendLine("                         }),");
                        }

                        sbCodes.AppendLine("                         open: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: options.model." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                        }
                        sbCodes.AppendLine("                         },");

                        sbCodes.AppendLine("                         close: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter([]);");
                        }
                        sbCodes.AppendLine("                         }");

                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.DatePicker.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoDatePicker({");
                        sbCodes.AppendLine("                        componentType: mnApp.kendoDatePiker_ComponentType,");
                        sbCodes.AppendLine("                        dateInput: mnApp.kendoDatePiker_DateInput");
                        sbCodes.AppendLine("                     });");
                    }
                    else if (col.GridComponentType == EnmComponentType.TimePicker.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoTimePicker({");
                        sbCodes.AppendLine("                         componentType: mnApp.kendoTimePiker_ComponentType,");
                        sbCodes.AppendLine("                         dateInput: mnApp.kendoTimePiker_DateInput");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.DateTimePicker.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoDateTimePicker({");
                        sbCodes.AppendLine("                         componentType: mnApp.kendoDateTimePiker_ComponentType,");
                        sbCodes.AppendLine("                         dateInput: mnApp.kendoDateTimePiker_DateInput");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.MaskedTextBox.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoMaskedTextBox({");
                        sbCodes.AppendLine("                         mask: '" + col.FormComponentFormat + "' ");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.MultiSelect.ToString())
                    {
                        sbCodes.AppendLine("                    var msInput = input.kendoMultiSelect({");
                        sbCodes.AppendLine("                        valuePrimitive: true,");
                        sbCodes.AppendLine("                        autoClose: false,");
                        sbCodes.AppendLine("                        optionLabel: { value: '', text: mnLang.TranslateWithWord('xLng.Seciniz') },");
                        sbCodes.AppendLine("                        dataValueField: 'value',");
                        sbCodes.AppendLine("                        dataTextField: 'text',");

                        if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                        {
                            sbCodes.AppendLine("                        dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                        }
                        else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                        {
                            sbCodes.AppendLine("                        dataSource: mnLookup.listLoad({");
                            sbCodes.AppendLine("                            TableName: '" + col.ColumnReferansTableName + "',");
                            sbCodes.AppendLine("                            ValueField: '" + col.ColumnReferansValueColumnName + "',");
                            sbCodes.AppendLine("                            TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                            sbCodes.AppendLine("                            OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                            sbCodes.AppendLine("                            Filters: [");
                            sbCodes.AppendLine("                                { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                            sbCodes.AppendLine("                            ],");
                            sbCodes.AppendLine("                            Sorts: [");
                            if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                            {
                                sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                            }
                            sbCodes.AppendLine("                            ]");
                            sbCodes.AppendLine("                        }),");
                        }

                        sbCodes.AppendLine("                        change: function (e) {");
                        sbCodes.AppendLine("                            options.model.set('" + col.ColumnName + "', this.value().join());");
                        sbCodes.AppendLine("                        },");

                        sbCodes.AppendLine("                        open: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: options.model." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                        }
                        sbCodes.AppendLine("                        },");

                        sbCodes.AppendLine("                        close: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter([]);");
                        }
                        sbCodes.AppendLine("                        }");

                        sbCodes.AppendLine("                    }).getKendoMultiSelect();");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                    if (options.model." + col.ColumnName + " !== null || undefined || '') {");
                        sbCodes.AppendLine("                        setTimeout(function () {");
                        sbCodes.AppendLine("                            msInput.value(options.model." + col.ColumnName + ".split(','));");
                        sbCodes.AppendLine("                        });");
                        sbCodes.AppendLine("                    }");
                        sbCodes.AppendLine("");

                    }
                    else if (col.GridComponentType == EnmComponentType.TextEditor.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoEditor({");
                        sbCodes.AppendLine("                         serialization: { entities: false },");
                        sbCodes.AppendLine("                         resizable: { content: true }");
                        sbCodes.AppendLine("                     });");

                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.ColorPicker.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoColorPicker({");
                        sbCodes.AppendLine("                         buttons: true");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.Checkbox.ToString())
                    {
                        sbCodes.AppendLine("                     input.attr('type', 'checkbox');");
                    }
                    else if (col.GridComponentType == EnmComponentType.Radio.ToString())
                    {
                        sbCodes.AppendLine("                     ");
                    }
                    else if (col.GridComponentType == EnmComponentType.AutoComplete.ToString())
                    {
                        sbCodes.AppendLine("                     ");
                        sbCodes.AppendLine("                     input.kendoAutoComplete({");
                        sbCodes.AppendLine("                         valuePrimitive: true,");
                        sbCodes.AppendLine("                         dataTextField: 'text',");

                        if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                        }
                        else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.listLoad({");
                            sbCodes.AppendLine("                             TableName: '" + col.ColumnReferansTableName + "',");
                            sbCodes.AppendLine("                             ValueField: '" + col.ColumnReferansValueColumnName + "',");
                            sbCodes.AppendLine("                             TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                            sbCodes.AppendLine("                             OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                            sbCodes.AppendLine("                             Filters: [");
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                            sbCodes.AppendLine("                             ],");
                            sbCodes.AppendLine("                             Sorts: [");
                            if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                            {
                                sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                            }
                            sbCodes.AppendLine("                             ]");
                            sbCodes.AppendLine("                         }),");
                        }

                        sbCodes.AppendLine("                        open: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: options.model." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                        }
                        sbCodes.AppendLine("                        },");

                        sbCodes.AppendLine("                            close: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter([]);");
                        }
                        sbCodes.AppendLine("                        }");

                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.ComboBox.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoComboBox({");
                        sbCodes.AppendLine("                         valuePrimitive: true,");
                        sbCodes.AppendLine("                         dataValueField: 'value',");
                        sbCodes.AppendLine("                         dataTextField: 'text',");

                        if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                        }
                        else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                        {
                            sbCodes.AppendLine("                         dataSource: mnLookup.listLoad({");
                            sbCodes.AppendLine("                             TableName: '" + col.ColumnReferansTableName + "',");
                            sbCodes.AppendLine("                             ValueField: '" + col.ColumnReferansValueColumnName + "',");
                            sbCodes.AppendLine("                             TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                            sbCodes.AppendLine("                             OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                            sbCodes.AppendLine("                             Filters: [");
                            sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                            sbCodes.AppendLine("                             ],");
                            sbCodes.AppendLine("                             Sorts: [");
                            if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                            {
                                sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                            }
                            sbCodes.AppendLine("                             ]");
                            sbCodes.AppendLine("                         }),");
                        }

                        sbCodes.AppendLine("                        open: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter({ field: '" + col.ColumnReferansFilterValueColumnName + "', operator: 'eq', value: options.model." + col.ColumnReferansFilterValueColumnName + ", ValueType: 'Int' });");
                        }
                        sbCodes.AppendLine("                        },");

                        sbCodes.AppendLine("                        close: function (e) {");
                        if (col.ColumnReferansFilterValueColumnName.MyToTrim().Length > 0)
                        {
                            sbCodes.AppendLine("                            this.dataSource.filter([]);");
                        }
                        sbCodes.AppendLine("                        }");

                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.Slider.ToString())
                    {
                        sbCodes.AppendLine("                     input.kendoSlider({");
                        sbCodes.AppendLine("                         min: 0,");
                        sbCodes.AppendLine("                         max: 100,");
                        sbCodes.AppendLine("                         smallStep: 10,");
                        sbCodes.AppendLine("                         largeStep: 25");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                    }
                    else if (col.GridComponentType == EnmComponentType.ExternalSearchEdit.ToString())
                    {
                        sbCodes.AppendLine("                     input.attr('type', 'hidden');");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     var tempInput = '';");
                        sbCodes.AppendLine("                     tempInput += '<span name=\"ExternalSearchButtonWrapper' + options.field + '\" class=\"mnSearchEdit k-textbox k-space-right w-100\">';");
                        sbCodes.AppendLine("                     tempInput += ' <input class=\"font-weight-light\" type=\"text\" data-bind=\"value:Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "\" data-langkey-placeholder=\"" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "\" disabled>';");
                        sbCodes.AppendLine("                     tempInput += ' <button name=\"btnClear\" class=\"btn btn-light fa fa-close\"></button>';");
                        sbCodes.AppendLine("                     tempInput += ' <button name=\"btnSearch\" class=\"btn btn-light fa fa-search\"></button>';");
                        sbCodes.AppendLine("                     tempInput += '</span>';");
                        sbCodes.AppendLine("                     $(tempInput).appendTo(container);");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     setTimeout(function () {");
                        sbCodes.AppendLine("                        $(container).find('[data-bind=\"value:Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "\"]').val(options.model.Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ");");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     $(container).find('[name=\"ExternalSearchButtonWrapper' + options.field + '\"]').click(function (e) {");
                        sbCodes.AppendLine("                        var dataItem = options.model;");
                        sbCodes.AppendLine("                        mn" + col.SearchShowType + "View.create({");
                        sbCodes.AppendLine("                            areaName: '" + col.SearchAreaName + "',");
                        sbCodes.AppendLine("                            viewFileName: '" + col.ColumnReferansTableName + "ForSearch',");
                        sbCodes.AppendLine("                            viewName: '" + col.ColumnReferansTableName + "ForSearch',");
                        sbCodes.AppendLine("                            subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),");
                        sbCodes.AppendLine("                            onShow: function (e) {");

                        sbCodes.AppendLine("                                e.beforeShow({");
                        sbCodes.AppendLine("                                    'ownerViewName':'" + viewName + "',");
                        if (!string.IsNullOrEmpty(col.SearchFilterColumnName) && !string.IsNullOrEmpty(col.SearchFilterOperator) && !string.IsNullOrEmpty(col.SearchFilterValue))
                        {
                            sbCodes.AppendLine("                                    'filters': [");
                            sbCodes.AppendLine("                                        { 'filterColumnName': '" + col.SearchFilterColumnName + "', 'filterOperator': '" + col.SearchFilterOperator + "', 'filterValue': " + col.SearchFilterValue + " }");
                            sbCodes.AppendLine("                                    ]");
                        }
                        sbCodes.AppendLine("                                });");

                        sbCodes.AppendLine("                           },");
                        sbCodes.AppendLine("                           onClose: function (e) {");
                        sbCodes.AppendLine("                               if (e.opt.isSelected) {");
                        sbCodes.AppendLine("                                   dataItem.set('" + col.ColumnName + "', e.opt.selectedDataItem." + col.ColumnReferansValueColumnName + ");");

                        string displayItemGetter = "";
                        foreach (var referansDisplayColumnName in col.ColumnReferansDisplayColumnNames.Split(","))
                        {
                            if (displayItemGetter.Length > 0)
                            {
                                displayItemGetter += "+ ' ' +";
                            }
                            displayItemGetter += "e.opt.selectedDataItem." + referansDisplayColumnName;
                        }

                        sbCodes.AppendLine("                                   dataItem.set('Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "', " + displayItemGetter + ");");
                        sbCodes.AppendLine("                               }");
                        sbCodes.AppendLine("                           }");
                        sbCodes.AppendLine("                        });");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     $(container).find('[name=\"ExternalSearchButtonWrapper' + options.field + '\"] [name=btnClear]').click(function (e) {");
                        sbCodes.AppendLine("                         var dataItem = options.model;");
                        sbCodes.AppendLine("                         e.stopPropagation();");
                        if (col.ColumnDefault.MyToStr() == "0")
                        {
                            sbCodes.AppendLine("                         dataItem.set('" + col.ColumnName + "', 0);");
                        }
                        else
                        {
                            sbCodes.AppendLine("                         dataItem.set('" + col.ColumnName + "', '');");
                        }
                        sbCodes.AppendLine("                         dataItem.set('Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "', '');");
                        sbCodes.AppendLine("                     });");
                    }
                    else if (col.GridComponentType == EnmComponentType.ImageSearchEdit.ToString())
                    {
                        sbCodes.AppendLine("                     input.attr('type', 'hidden');");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     var tempInput = '';");
                        sbCodes.AppendLine("                     tempInput += '<span name=\"ImageSearchEditWrapper' + options.field + '\" class=\"mnSearchEdit k-textbox k-space-right w-100\">';");
                        sbCodes.AppendLine("                     tempInput += ' <input class=\"font-weight-light\" type=\"text\" data-bind=\"value:" + col.ColumnName + "\" data-langkey-placeholder=\"" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "\" >';");
                        sbCodes.AppendLine("                     tempInput += ' <button name=\"btnClear\" class=\"btn btn-light fa fa-close\"></button>';");
                        sbCodes.AppendLine("                     tempInput += ' <button name=\"btnSearch\" class=\"btn btn-light fa fa-search\"></button>';");
                        sbCodes.AppendLine("                     tempInput += '</span>';");
                        sbCodes.AppendLine("                     $(tempInput).appendTo(container);");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     $(container).find('[name=\"ImageSearchEditWrapper' + options.field + '\"] [name=btnSearch]').click(function (e) {");
                        sbCodes.AppendLine("                        var dataItem = options.model;");
                        sbCodes.AppendLine("                        mn" + col.SearchShowType + "View.create({");
                        sbCodes.AppendLine("                           areaName: '_',");
                        sbCodes.AppendLine("                           viewFileName: 'viewGorseller',");
                        sbCodes.AppendLine("                           viewName: 'viewGorseller',");
                        sbCodes.AppendLine("                           subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),");
                        sbCodes.AppendLine("                           onShow: function (e) {");
                        sbCodes.AppendLine("                               e.beforeShow({ 'ownerViewName':'" + viewName + "' });");
                        sbCodes.AppendLine("                           },");
                        sbCodes.AppendLine("                           onClose: function (e) {");
                        sbCodes.AppendLine("                               if (e.opt.isSelected) {");
                        sbCodes.AppendLine("                                   dataItem.set('" + col.ColumnName + "', e.opt.selectedDataItem.FileUrl);");
                        sbCodes.AppendLine("                               }");
                        sbCodes.AppendLine("                           }");
                        sbCodes.AppendLine("                        });");
                        sbCodes.AppendLine("                     });");
                        sbCodes.AppendLine("");
                        sbCodes.AppendLine("                     $(container).find('[name=\"ImageSearchEditWrapper' + options.field + '\"] [name=btnClear]').click(function (e) {");
                        sbCodes.AppendLine("                         var dataItem = options.model;");
                        sbCodes.AppendLine("                         e.stopPropagation();");
                        sbCodes.AppendLine("                         dataItem.set('" + col.ColumnName + "', '');");
                        sbCodes.AppendLine("                     });");
                    }

                    sbCodes.AppendLine("                 },");
                }

                sbCodes.AppendLine("                 width: '" + col.GridComponentWidth + "px'");

                sbCodes.AppendLine("             },");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("             {}");
            sbCodes.AppendLine("             ],");
            sbCodes.AppendLine("             edit: function (e) {");

            //grid Seç Dön var ise
            if (_SearchView)
            {
                sbCodes.AppendLine("                 mnApi.controlEnable(e.container.find('#btnSecDon'), false);");
                sbCodes.AppendLine("");
            }

            if (!_SearchView)
            {
                //grid detay geçiş var ise
                foreach (var item in dataSourceDetails)
                {
                    sbCodes.AppendLine("                 mnApi.controlEnable(e.container.find('.k-grid-btnDetay_" + item.TableName + "'), false);");
                    sbCodes.AppendLine("");
                }
            }

            if (_oTableOptions.SearchViewCrudEditorType == "Popup" || _oTableOptions.GridViewCrudEditorType == "Page" || _oTableOptions.GridViewCrudEditorType == "Popup")
            {
                sbCodes.AppendLine("                 var model=e.model;");
                sbCodes.AppendLine("                 this.cancelRow();");
                sbCodes.AppendLine("                 mn" + _oTableOptions.GridViewCrudEditorType + "View.create({");
                sbCodes.AppendLine("                     areaName: '" + _oTableOptions.AreaName + "',");
                sbCodes.AppendLine("                     viewFileName: '" + _oTableOptions.TableName + "ForForm',");
                sbCodes.AppendLine("                     viewName: '" + _oTableOptions.TableName + "ForForm',");
                sbCodes.AppendLine("                     subTitle: mnLang.TranslateWithWord('xLng.DuzenlemeIslemleri'),");
                sbCodes.AppendLine("                     onShow: function (e) {");
                sbCodes.AppendLine("                        var qprms = { 'Id': model.Id };");
                if (string.IsNullOrEmpty(_oTableOptions.GridViewMasterColumnName))
                {
                    sbCodes.AppendLine("                         e.beforeShow({'ownerViewName':'" + viewName + "', 'qprms': qprms });");
                }
                else
                {
                    sbCodes.AppendLine("                         e.beforeShow({'ownerViewName':'" + viewName + "', 'qprms': qprms, 'filters':self.opt.filters});");
                }

                sbCodes.AppendLine("                     },");
                sbCodes.AppendLine("                     onClose: function (e) {");
                sbCodes.AppendLine("                         self.dataSource.read();");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                 });");
            }

            sbCodes.AppendLine("             },");
            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("             cancel: function (e) {");
                sbCodes.AppendLine("                 if (e.model.id !== null) {");
                sbCodes.AppendLine("                     e.sender.refresh(); //databound da renklendirme ve button set etme var ise ,cancelden sonra databound çalışmıyor(yetki,renk style ayarlanamıyor) bununla refresh ediyosun");
                sbCodes.AppendLine("                 }");
                sbCodes.AppendLine("             },");
            }

            sbCodes.AppendLine("             dataBound: function (e) {");
            sbCodes.AppendLine("                var data = e.sender.dataSource.data();");
            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("                 //yetki");
                sbCodes.AppendLine("                 self.YetkiUygula(data);");
            }
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                 // Language");
            sbCodes.AppendLine("                 mnLang.TranslateWithSelector(e.sender.wrapper);");
            sbCodes.AppendLine("");

            if (_SearchView)
            {
                sbCodes.AppendLine("                 //SearchView selector enable/disable");
                sbCodes.AppendLine("                 for (var i = 0; i < data.length; i++) {");
                sbCodes.AppendLine("                     var dataItem = data[i];");
                sbCodes.AppendLine("                     var bDisable = false;");
                sbCodes.AppendLine("");
                foreach (var item in SearchRequiredFileds)
                {
                    sbCodes.AppendLine("                     if (dataItem." + item.ColumnName + " === null || dataItem." + item.ColumnName + ".length === 0) {");
                    sbCodes.AppendLine("                        bDisable = true;");
                    sbCodes.AppendLine("                     }");
                    sbCodes.AppendLine("");
                }

                sbCodes.AppendLine("                     if (bDisable) {");
                sbCodes.AppendLine("                        var tr = e.sender.wrapper.find('[data-uid=' + dataItem.uid + ']');");
                sbCodes.AppendLine("                        mnApi.controlEnable(tr.find('#btnSecDon'), false);");
                sbCodes.AppendLine("                        tr.find('#btnSecDon').closest('td').attr('title', mnLang.TranslateWithWord('xLng.DoldurulmasiGerekenAlanlarVar'));");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                 }");
                sbCodes.AppendLine("");
            }


            sbCodes.AppendLine("                 //row style");
            if (_oTableOptions.GridViewRowStyleStatusColumnName.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("                 $.each(data, function (i, dataItem) {");
                sbCodes.AppendLine("                    if (dataItem." + _oTableOptions.GridViewRowStyleStatusColumnName + " === false) {");
                sbCodes.AppendLine("                        var tr = e.sender.wrapper.find('tr[data-uid=' + dataItem.uid + ']');");
                sbCodes.AppendLine("                        tr.css('color', 'silver');");
                sbCodes.AppendLine("                    }");
                sbCodes.AppendLine("                 });");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("             },");
            sbCodes.AppendLine("             dataSource: self.dataSource");
            sbCodes.AppendLine("         }).getKendoGrid();");
            sbCodes.AppendLine("");

            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("         self.grid.wrapper.find('#btnEkle').kendoButton({");
                sbCodes.AppendLine("             icon: 'plus',");
                sbCodes.AppendLine("             click: function () {");


                if (_oTableOptions.SearchViewCrudEditorType == "Popup" || _oTableOptions.GridViewCrudEditorType == "Page" || _oTableOptions.GridViewCrudEditorType == "Popup")
                {
                    sbCodes.AppendLine("                 mn" + _oTableOptions.GridViewCrudEditorType + "View.create({");
                    sbCodes.AppendLine("                     areaName: '" + _oTableOptions.AreaName + "',");
                    sbCodes.AppendLine("                     viewFileName: '" + _oTableOptions.TableName + "ForForm',");
                    sbCodes.AppendLine("                     viewName: '" + _oTableOptions.TableName + "ForForm',");
                    sbCodes.AppendLine("                     subTitle: mnLang.TranslateWithWord('xLng.DuzenlemeIslemleri'),");
                    sbCodes.AppendLine("                     onShow: function (e) {");

                    sbCodes.AppendLine("                        var qprms = { 'Id': null };");

                    if (string.IsNullOrEmpty(_oTableOptions.GridViewMasterColumnName))
                    {
                        sbCodes.AppendLine("                         e.beforeShow({'ownerViewName':'" + viewName + "', 'qprms': qprms });");
                    }
                    else
                    {
                        sbCodes.AppendLine("                         e.beforeShow({'ownerViewName':'" + viewName + "', 'qprms': qprms, 'filters':self.opt.filters});");
                    }

                    sbCodes.AppendLine("                     },");
                    sbCodes.AppendLine("                     onClose: function (e) {");
                    sbCodes.AppendLine("                         self.dataSource.read();");
                    sbCodes.AppendLine("                     }");
                    sbCodes.AppendLine("                 });");
                }
                else
                {
                    sbCodes.AppendLine("                 self.grid.addRow();");
                }

                sbCodes.AppendLine("             }");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("");
            }



            if (!_SearchView)
            {
                foreach (var item in dataSourceDetails)
                {
                    sbCodes.AppendLine("        self.grid.wrapper.on('click', '.k-grid-btnDetay_" + item.TableName + "', function (e) {");
                    sbCodes.AppendLine("            e.preventDefault();");
                    sbCodes.AppendLine("            var dataItem = self.grid.dataItem($(e.currentTarget).closest('tr'));");
                    sbCodes.AppendLine("            mn" + item.ShowType + "View.create({");
                    sbCodes.AppendLine("                areaName: '" + item.AreaName + "',");
                    sbCodes.AppendLine("                viewFileName: '" + item.TableName + "For" + item.ViewType + "',");
                    sbCodes.AppendLine("                viewName: '" + item.TableName + "For" + item.ViewType + "',");
                    sbCodes.AppendLine("                subTitle: mnLang.TranslateWithWord('xLng.DuzenlemeIslemleri'),");
                    sbCodes.AppendLine("                onShow: function (e) {");
                    sbCodes.AppendLine("                    e.beforeShow({");
                    sbCodes.AppendLine("                        'ownerViewName': '" + viewName + "',");
                    sbCodes.AppendLine("                        'filters': [");
                    if (!string.IsNullOrEmpty(item.FilterColumnName) && !string.IsNullOrEmpty(item.FilterOperator) && !string.IsNullOrEmpty(item.FilterValue))
                    {
                        sbCodes.AppendLine("                            { 'filterColumnName': '" + item.FilterColumnName + "', 'filterOperator': '" + item.FilterOperator + "', 'filterValue': " + item.FilterValue + " },");
                    }
                    sbCodes.AppendLine("                            { 'filterColumnName': '" + item.ColumnName + "', 'filterOperator': 'eq', 'filterValue': dataItem.Id }");
                    sbCodes.AppendLine("                        ]");
                    sbCodes.AppendLine("                    });");
                    sbCodes.AppendLine("                },");
                    sbCodes.AppendLine("                onClose: function (e) {");
                    sbCodes.AppendLine("                    self.dataSource.read();");
                    sbCodes.AppendLine("                }");
                    sbCodes.AppendLine("            });");
                    sbCodes.AppendLine("        });");
                    sbCodes.AppendLine("");
                }
            }

            if (_SearchView)
            {
                sbCodes.AppendLine("         self.grid.wrapper.on('click', '#btnSecDon', function (e) {");
                sbCodes.AppendLine("             e.preventDefault();");
                sbCodes.AppendLine("             self.opt.isSelected = true;");
                sbCodes.AppendLine("             var dataItem = self.grid.dataItem($(e.currentTarget).closest('tr'));");
                sbCodes.AppendLine("             self.opt.selectedDataItem = dataItem;");
                sbCodes.AppendLine("             self.close();");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("         self.grid.wrapper.find('#btnSaveAsExcel').kendoButton({");
            sbCodes.AppendLine("             icon: 'excel',");
            sbCodes.AppendLine("             click: function () {");
            sbCodes.AppendLine("                 kendo.ui.progress(self.grid.wrapper, true); //progress On");
            sbCodes.AppendLine("                 self.grid.saveAsExcel();");
            sbCodes.AppendLine("             }");
            sbCodes.AppendLine("         });");


            sbCodes.AppendLine("     }");
            sbCodes.AppendLine("");

            #endregion

            #region Create Find Elements
            if (findTabs.Any())
            {
                sbCodes.AppendLine("     function fCreateFindElements() {");
                sbCodes.AppendLine("         // tabstrip");
                sbCodes.AppendLine("         self.tabstrip = $(self.selector).find('#tabstrip').kendoTabStrip({");
                sbCodes.AppendLine("             animation: false, tabPosition: 'top'");
                sbCodes.AppendLine("         }).getKendoTabStrip();");
                sbCodes.AppendLine("         self.tabstrip.select(0);");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("         // filter buton");
                sbCodes.AppendLine("         $(self.selector).find('#btnAra').click(function (e) {");
                sbCodes.AppendLine("             self.filter();");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("         // filter tetikleyicileri");
                sbCodes.AppendLine("         $(self.selector).find('[data-find_option]').keydown(function (e) {");
                sbCodes.AppendLine("             if (e.which === 13) {");
                sbCodes.AppendLine("                 e.preventDefault();");
                sbCodes.AppendLine("                 self.filter();");
                sbCodes.AppendLine("             }");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("         // Filter elementleri");
                foreach (var findTab in findTabs)
                {
                    var tabFindField = findField.Where(c => c.GridFindTab == findTab.Key);
                    if (_SearchView)
                    {
                        tabFindField = findField.Where(c => c.SearchFindTab == findTab.Key);
                    }
                    foreach (var col in tabFindField)
                    {
                        Boolean findDefaultValueOk = false;
                        string findDefaultValue = col.GridFindDefaultValue.MyToTrim();
                        if (findDefaultValue.Length > 0)
                        {
                            findDefaultValueOk = true;
                            if (findDefaultValue == "Empty")
                            {
                                findDefaultValue = string.Empty;
                            }
                            else if (findDefaultValue == "LocalDateTime")
                            {
                                findDefaultValue = "mnApi.LocalDateTime()";
                            }
                            else if (findDefaultValue == "LocalDate")
                            {
                                findDefaultValue = "mnApi.LocalDate()";
                            }
                            else if (findDefaultValue == "LocalTime")
                            {
                                findDefaultValue = "mnApi.LocalTime()";
                            }
                            else if (findDefaultValue == "UserId")
                            {
                                findDefaultValue = "mnUser.Info.UserId";
                            }
                        }


                        if (col.FormComponentType == EnmComponentType.TextBox.ToString() || col.FormComponentType == EnmComponentType.PasswordTextBox.ToString() || col.FormComponentType == EnmComponentType.TextEditor.ToString())
                        {
                            //text box için kendo da gerek yok, k-textbox ile hallediliyor

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').val();");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.Checkbox.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDropDownList({");
                            sbCodes.AppendLine("             valuePrimitive: true,");
                            sbCodes.AppendLine("             optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                            sbCodes.AppendLine("             dataValueField: 'value',");
                            sbCodes.AppendLine("             dataTextField: 'text',");
                            sbCodes.AppendLine("             dataSource: mnLookup.list.dsAktifPasif");
                            sbCodes.AppendLine("         }).getKendoDropDownList();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }

                        }
                        else if (col.FormComponentType == EnmComponentType.NumericTextBox.ToString())
                        {
                            string decimals = "0";
                            if (!string.IsNullOrEmpty(col.FormComponentFormat))
                            {
                                decimals = col.FormComponentFormat.Substring(1);
                            }

                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoNumericTextBox({");
                            sbCodes.AppendLine("             format: '" + col.FormComponentFormat + "', decimals: " + decimals + ", min: 0, spinners: false, step: 0");
                            sbCodes.AppendLine("         }).getKendoNumericTextBox();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.DropDownList.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDropDownList({");
                            if (!string.IsNullOrEmpty(col.FormComponentFilterType))
                            {
                                sbCodes.AppendLine("             filter: '" + col.FormComponentFilterType + "',");
                            }
                            sbCodes.AppendLine("             valuePrimitive: true,");
                            sbCodes.AppendLine("             optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                            sbCodes.AppendLine("             dataValueField: 'value',");
                            sbCodes.AppendLine("             dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                 Filters: [");
                                sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                 ],");
                                sbCodes.AppendLine("                 Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                 ]");
                                sbCodes.AppendLine("             })");
                            }

                            sbCodes.AppendLine("         }).getKendoDropDownList();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.DatePicker.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDatePicker({");
                            sbCodes.AppendLine("            componentType: mnApp.kendoDatePiker_ComponentType,");
                            sbCodes.AppendLine("            dateInput: mnApp.kendoDatePiker_DateInput");
                            sbCodes.AppendLine("         }).getKendoDatePicker();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "][data-find_operator=gte]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.TimePicker.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoTimePicker({");
                            sbCodes.AppendLine("             componentType: mnApp.kendoTimePiker_ComponentType,");
                            sbCodes.AppendLine("             dateInput: mnApp.kendoTimePiker_DateInput");
                            sbCodes.AppendLine("         }).getKendoTimePicker();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "][data-find_operator=gte]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDateTimePicker({");
                            sbCodes.AppendLine("             componentType: mnApp.kendoDateTimePiker_ComponentType,");
                            sbCodes.AppendLine("             dateInput: mnApp.kendoDateTimePiker_DateInput,");
                            sbCodes.AppendLine("             close: function (e) {");
                            sbCodes.AppendLine("                 if ($(e.sender.wrapper).find('input').attr('data-find_operator') === 'lte') {");
                            sbCodes.AppendLine("                     var tarihSaat = this.value();");
                            sbCodes.AppendLine("                     if (tarihSaat !== null) {");
                            sbCodes.AppendLine("                         if (tarihSaat.getHours() === 0 || tarihSaat.getMinutes() === 0) {");
                            sbCodes.AppendLine("                             tarihSaat.setHours(23);");
                            sbCodes.AppendLine("                             tarihSaat.setMinutes(59);");
                            sbCodes.AppendLine("                         }");
                            sbCodes.AppendLine("                     }");
                            sbCodes.AppendLine("                     this.value(tarihSaat);");
                            sbCodes.AppendLine("                 }");
                            sbCodes.AppendLine("             }");
                            sbCodes.AppendLine("         }).getKendoDateTimePicker();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "][data-find_operator=gte]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.MaskedTextBox.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoMaskedTextBox({");
                            sbCodes.AppendLine("             mask: '" + col.FormComponentFormat + "' ");
                            sbCodes.AppendLine("         }).getKendoMaskedTextBox();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            sbCodes.AppendLine("         self.ms" + col.ColumnName + " = $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoMultiSelect({");
                            sbCodes.AppendLine("             valuePrimitive: true,");
                            sbCodes.AppendLine("             autoClose: false,");
                            sbCodes.AppendLine("             optionLabel: { value: '', text: mnLang.TranslateWithWord('xLng.Seciniz') },");
                            sbCodes.AppendLine("             dataValueField: 'value',");
                            sbCodes.AppendLine("             dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                 Filters: [");
                                sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                 ],");
                                sbCodes.AppendLine("                 Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                 ]");
                                sbCodes.AppendLine("             }),");
                            }

                            sbCodes.AppendLine("             change: function (e) {");
                            //sbCodes.AppendLine("                 //var values = this.value().join()");
                            sbCodes.AppendLine("             }");
                            sbCodes.AppendLine("         }).getKendoMultiSelect();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.ColorPicker.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoColorPicker({");
                            sbCodes.AppendLine("             buttons: true");
                            sbCodes.AppendLine("         }).getKendoColorPicker();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.AutoComplete.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoAutoComplete({");
                            sbCodes.AppendLine("             valuePrimitive: true,");
                            sbCodes.AppendLine("             dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                 Filters: [");
                                sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                 ],");
                                sbCodes.AppendLine("                 Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                 ]");
                                sbCodes.AppendLine("             })");
                            }

                            sbCodes.AppendLine("         }).getKendoAutoComplete();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.ComboBox.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoComboBox({");
                            sbCodes.AppendLine("             valuePrimitive: true,");
                            sbCodes.AppendLine("             dataValueField: 'value',");
                            sbCodes.AppendLine("             dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("             dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                 TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                 ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                 TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                 OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                 Filters: [");
                                sbCodes.AppendLine("                     { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                 ],");
                                sbCodes.AppendLine("                 Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                 ]");
                                sbCodes.AppendLine("             })");
                            }

                            sbCodes.AppendLine("         }).getKendoComboBox();");
                            sbCodes.AppendLine("");

                            //find field lokal set edilmesi
                            if (findDefaultValueOk)
                            {
                                sbCodes.AppendLine("         //find field lokal set edilmesi");
                                sbCodes.AppendLine("         kendo.widgetInstance($(self.selector).find('[data-find_field=" + col.ColumnName + "]')).value(" + findDefaultValue + ");");
                                sbCodes.AppendLine("");
                            }
                        }
                        else if (col.FormComponentType == EnmComponentType.ExternalSearchEdit.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[name=FindExternalSearchButtonWrapper" + col.ColumnName + "]').click(function (e) {");
                            sbCodes.AppendLine("             mn" + col.SearchShowType + "View.create({");
                            sbCodes.AppendLine("                areaName: '" + col.SearchAreaName + "',");
                            sbCodes.AppendLine("                viewFileName: '" + col.ColumnReferansTableName + "ForSearch',");
                            sbCodes.AppendLine("                viewName: '" + col.ColumnReferansTableName + "ForSearch',");
                            sbCodes.AppendLine("                subTitle: mnLang.TranslateWithWord('xLng.AramaIslemleri'),");
                            sbCodes.AppendLine("                onShow: function (e) {");
                            sbCodes.AppendLine("                    e.beforeShow({");
                            sbCodes.AppendLine("                        'ownerViewName':'" + viewName + "',");
                            if (!string.IsNullOrEmpty(col.SearchFilterColumnName) && !string.IsNullOrEmpty(col.SearchFilterOperator) && !string.IsNullOrEmpty(col.SearchFilterValue))
                            {
                                sbCodes.AppendLine("                        'filters': [");
                                sbCodes.AppendLine("                            { 'filterColumnName': '" + col.SearchFilterColumnName + "', 'filterOperator': '" + col.SearchFilterOperator + "', 'filterValue': " + col.SearchFilterValue + " }");
                                sbCodes.AppendLine("                        ]");
                            }
                            sbCodes.AppendLine("                    });");

                            sbCodes.AppendLine("                },");
                            sbCodes.AppendLine("                onClose: function (e) {");
                            sbCodes.AppendLine("                    var $valueElement = $(self.selector).find('[data-find_field=" + col.ColumnName + "]');");
                            sbCodes.AppendLine("                    var $displayElement = $(self.selector).find('[data-find_display_field=" + col.ColumnName + "]');");
                            sbCodes.AppendLine("                    if (e.opt.isSelected) {");
                            sbCodes.AppendLine("                        $valueElement.val(e.opt.selectedDataItem." + col.ColumnReferansValueColumnName + ");");

                            string displayItemGetter = "";
                            foreach (var referansDisplayColumnName in col.ColumnReferansDisplayColumnNames.Split(","))
                            {
                                if (displayItemGetter.Length > 0)
                                {
                                    displayItemGetter += "+ ' ' +";
                                }
                                displayItemGetter += "e.opt.selectedDataItem." + referansDisplayColumnName;
                            }
                            sbCodes.AppendLine("                        $displayElement.val(" + displayItemGetter + ");");
                            sbCodes.AppendLine("                    }");
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("             });");
                            sbCodes.AppendLine("         });");
                            sbCodes.AppendLine("");

                            sbCodes.AppendLine("         $(self.selector).find('[name=FindExternalSearchButtonWrapper" + col.ColumnName + "] [name=btnClear]').click(function (e) {");
                            sbCodes.AppendLine("             e.stopPropagation();");
                            if (col.ColumnDefault.MyToStr() == "0")
                            {
                                sbCodes.AppendLine("             $(self.selector).find('[data-find_field=" + col.ColumnName + "]').val('');");
                            }
                            else
                            {
                                sbCodes.AppendLine("             $(self.selector).find('[data-find_field=" + col.ColumnName + "]').val('');");
                            }
                            sbCodes.AppendLine("             $(self.selector).find('[data-find_display_field=" + col.ColumnName + "]').val('');");
                            sbCodes.AppendLine("         });");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.Slider.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoSlider({");
                            sbCodes.AppendLine("             min: 0,");
                            sbCodes.AppendLine("             max: 100,");
                            sbCodes.AppendLine("             smallStep: 10,");
                            sbCodes.AppendLine("             largeStep: 25");
                            sbCodes.AppendLine("         }).getKendoSlider();");
                            sbCodes.AppendLine("");
                        }
                    }
                }

                sbCodes.AppendLine("     }");
                sbCodes.AppendLine("");
            }
            #endregion

            #region Yetki Uygula
            if (_oTableOptions.GridViewCrudEditorType.MyToTrim().Length > 0 || _oTableOptions.SearchViewCrudEditorType.MyToTrim().Length > 0)
            {
                sbCodes.AppendLine("     self.YetkiUygula = function (_data) {");
                sbCodes.AppendLine("         //Standart Yetkiler");
                sbCodes.AppendLine("         var _C = mnUser.isYetkili(self.tableName + '.D_C.');");
                sbCodes.AppendLine("         var _U = mnUser.isYetkili(self.tableName + '.D_U.');");
                sbCodes.AppendLine("         var _D = mnUser.isYetkili(self.tableName + '.D_D.');");
                sbCodes.AppendLine("         var _E = mnUser.isYetkili(self.tableName + '.D_E.');");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("         //ek yetkiler için");
                sbCodes.AppendLine("         //...");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("        //form element görünümleri");
                sbCodes.AppendLine("        var fieldList = [];");
                foreach (var col in findField)
                {
                    sbCodes.AppendLine("        fieldList.push({ 'Name': '" + col.ColumnName + "', 'Visible': '" + col.GridColumnShowRoleGroupIds + "'.indexOf(mnUser.Info.nYetkiGrup) >= 0 });");
                }
                sbCodes.AppendLine("");
                sbCodes.AppendLine("        for (var i in fieldList) {");
                sbCodes.AppendLine("            var $elm = $(self.selector).find('.mnFindElementContainer .mnFindElementDiv[name=div' + fieldList[i].Name + ']');");
                sbCodes.AppendLine("            if (fieldList[i].Visible) {");
                sbCodes.AppendLine("                $elm.show();");
                sbCodes.AppendLine("            } else {");
                sbCodes.AppendLine("                $elm.hide();");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("         //grid ekle button için");
                sbCodes.AppendLine("         mnApi.controlShowHide(self.grid.wrapper.find('#btnEkle'), _C);");

                sbCodes.AppendLine("         //grid exel button için");
                sbCodes.AppendLine("         mnApi.controlShowHide(self.grid.wrapper.find('#btnSaveAsExcel'), _E);");

                sbCodes.AppendLine("");

                sbCodes.AppendLine("         //grid rows için");
                sbCodes.AppendLine("         $.each(_data, function (i, row) {");
                sbCodes.AppendLine("             var _tr = self.grid.wrapper.find('tr[data-uid=' + row.uid + ']');");
                sbCodes.AppendLine("             var tr_U = _U;");
                sbCodes.AppendLine("             var tr_D = _D;");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("             //update-delete button için");
                if (!_SearchView && _oTableOptions.GridViewCrudEditorType == "Inline")
                {
                    sbCodes.AppendLine("             mnApi.controlEnable(_tr.find('.k-grid-edit'), tr_U);");
                }

                if (_SearchView && _oTableOptions.SearchViewCrudEditorType == "Inline")
                {
                    sbCodes.AppendLine("             mnApi.controlEnable(_tr.find('.k-grid-edit'), tr_U);");
                }
                sbCodes.AppendLine("             mnApi.controlEnable(_tr.find('.k-grid-delete'), tr_D);");
                sbCodes.AppendLine("");

                if (dataSourceDetails.Any())
                {
                    sbCodes.AppendLine("             // ek yetkiler (row daki nesneler için)");
                    foreach (var item in dataSourceDetails)
                    {
                        string btnName = "btnDetay_" + item.TableName;
                        string btnName_R = btnName + "_R";
                        string btnName_Prefix_Yetki = item.TableName + ".D_R.";

                        sbCodes.AppendLine("             var " + btnName_R + " = mnUser.isYetkili('" + btnName_Prefix_Yetki + "');");
                        sbCodes.AppendLine("             mnApi.controlEnable(_tr.find('.k-grid-" + btnName + "'), " + btnName_R + ");");
                        sbCodes.AppendLine("");
                    }
                    sbCodes.AppendLine("");
                }


                sbCodes.AppendLine("");
                sbCodes.AppendLine("             //row değerine göre yetkiler");
                sbCodes.AppendLine("             //...");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("         });");
                sbCodes.AppendLine("     };");
                sbCodes.AppendLine("");
            }
            #endregion

            #region prepare

            sbCodes.AppendLine("     self.prepare = function () {");
            sbCodes.AppendLine("         // DataSource");
            sbCodes.AppendLine("         fCreateDataSource();");
            sbCodes.AppendLine("");

            if (findTabs.Any())
            {
                sbCodes.AppendLine("         // find Elementler");
                sbCodes.AppendLine("         fCreateFindElements();");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("         // Grid");
            sbCodes.AppendLine("         fCreateGrid();");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("         // Language");
            sbCodes.AppendLine("         mnLang.TranslateWithSelector(self.selector);");
            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion

            #region beforeShow

            sbCodes.AppendLine("     self.beforeShow = function (_opt) {");
            sbCodes.AppendLine("         self.opt = $.extend({}, _opt);");
            if (_SearchView)
            {
                sbCodes.AppendLine("         self.opt.isSelected = false;");
            }
            sbCodes.AppendLine("");

            sbCodes.AppendLine("         self.filter();");
            sbCodes.AppendLine("");
            if (findTabs.Any())
            {
                sbCodes.AppendLine("         self.tabstrip.select(0);");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion

            #region filter

            sbCodes.AppendLine("     self.filter = function (_data) {");
            sbCodes.AppendLine("         self.dataSource.filter(mnApp.find_options_ToFilterList(self));");
            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion                     

            #region Close

            sbCodes.AppendLine("     self.close = function () {");
            sbCodes.AppendLine("         if ($(self.selector).closest('.k-window-content').getKendoWindow()) {");
            sbCodes.AppendLine("             $(self.selector).closest('.k-window-content').getKendoWindow().close(); // popup ise");
            sbCodes.AppendLine("         } else {");
            sbCodes.AppendLine("             $(self.selector).closest('.mnPageView').find('#btnGeri').click(); // page ise");
            sbCodes.AppendLine("         }");
            sbCodes.AppendLine("     };");
            sbCodes.AppendLine("");

            #endregion

            sbCodes.AppendLine("     return self;");
            sbCodes.AppendLine(" }();");
            sbCodes.AppendLine("</script>");
            #endregion
            return sbCodes;
        }

        public StringBuilder TreeListGenerate(MyTableOption _oTableOptions, Boolean _SearchView)
        {
            StringBuilder sbCodes = new();
            var findTabs = _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridFindTab.MyToTrim().Length > 0).GroupBy(g => g.GridFindTab).OrderBy(o => o.Key);
            var findField = _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridFindTab.MyToTrim().Length > 0).OrderBy(o => o.GridOrder);

            var dataSourceFileds = _oTableOptions.Fields.Where(c => c.ColumnUse).OrderBy(o => o.ColumnOrder);
            var SearchRequiredFileds = _oTableOptions.Fields.Where(c => c.SearchRequired).OrderBy(o => o.ColumnOrder);

            var viewName = _oTableOptions.TreeListName;

            if (_SearchView)
            {
                viewName = _oTableOptions.SearchViewName;
                findTabs = _oTableOptions.Fields.Where(c => c.ColumnUse && c.SearchFindTab.MyToTrim().Length > 0).GroupBy(g => g.SearchFindTab).OrderBy(o => o.Key);
                findField = _oTableOptions.Fields.Where(c => c.ColumnUse && c.SearchFindTab.MyToTrim().Length > 0).OrderBy(o => o.SearchOrder);
            }

            #region html codes
            sbCodes.AppendLine("");
            sbCodes.AppendLine("<!-- Auto Generated " + Environment.UserName + " -->");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("<div id='" + viewName + "'>");

            if (findTabs.Any())
            {
                sbCodes.AppendLine("    <div>");
                sbCodes.AppendLine("        <div id='divButtonGroup' class='mnButtonGroup float-right' >");
                sbCodes.AppendLine("            <button id='btnAra' type='button' class='btn btn-outline-warning text-nowrap' data-langkey-title='xLng.Ara' style='position:absolute; right:0px;' > <i class='fa fa-binoculars'></i> <small data-langkey-text='xLng.Ara'></small> </button>");
                sbCodes.AppendLine("        </div>");
                sbCodes.AppendLine("    </div>");
            }

            sbCodes.AppendLine("    <div>");
            if (findTabs.Any())
            {
                sbCodes.AppendLine("        <div id='tabstrip'>");
                sbCodes.AppendLine("            <ul>");
                foreach (var findTab in findTabs)
                {
                    sbCodes.AppendLine("                <li> <span data-langkey-text='" + _oTableOptions.LangKeyRoot + ".SearchFindTab." + findTab.Key + "'></span> </li>");
                }
                sbCodes.AppendLine("            </ul>");
                sbCodes.AppendLine("");

                foreach (var findTab in findTabs)
                {
                    sbCodes.AppendLine("            <div style='padding:5px 0px;'>");
                    sbCodes.AppendLine("                <div class='form-inline mnFindElementContainer'>");

                    var tabFindField = findField.Where(c => c.GridFindTab == findTab.Key);
                    if (_SearchView)
                    {
                        tabFindField = findField.Where(c => c.SearchFindTab == findTab.Key);
                    }

                    foreach (var col in tabFindField)
                    {
                        string find_operator = "eq";
                        if (col.ColumnNetType == "System.String")
                        {
                            find_operator = "contains";
                        }

                        sbCodes.AppendLine("                    <label name='div" + col.ColumnName + "' class='mnFindElementDiv' >");

                        if (col.FormComponentType == EnmComponentType.TextBox.ToString() || col.FormComponentType == EnmComponentType.PasswordTextBox.ToString() || col.FormComponentType == EnmComponentType.TextEditor.ToString() || col.FormComponentType == EnmComponentType.TextArea.ToString())
                        {
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' class='k-textbox' style='width:" + col.GridComponentWidth + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "' autocomplete='off' />");
                        }
                        else if (col.FormComponentType == EnmComponentType.DatePicker.ToString() || col.FormComponentType == EnmComponentType.TimePicker.ToString() || col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                        {
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' class='mr-2' style='width:" + (col.GridComponentWidth + 80).ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='gte'/>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + (col.GridComponentWidth + 80).ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='lte'/>");
                        }
                        else if (col.FormComponentType == EnmComponentType.Checkbox.ToString())
                        {
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + (col.GridComponentWidth + 30).ToString() + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "'/>");
                        }
                        else
                        {
                            sbCodes.AppendLine("                        <label class='col-form-label mr-2' data-langkey-text='" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'></label>");
                            sbCodes.AppendLine("                        <input type='text' style='width:" + col.GridComponentWidth + "px' data-find_option='auto' data-find_type='" + col.ColumnNetType + "' data-find_field='" + col.ColumnName + "' data-find_operator='" + find_operator + "'/>");
                        }
                        sbCodes.AppendLine("                    </label>");

                        sbCodes.AppendLine("");
                    }
                    sbCodes.AppendLine("                </div>");
                    sbCodes.AppendLine("            </div>");
                    sbCodes.AppendLine("");
                }
                sbCodes.AppendLine("        </div>");
                sbCodes.AppendLine("");
            }
            sbCodes.AppendLine("        <div id='treeList' style='height:" + (findTabs.Any() ? "480" : "560") + "px;'></div>");
            sbCodes.AppendLine("    </div>");

            sbCodes.AppendLine("</div>");
            sbCodes.AppendLine("");

            #endregion

            #region script codes
            sbCodes.AppendLine("<script>");
            sbCodes.AppendLine("    window." + viewName + " = function () {");
            sbCodes.AppendLine("        var self = {};");
            sbCodes.AppendLine("        self.opt = null;");
            sbCodes.AppendLine("        self.area = '" + _oTableOptions.AreaName + "';");
            sbCodes.AppendLine("        self.title = '" + _oTableOptions.LangKeyRoot + ".Title" + "';");
            sbCodes.AppendLine("        self.selector = '#" + viewName + "';");
            sbCodes.AppendLine("        self.primaryKey = '" + _oTableOptions.PrimaryKey + "';");
            sbCodes.AppendLine("        self.tableName = '" + _oTableOptions.TableName + "';");
            sbCodes.AppendLine("        self.apiUrlPrefix = '/' + self.area + '/" + MyCodeGen.CodeGen_ControllerPrefix + "' + self.tableName;");
            sbCodes.AppendLine("");

            #region DataSource
            sbCodes.AppendLine("        function fCreateDataSource() {");
            sbCodes.AppendLine("            self.dataSource = new kendo.data.TreeListDataSource({");
            sbCodes.AppendLine("                transport: {");
            sbCodes.AppendLine("                    read: { type: 'GET', url: self.apiUrlPrefix + '/Read', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                    create: { type: 'POST', url: self.apiUrlPrefix + '/Create', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                    update: { type: 'POST', url: self.apiUrlPrefix + '/Update', dataType: 'json', contentType: 'application/json; charset=utf-8' },");
            sbCodes.AppendLine("                    destroy: { type: 'POST', url: self.apiUrlPrefix + '/Delete', dataType: 'json' },");
            sbCodes.AppendLine("                    parameterMap: function (data, operation) {");
            sbCodes.AppendLine("                        if (operation === 'read') {");
            sbCodes.AppendLine("                            return (new kendo.data.transports['aspnetmvc-ajax']({})).options.parameterMap(data);");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("                        else if (operation === 'create' || operation === 'update') {");
            sbCodes.AppendLine("                            if (data.Cc" + _oTableOptions.ParentColumnName + " > 0) {");
            sbCodes.AppendLine("                                data." + _oTableOptions.ParentColumnName + " = data.Cc" + _oTableOptions.ParentColumnName + ";");
            sbCodes.AppendLine("                            } else {");
            sbCodes.AppendLine("                                data." + _oTableOptions.ParentColumnName + " = 0;");
            sbCodes.AppendLine("                            }");

            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnDbType == "datetime")
                {
                    sbCodes.AppendLine("                            data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                }
                if (col.ColumnDbType == "date")
                {
                    sbCodes.AppendLine("                            data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 's');");
                }
                if (col.ColumnDbType == "time")
                {
                    sbCodes.AppendLine("                            data." + col.ColumnName + " = kendo.toString(data." + col.ColumnName + ", 't');");
                }
            }

            sbCodes.AppendLine("                            return kendo.stringify(data);");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("                        else if (operation === 'destroy') {");
            sbCodes.AppendLine("                            return { _id: data[self.primaryKey] };");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("                    }");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                schema: {");
            sbCodes.AppendLine("                    errors: 'Errors', data: 'Data', aggregates: 'AggregateResults',");
            sbCodes.AppendLine("                    model: {");
            sbCodes.AppendLine("                        id: self.primaryKey,");
            sbCodes.AppendLine("                        fields: {");

            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnName == _oTableOptions.PrimaryKey)
                {
                    sbCodes.AppendLine("                            " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "', defaultValue: null },");
                    sbCodes.AppendLine("                            parentId: { type: 'number', field: 'Cc" + _oTableOptions.ParentColumnName + "', nullable: true },");
                    sbCodes.AppendLine("                            hasChildren: { type: 'boolean', field: 'HasChildren' },");
                    //sabit computed tree için
                    sbCodes.AppendLine("                            Cc" + _oTableOptions.ParentColumnName + ": { type: 'number', nullable: true },");
                }
                else
                {
                    string editable = "";
                    if (!col.GridEditable)
                    {
                        editable = ", editable:false";
                    }
                    sbCodes.AppendLine("                            " + col.ColumnName + ": { type: '" + col.ColumnJsonType + "'" + editable + " },");
                }
            }

            // computed alanların tanımlanması
            foreach (var col in dataSourceFileds)
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                            Cc" + col.ColumnName + ": { type: 'string' },");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                            Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ": { type: 'string' },");
                }
                else if (col.FormComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString() || col.GridComponentType.MyToStr() == EnmComponentType.DownloadFileLink.ToString())
                {
                    sbCodes.AppendLine("                         Cc" + col.ColumnName + "Link: { type: 'string' },");
                }
            }

            sbCodes.Remove(sbCodes.Length - 3, 1);// son virgülü atmak için (sonundaki entera dokunmadan)

            sbCodes.AppendLine("                        },");
            sbCodes.AppendLine("                        expanded: false");
            sbCodes.AppendLine("                    }");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                error: function (e) {");
            sbCodes.AppendLine("                    if (e.xhr === null) { mnNotification.warning(e.errors); } else { mnErrorHandler.Handle(e.xhr); }");
            sbCodes.AppendLine("                    this.cancelChanges();");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                requestStart: function (e) {},");
            sbCodes.AppendLine("                requestEnd: function (e) {");
            sbCodes.AppendLine("                    if (e.response !== undefined && e.response.Errors === null) {");
            sbCodes.AppendLine("                        if (e.type === 'create' || e.type === 'update' || e.type === 'destroy') {");
            //sbCodes.AppendLine("                            self.isDirty = true;");
            sbCodes.AppendLine("                            mnLookup.listRead(self.tableName);");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                        if (e.type === 'create') {");
            sbCodes.AppendLine("                            mnNotification.success(mnLang.TranslateWithWord('xLng.KayitEklendi'));");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                        if (e.type === 'update') {");
            sbCodes.AppendLine("                            mnNotification.success(mnLang.TranslateWithWord('xLng.KayitDuzeltildi'));");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                        if (e.type === 'destroy') {");
            sbCodes.AppendLine("                            mnNotification.success(mnLang.TranslateWithWord('xLng.KayitSilindi'));");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("                    }");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                change: function (e) {");
            sbCodes.AppendLine("                    if (e.items[0] !== undefined) {");
            sbCodes.AppendLine("                        if (e.items[0].get(self.primaryKey) === null) {");
            sbCodes.AppendLine("                            $.ajax({");
            sbCodes.AppendLine("                                url: self.apiUrlPrefix + '/GetByNew', type: 'GET', dataType: 'json', async: false,");
            sbCodes.AppendLine("                                success: function (result) {");
            foreach (var col in dataSourceFileds)
            {
                if (col.ColumnNetType == "System.DateTime")
                {
                    sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = kendo.parseDate(result." + col.ColumnName + ");");
                }
                else
                {
                    sbCodes.AppendLine("                                 e.items[0]." + col.ColumnName + " = result." + col.ColumnName + ";");
                }
            }
            sbCodes.AppendLine("                                    e.items[0].Cc" + _oTableOptions.ParentColumnName + " = result.Cc" + _oTableOptions.ParentColumnName + ";");
            sbCodes.AppendLine("");

            sbCodes.AppendLine("                                 //computed alanların tanımlanması");
            foreach (var col in dataSourceFileds)
            {
                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + " = result.Cc" + col.ColumnName + ";");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                                 e.items[0].Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + " = result.Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + ";");
                }
            }

            sbCodes.AppendLine("                                  //filterdan gelen default value set için");
            sbCodes.AppendLine("                                     $(self.opt.filters).each(function (index, row) {");
            sbCodes.AppendLine("                                     if (row.filterColumnName !== 'Id') {");
            sbCodes.AppendLine("                                        e.items[0].set(row.filterColumnName, row.filterValue);");
            sbCodes.AppendLine("                                     }");
            sbCodes.AppendLine("                                   });");
            sbCodes.AppendLine("                                 },");
            sbCodes.AppendLine("                                error: function (xhr, status) {");
            sbCodes.AppendLine("                                     mnErrorHandler.Handle(xhr);");
            sbCodes.AppendLine("                                }");
            sbCodes.AppendLine("                            });");
            sbCodes.AppendLine("                        }");
            sbCodes.AppendLine("                    }");
            sbCodes.AppendLine("                    // ve gerekebilecek diğer işlemler");
            sbCodes.AppendLine("                    //...");
            sbCodes.AppendLine("                }");
            sbCodes.AppendLine("            });");
            sbCodes.AppendLine("        }");
            sbCodes.AppendLine("");
            #endregion

            #region TreeList
            sbCodes.AppendLine("        function fCreateTreeList() {");
            sbCodes.AppendLine("            self.treeList = $(self.selector).find('#treeList').kendoTreeList({");
            sbCodes.AppendLine("                autoBind: false, resizable: true, reorderable: true,");

            //crud var ise
            if (!string.IsNullOrEmpty(_oTableOptions.GridViewCrudEditorType))
            {
                sbCodes.AppendLine("                editable: {");
                sbCodes.AppendLine("                    mode: 'inline', create: true, update: true, destroy: true");
                sbCodes.AppendLine("                },");
                sbCodes.AppendLine("                toolbar: [{ name: 'create', text: mnLang.TranslateWithWord('xLng.Ekle') }],");
            }

            sbCodes.AppendLine("                columns: [");

            if (_SearchView)
            {
                sbCodes.AppendLine("                    {");
                sbCodes.AppendLine("                        locked: true,");
                sbCodes.AppendLine("                        width: '35px',");
                sbCodes.AppendLine("                        template: '<button id=\"btnSecDon\" class=\"btn btn-link btn-xs fa fa-download\" style=\"padding:0px;\" title=\"' + mnLang.TranslateWithWord(\"xLng.SecDon\") + '\"> </button>',");
                sbCodes.AppendLine("                        editor: function (container, options) {}");
                sbCodes.AppendLine("                    },");
            }

            foreach (var col in _oTableOptions.Fields.Where(c => c.ColumnUse && c.GridUse && c.ColumnName != _oTableOptions.PrimaryKey).OrderBy(o => o.GridOrder))
            {
                sbCodes.AppendLine("                    {");
                if (col.TreeExpandable)
                {
                    sbCodes.AppendLine("                        expandable: true,");
                }
                if (_SearchView && col.SearchHidden)
                {
                    sbCodes.AppendLine("                        hidden: true,");
                }
                else
                {
                    sbCodes.AppendLine("                 hidden: ![" + col.GridColumnShowRoleGroupIds + "].includes(mnUser.Info.nYetkiGrup),");
                }

                sbCodes.AppendLine("                        title: mnLang.TranslateWithWord('" + _oTableOptions.LangKeyRoot + "." + col.ColumnName + "'),");
                sbCodes.AppendLine("                        field: '" + col.ColumnName + "',");

                if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                {
                    sbCodes.AppendLine("                        template: '#:Cc" + col.ColumnName + "#',");
                }
                else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                {
                    sbCodes.AppendLine("                        template: '#:Cc" + col.ColumnName + col.ColumnReferansDisplayColumnNames.Replace(",", "") + "#',");
                }

                // grid crud var ise ve format belirlenmiş ise
                if (!string.IsNullOrEmpty(_oTableOptions.GridViewCrudEditorType) && !string.IsNullOrEmpty(col.GridComponentFormat))
                {
                    sbCodes.AppendLine("                        format: '" + col.GridComponentFormat + "',");
                }

                sbCodes.AppendLine("                        width: '" + col.GridComponentWidth + "px'");

                sbCodes.AppendLine("                    },");
            }

            //crud var ise
            if (!string.IsNullOrEmpty(_oTableOptions.GridViewCrudEditorType))
            {
                sbCodes.AppendLine("                    {");
                sbCodes.AppendLine("                        title: '', width: '170px',");
                sbCodes.AppendLine("                        command: [");
                sbCodes.AppendLine("                            { name: 'edit', buttonType: 'ImageAndText', text: ' ', attr: 'style=\"width:32px; padding-right:0px; margin-right:3px;\" data-langkey-title=\"xLng.Duzelt\" ' },");
                sbCodes.AppendLine("                            { name: 'destroy', buttonType: 'ImageAndText', text: ' ', attr: 'style=\"width:32px; padding-right:0px; margin-right:3px;\" data-langkey-title=\"xLng.Sil\"' },");
                sbCodes.AppendLine("                            { name: 'createchild', buttonType: 'ImageAndText', text: ' ', attr: 'style=\"width:32px; padding-right:0px; margin-right:3px;\" data-langkey-title=\"xLng.Ekle\"' }");
                sbCodes.AppendLine("                        ]");
                sbCodes.AppendLine("                    },");
            }
            sbCodes.AppendLine("                    {} //sona boş field");
            sbCodes.AppendLine("                ],");
            sbCodes.AppendLine("                messages: {");
            sbCodes.AppendLine("                    commands: {edit: ' ',update: ' ', canceledit: ' ', createchild: ' ', destroy: ' ', excel: 'Excel', pdf: 'PDF'}");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                selectable: 'row',");
            sbCodes.AppendLine("                change: function (e) {");
            sbCodes.AppendLine("                    var selectedRows = this.select();");
            sbCodes.AppendLine("                    var selectedDataItems = []; // selectedDataItems contains all selected data items");
            sbCodes.AppendLine("                    for (var i = 0; i < selectedRows.length; i++) {");
            sbCodes.AppendLine("                        var dataItem = this.dataItem(selectedRows[i]);");
            sbCodes.AppendLine("                        selectedDataItems.push(dataItem);");
            sbCodes.AppendLine("                    }");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                cancel: function (e) {");
            sbCodes.AppendLine("                    e.sender.refresh();//sender:tree dir, databound da renklendirme ve button set etme var ise , gridde editedilebilir ise cancelden sonra rengi koruyamıyor bununla refresh ediyosun");
            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                dataBound: function (e) {");
            sbCodes.AppendLine("                    var data = e.sender.dataSource.data();");
            sbCodes.AppendLine("                    $.each(data, function (i, row) {");
            sbCodes.AppendLine("                        var _tr = e.sender.wrapper.find('tr[data-uid=' + row.uid + ']');");
            sbCodes.AppendLine("");
            sbCodes.AppendLine("                        //enable disable button");
            sbCodes.AppendLine("                        //mnApi.controlEnable(_tr.find('.k-grid-add'), false);");
            sbCodes.AppendLine("                    });");
            if (!_SearchView)
            {
                sbCodes.AppendLine("                    //yetki");
                sbCodes.AppendLine("                    self.YetkiUygula(data);");
            }

            if (_SearchView)
            {
                sbCodes.AppendLine("                 //SearchView selector enable/disable");
                sbCodes.AppendLine("                 for (var i = 0; i < data.length; i++) {");
                sbCodes.AppendLine("                     var dataItem = data[i];");
                sbCodes.AppendLine("                     var bDisable = false;");
                sbCodes.AppendLine("");
                foreach (var item in SearchRequiredFileds)
                {
                    sbCodes.AppendLine("                     if (dataItem." + item.ColumnName + " === null || dataItem." + item.ColumnName + ".length === 0) {");
                    sbCodes.AppendLine("                        bDisable = true;");
                    sbCodes.AppendLine("                     }");
                    sbCodes.AppendLine("");
                }

                sbCodes.AppendLine("                     if (bDisable) {");
                sbCodes.AppendLine("                        var tr = e.sender.wrapper.find('[data-uid=' + dataItem.uid + ']');");
                sbCodes.AppendLine("                        mnApi.controlEnable(tr.find('#btnSecDon'), false);");
                sbCodes.AppendLine("                        tr.find('#btnSecDon').closest('td').attr('title', mnLang.TranslateWithWord('xLng.DoldurulmasiGerekenAlanlarVar'));");
                sbCodes.AppendLine("                     }");
                sbCodes.AppendLine("                 }");
            }

            sbCodes.AppendLine("                },");
            sbCodes.AppendLine("                dataSource: self.dataSource");
            sbCodes.AppendLine("            }).getKendoTreeList();");
            sbCodes.AppendLine("");

            if (_SearchView)
            {
                sbCodes.AppendLine("            self.treeList.wrapper.on('click', '#btnSecDon', function (e) {");
                sbCodes.AppendLine("               e.preventDefault();");
                sbCodes.AppendLine("               self.opt.isSelected = true;");
                sbCodes.AppendLine("               var dataItem = self.treeList.dataItem($(e.currentTarget).closest('tr'));");
                sbCodes.AppendLine("               self.opt.selectedDataItem = dataItem;");
                sbCodes.AppendLine("               self.close();");
                sbCodes.AppendLine("            });");
                sbCodes.AppendLine("");
            }

            sbCodes.AppendLine("        };");
            sbCodes.AppendLine("");
            #endregion

            #region Create Find Elements
            if (findTabs.Any())
            {
                sbCodes.AppendLine("        function fCreateFindElements() {");
                sbCodes.AppendLine("            // tabstrip");
                sbCodes.AppendLine("            self.tabstrip = $(self.selector).find('#tabstrip').kendoTabStrip({");
                sbCodes.AppendLine("                animation: false, tabPosition: 'top'");
                sbCodes.AppendLine("            }).getKendoTabStrip();");
                sbCodes.AppendLine("            self.tabstrip.select(0);");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            // filter buton");
                sbCodes.AppendLine("            $(self.selector).find('#btnAra').click(function (e) {");
                sbCodes.AppendLine("                self.filter();");
                sbCodes.AppendLine("            });");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            // filter tetikleyicileri");
                sbCodes.AppendLine("            $(self.selector).find('[data-find_option]').keydown(function (e) {");
                sbCodes.AppendLine("                if (e.which === 13) {");
                sbCodes.AppendLine("                    e.preventDefault();");
                sbCodes.AppendLine("                    self.filter();");
                sbCodes.AppendLine("                }");
                sbCodes.AppendLine("            });");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            // Filter elementleri");
                foreach (var findTab in findTabs)
                {
                    var tabFindField = findField.Where(c => c.GridFindTab == findTab.Key);
                    if (_SearchView)
                    {
                        tabFindField = findField.Where(c => c.SearchFindTab == findTab.Key);
                    }

                    foreach (var col in tabFindField)
                    {
                        if (col.FormComponentType == EnmComponentType.TextBox.ToString() || col.FormComponentType == EnmComponentType.PasswordTextBox.ToString() || col.FormComponentType == EnmComponentType.TextEditor.ToString())
                        {
                            //text box için kendo da gerek yok, k-textbox ile hallediliyor
                        }
                        else if (col.FormComponentType == EnmComponentType.Checkbox.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDropDownList({");
                            sbCodes.AppendLine("                valuePrimitive: true,");
                            sbCodes.AppendLine("                optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                            sbCodes.AppendLine("                dataValueField: 'value',");
                            sbCodes.AppendLine("                dataTextField: 'text',");
                            sbCodes.AppendLine("                dataSource: mnLookup.list.dsAktifPasif");
                            sbCodes.AppendLine("            }).getKendoDropDownList();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.NumericTextBox.ToString())
                        {
                            string decimals = "0";
                            if (!string.IsNullOrEmpty(col.FormComponentFormat))
                            {
                                decimals = col.FormComponentFormat.Substring(1);
                            }

                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoNumericTextBox({");
                            sbCodes.AppendLine("                format: '" + col.FormComponentFormat + "', decimals: " + decimals + ", min: 0, spinners: false, step: 0");
                            sbCodes.AppendLine("            }).getKendoNumericTextBox();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.DropDownList.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDropDownList({");
                            if (!string.IsNullOrEmpty(col.FormComponentFilterType))
                            {
                                sbCodes.AppendLine("                filter: '" + col.FormComponentFilterType + "',");
                            }
                            sbCodes.AppendLine("                valuePrimitive: true,");
                            sbCodes.AppendLine("                optionLabel: mnLang.TranslateWithWord('xLng.Seciniz'),");
                            sbCodes.AppendLine("                dataValueField: 'value',");
                            sbCodes.AppendLine("                dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                    TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                    ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                    TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                    OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                    Filters: [");
                                sbCodes.AppendLine("                        { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                    ],");
                                sbCodes.AppendLine("                    Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                    ]");
                                sbCodes.AppendLine("                })");
                            }

                            sbCodes.AppendLine("            }).getKendoDropDownList();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.DatePicker.ToString())
                        {
                            sbCodes.AppendLine("         $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDatePicker({");
                            sbCodes.AppendLine("            componentType: mnApp.kendoDatePiker_ComponentType,");
                            sbCodes.AppendLine("            dateInput: mnApp.kendoDatePiker_DateInput");
                            sbCodes.AppendLine("         }).getKendoDatePicker();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.TimePicker.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoTimePicker({");
                            sbCodes.AppendLine("                componentType: mnApp.kendoTimePiker_ComponentType,");
                            sbCodes.AppendLine("                dateInput: mnApp.kendoTimePiker_DateInput");
                            sbCodes.AppendLine("            }).getKendoTimePicker();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.DateTimePicker.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoDateTimePicker({");
                            sbCodes.AppendLine("                componentType: mnApp.kendoDateTimePiker_ComponentType,");
                            sbCodes.AppendLine("                dateInput: mnApp.kendoDateTimePiker_DateInput,");
                            sbCodes.AppendLine("                close: function (e) {");
                            sbCodes.AppendLine("                    if ($(e.sender.wrapper).find('input').attr('data-find_operator') === 'lte') {");
                            sbCodes.AppendLine("                        var tarihSaat = this.value();");
                            sbCodes.AppendLine("                        if (tarihSaat !== null) {");
                            sbCodes.AppendLine("                            if (tarihSaat.getHours() === 0 || tarihSaat.getMinutes() === 0) {");
                            sbCodes.AppendLine("                                tarihSaat.setHours(23);");
                            sbCodes.AppendLine("                                tarihSaat.setMinutes(59);");
                            sbCodes.AppendLine("                            }");
                            sbCodes.AppendLine("                        }");
                            sbCodes.AppendLine("                        this.value(tarihSaat);");
                            sbCodes.AppendLine("                    }");
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("            }).getKendoDateTimePicker();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.MaskedTextBox.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoMaskedTextBox({");
                            sbCodes.AppendLine("                mask: '" + col.FormComponentFormat + "' ");
                            sbCodes.AppendLine("            }).getKendoMaskedTextBox();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.MultiSelect.ToString())
                        {
                            sbCodes.AppendLine("            self.ms" + col.ColumnName + " = $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoMultiSelect({");
                            sbCodes.AppendLine("                valuePrimitive: true,");
                            sbCodes.AppendLine("                autoClose: false,");
                            sbCodes.AppendLine("                optionLabel: { value: '', text: mnLang.TranslateWithWord('xLng.Seciniz') },");
                            sbCodes.AppendLine("                dataValueField: 'value',");
                            sbCodes.AppendLine("                dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource + ",");
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                    TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                    ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                    TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                    OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                    Filters: [");
                                sbCodes.AppendLine("                        { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                    ],");
                                sbCodes.AppendLine("                    Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                    ]");
                                sbCodes.AppendLine("                }),");
                            }

                            sbCodes.AppendLine("                change: function (e) {");
                            sbCodes.AppendLine("                    if (self.dataSource.at(0) !== undefined) {");
                            sbCodes.AppendLine("                        self.dataSource.at(0).set('" + col.ColumnName + "', this.value().join());");
                            sbCodes.AppendLine("                    }");
                            sbCodes.AppendLine("                }");
                            sbCodes.AppendLine("            }).getKendoMultiSelect();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.ColorPicker.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoColorPicker({");
                            sbCodes.AppendLine("                buttons: true");
                            sbCodes.AppendLine("            }).getKendoColorPicker();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.AutoComplete.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoAutoComplete({");
                            sbCodes.AppendLine("                valuePrimitive: true,");
                            sbCodes.AppendLine("                dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                    TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                    ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                    TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                    OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                    Filters: [");
                                sbCodes.AppendLine("                        { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                    ],");
                                sbCodes.AppendLine("                    Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                    ]");
                                sbCodes.AppendLine("                })");
                            }

                            sbCodes.AppendLine("            }).getKendoAutoComplete();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.ComboBox.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoComboBox({");
                            sbCodes.AppendLine("                valuePrimitive: true,");
                            sbCodes.AppendLine("                dataValueField: 'value',");
                            sbCodes.AppendLine("                dataTextField: 'text',");

                            if (!string.IsNullOrEmpty(col.ColumnReferansJsonDataSource))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.list." + col.ColumnReferansJsonDataSource);
                            }
                            else if (!string.IsNullOrEmpty(col.ColumnReferansTableName) && !string.IsNullOrEmpty(col.ColumnReferansValueColumnName) && !string.IsNullOrEmpty(col.ColumnReferansDisplayColumnNames))
                            {
                                sbCodes.AppendLine("                dataSource: mnLookup.listLoad({");
                                sbCodes.AppendLine("                    TableName: '" + col.ColumnReferansTableName + "',");
                                sbCodes.AppendLine("                    ValueField: '" + col.ColumnReferansValueColumnName + "',");
                                sbCodes.AppendLine("                    TextField: '" + col.ColumnReferansDisplayColumnNames + "',");
                                sbCodes.AppendLine("                    OtherFields: '" + col.ColumnReferansFilterValueColumnName + "',");
                                sbCodes.AppendLine("                    Filters: [");
                                sbCodes.AppendLine("                        { Field: '" + col.ColumnReferansValueColumnName + "', Operator: '>=', Value: '0', ValueType: 'Int' }");
                                sbCodes.AppendLine("                    ],");
                                sbCodes.AppendLine("                    Sorts: [");
                                if (!string.IsNullOrEmpty(col.ColumnReferansSortColumnNames))
                                {
                                    sbCodes.AppendLine("                                 { Field: '" + col.ColumnReferansSortColumnNames + "', Dir: 'asc' }");
                                }
                                sbCodes.AppendLine("                    ]");
                                sbCodes.AppendLine("                })");
                            }

                            sbCodes.AppendLine("            }).getKendoComboBox();");
                            sbCodes.AppendLine("");
                        }
                        else if (col.FormComponentType == EnmComponentType.Slider.ToString())
                        {
                            sbCodes.AppendLine("            $(self.selector).find('[data-find_field=" + col.ColumnName + "]').kendoSlider({");
                            sbCodes.AppendLine("                min: 0,");
                            sbCodes.AppendLine("                max: 100,");
                            sbCodes.AppendLine("                smallStep: 10,");
                            sbCodes.AppendLine("                largeStep: 25");
                            sbCodes.AppendLine("            }).getKendoSlider();");
                            sbCodes.AppendLine("");
                        }
                    }
                }

                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");
            }
            #endregion

            #region Yetki Uygula
            if (!_SearchView)
            {
                sbCodes.AppendLine("        self.YetkiUygula = function (_data) {");
                sbCodes.AppendLine("            //Standart Yetkiler");
                sbCodes.AppendLine("            var _C = mnUser.isYetkili(self.tableName + '.D_C.');");
                sbCodes.AppendLine("            var _U = mnUser.isYetkili(self.tableName + '.D_U.');");
                sbCodes.AppendLine("            var _D = mnUser.isYetkili(self.tableName + '.D_D.');");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            //ek yetkiler için");
                sbCodes.AppendLine("            //...");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("        //form element görünümleri");
                sbCodes.AppendLine("        var fieldList = [];");
                foreach (var col in findField)
                {
                    sbCodes.AppendLine("        fieldList.push({ 'Name': '" + col.ColumnName + "', 'Visible': '" + col.GridColumnShowRoleGroupIds + "'.indexOf(mnUser.Info.nYetkiGrup) >= 0 });");
                }
                sbCodes.AppendLine("");
                sbCodes.AppendLine("        for (var i in fieldList) {");
                sbCodes.AppendLine("            var $elm = $(self.selector).find('.mnFindElementContainer .mnFindElementDiv[name=div' + fieldList[i].Name + ']');");
                sbCodes.AppendLine("            if (fieldList[i].Visible) {");
                sbCodes.AppendLine("                $elm.show();");
                sbCodes.AppendLine("            } else {");
                sbCodes.AppendLine("                $elm.hide();");
                sbCodes.AppendLine("            }");
                sbCodes.AppendLine("        }");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            //treeList ekle button için");
                sbCodes.AppendLine("            mnApi.controlShowHide(self.treeList.wrapper.find('#btnEkle'), _C);");
                sbCodes.AppendLine("");

                sbCodes.AppendLine("            //treeList rows için");
                sbCodes.AppendLine("            $.each(_data, function (i, row) {");
                sbCodes.AppendLine("                var _tr = self.treeList.wrapper.find('tr[data-uid=' + row.uid + ']');");
                sbCodes.AppendLine("                var tr_U = _U;");
                sbCodes.AppendLine("                var tr_D = _D;");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("                //update-delete button için");
                if (!_SearchView && _oTableOptions.GridViewCrudEditorType == "Inline")
                {
                    sbCodes.AppendLine("             mnApi.controlEnable(_tr.find('.k-grid-edit'), tr_U);");
                }

                if (_SearchView && _oTableOptions.SearchViewCrudEditorType == "Inline")
                {
                    sbCodes.AppendLine("                mnApi.controlEnable(_tr.find('.k-grid-edit'), tr_U);");
                }

                sbCodes.AppendLine("                mnApi.controlEnable(_tr.find('.k-tree-delete'), tr_D);");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("                // ek yetkiler (row daki nesneler için)");
                sbCodes.AppendLine("                //...");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("                //row değerine göre yetkiler");
                sbCodes.AppendLine("                //...");
                sbCodes.AppendLine("");
                sbCodes.AppendLine("            });");
                sbCodes.AppendLine("        };");
                sbCodes.AppendLine("");
            }
            #endregion

            #region prepare
            sbCodes.AppendLine("        self.prepare = function () {");
            sbCodes.AppendLine("            // DataSource");
            sbCodes.AppendLine("            fCreateDataSource();");
            sbCodes.AppendLine("");
            if (findTabs.Any())
            {
                sbCodes.AppendLine("            // find Elementler");
                sbCodes.AppendLine("            fCreateFindElements();");
                sbCodes.AppendLine("");
            }
            sbCodes.AppendLine("            //View");
            sbCodes.AppendLine("            fCreateTreeList();");
            sbCodes.AppendLine("        };");
            sbCodes.AppendLine("");
            #endregion

            #region beforeShow
            sbCodes.AppendLine("        self.beforeShow = function (_opt) {");
            sbCodes.AppendLine("            self.opt = $.extend({}, _opt);");
            if (_SearchView)
            {
                sbCodes.AppendLine("         self.opt.isSelected = false;");
            }
            sbCodes.AppendLine("");
            sbCodes.AppendLine("            self.filter();");
            sbCodes.AppendLine("");
            if (findTabs.Any())
            {
                sbCodes.AppendLine("            self.tabstrip.select(0);");
                sbCodes.AppendLine("");
            }
            sbCodes.AppendLine("");
            sbCodes.AppendLine("        };");
            sbCodes.AppendLine("");
            #endregion

            #region filter

            sbCodes.AppendLine("        self.filter = function (_data) {");
            sbCodes.AppendLine("            self.dataSource.filter(mnApp.find_options_ToFilterList(self));");
            sbCodes.AppendLine("        };");
            sbCodes.AppendLine("");

            #endregion

            #region Close

            sbCodes.AppendLine("        self.close = function () {");
            sbCodes.AppendLine("            if ($(self.selector).closest('.k-window-content').getKendoWindow()) {");
            sbCodes.AppendLine("                $(self.selector).closest('.k-window-content').getKendoWindow().close(); // popup ise");
            sbCodes.AppendLine("            } else {");
            sbCodes.AppendLine("                $(self.selector).closest('.mnPageView').find('#btnGeri').click(); // page ise");
            sbCodes.AppendLine("            }");
            sbCodes.AppendLine("        };");
            sbCodes.AppendLine("");

            #endregion

            sbCodes.AppendLine("        return self;");
            sbCodes.AppendLine("    }();");
            sbCodes.AppendLine("</script>");
            #endregion
            return sbCodes;
        }

        #endregion

    }

#pragma warning restore IDE0057
}
