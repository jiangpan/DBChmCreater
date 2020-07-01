using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Npgsql;
using Dapper;
//using Dapper.Contrib;
//using Dapper.Contrib.Extensions;

namespace DBChmCreater.MDM
{

    public class MdmBizHelper
    {
        #region 构建代码系统集合
        public void BuildMdmCodeSystem(string excelPath, string sheetName, string version)
        {
            var codeSyslist = this.ParseExcelToEntity(excelPath, sheetName);
        }

        #endregion

        #region 解析excel生成实体
        /// <summary>
        /// 解析excel生成实体
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public IList<mdmExcelRawEntity> ParseExcelToEntity(string excelPath, string sheetName)
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

            #region excel解析
            for (int i = 0; i <= cells.MaxDataRow; i++)
            {
                var row = cells.CheckRow(i);
                if (row == null || row.FirstCell == null)
                {
                    continue;
                }

                Cell cel = row[CellsHelper.ColumnNameToIndex("A")];
                if (row[CellsHelper.ColumnNameToIndex("A")] == null || cel.Value == null)
                {
                    continue;
                }

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
                        aa.一级域代码 = row[CellsHelper.ColumnNameToIndex("A")].GetStringValue(CellValueFormatStrategy.None)?.Trim();   //	一级域代码
                        aa.一级域 = row[CellsHelper.ColumnNameToIndex("B")].GetStringValue(CellValueFormatStrategy.None)?.Trim(); //一级域
                        aa.二级域代码 = row[CellsHelper.ColumnNameToIndex("C")].GetStringValue(CellValueFormatStrategy.None)?.Trim();   //二级域代码
                        aa.二级域 = row[CellsHelper.ColumnNameToIndex("D")].GetStringValue(CellValueFormatStrategy.None)?.Trim(); //二级域
                        aa.三级域代码 = row[CellsHelper.ColumnNameToIndex("E")].GetStringValue(CellValueFormatStrategy.None)?.Trim();   //三级域代码
                        aa.三级域 = row[CellsHelper.ColumnNameToIndex("F")].GetStringValue(CellValueFormatStrategy.None)?.Trim(); //三级域
                        aa.代码系统 = row[CellsHelper.ColumnNameToIndex("G")].GetStringValue(CellValueFormatStrategy.None)?.Trim();    //代码系统
                        aa.代码系统名称 = row[CellsHelper.ColumnNameToIndex("H")].GetStringValue(CellValueFormatStrategy.None)?.Trim();  //代码系统名称
                        aa.标准来源 = row[CellsHelper.ColumnNameToIndex("I")].GetStringValue(CellValueFormatStrategy.None)?.Trim();    //标准来源
                        aa.标准级别 = row[CellsHelper.ColumnNameToIndex("J")].GetStringValue(CellValueFormatStrategy.None)?.Trim();    //标准级别
                        aa.标准完整性 = row[CellsHelper.ColumnNameToIndex("K")].GetStringValue(CellValueFormatStrategy.None)?.Trim();   //标准完整性
                        aa.值集合 = row[CellsHelper.ColumnNameToIndex("L")].GetStringValue(CellValueFormatStrategy.None)?.Trim();  //"值集合"
                        aa.ETL转换 = row[CellsHelper.ColumnNameToIndex("M")].GetStringValue(CellValueFormatStrategy.None)?.Trim();    //"ETL转换"
                        aa.应用转换 = row[CellsHelper.ColumnNameToIndex("N")].GetStringValue(CellValueFormatStrategy.None)?.Trim(); //"应用转换"
                        aa.示例数据 = row[CellsHelper.ColumnNameToIndex("O")].GetStringValue(CellValueFormatStrategy.None)?.Trim();//示例数据
                        var celcodeset = row[CellsHelper.ColumnNameToIndex("P")];
                        aa.代码集 = celcodeset.GetStringValue(CellValueFormatStrategy.None);//代码集

                        var codesetsheetaddress = celcodeset.Worksheet.Hyperlinks.FirstOrDefault(p => (p.Area.StartRow == celcodeset.Row && p.Area.StartColumn == celcodeset.Column))?.Address;

                        if (!string.IsNullOrWhiteSpace(codesetsheetaddress))
                        {
                            var codesetsheetname = codesetsheetaddress.Split('!')[0];//存放对应的sheet的名称
                            var codesetsheetcells = workbook.Worksheets[codesetsheetname]?.Cells;
                            aa.代码集明细 = ParseExcelCodeSetItem(aa.代码系统, aa.代码系统名称, codesetsheetcells);
                        }
                        codsyslist.Add(aa);
                    }
                }
            }
            #endregion
            return codsyslist;
        }


        #endregion



        #region 读取单个CodeSet的所有代码
        private IList<code_set_excel_entity> ParseExcelCodeSetItem(string code_sys_code, string code_sys_name, Cells codeSetCells)
        {
            IList<code_set_excel_entity> codeSetItem = new List<code_set_excel_entity>();
            if (codeSetCells == null)
            {
                return codeSetItem;
            }

            //读取worksheet codeset
            for (int k = 3; k <= codeSetCells.MaxDataRow; k++) //单个codeset 从第二行开始
            {
                var row = codeSetCells.CheckRow(k);
                if (row == null || row.FirstCell == null)
                {
                    continue;
                }
                else if (row.GetCellOrNull(0).StringValue.StartsWith("代码", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                else
                {
                    var aaa = new code_set_excel_entity();
                    aaa.code_sys_code = code_sys_code;
                    aaa.code_sys_name = code_sys_name;
                    aaa.code = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("A")).StringValue; //第一列
                    aaa.name = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("B")).StringValue;  //第二列
                    aaa.show_name = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("C")).StringValue; //第三列
                    codeSetItem.Add(aaa);
                }
            }
            return codeSetItem;
        }

        #endregion


        #region 构建代码集合

        public void BuildMdmCodeSet(string excelPath, string sheetName, string dbConnName)
        {

            Workbook workbook = null;

            IList<code_set_excel_entity> codesystemCels = new List<code_set_excel_entity>();

            IList<code_set_excel_entity> codesetexcels1 = new List<code_set_excel_entity>();

            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }

            //新版MDMExcel 的说明

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "代码系统";
            }
            Worksheet sheet = workbook.Worksheets[sheetName];
            var cells = sheet.Cells;

            #region 加载代码系统，包含代码集的代码系统
            for (int i = 1; i <= cells.MaxDataRow; i++)
            {
                var row = cells.CheckRow(i);
                if (row == null || row.FirstCell == null)
                {
                    continue;
                }
                else if (row.GetCellOrNull(CellsHelper.ColumnNameToIndex("A")) == null)
                {
                    continue;
                }
                else if (row.GetCellOrNull(CellsHelper.ColumnNameToIndex("A")).StringValue.Contains("代码"))
                {
                    continue;
                }
                else
                {
                    var hasCodeSet = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("P")).StringValue;
                    if (string.IsNullOrWhiteSpace(hasCodeSet))
                    {
                        continue;
                    }

                    var aaa = new code_set_excel_entity();
                    aaa.code_sys_code = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("G")).StringValue;
                    aaa.code_sys_name = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("H")).StringValue;

                    var ser = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("P"));
                    aaa.memo = sheet.Hyperlinks.FirstOrDefault(p => (p.Area.StartRow == ser.Row && p.Area.StartColumn == ser.Column)).Address;
                    if (!string.IsNullOrWhiteSpace(aaa.memo))
                    {
                        aaa.show_name = aaa.memo.Split('!')[0];//存放对应的sheet的名称
                    }
                    codesystemCels.Add(aaa);
                }
            }
            #endregion

            #region 循环处理所有的代码系统，并提取代码集
            foreach (var item in codesystemCels) //所有的代码系统
            {
                string sheetNameCodeSet = item.code_sys_code;

                Cells codeSetCells = workbook.Worksheets[item.show_name]?.Cells;
                if (codeSetCells != null)
                {//读取worksheet codeset
                    for (int k = 3; k <= codeSetCells.MaxDataRow; k++) //单个codeset 从第二行开始
                    {
                        var row = codeSetCells.CheckRow(k);
                        if (row == null || row.FirstCell == null)
                        {
                            continue;
                        }
                        else if (row.GetCellOrNull(0).StringValue.StartsWith("代码", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }
                        else
                        {
                            var aaa = new code_set_excel_entity();
                            aaa.code_sys_code = item.code_sys_code;
                            aaa.code_sys_name = item.code_sys_name;
                            aaa.code = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("A")).StringValue; //第一列
                            aaa.name = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("B")).StringValue;  //第二列
                            aaa.show_name = row.GetCellOrNull(CellsHelper.ColumnNameToIndex("C")).StringValue; //第三列
                            codesetexcels1.Add(aaa);
                        }
                    }
                }
            }//循环所有的代码系统

            #endregion

        }

        #endregion

    }
}
