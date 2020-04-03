using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Aspose.Cells.Drawing;
using synyi.hdr.suite.Entity;

namespace synyi.hdr.suite
{
    public class HdrExcelProcesser
    {
        /// <summary>
        /// 读取hdr表说明 转换成list 导入数据库 与数据库中的列 进行比较 判断excel与表结构是否一致
        /// </summary>
        /// <param name="cells"></param>
        /// <returns></returns>
        public IList<ExcelColumn> Process(Cells cells)
        {
            List<ExcelColumn> list = new List<ExcelColumn>();

            string TableName = string.Empty;//表名 （organization（机构信息））
            string Schema = string.Empty; //业务域（mdm（主数据））
            string TableComment = string.Empty; //表说明 （机构信息，如：医疗机构、科研机构等） 

            for (int row = 0; row < cells.Rows.Count; row++)
            {
                Row current_row = cells.CheckRow(row);

                if ( current_row == null || current_row.FirstDataCell == null)
                {
                    continue;
                }
                //表名
                Cell cel = current_row[CellsHelper.ColumnNameToIndex("A")];
                if (cel.IsMerged)
                {  //业务域 及 表说明
                    var range = cel.GetMergedRange();
                    if (range.CellCount == 9)
                    {//表的第一行
                        TableName = cel.GetStringValue(CellValueFormatStrategy.None);
                    }
                    if (range.CellCount == 2)
                    {//第二行

                        Schema = current_row[CellsHelper.ColumnNameToIndex("C")].GetStringValue(CellValueFormatStrategy.None);
                        TableComment = current_row[CellsHelper.ColumnNameToIndex("F")].GetStringValue(CellValueFormatStrategy.None);
                    }
                    continue;
                }
                else if (cel.GetStringValue(CellValueFormatStrategy.None) == "序号") //列头
                {
                    continue;
                }
                else //表列
                {
                    ExcelColumn ec = new ExcelColumn();
                    var tableNamearray = TableName.Split(new string[4] { "（", "）", "(", ")" }, StringSplitOptions.None);
                    ec.TableName = tableNamearray[0];//表名 （organization（机构信息））
                    ec.TableNameCh = string.Join("", tableNamearray.Where(p => p != tableNamearray[0]).ToArray());

                    var schemaarray = Schema.Split(new string[4] { "（", "）", "(", ")" }, StringSplitOptions.None);

                    ec.Schema = schemaarray[0];//业务域（mdm（主数据））
                    ec.SchemaCh = string.Join("", schemaarray.Where(p => p != schemaarray[0]).ToArray()) ;//业务域（mdm（主数据））

                    ec.TableComment = TableComment;//表说明 （机构信息，如：医疗机构、科研机构等） 
                    ec.SerialNumber = cel.GetStringValue(CellValueFormatStrategy.None);//序号	
                    ec.ColumnName = current_row[CellsHelper.ColumnNameToIndex("B")].GetStringValue(CellValueFormatStrategy.None);//列名	
                    ec.ColumnComment = current_row[CellsHelper.ColumnNameToIndex("C")].GetStringValue(CellValueFormatStrategy.None);//中文名	
                    ec.DataType = current_row[CellsHelper.ColumnNameToIndex("D")].GetStringValue(CellValueFormatStrategy.None);//字段类型	
                    ec.IsNull = current_row[CellsHelper.ColumnNameToIndex("E")].GetStringValue(CellValueFormatStrategy.None);//允许空	
                    ec.ForeignKey = current_row[CellsHelper.ColumnNameToIndex("F")].GetStringValue(CellValueFormatStrategy.None);//外键（关联字段）	
                    ec.CodeSystem = current_row[CellsHelper.ColumnNameToIndex("G")].GetStringValue(CellValueFormatStrategy.None);//字典系统（值域）	
                    ec.IsStandard = current_row[CellsHelper.ColumnNameToIndex("H")].GetStringValue(CellValueFormatStrategy.None);//字典表标准化	
                    ec.Description = current_row[CellsHelper.ColumnNameToIndex("I")].GetStringValue(CellValueFormatStrategy.None);//说明
                    list.Add(ec);
                    continue;
                }

            }

            return list;

        }


        //mdm（主数据）
        public IList<ExcelColumn> ProcessSchemaMdm(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "mdm").FirstOrDefault();

            var cells = sheet.Cells;

            var result = Process(cells);

            return result;
        }
        //patient（病人信息）
        public IList<ExcelColumn> ProcessSchemaPatient(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "patient").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //visit（就诊信息）
        public IList<ExcelColumn> ProcessSchemaVisit(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "visit").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //orders（医嘱信息）
        public IList<ExcelColumn> ProcessSchemaOrders(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "orders").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;


        }
        //fee（费用）
        public IList<ExcelColumn> ProcessSchemaFee(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "fee").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //diag（诊断）
        public IList<ExcelColumn> ProcessSchemaDiag(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "diag").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;


        }
        //allergy（过敏）
        public IList<ExcelColumn> ProcessSchemaAllergy(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "allergy").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //checks（检查）
        public IList<ExcelColumn> ProcessSchemaChecks(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "checks").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //lab（检验）
        public IList<ExcelColumn> ProcessSchemaLab(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "lab").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //operation（手术）
        public IList<ExcelColumn> ProcessSchemaOperation(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "operation").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //nurse（护理）
        public IList<ExcelColumn> ProcessSchemaNurse(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "nurse").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //cases（病案）
        public IList<ExcelColumn> ProcessSchemaCases(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "cases").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //emr（病历文书）
        public IList<ExcelColumn> ProcessSchemaEmr(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "emr").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //tumour（肿瘤）
        public IList<ExcelColumn> ProcessSchemaTumour(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "tumour").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //other（其他数据）
        public IList<ExcelColumn> ProcessSchemaOther(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "other").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //reportcard（报告卡）
        public IList<ExcelColumn> ProcessSchemaReportcard(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "reportcard").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //phyexam（体检）
        public IList<ExcelColumn> ProcessSchemaPhyexam(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "phyexam").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //biobank（样本库）
        public IList<ExcelColumn> ProcessSchemaBiobank(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "biobank").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //nlp（文本处理）
        public IList<ExcelColumn> ProcessSchemaNlp(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "nlp").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;


        }
        //eventflow（事件流及闭环）
        public IList<ExcelColumn> ProcessSchemaEventflow(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "eventflow").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
        //etl（数据集成）
        public IList<ExcelColumn> ProcessSchemaEtl(WorksheetCollection worksheets)
        {
            var sheet = worksheets.Where(p => p.Name == "etl").FirstOrDefault();
            var cells = sheet.Cells;

            var result = Process(cells);

            return result;

        }
    }
}