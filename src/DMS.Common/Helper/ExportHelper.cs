using Aspose.Cells;
using DMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DMS.Common.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportHelper
    {
        #region 将DataTable生成Excel
        /// <summary>
        /// 将DataTable生成Excel,利用控件生成Excel
        /// </summary>
        /// <param name="dtList"></param>
        /// <param name="ExportFilesPath"></param>
        /// <returns></returns>
        public static string ExportToExcel(DataTable dtList, string exportFilesPath)
        {
            //这里是利用Aspose.Cells.dll 生成excel文件的

            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            Cells cell = ws.Cells;
            //设置行高
            //cell.SetRowHeight(0, 20);
            //表头样式
            Style stHeadLeft = wb.CreateStyle();
            stHeadLeft.HorizontalAlignment = TextAlignmentType.Left;       //文字居中
            stHeadLeft.Font.Name = "宋体";
            stHeadLeft.Font.IsBold = true;                                 //设置粗体
            stHeadLeft.Font.Size = 14;                                     //设置字体大小

            Style stHeadRight = wb.CreateStyle();
            stHeadRight.HorizontalAlignment = TextAlignmentType.Right;       //文字居中
            stHeadRight.Font.Name = "宋体";
            stHeadRight.Font.IsBold = true;                                  //设置粗体
            stHeadRight.Font.Size = 14;                                      //设置字体大小

            //内容样式
            Style stContentLeft = wb.CreateStyle();
            stContentLeft.HorizontalAlignment = TextAlignmentType.Left;
            stContentLeft.Font.Size = 10;

            Style stContentRight = wb.CreateStyle();
            stContentRight.HorizontalAlignment = TextAlignmentType.Right;
            stContentRight.Font.Size = 10;
            //赋值给Excel内容
            for (int col = 0; col < dtList.Columns.Count; col++)
            {
                Style stHead = null;
                Style stContent = null;
                //设置表头
                string columnType = dtList.Columns[col].DataType.ToString();
                switch (columnType)
                {
                    //如果类型是string，则靠左对齐(对齐方式看项目需求修改)
                    case "System.String":
                    case "System.Int32":
                        stHead = stHeadLeft;
                        stContent = stContentLeft;
                        break;
                    default:
                        stHead = stHeadRight;
                        stContent = stContentRight;
                        break;

                }
                putValue(cell, dtList.Columns[col].ColumnName, 0, col, stHead);
                for (int row = 0; row < dtList.Rows.Count; row++)
                {
                    putValue(cell, dtList.Rows[row][col], row + 1, col, stContent);
                }
            }
            wb.Save(exportFilesPath);
            return exportFilesPath;
        }

        //填充数据到excel中
        private static void putValue(Cells cell, object value, int row, int column, Style st)
        {
            cell[row, column].PutValue(value);
            cell[row, column].SetStyle(st);
        }


        public static string ExportToExcel<T>(List<T> list, string fileName, string[] titles, params Func<T, object>[] fieldFuncs)
        {
            DataTable dataTable = list.ToDataTable(titles, fieldFuncs);

            ImportTableOptions importOptions = new ImportTableOptions();
            importOptions.IsFieldNameShown = true;
            importOptions.IsHtmlString = true;

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Cells.ImportData(dataTable, 0, 0, importOptions);
            worksheet.AutoFitRows();
            worksheet.AutoFitColumns();

            workbook.Save(fileName);
            return "成功";
        }



        #endregion

        #region 将List数据导出Excel
        /// <summary>
        /// web导出excel
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="list">导出的列表对象</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="titles">标题</param>
        /// <param name="fieldFuncs">字段委托，如果不传则T的全部属性</param>
        public static byte[] ExportToFsExcel<T>(List<T> list, string[] titles, params Func<T, object>[] fieldFuncs)
        {
            DataTable dtSource = list.ToDataTable(titles, fieldFuncs);
            byte[] buffers = WriteToStream(dtSource).GetBuffer();
            return buffers;
        }

        /// <summary>
        /// datatable生成excel流
        /// </summary>
        /// <param name="dtSource">原数据</param>
        /// <returns></returns>
        private static MemoryStream WriteToStream(DataTable dtSource)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    sheet.Cells[i, j].PutValue(dtSource.Rows[i][j]);
                }
            }
            sheet.AutoFitColumns();
            MemoryStream stream = workbook.SaveToStream();
            return stream;
        }
        #endregion


        #region 导入Excel数据
        /// <summary>
        /// 导入Excel数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ImportToExcel(string filePath)
        {
            Workbook workbook = new Workbook(filePath);
            Worksheet worksheet = workbook.Worksheets[0];

            ExportTableOptions opts = new ExportTableOptions();
            opts.ExportColumnName = true;
            opts.PlotVisibleColumns = true;
            //opts.ExportAsHtmlString = true;
            //opts.RenameStrategy = RenameStrategy.Letter;

            int totalRows = worksheet.Cells.MaxRow + 1;
            int totalColumns = worksheet.Cells.MaxColumn + 1;
            DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, totalRows, totalColumns, opts);


            return dataTable;

        }

        /// <summary>
        /// 导入Excel数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static DataTable ImportToExcel(Stream stream)
        {
            Workbook workbook = new Workbook(stream);
            Worksheet worksheet = workbook.Worksheets[0];
            ExportTableOptions opts = new ExportTableOptions();
            opts.ExportColumnName = true;
            opts.PlotVisibleColumns = true;

            int totalRows = worksheet.Cells.MaxRow + 1;
            int totalColumns = worksheet.Cells.MaxColumn + 1;
            DataTable dataTable = worksheet.Cells.ExportDataTable(0, 0, totalRows, totalColumns, opts);

            return dataTable;
        }
        #endregion
    }
}
