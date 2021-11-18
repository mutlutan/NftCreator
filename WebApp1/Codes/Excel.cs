
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp1.Codes
{
    public class MyExcel
    {
        public static List<Dictionary<string, object>> StreamToDictionaryList(Stream stream)
        {
            var data = new List<Dictionary<string, object>>();
            NPOI.SS.UserModel.ISheet sheet;

            //// ".xls"
            //NPOI.HSSF.UserModel.HSSFWorkbook hssfwb = new NPOI.HSSF.UserModel.HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
            //sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  

            // ".xlsx"
            NPOI.XSSF.UserModel.XSSFWorkbook hssfwb = new NPOI.XSSF.UserModel.XSSFWorkbook(stream); //This will read 2007 Excel format  
            sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   

            //header
            NPOI.SS.UserModel.IRow headerRow = sheet.GetRow(0); //Get Header Row
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                var s = cell.ToString();
            }

            //rows
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
            {
                var dict = new Dictionary<string, object>();
                NPOI.SS.UserModel.IRow row = sheet.GetRow(i);

                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == NPOI.SS.UserModel.CellType.Blank)) continue;

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        var cellType = row.GetCell(j).CellType;
                        string cellValue = "";

                        if (cellType == CellType.Formula)
                        {
                            cellValue = row.GetCell(j).StringCellValue;
                        }
                        else
                        {
                            cellValue = row.GetCell(j).ToString();
                        }

                        dict.Add(headerRow.GetCell(j).ToString(), cellValue);
                    }
                    else
                    {
                        dict.Add(headerRow.GetCell(j).ToString(), string.Empty);
                    }
                }

                data.Add(dict);
            }

            return data;
        }
    }
}
