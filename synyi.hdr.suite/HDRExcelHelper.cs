using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using synyi.hdr.suite.Entity;

namespace synyi.hdr.suite
{
    public class HDRExcelHelper
    {

        public void ExportVithStyle(IList<HdrTablesCollection> schemas, List<ColumnEntity> tableColumns, Workbook workbook, int columnsOffset = 0, bool generateDiffColumn = false)
        {
            var schemaAndTablas = schemas.GroupBy(p => p.Schema).ToList();
            AsposeHelper heper = new AsposeHelper();
            IList<Style> styles = heper.CheckTest(workbook);
            for (int ischema = 0; ischema < schemaAndTablas.Count; ischema++)
            {
                //一个schema
                var item = schemaAndTablas[ischema];
                string schemaname = item.Key;
                var itemlst = schemaAndTablas[ischema].ToList();//所有的表



                Worksheet sheetSchema = workbook.Worksheets.FirstOrDefault(p => p.Name == schemaname);
                if (sheetSchema == null)
                {
                    sheetSchema = workbook.Worksheets.Add(schemaname);
                }
                var cells = sheetSchema.Cells;
                //架构下所有的表

                int rowIndex = 2;
                int columnOffset = columnsOffset;
                for (int jtable = 0; jtable < itemlst.Count; jtable++)
                {
                    string tableName = itemlst[jtable].TableName;

                    #region 写入表级别 说明
                    cells[CellsHelper.CellIndexToName(rowIndex, 0 + columnOffset)].PutValue(tableName);
                    rowIndex++;
                    cells[CellsHelper.CellIndexToName(rowIndex, 0 + columnOffset)].PutValue(schemaname);
                    //占用行
                    //列
                    rowIndex++;
                    cells[CellsHelper.CellIndexToName(rowIndex, 0 + columnOffset)].PutValue("序号");
                    cells[CellsHelper.CellIndexToName(rowIndex, 1 + columnOffset)].PutValue("列名");
                    cells[CellsHelper.CellIndexToName(rowIndex, 2 + columnOffset)].PutValue("中文名");
                    cells[CellsHelper.CellIndexToName(rowIndex, 3 + columnOffset)].PutValue("字段类型");
                    cells[CellsHelper.CellIndexToName(rowIndex, 4 + columnOffset)].PutValue("允许空");
                    cells[CellsHelper.CellIndexToName(rowIndex, 5 + columnOffset)].PutValue("外键");
                    cells[CellsHelper.CellIndexToName(rowIndex, 6 + columnOffset)].PutValue("字典系统");
                    cells[CellsHelper.CellIndexToName(rowIndex, 7 + columnOffset)].PutValue("字典标准化");
                    cells[CellsHelper.CellIndexToName(rowIndex, 8 + columnOffset)].PutValue("说明");
                    if (generateDiffColumn)
                    {
                        cells[CellsHelper.CellIndexToName(rowIndex, 9 + columnOffset)].PutValue("列名对比");
                        cells[CellsHelper.CellIndexToName(rowIndex, 10 + columnOffset)].PutValue("字段类型对比");
                        cells[CellsHelper.CellIndexToName(rowIndex, 11 + columnOffset)].PutValue("允许空对比");

                    }

                    rowIndex++;
                    #endregion

                    #region 处理列级别 表对应的列
                    //写入具体的表列
                    var tabItemCols = tableColumns.Where(p => p.table_schema == schemaname).Where(p => p.table_name == tableName).OrderBy(p=>p.ordinal_position).ToList();
                    for (int icolumn = 0; icolumn < tabItemCols.Count; icolumn++)
                    {
                        var colItem = tabItemCols[icolumn];

                        cells[CellsHelper.CellIndexToName(rowIndex, 0 + columnOffset)].PutValue(colItem.ordinal_position);
                        cells[CellsHelper.CellIndexToName(rowIndex, 0 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 1 + columnOffset)].PutValue(colItem.column_name);
                        cells[CellsHelper.CellIndexToName(rowIndex, 1 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 2 + columnOffset)].PutValue(colItem.chinese_name);
                        cells[CellsHelper.CellIndexToName(rowIndex, 2 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 3 + columnOffset)].PutValue(colItem.data_typ);
                        cells[CellsHelper.CellIndexToName(rowIndex, 3 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 4 + columnOffset)].PutValue(colItem.is_nullable);
                        cells[CellsHelper.CellIndexToName(rowIndex, 4 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 5 + columnOffset)].PutValue(colItem.ref_key);
                        cells[CellsHelper.CellIndexToName(rowIndex, 5 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 6 + columnOffset)].PutValue(colItem.vale_range);
                        cells[CellsHelper.CellIndexToName(rowIndex, 6 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 7 + columnOffset)].PutValue(colItem.is_standard);
                        cells[CellsHelper.CellIndexToName(rowIndex, 7 + columnOffset)].SetStyle(styles[2]);

                        cells[CellsHelper.CellIndexToName(rowIndex, 8 + columnOffset)].PutValue(colItem.col_memo);
                        cells[CellsHelper.CellIndexToName(rowIndex, 8 + columnOffset)].SetStyle(styles[2]);


                        if (generateDiffColumn)
                        {
                            string columnNameIndexName = CellsHelper.CellIndexToName(rowIndex, 1);// 列名
                            string columnTypeIndexName = CellsHelper.CellIndexToName(rowIndex, 3);// 字段类型
                            string columnIsNullIndexName = CellsHelper.CellIndexToName(rowIndex, 4);// 允许空


                            string columnNameIndexName1 = CellsHelper.CellIndexToName(rowIndex, 1 + 9);// 列名
                            string columnTypeIndexName1 = CellsHelper.CellIndexToName(rowIndex, 3 + 9);// 字段类型
                            string columnIsNullIndexName1 = CellsHelper.CellIndexToName(rowIndex, 4 + 9);// 允许空

                            //★☆◊

                            cells[CellsHelper.CellIndexToName(rowIndex, 9 + columnOffset)].Formula = $"=IF(EXACT({columnNameIndexName}, {columnNameIndexName1}), \"★\", \"☆\")";
                            cells[CellsHelper.CellIndexToName(rowIndex, 10 + columnOffset)].Formula = $"=IF(EXACT({columnTypeIndexName}, {columnTypeIndexName1}), \"★\", \"☆\")";
                            cells[CellsHelper.CellIndexToName(rowIndex, 11 + columnOffset)].Formula = $"=IF(EXACT({columnIsNullIndexName}, {columnIsNullIndexName1}), \"★\", \"☆\")";

                        }


                        rowIndex++;
                    }


                    #endregion
                }


                sheetSchema.AutoFitColumns();
                sheetSchema.AutoFitRows();


            }


        }

                     
    }
}
