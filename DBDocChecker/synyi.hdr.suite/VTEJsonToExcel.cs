using Aspose.Cells;
using synyi.hdr.suite.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class VTEJsonToExcel
    {

        public static void execute(JsonEntity movie2, string basePath)
        {


            Workbook workbook = new Workbook();

            workbook.Worksheets.Clear();
            int sheetIdx = workbook.Worksheets.Add(SheetType.Worksheet);
            Worksheet sheet = workbook.Worksheets[sheetIdx];
            Cells cells = sheet.Cells;
            var list = movie2.RECORDS;

            var listgrp = list.GroupBy(p => p.format).ToList();

            int rows = -1;
            var style = cells.GetCellStyle(0, 0);
            style.SetBorder(BorderType.LeftBorder, CellBorderType.Thin, Color.Black);
            style.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
            style.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            style.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            for (int i = 0; i < listgrp.Count; i++)
            {
                rows++;
                var table_name = listgrp[i].Key;
                var table_ent = listgrp[i].FirstOrDefault();



                cells[rows, CellsHelper.ColumnNameToIndex("A")].PutValue($"{table_ent.schema}.{table_ent.table}（{table_ent.tablecomment}）");
                cells.Merge(rows, CellsHelper.ColumnNameToIndex("A"), 1, 10);
                cells[rows, CellsHelper.ColumnNameToIndex("A")].GetMergedRange().SetStyle(style);

                //表名 表说明

                //表头行
                rows++;
                cells[rows, CellsHelper.ColumnNameToIndex("A")].PutValue("序号");
                cells[rows, CellsHelper.ColumnNameToIndex("A")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("B")].PutValue("列名");
                cells[rows, CellsHelper.ColumnNameToIndex("B")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("C")].PutValue("中文名");
                cells[rows, CellsHelper.ColumnNameToIndex("C")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("D")].PutValue("字段类型");
                cells[rows, CellsHelper.ColumnNameToIndex("D")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("E")].PutValue("允许空");
                cells[rows, CellsHelper.ColumnNameToIndex("E")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("F")].PutValue("外键（关联字段）");
                cells[rows, CellsHelper.ColumnNameToIndex("F")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("G")].PutValue("字典系统（值域）");
                cells[rows, CellsHelper.ColumnNameToIndex("G")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("H")].PutValue("字典标准化");
                cells[rows, CellsHelper.ColumnNameToIndex("H")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("I")].PutValue("说明");
                cells[rows, CellsHelper.ColumnNameToIndex("I")].SetStyle(style);
                cells[rows, CellsHelper.ColumnNameToIndex("J")].PutValue("产品层面必须");
                cells[rows, CellsHelper.ColumnNameToIndex("J")].SetStyle(style);
                foreach (var cols in listgrp[i])
                {
                    rows++;
                    //列说明
                    cells[rows, CellsHelper.ColumnNameToIndex("A")].PutValue(cols.serialnumber);
                    cells[rows, CellsHelper.ColumnNameToIndex("A")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("B")].PutValue(cols.columnname);
                    cells[rows, CellsHelper.ColumnNameToIndex("B")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("C")].PutValue(cols.columncomment);
                    cells[rows, CellsHelper.ColumnNameToIndex("C")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("D")].PutValue(cols.datatype);
                    cells[rows, CellsHelper.ColumnNameToIndex("D")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("E")].PutValue(cols.isnull);
                    cells[rows, CellsHelper.ColumnNameToIndex("E")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("F")].PutValue(cols.foreignkey);
                    cells[rows, CellsHelper.ColumnNameToIndex("F")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("G")].PutValue(cols.codesystem);
                    cells[rows, CellsHelper.ColumnNameToIndex("G")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("H")].PutValue(cols.isstandard);
                    cells[rows, CellsHelper.ColumnNameToIndex("H")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("I")].PutValue(cols.description);
                    cells[rows, CellsHelper.ColumnNameToIndex("I")].SetStyle(style);
                    cells[rows, CellsHelper.ColumnNameToIndex("J")].PutValue("√");
                    cells[rows, CellsHelper.ColumnNameToIndex("J")].SetStyle(style);



                }
                rows++;

            }

            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("A"), 4);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("B"), 15);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("C"), 14);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("D"), 11);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("E"), 7);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("F"), 10);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("G"), 14);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("H"), 10);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("I"), 8);
            cells.SetColumnWidth(CellsHelper.ColumnNameToIndex("J"), 13);

            string outfilePath = Path.Combine(basePath, $"vte_columns_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");


            workbook.Save(outfilePath, SaveFormat.Xlsx);

        }
    }
}
