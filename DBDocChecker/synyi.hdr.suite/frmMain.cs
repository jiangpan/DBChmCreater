using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using Aspose.Cells;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using Aspose.Cells.Drawing;
using synyi.hdr.suite.Entity;
using Dapper.Contrib.Extensions;
using System.Diagnostics;
using System.Configuration;
using Dapper.Contrib;
using synyi.hdr.suite.mdm;

namespace synyi.hdr.suite
{
    public partial class frmMain : Form
    {
        #region 属性
        private string basePath = AppDomain.CurrentDomain.BaseDirectory;

        private string dbConnectionString = ConfigurationManager.ConnectionStrings["hdr"].ConnectionString;

        private string testdbConnectionString = ConfigurationManager.ConnectionStrings["exampledb"].ConnectionString;

        #endregion

        #region 构造函数
        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region 事件
        private void frmMain_Shown(object sender, EventArgs e)
        {
            //this.btnLoadHdr_v105.PerformClick();
            //this.btnLoadJson.PerformClick();
            //this.btnTest.PerformClick();
            //this.btnLoadHdr_v106.PerformClick();
        }
        #endregion


        private string[] GetTableName(string baseName)
        {
            List<string> aa = null;

            using (var conn = new PostgresHelper(dbConnectionString))
            {
                aa = conn.Query<string>(@"select table_name  from information_schema.tables where table_schema = 'public' and table_name like @tbname ", new { tbname = baseName.ToLower() + "%" }).ToList<string>();
            }


            string[] arrTname = aa.ToArray();

            string[] arrTname1 = new string[arrTname.Length];
            int j = 2;
            for (int i = 0; i < arrTname.Length; i++)
            {
                if (arrTname[i].EndsWith("_Header"))
                {
                    arrTname1[0] = arrTname[i];
                    continue;
                }
                if (arrTname[i].EndsWith("_Body"))
                {
                    arrTname1[1] = arrTname[i];
                    continue;

                }
                arrTname1[j++] = arrTname[i];
            }

            return arrTname1;
        }


        private void btnExportHDRExcel_Click(object sender, EventArgs e)
        {
            IList<HdrTablesCollection> schemaAndTablesEntities = JsonConvert.DeserializeObject<IList<HdrTablesCollection>>(this.richTextBox1.Text);

            var schemaAndTableEntGroup = schemaAndTablesEntities.GroupBy(p => p.Schema);

            var schema = schemaAndTablesEntities.Select(p => p.Schema).Distinct().ToList();

            string schemas = schema.Select(p => "'" + p + "'").Aggregate((a, b) => a + "," + b);

            List<ColumnEntity> result = null;

            #region 查询数据库 获取 Columns 信息
            using (var conn = new PostgresHelper(dbConnectionString))
            {
                BizHelper bizHelper = new BizHelper();
                var sql = bizHelper.BuildQueryTableColumnsSQL(schema);
                result = conn.Query<ColumnEntity>(sql).ToList<ColumnEntity>();
            }

            //类型处理
            result = result.Select(p =>
            {
                if (p.data_typ == "int2")
                {
                    p.data_typ = "smallint";
                }
                if (p.data_typ == "int4")
                {
                    p.data_typ = "int(4)";
                }
                if (p.data_typ == "int8")
                {
                    p.data_typ = "bigint";
                }
                if (p.data_typ == "timestamp(6)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ == "timestamp(3)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ.Length == 6 && p.data_typ.Contains("[]") && p.data_typ.Contains("int"))
                {
                    p.data_typ = "int[]";
                }
                return p;
            }).ToList();

            #endregion
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();
            AsposeHelper heper = new AsposeHelper();
            IList<Style> styles = heper.CheckTest(workbook);

            foreach (var item in schemaAndTableEntGroup)
            {
                var schemaName = item.Key; //架构名
                var schemaTables = item.ToList(); //表集合

                var workSheet = workbook.Worksheets.Add(schemaName); //架构的 工作表
                var schemaConfigTables = schemaTables.Select(p => p.TableName.Trim()); //架构下所有的表

                var cells = workSheet.Cells;
                //生成行 列名行 

                int rowIndex = 2;

                foreach (var tabItem in schemaTables)
                {
                    //表 cells["A"].PutValue(tabItem.);

                    cells["A" + rowIndex.ToString()].PutValue(tabItem.TableName.Trim());
                    rowIndex++;
                    cells["A" + rowIndex.ToString()].PutValue(schemaName);
                    //占用行
                    //列
                    rowIndex++;
                    cells["A" + rowIndex.ToString()].PutValue("序号");
                    cells["B" + rowIndex.ToString()].PutValue("列名");
                    cells["C" + rowIndex.ToString()].PutValue("中文名");
                    cells["D" + rowIndex.ToString()].PutValue("字段类型");
                    cells["E" + rowIndex.ToString()].PutValue("允许空");
                    cells["F" + rowIndex.ToString()].PutValue("外键");
                    cells["G" + rowIndex.ToString()].PutValue("字典系统");
                    cells["H" + rowIndex.ToString()].PutValue("字典标准化");
                    cells["I" + rowIndex.ToString()].PutValue("说明");
                    rowIndex++;

                    var tabItemCols = result.Where(p => p.table_schema == schemaName).Where(p => p.table_name == tabItem.TableName.Trim()).ToList();

                    for (int i = 0; i < tabItemCols.Count; i++)
                    {
                        var colItem = tabItemCols[i];
                        cells["A" + rowIndex.ToString()].PutValue(colItem.ordinal_position);
                        cells["A" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["B" + rowIndex.ToString()].PutValue(colItem.column_name);
                        cells["B" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["C" + rowIndex.ToString()].PutValue(colItem.chinese_name);
                        cells["C" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["D" + rowIndex.ToString()].PutValue(colItem.data_typ);
                        cells["D" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["E" + rowIndex.ToString()].PutValue(colItem.is_nullable);
                        cells["E" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["F" + rowIndex.ToString()].PutValue(colItem.ref_key);
                        cells["F" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["G" + rowIndex.ToString()].PutValue(colItem.vale_range);
                        cells["G" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["H" + rowIndex.ToString()].PutValue(colItem.is_standard);
                        cells["H" + rowIndex.ToString()].SetStyle(styles[2]);

                        cells["I" + rowIndex.ToString()].PutValue(colItem.col_memo);
                        cells["I" + rowIndex.ToString()].SetStyle(styles[2]);

                        rowIndex++;
                    }
                }
                workSheet.AutoFitColumns();
                workSheet.AutoFitRows();

            }
            //workbook.Settings.Password = "synyi";

            string fileName = $"HDR_Model_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            workbook.Save(Path.Combine(basePath, fileName), SaveFormat.Xlsx);
            System.Diagnostics.Process.Start("Explorer", "/select," + Path.Combine(basePath, fileName));
            this.Close();

        }


        private void btnLoadSchemaAndTables_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHDRExcelPath.Text))
            {
                return;
            }
            if (!File.Exists(this.txtHDRExcelPath.Text))
            {
                return;
            }
            string filePathWithName = this.txtHDRExcelPath.Text;

            Workbook workbook = null;

            using (FileStream fs = new FileStream(filePathWithName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }

            Worksheet worksheet = workbook.Worksheets["hdr表集合"];

            int cols = worksheet.Cells.Columns.Count;
            int rows = worksheet.Cells.Rows.Count;

            var result = AsposeHelper.AutoFitMergedCells(workbook, "hdr表集合", 2, 0);


            var bindingList = new BindingList<HdrTablesCollection>(result);
            var source = new BindingSource(bindingList, null);
            grid.DataSource = source;


            var resultjson = JsonConvert.SerializeObject(result);

            this.richTextBox1.AppendText(resultjson);

        }


        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(basePath, @"..\VTE_Columns.json");
            if (!File.Exists(filePath))
            {
                MessageBox.Show("文件不存在！");
                return;
            }

            JsonEntity movie2 = null;
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                movie2 = (JsonEntity)serializer.Deserialize(file, typeof(JsonEntity));
            }
            VTEJsonToExcel.execute(movie2, basePath);
        }


        #region 转换hdr的excel说明为hdr_columns结构，并保存至public hdr_columns
        private void btnLoadHdr_To_hdr_columns_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHDRExcelPath.Text))
            {
                MessageBox.Show("请选择hdr模型描述excel文件");
                return;
            }
            if (!File.Exists(this.txtHDRExcelPath.Text))
            {
                MessageBox.Show("hdr模型描述excel文件不存在");
                return;
            }

            //string filePath = Path.Combine(basePath, @"..\HDR库表集合_V1.0.5-201908.xlsx");
            string filePath = this.txtHDRExcelPath.Text;

            var inputfilename = Path.GetFileNameWithoutExtension(this.txtHDRExcelPath.Text);



            Workbook workbook = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }
            var worksheets = workbook.Worksheets;
            HdrExcelProcesser hdrExcelProcesser = new HdrExcelProcesser();
            List<ExcelColumn> cols = new List<ExcelColumn>();


            var result = hdrExcelProcesser.ProcessSchemaMdm(worksheets);
            cols.AddRange(result);


            var result1 = hdrExcelProcesser.ProcessSchemaPatient(worksheets);
            cols.AddRange(result1);


            var result2 = hdrExcelProcesser.ProcessSchemaVisit(worksheets);
            cols.AddRange(result2);



            var result3 = hdrExcelProcesser.ProcessSchemaOrders(worksheets);
            cols.AddRange(result3);



            var result4 = hdrExcelProcesser.ProcessSchemaFee(worksheets);
            cols.AddRange(result4);

            var result5 = hdrExcelProcesser.ProcessSchemaDiag(worksheets);
            cols.AddRange(result5);

            var result6 = hdrExcelProcesser.ProcessSchemaAllergy(worksheets);
            cols.AddRange(result6);

            var result7 = hdrExcelProcesser.ProcessSchemaChecks(worksheets);
            cols.AddRange(result7);

            var result8 = hdrExcelProcesser.ProcessSchemaLab(worksheets);
            cols.AddRange(result8);


            var result9 = hdrExcelProcesser.ProcessSchemaOperation(worksheets);
            cols.AddRange(result9);


            var result10 = hdrExcelProcesser.ProcessSchemaNurse(worksheets);
            cols.AddRange(result10);

            var result11 = hdrExcelProcesser.ProcessSchemaCases(worksheets);
            cols.AddRange(result11);


            var result12 = hdrExcelProcesser.ProcessSchemaEmr(worksheets);
            cols.AddRange(result12);


            var result13 = hdrExcelProcesser.ProcessSchemaTumour(worksheets);
            cols.AddRange(result13);


            var result14 = hdrExcelProcesser.ProcessSchemaOther(worksheets);
            cols.AddRange(result14);


            var result15 = hdrExcelProcesser.ProcessSchemaReportcard(worksheets);
            cols.AddRange(result15);


            var result16 = hdrExcelProcesser.ProcessSchemaPhyexam(worksheets);
            cols.AddRange(result16);


            var result17 = hdrExcelProcesser.ProcessSchemaBiobank(worksheets);
            cols.AddRange(result17);


            var result18 = hdrExcelProcesser.ProcessSchemaNlp(worksheets);
            cols.AddRange(result18);


            var result19 = hdrExcelProcesser.ProcessSchemaEventflow(worksheets);
            cols.AddRange(result19);


            var result20 = hdrExcelProcesser.ProcessSchemaEtl(worksheets);
            cols.AddRange(result20);

            var result21 = hdrExcelProcesser.ProcessSchemaPublic(worksheets);
            cols.AddRange(result21);

            string outfilePath = Path.Combine(basePath, $"{inputfilename}_hdrcolumns_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

            Workbook wb = new Workbook();

            wb.Worksheets.Clear();

            Worksheet sheet1 = wb.Worksheets.Add("hdr_columns");

            sheet1.Cells.ImportCustomObjects(cols as ICollection, 0, 0, new ImportTableOptions { });

            wb.Save(outfilePath, SaveFormat.Xlsx);

            using (var conn = new PostgresHelper(dbConnectionString))
            {
                var bulkinserthelper = conn.BulkinsertHdrColumns(cols, false);
            }

            Process.Start("Explorer", "/select," + outfilePath);
        }

        #endregion


        #region 读取最新的HDR 生成对比Excel

        private void btnLoadHdr_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHDRExcelPath.Text))
            {
                return;
            }
            if (!File.Exists(this.txtHDRExcelPath.Text))
            {
                return;
            }
            string filePathWithName = this.txtHDRExcelPath.Text;

            Workbook workbook = null;
            using (FileStream fs = new FileStream(filePathWithName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }
            var worksheets = workbook.Worksheets;

            var result = AsposeHelper.AutoFitMergedCells(workbook, "hdr表集合", 2, 0);

            #region 读取数据
            List<ColumnEntity> tableColumns = null;
            var schema = result.Select(p => p.Schema).Distinct().ToList();
            using (var conn = new PostgresHelper(dbConnectionString))
            {
                BizHelper bizHelper = new BizHelper();
                var sql = bizHelper.BuildQueryTableColumnsSQL(schema);
                tableColumns = conn.Query<ColumnEntity>(sql).ToList<ColumnEntity>();
            }

            var tableColumns1 = tableColumns.Select(p =>
            {
                if (p.data_typ == "int2")
                {
                    p.data_typ = "smallint";
                }
                if (p.data_typ == "int4")
                {
                    p.data_typ = "int(4)";
                }
                if (p.data_typ == "int8")
                {
                    p.data_typ = "bigint";
                }
                if (p.data_typ == "timestamp(6)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ == "timestamp(3)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ.Length == 6 && p.data_typ.Contains("[]") && p.data_typ.Contains("int"))
                {
                    p.data_typ = "int[]";
                }
                return p;
            }).ToList();
            #endregion


            var schemaAndTablas = result.GroupBy(p => p.Schema).ToList();



            //var schemas = result.Select(p => p.Schema).Distinct();


            var schemaTables = JsonConvert.SerializeObject(result);


            string outfilePath = Path.Combine(basePath, $"hdr_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

            Workbook wb;
            using (FileStream fs = new FileStream(filePathWithName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                wb = new Workbook(fs);
            }

            AsposeHelper hDRExcelHelper = new AsposeHelper();

            hDRExcelHelper.ExportVithStyle(result, tableColumns1, wb, 9, true);


            wb.Save(outfilePath, SaveFormat.Xlsx);

            Process.Start("Explorer", "/select," + outfilePath);


        }

        #endregion


        #region 读取最新的SD 生成对比Excel
        private void btnLoadHDR_SD_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtHDRExcelPath.Text))
            {
                return;
            }
            if (!File.Exists(this.txtHDRExcelPath.Text))
            {
                return;
            }
            string filePathWithName = this.txtHDRExcelPath.Text;

            Workbook workbook = null;
            using (FileStream fs = new FileStream(filePathWithName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }
            var worksheets = workbook.Worksheets;

            var result = AsposeHelper.AutoFitMergedCells_SD(workbook, "sd域表集合", 2, 0);

            #region 读取数据
            List<ColumnEntity> tableColumns = null;
            var schema = new List<string> { "sd" };
            using (var conn = new PostgresHelper(dbConnectionString))
            {
                BizHelper bizHelper = new BizHelper();
                var sql = bizHelper.BuildQueryTableColumnsSQL(schema);
                tableColumns = conn.Query<ColumnEntity>(sql).ToList<ColumnEntity>();
            }

            var tableColumns1 = tableColumns.Select(p =>
            {
                if (p.data_typ == "int2")
                {
                    p.data_typ = "smallint";
                }
                if (p.data_typ == "int4")
                {
                    p.data_typ = "int";
                }
                if (p.data_typ == "int8")
                {
                    p.data_typ = "bigint";
                }
                if (p.data_typ == "timestamp(6)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ == "timestamp(3)")
                {
                    p.data_typ = "timestamp";
                }
                if (p.data_typ.Length == 6 && p.data_typ.Contains("[]") && p.data_typ.Contains("int"))
                {
                    p.data_typ = "int[]";
                }
                return p;
            }).ToList();
            #endregion


            var schemaAndTablas = result.GroupBy(p => p.Schema).ToList();
            //var schemas = result.Select(p => p.Schema).Distinct();

            var schemaTables = JsonConvert.SerializeObject(result);

            string outfilePath = Path.Combine(basePath, $"hdr_sd_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

            Workbook wb;
            using (FileStream fs = new FileStream(filePathWithName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                wb = new Workbook(fs);
            }

            AsposeHelper hDRExcelHelper = new AsposeHelper();

            hDRExcelHelper.ExportVithStyle(result, tableColumns1, wb, 9, true);

            wb.Save(outfilePath, SaveFormat.Xlsx);

            Process.Start("Explorer", "/select," + outfilePath);
        }
        #endregion


        #region 文件对话框
        private void btnLocationHDRExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Reset();
                //openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Filter = "Excel files (*.xls or *.xlsx)|*.xls;*.xlsx";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtHDRExcelPath.Text = openFileDialog.FileName;
                }
            }
        }
        #endregion


        #region 生成SD的数据  对比数据

        private void btnLoad_SD_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(this.txtHDRExcelPath.Text))
            {
                MessageBox.Show("请选择hdr sd模型描述excel文件");
                return;
            }
            if (!File.Exists(this.txtHDRExcelPath.Text))
            {
                MessageBox.Show("hdr sd模型描述excel文件不存在");
                return;
            }

            //string filePath = Path.Combine(basePath, @"..\HDR库表集合_V1.0.5-201908.xlsx");
            string filePath = this.txtHDRExcelPath.Text;

            var inputfilename = Path.GetFileNameWithoutExtension(this.txtHDRExcelPath.Text);



            Workbook workbook = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }
            var worksheets = workbook.Worksheets;
            HdrExcelProcesser hdrExcelProcesser = new HdrExcelProcesser();
            List<ExcelColumn> cols = new List<ExcelColumn>();


            var result = hdrExcelProcesser.ProcessSchemaSd(worksheets);
            cols.AddRange(result);

            string outfilePath = Path.Combine(basePath, $"{inputfilename}_hdrcolumns_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

            Workbook wb = new Workbook();

            wb.Worksheets.Clear();

            Worksheet sheet1 = wb.Worksheets.Add("hdr_columns");

            sheet1.Cells.ImportCustomObjects(cols as ICollection, 0, 0, new ImportTableOptions { });

            wb.Save(outfilePath, SaveFormat.Xlsx);

            using (var conn = new PostgresHelper(dbConnectionString))
            {
                var bulkinserthelper = conn.BulkinsertHdrColumns(cols, false);
            }
            Process.Start("Explorer", "/select," + outfilePath);
        }


        #endregion


        #region 切分脚本
        private void btnSplitScripts_Click(object sender, EventArgs e)
        {
            Dictionary<string, IList<string>> hdrscripts = new Dictionary<string, IList<string>>();

            List<string> hdrbackups = new List<string> { "d:\\schema_dmp\\hdr_public_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_visit_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_tumour_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_sd_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_reportcard_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_phyexam_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_patient_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_other_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_orders_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_operation_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_nurse_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_nlp_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_mdm_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_lab_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_fee_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_eventflow_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_etl_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_emr_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_diag_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_checks_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_cases_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_biobank_20200404.sql",
                                                        "d:\\schema_dmp\\hdr_allergy_20200404.sql" };
            var st = Stopwatch.StartNew();
            foreach (var item in hdrbackups)
            {
                var fileName = Path.GetFileNameWithoutExtension(item);
                var schemaName = fileName.Split('_')[1];
                var result = StringHelper.Split(item);
                hdrscripts.Add(schemaName, result);
                //aaaa(result, "");
            }

            var scripthand = new ScriptHandler();
            string basePath = Path.Combine(".", $"hdr{DateTime.Now.ToString("yyyyMMddHHmmss")}");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            #region 生成实体对象
            Dictionary<string, IList<ScriptSnippet>> dicsnippets = new Dictionary<string, IList<ScriptSnippet>>();

            foreach (var item in hdrscripts)
            {
                var result1 = scripthand.HandBackupScript($"d:\\schema_dmp\\hdr_{item.Key}_20200404.sql", item.Key, item.Value);

                dicsnippets.Add(item.Key, result1);
            }

            #endregion


            #region 持久化到数据库保存
            var conn1 = new NpgsqlConnection(testdbConnectionString);


            //conn.Execute("truncate table hdr_script_snippets");
            conn1.Execute("drop table IF EXISTS hdr_script_snippets");
            conn1.Execute(@"create table hdr_script_snippets(
                                    id serial primary key,
                                    startline int,
                                    endline  int ,
                                    filename varchar(200),
                                    schemaname varchar(50),
                                    snippets text[],
                                    tyype  varchar(100),
                                    name   text,
                                    ctime TIMESTAMP default now()
                                   )");
            foreach (var item in dicsnippets)
            {
                foreach (var snippetitem in item.Value)
                {
                    conn1.Insert<ScriptSnippet>(snippetitem);
                }
            }

            conn1.Dispose();
            #endregion


            #region 保存到对应的文件目录

            foreach (var item in dicsnippets)
            {
                scripthand.ScriptSnippetToFile(basePath, item.Key, item.Value);
            }


            #endregion

            st.Stop();
            this.richTextBox1.AppendText($"Finish! Used {st.Elapsed.TotalMilliseconds}ms");

        }

        #endregion


        #region MDM维护
        private void btnHDrMDMMaintain_Click(object sender, EventArgs e)
        {
            frmMdmMgr frmMdmMgr = new frmMdmMgr();
            frmMdmMgr.Show();
        }
        #endregion

    }



}
