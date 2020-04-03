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

    }
}
