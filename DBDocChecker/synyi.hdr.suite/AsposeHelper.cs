using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using synyi.hdr.suite.Entity;

namespace synyi.hdr.suite
{
    public class AsposeHelper
    {


        /// <summary>  
        /// 导入Excel  
        /// </summary>  
        /// <param name="lists"></param>  
        /// <param name="head">中文列名对照</param>  
        /// <param name="workbookFile">Excel所在路径</param>  
        /// <returns></returns>  
        public static List<ExcelDesc> fromExcel(Hashtable head, string workbookFile, int tailIgnoreRows, int sheetIndex)
        {
            try
            {
                Workbook workbook = null;
                List<ExcelDesc> lists = new List<ExcelDesc>();
                using (FileStream file = new FileStream(workbookFile, FileMode.Open, FileAccess.Read))
                {
                    workbook = new Workbook(file);
                }
                if (sheetIndex < 0)
                {
                    sheetIndex = 0;
                }
                Worksheet sheet = workbook.Worksheets[sheetIndex];
                Cells cells = sheet.Cells;
                int totalColumns = cells.MaxColumn;
                int totalRows = cells.MaxRow;
                RowCollection rows = cells.Rows;


                Row headerRow = rows[0];
                //int cellCount = headerRow.l;
                //Type type = typeof(T);  
                PropertyInfo[] properties;
                ExcelDesc t = default(ExcelDesc);
                for (int i = cells.FirstCell.Row + 1; i <= totalRows - tailIgnoreRows; i++)
                {

                    Row row = rows[i];
                    t = Activator.CreateInstance<ExcelDesc>();
                    properties = t.GetType().GetProperties();
                    foreach (PropertyInfo column in properties)
                    {
                        Cell cellIndex = null;
                        foreach (Cell item in headerRow)
                        {
                            if (item.StringValue == (head[column.Name] == null ? column.Name : head[column.Name].ToString()))
                            {
                                cellIndex = item;
                                break;
                            }
                        }

                        int index = cellIndex.Column;  // cellIndex.Name;
                        //int j = headerRow.  .. .Cells.FindIndex(delegate (ICell c)
                        //{
                        //    return c.StringCellValue == (head[column.Name] == null ? column.Name : head[column.Name].ToString());
                        //});
                        if (index >= 0 && row[index] != null)
                        {
                            object value = valueType(column.PropertyType, row[index]);
                            column.SetValue(t, value, null);
                        }
                    }
                    lists.Add(t);
                }
                return lists;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
                return null;
            }
        }


        public static object valueType(Type t, Cell value)
        {
            object o = null;
            string strt = "String";
            if (t.Name == "Nullable`1")
            {
                strt = t.GetGenericArguments()[0].Name;
            }
            switch (strt)
            {
                case "Decimal":
                    o = value.DoubleValue;
                    break;
                case "Int":
                    o = value.IntValue;
                    break;
                case "Float":
                    o = value.FloatValue;
                    break;
                case "DateTime":
                    o = value.DateTimeValue;
                    break;
                default:
                    o = value.StringValue;
                    break;
            }
            return o;
        }

        public IList<Style> CheckTest(Workbook workbook)
        {
            IList<Style> styles = new List<Style>();


            Style styleHeader = workbook.CreateStyle();
            styles.Add(styleHeader);
            styleHeader.Font.Name = "Arial";
            styleHeader.Font.Size = 10;
            styleHeader.Font.IsBold = true;
            styleHeader.Pattern = BackgroundType.Solid;
            styleHeader.ForegroundColor = Color.FromArgb(0, 204, 255);
            styleHeader.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleHeader.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


            Style styleData = workbook.CreateStyle();
            styles.Add(styleData);
            styleData.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleData.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


            Style styleAngka = workbook.CreateStyle();
            styles.Add(styleAngka);
            styleAngka.HorizontalAlignment = TextAlignmentType.Left;
            styleAngka.VerticalAlignment = TextAlignmentType.Center;

            styleAngka.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            styleAngka.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            styleAngka.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            styleAngka.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


            Style stylePemisah = workbook.CreateStyle();
            styles.Add(stylePemisah);
            stylePemisah.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            stylePemisah.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            stylePemisah.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            stylePemisah.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            stylePemisah.Pattern = BackgroundType.Solid;
            stylePemisah.ForegroundColor = Color.Yellow;
            stylePemisah.Font.IsBold = true;

            return styles;
        }

  

        /// <summary>
        /// 展开excel合并的单元格
        /// </summary>
        /// <param name="head"></param>
        /// <param name="workbook"></param>
        /// <param name="startSkipRows"></param>
        /// <param name="tailIgnoreRows"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static List<HdrTablesCollection> AutoFitMergedCells(Workbook workbook, string sheetName, int startSkipRows, int tailIgnoreRows)
        {
            try
            {
                List<HdrTablesCollection> lists = new List<HdrTablesCollection>();

                Worksheet sheet = workbook.Worksheets.FirstOrDefault(p => p.Name == sheetName);
                Cells cells = sheet.Cells;
                int totalColumns = cells.MaxColumn;
                int totalRows = cells.MaxRow;
                int startrow = cells.FirstCell.Row;
                RowCollection rows = cells.Rows;

                Row headerRow = rows[0];
                //int cellCount = headerRow.l;
                //Type type = typeof(T);  
                
                HdrTablesCollection t = default(HdrTablesCollection);
                for (int i = startrow + startSkipRows; i <= totalRows - tailIgnoreRows; i++)
                {
                    t = Activator.CreateInstance<HdrTablesCollection>();
                    Row row = rows[i];
                    Cell schema = row[0];
                    
                    if (string.IsNullOrEmpty(schema.StringValue))
                    {
                        if (schema.IsMerged)//合并单元格
                        {
                            Range rge = schema.GetMergedRange();
                            var result1 = ((object[,])(rge.Value))[0, 0].ToString();
                            t.Domain = result1;
                        }
                        else
                        {
                            t.Domain = string.Empty;
                        }
                    }
                    else
                    {
                        t.Domain = schema.StringValue;
                    }

                    Cell table = row[1];
                    Cell tabledesc = row[2];


                    t.TableName = table.StringValue;
                    t.TableDesc = tabledesc.StringValue;

                    if (!string.IsNullOrEmpty(t.Domain))
                    {
                        var spltDomain = t.Domain.Split('（', '）','(',')');
                        t.Schema = spltDomain[0];
                        t.SchemaDesc = spltDomain[1];
                    }

                    lists.Add(t);
                }
                return lists;
            }
            catch (Exception ee)
            {
                string see = ee.Message;
                return null;
            }
        }


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
                    var tabItemCols = tableColumns.Where(p => p.table_schema == schemaname).Where(p => p.table_name == tableName).OrderBy(p => p.ordinal_position).ToList();
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
