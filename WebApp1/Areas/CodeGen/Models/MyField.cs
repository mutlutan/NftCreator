﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp1.Areas.CodeGen.Models
{
    public class MyField
    {
        public string ColumnStatus { get; set; } = "Yeni"; //Yeni,Mevcut,Sil

        public int ColumnOrder { get; set; } = 0;
        public string ColumnName { get; set; } = "";
        public Boolean ColumnUse { get; set; } = false;
        public Boolean ColumnRequired { get; set; } = false;
        public string ColumnDictionaryTr { get; set; } = "";
        public string ColumnDictionaryEn { get; set; } = "";

        public string ColumnDbType { get; set; } = "";
        public string ColumnNetType { get; set; } = "";
        public string ColumnJsonType { get; set; } = "";
        public string ColumnDefault { get; set; } = ""; //ne yazılır ise onu direk koyacak, string birşey yazacaksan 'birşey' ile yazılacak, c# koduda yazılabilir, bazı hazır action larda olabilir
        public string ColumnReadConvertValue { get; set; } = ""; //alan değeri okunurken değişim gerekli yerlerde action lar kullanılır
        public string ColumnWriteConvertValue { get; set; } = ""; //alan değeri yazılırken değişim gerekli yerlerde action lar kullanılır
        public string ColumnReferansTableName { get; set; } = "";
        public string ColumnReferansValueColumnName { get; set; } = "";
        public string ColumnReferansDisplayColumnNames { get; set; } = "";//ilk seçilen column display, diğerleri sadece read edilecek, data olarak kullanılma ihtimali olanlar
        public string ColumnReferansSortColumnNames { get; set; } = "";//seçilenler sort edilecek 
        public Boolean ColumnReferansDisplayColumnRead { get; set; } = false;
        public string ColumnReferansJsonDataSource { get; set; } = ""; //bu lookup.jd deki datasourceların listesi

        public string ColumnReferansFilterValueColumnName { get; set; } = ""; // Seçilen column un satırdaki değeri, lookup referans datasource da filter olarak kullanılır

        public Boolean FormUse { get; set; } = false;
        public int FormOrder { get; set; } = 0;
        public Boolean FormEditable { get; set; } = false;
        public Boolean FormReadonly { get; set; } = false;
        public Boolean FormLabelLocationTop { get; set; } = false;

        public string FormViewTab { get; set; } = "";
        public string FormViewLocation { get; set; } = "";
        public string FormComponentType { get; set; } = "";
        public string FormComponentFormat { get; set; } = "";
        public string FormComponentFilterType { get; set; } = "";
        public int FormComponentHeight { get; set; } = 38;
        public string FormResetPasswordMailColumnName { get; set; } = "";
        public string FormVisibilityColumnName { get; set; } = ""; /*kendisinin görünür olması için, hangi fielda bakılacak*/
        public string FormVisibilityColumnValue { get; set; } = ""; /*kendisinin görünür olması için, hangi değere bakılacak*/
        public string FormColumnShowRoleGroupIds { get; set; } = "10,11,12";

        public Boolean GridUse { get; set; } = false;
        public int GridOrder { get; set; } = 0;
        public Boolean GridEditable { get; set; } = false;
        public Boolean GridLocked { get; set; } = false;
        public Boolean GridEncoded { get; set; } = true;
        public Boolean GridTextNowrap { get; set; } = false;

        public string GridFindTab { get; set; } = "";
        public string GridFindDefaultValue { get; set; } = "";

        public string GridDataSourceSortDir { get; set; } = "";
        public string GridComponentType { get; set; } = "";
        public string GridComponentFormat { get; set; } = "";
        public string GridComponentFilterType { get; set; } = "";
        public int GridComponentWidth { get; set; } = 0;
        public string GridColumnShowRoleGroupIds { get; set; } = "10,11,12";

        public Boolean TreeExpandable { get; set; } = false;

        public Boolean SearchUse { get; set; } = false;
        public Boolean SearchRequired { get; set; } = false;
        public Boolean SearchHidden { get; set; } = false;
        public Boolean SearchLocked { get; set; } = false;
        public Boolean SearchEncoded { get; set; } = true;
        public string SearchFindTab { get; set; } = "";
        public int SearchOrder { get; set; } = 0;

        // 
        public string SearchShowType { get; set; } = ""; //Page, Popup
        public string SearchAreaName { get; set; } = "";

        //ekstra filter yapmak için manuel yazılacak
        public string SearchFilterColumnName { get; set; } = "";
        public string SearchFilterOperator { get; set; } = "";
        public string SearchFilterValue { get; set; } = "";
    }



}
