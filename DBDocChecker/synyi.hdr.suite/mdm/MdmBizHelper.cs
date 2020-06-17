using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
namespace synyi.hdr.suite.mdm
{
    public class MdmBizHelper
    {
        public void BuildMdmCodeSystem(string excelPath, string sheetName)
        {
            var codeSyslist  =  this.ParseExcelToEntity(excelPath, sheetName);





        }

        /// <summary>
        /// 解析excel生成实体
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private IList<mdmExcelRawEntity> ParseExcelToEntity(string excelPath, string sheetName)
        {
            Workbook workbook = null;

            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "代码系统";
            }

            Worksheet worksheet = workbook.Worksheets[sheetName];

            Cells cells = worksheet.Cells;


            IList<mdmExcelRawEntity> codsyslist = new List<mdmExcelRawEntity>();

            #region excel接卸
            for (int i = 0; i <= cells.MaxDataRow; i++)
            {
                var row = cells.CheckRow(i);
                if (row == null || row.FirstDataCell == null)
                {
                    continue;
                }

                Cell cel = row[CellsHelper.ColumnNameToIndex("A")];

                if (cel.IsMerged)
                {
                    continue;
                }
                else
                {
                    if (cel.GetStringValue(CellValueFormatStrategy.None) == "一级域代码")
                    {
                        //表头

                    }
                    else
                    {
                        mdmExcelRawEntity aa = new mdmExcelRawEntity();

                        aa.一级域代码 = row[CellsHelper.ColumnNameToIndex("A")].GetStringValue(CellValueFormatStrategy.None);   //	一级域代码
                        aa.一级域 = row[CellsHelper.ColumnNameToIndex("B")].GetStringValue(CellValueFormatStrategy.None); //一级域
                        aa.二级域代码 = row[CellsHelper.ColumnNameToIndex("C")].GetStringValue(CellValueFormatStrategy.None);   //二级域代码
                        aa.二级域 = row[CellsHelper.ColumnNameToIndex("D")].GetStringValue(CellValueFormatStrategy.None); //二级域
                        aa.三级域代码 = row[CellsHelper.ColumnNameToIndex("E")].GetStringValue(CellValueFormatStrategy.None);   //三级域代码
                        aa.三级域 = row[CellsHelper.ColumnNameToIndex("F")].GetStringValue(CellValueFormatStrategy.None); //三级域
                        aa.代码系统 = row[CellsHelper.ColumnNameToIndex("G")].GetStringValue(CellValueFormatStrategy.None);    //代码系统
                        aa.代码系统名称 = row[CellsHelper.ColumnNameToIndex("H")].GetStringValue(CellValueFormatStrategy.None);  //代码系统名称
                        aa.标准来源 = row[CellsHelper.ColumnNameToIndex("I")].GetStringValue(CellValueFormatStrategy.None);    //标准来源
                        aa.标准级别 = row[CellsHelper.ColumnNameToIndex("J")].GetStringValue(CellValueFormatStrategy.None);    //标准级别
                        aa.标准完整性 = row[CellsHelper.ColumnNameToIndex("K")].GetStringValue(CellValueFormatStrategy.None);   //标准完整性
                        aa.值集合 = row[CellsHelper.ColumnNameToIndex("L")].GetStringValue(CellValueFormatStrategy.None);  //"值集合"
                        aa.ETL转换 = row[CellsHelper.ColumnNameToIndex("M")].GetStringValue(CellValueFormatStrategy.None);    //"ETL转换"
                        aa.应用转换 = row[CellsHelper.ColumnNameToIndex("N")].GetStringValue(CellValueFormatStrategy.None); //"应用转换"
                        aa.示例数据 = row[CellsHelper.ColumnNameToIndex("O")].GetStringValue(CellValueFormatStrategy.None);//示例数据

                        codsyslist.Add(aa);
                    }
                }
            }
            #endregion
            return codsyslist;
        }


    }
}
