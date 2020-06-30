using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using Npgsql;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace synyi.hdr.suite.mdm
{
    /*
        select * from mdm.code_domain 
order by domain_id desc

    //删除
        delete from mdm.code_domain where oper_time > current_TIMESTAMP - interval '10 hour'

    //重置序列
    select setval(pg_get_serial_sequence('mdm.code_system', 'code_sys_id'), max(code_sys_id), true)  from mdm.code_system

    //删除 代码系统
    delete from mdm.code_system where oper_time > current_TIMESTAMP - interval '10 hour'

        */

    public class MdmBizHelper
    {
        #region 构建代码系统集合
        public void BuildMdmCodeSystem(string excelPath, string sheetName, string dbConnName, string version)
        {
            var codeSyslist = this.ParseExcelToEntity(excelPath, sheetName);

            if (string.IsNullOrEmpty(dbConnName))
            {
                dbConnName = "hdr";
            }
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings[dbConnName].ConnectionString;


            InsertCodeDomainLevel1(codeSyslist, connstr);

            InsertCodeDomainLevel2(codeSyslist, connstr);

            InsertCodeDomainLevel3(codeSyslist, connstr);

            InsertCodeSystem(codeSyslist, connstr, version);

        }

        #endregion


        #region 解析excel生成实体
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

            #region excel解析
            for (int i = 0; i <= cells.MaxDataRow; i++)
            {
                var row = cells.CheckRow(i);
                if (row == null || row.FirstCell == null)
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


        #endregion


        #region 一级 domain域
        private void InsertCodeDomainLevel1(IList<mdmExcelRawEntity> codesyslist, string connstr)
        {
            var domains = codesyslist.Select(p => new { levelcode = p.一级域代码, levelname = p.一级域 }).Distinct();
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {

                foreach (var item in domains)
                {

                    var codedomain = new code_domain_entity
                    {
                        //domain_id = "168",
                        domain_code = item.levelcode,
                        domain_name = item.levelname,
                        spell_code = " ",
                        wb_code = " ",
                        parent_domain_id = 0,
                        note = "1",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };

                    var result = conn.Insert<code_domain_entity>(codedomain, commandTimeout: 15);

                }
            }
        }

        #endregion


        #region 二级 domain域
        private void InsertCodeDomainLevel2(IList<mdmExcelRawEntity> codesyslist, string connstr)
        {
            var domains = codesyslist.Where(p => !string.IsNullOrEmpty(p.二级域)).Select(p => new { levelcode = p.二级域代码, levelname = p.二级域, levl1code = p.一级域代码, levl1name = p.一级域 }).Distinct();
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in domains)
                {
                    var codedomain = new code_domain_entity
                    {
                        //domain_id = "168",
                        domain_code = item.levelcode,
                        domain_name = item.levelname,
                        spell_code = " ",
                        wb_code = " ",
                        parent_domain_id = 0,
                        note = "2",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };


                    var parentdomid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"1"}' and domain_code = '{item.levl1code}'").FirstOrDefault();

                    codedomain.parent_domain_id = parentdomid;

                    var result = conn.Insert<code_domain_entity>(codedomain, commandTimeout: 15);

                }
            }
        }

        #endregion


        #region 三级 domain域
        private void InsertCodeDomainLevel3(IList<mdmExcelRawEntity> codesyslist, string connstr)
        {
            var domains = codesyslist.Where(p => !string.IsNullOrEmpty(p.三级域)).Select(p => new { levelcode = p.三级域代码, levelname = p.三级域, levl1code = p.一级域代码, levl1name = p.一级域, levl2code = p.二级域代码, levl2name = p.二级域 }).Distinct();
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in domains)
                {
                    var codedomain = new code_domain_entity
                    {
                        //domain_id = "168",
                        domain_code = item.levelcode,
                        domain_name = item.levelname,
                        spell_code = " ",
                        wb_code = " ",
                        parent_domain_id = 0,
                        note = "3",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };

                    var parentdomid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"2"}' and domain_code = '{item.levl2code}'").FirstOrDefault();

                    codedomain.parent_domain_id = parentdomid;


                    var result = conn.Insert<code_domain_entity>(codedomain, commandTimeout: 15);

                }
            }
        }

        #endregion


        #region 生成代码系统

        public void InsertCodeSystem(IList<mdmExcelRawEntity> codesyslist, string connstr, string version)
        {
            var domains = codesyslist.Where(p => !string.IsNullOrEmpty(p.代码系统)).Distinct();
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in domains)
                {
                    var levelcode = GetCodeSystemLevel(item.标准级别.Trim());
                    var codedomain = new code_system_entity
                    {
                        //code_sys_id = "779",
                        code_sys_code = item.代码系统,
                        code_sys_name = item.代码系统名称,
                        spell_code = " ",
                        wb_code = " ",
                        version = version,
                        domain_id = 1,
                        level_code = levelcode.ToString(),
                        source_sys_id = 0,
                        source_note = item.标准来源,
                        is_value_set = true,
                        is_class = false,
                        note = item.示例数据,
                        is_valid = true,
                        oper_id = 3,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };


                    int domainid = 0;
                    if (!string.IsNullOrEmpty(item.三级域代码))
                    {
                        domainid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"3"}' and domain_code = '{item.三级域代码}'").FirstOrDefault();
                    }
                    else if (!string.IsNullOrEmpty(item.二级域代码))
                    {
                        domainid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"2"}' and domain_code = '{item.二级域代码}'").FirstOrDefault();
                    }
                    else
                    {
                        domainid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"1"}' and domain_code = '{item.一级域代码}'").FirstOrDefault();
                    }
                    codedomain.domain_id = domainid;


                    var result = conn.Insert<code_system_entity>(codedomain, commandTimeout: 15);

                }
            }

        }


        public int GetCodeSystemLevel(string name)
        {
            if (name.Contains("国家标准"))
            {
                return 2;
            }
            else if (name.Contains("行业标准"))
            {
                return 3;
            }
            else if (name.Contains("地方标准"))
            {
                return 4;
            }
            else if (name.Contains("企业标准"))
            {
                return 5;
            }
            else if (name.Contains("院内字典"))
            {
                return 6;
            }
            else if (name.Contains("其他标准"))
            {
                return 7;
            }
            else if (name.Contains("国际标准"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }


        #endregion


        #region 生成代码值集
        public void InsertCodeSet(string mdmexcelpath, string dbConnName, string sheetName)
        {
            Workbook workbook = null;

            IList<code_set_excel_entity> codesetexcels = new List<code_set_excel_entity>();

            using (FileStream fs = new FileStream(mdmexcelpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }

            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "代码明细";
            }
            Worksheet sheet = workbook.Worksheets[sheetName];
            var cells = sheet.Cells;

            for (int i = 0; i < cells.MaxDataRow; i++)
            {

                var row = cells.CheckRow(i);
                if (row == null || row.FirstCell == null)
                {
                    continue;
                }

                if (row.GetCellOrNull(0).StringValue == "代码系统编码")
                {
                    continue;
                }
                else
                {
                    var aaa = new code_set_excel_entity();
                    aaa.code_sys_code = row.GetCellOrNull(0).StringValue;
                    aaa.code_sys_name = row.GetCellOrNull(1).StringValue;
                    aaa.code = row.GetCellOrNull(2).StringValue;
                    aaa.name = row.GetCellOrNull(3).StringValue;
                    aaa.memo = row.GetCellOrNull(4).StringValue;
                    codesetexcels.Add(aaa);
                }



            }
            if (string.IsNullOrEmpty(dbConnName))
            {
                dbConnName = "hdr";
            }
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings[dbConnName].ConnectionString;
            //select setval(pg_get_serial_sequence('mdm.code_set', 'code_id'), max(code_id), true)  from mdm.code_set 设置序列最大值

            //删除数据
            //delete from mdm.code_set where etl_time > current_TIMESTAMP - interval '1 days' 
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in codesetexcels)
                {
                    var code_sys_id = conn.Query<int>($"select code_sys_id from mdm.code_system where code_sys_code = '{item.code_sys_code}' and oper_time > current_TIMESTAMP - interval '1 days'").FirstOrDefault();
                    var codeistm = new code_set_entity
                    {
                        //code_id = "879 ",
                        code = item.code,
                        name = item.name,
                        show_name = item.name,
                        spell_code = "",
                        wb_code = "",
                        code_sys_id = code_sys_id,
                        is_std = true,
                        std_class_id = 0,
                        unit_id = 0,
                        unit_name = null,
                        value_type = 0,
                        range = null,
                        range_low = null,
                        range_high = null,
                        note = item.memo,
                        quote_code_id = 0,
                        state = 1,
                        sort_no = 1,
                        oper_id = 3,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0,
                        is_valid = true
                    };



                    conn.Insert<code_set_entity>(codeistm);
                }


            }





        }
        #endregion


        #region 导出code system code set为excel文件  导出为文件 分类域  代码系统  代码集合

        /*
         with a as (select code_sys_id,jsonb_agg(jsonb_build_object('code_id',code_id , 'code',code , 'name',name , 'show_name',show_name , 'spell_code',spell_code , 'wb_code',wb_code , 'code_sys_id',code_sys_id , 'is_std',is_std , 'std_class_id',std_class_id , 'unit_id',unit_id , 'unit_name',unit_name , 'value_type',value_type , 'range',range , 'range_low',range_low , 'range_high',range_high , 'note',note , 'quote_code_id',quote_code_id , 'state',state , 'sort_no',sort_no , 'oper_id',oper_id , 'oper_time',oper_time , 'etl_time',etl_time , 'tenant_id',tenant_id , 'is_valid',is_valid ))  codeset from mdm.code_set group by code_sys_id )
,b as (select * from mdm.code_system b1 inner join a on a.code_sys_id = b1.code_sys_id)
select * from b
         
         */
        public void ExportCodeSysCodeSet(string excelPath, string sheetName, string dbConnName)
        {
            if (string.IsNullOrEmpty(dbConnName))
            {
                dbConnName = "hdr";
            }
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings[dbConnName].ConnectionString;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "代码系统";
            }

            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                //where b1.etl_time < current_TIMESTAMP - interval '1 days'
                var result = conn.Query<export_code_sys_code_set>(@"with a as (select code_sys_id,jsonb_agg(jsonb_build_object('code_id',code_id , 'code',code , 'name',name , 'show_name',show_name , 'spell_code',spell_code , 'wb_code',wb_code , 'code_sys_id',code_sys_id , 'is_std',is_std , 'std_class_id',std_class_id , 'unit_id',unit_id , 'unit_name',unit_name , 'value_type',value_type , 'range',range , 'range_low',range_low , 'range_high',range_high , 'note',note , 'quote_code_id',quote_code_id , 'state',state , 'sort_no',sort_no , 'oper_id',oper_id , 'oper_time',oper_time , 'etl_time',etl_time , 'tenant_id',tenant_id , 'is_valid',is_valid ))  codeset from mdm.code_set group by code_sys_id )
,b as (select b1.*,a.codeset from mdm.code_system b1 inner join a on a.code_sys_id = b1.code_sys_id  where b1.etl_time > current_TIMESTAMP - interval '1 days')
select * from b");

                ExportCodeSysCodeSetToExcel(excelPath, sheetName, result);

            }

        }
        #endregion


        #region 导出为Excel文件
        public void ExportCodeSysCodeSetToExcel(string excelPath, string sheetName, IEnumerable<export_code_sys_code_set> lst)
        {
            AsposeHelper helper = new AsposeHelper();

            Workbook workbook;
            using (FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new Workbook(fs);
            }

            string exportPath = Path.Combine(AppContext.BaseDirectory, $"{Path.GetFileNameWithoutExtension(excelPath)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

            IList<Style> styles = helper.CheckTest(workbook);

            Worksheet wshet = workbook.Worksheets[sheetName];
            var cellscodesys = wshet.Cells;

            int j = 0;

            for (int k = 0; k < cellscodesys.MaxDataRow; k++)
            {
                var row = cellscodesys.CheckRow(k);
                if (row == null || row.FirstCell == null)
                {
                    continue;
                }

                if (row.GetCellOrNull(0).StringValue == "代码系统代码")
                {
                    continue;
                }
                else
                {
                    var sdsf = row.GetCellOrNull(6).StringValue;

                    if (!string.IsNullOrWhiteSpace(sdsf))
                    {
                        var item = lst.FirstOrDefault(p => p.code_sys_code.Trim() == sdsf.Trim());
                        if (item == null)
                        {
                            Serilog.Log.Information("代码系统：{codesys} 未存在对应的代码集合！", sdsf);
                            continue;
                        }

                        ++j;

                        string sheetname = item.code_sys_code.Remove(0, 4);  //去除mdm_ 及 hdr_ 等前缀
                        if (sheetname.Length > 31)
                        {
                            sheetname = sheetname.Substring(0, 31);
                        }
                        var sheet = workbook.Worksheets.Add(sheetname);
                        var cells = sheet.Cells;

                        IList<code_set_entity> codesets = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<code_set_entity>>(item.codeset);


                        #region 代码集合 表头
                        //第一行
                        int rowheader = 0;
                        var cel = cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))];
                        cel.Value = "返回";


                        //第二行
                        ++rowheader;
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].Value = "代码系统代码";
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[0]);

                        cells.Merge(rowheader, CellsHelper.ColumnNameToIndex("B"), 1, 2);
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].Value = item.code_sys_code;
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].GetMergedRange().SetStyle(styles[2]);



                        //第三行
                        ++rowheader;
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].Value = "代码系统名称";
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[0]);

                        cells.Merge(rowheader, CellsHelper.ColumnNameToIndex("B"), 1, 2);
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].Value = item.code_sys_name;
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].GetMergedRange().SetStyle(styles[2]);


                        //第四行
                        ++rowheader;
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].Value = "代码";
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[0]);

                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].Value = "名称";
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("B"))].SetStyle(styles[0]);

                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("c"))].Value = "显示名";
                        cells[CellsHelper.CellIndexToName(rowheader, CellsHelper.ColumnNameToIndex("c"))].SetStyle(styles[0]);

                        #endregion

                        #region 创建链接
                        //sheetlist.Hyperlinks.Add(CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C")), 1, 1, $"{sheet.Name}!{cel.Name}"); //查看链接
                        //sheet.Hyperlinks.Add(cel.Name, 1, 1, $"{sheetlist.Name}!{ CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C")) }");//返回链接

                        //P列 增减快捷链接
                        var cellName = CellsHelper.CellIndexToName(k, CellsHelper.ColumnNameToIndex("P"));
                        cellscodesys[cellName].Value = "查看";
                        cellscodesys[cellName].Worksheet.Hyperlinks.Add(cellName, cellName, $"{sheet.Name}!{cel.Name}", "", "");
                        //返回链接
                        sheet.Hyperlinks.Add(cel.Name, 1, 1, $"{cellscodesys[cellName].Worksheet.Name}!{cellName}");//返回链接
                        #endregion

                        #region 填充代码集合 codeset
                        int offset = ++rowheader;
                        for (int i = 0; i < codesets.Count; i++)
                        {
                            var code = codesets[i].code;
                            var name = codesets[i].name;
                            var showname = codesets[i].show_name;

                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("A"))].Value = code;
                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("B"))].Value = name;
                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("B"))].SetStyle(styles[2]);

                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("C"))].Value = showname;
                            cells[CellsHelper.CellIndexToName(i + offset, CellsHelper.ColumnNameToIndex("C"))].SetStyle(styles[2]);

                        }
                        sheet.IsGridlinesVisible = false;


                        #region 设置宽度
                        cells.SetColumnWidth(0, 16);
                        cells.SetColumnWidth(1, 30);
                        cells.SetColumnWidth(2, 35);
                        //sheet.AutoFitColumns();
                        #endregion


                        #endregion
                    }


                }
            }


            //sheetlist.IsGridlinesVisible = false;
            //sheetlist.AutoFitColumns();
            workbook.Save(exportPath, SaveFormat.Xlsx);

        }

        #endregion


        #region 导入代码系统
        public void ImportCodeSysCodeSetToExcel(string codesysPath, string dbConnName, bool isclearandresetseq, string version)
        {

            if (string.IsNullOrEmpty(dbConnName))
            {
                dbConnName = "hdr";
            }
            string connstr = System.Configuration.ConfigurationManager.ConnectionStrings[dbConnName].ConnectionString;

            if (isclearandresetseq)
            {//清理
                ClearandResetMdmCodeSysCodeset(connstr);

            }
            //插入代码系统 
            BuildMdmCodeSystem(codesysPath, "", dbConnName, version);

            //插入值集
            BuildMdmCodeSet(codesysPath, "", dbConnName);
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
                else if (row.GetCellOrNull(0).StringValue.Contains("代码"))
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

            #region 插入数据
            var connstr = System.Configuration.ConfigurationManager.ConnectionStrings[dbConnName].ConnectionString;
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in codesystemCels)
                {
                    var code_sys_id_list = conn.Query<int>($"select code_sys_id from mdm.code_system where code_sys_code = '{item.code_sys_code}' ").ToList();
                    int code_sys_id = -1;
                    if (code_sys_id_list == null || code_sys_id_list.Count == 0)
                    {
                        Serilog.Log.Warning($"{item.code_sys_code}{'\t'}{item.code_sys_name}");
                    }
                    else
                    {
                        code_sys_id = code_sys_id_list.FirstOrDefault();
                    }

                    var codesets = codesetexcels1.Where(p => p.code_sys_code == item.code_sys_code);
                    int sortno = 0;
                    foreach (var codeset in codesets)
                    {

                        var codeistm = new code_set_entity
                        {
                            //code_id = "879 ",
                            code = codeset.code,
                            name = codeset.name,
                            show_name = codeset.show_name,
                            spell_code = "",
                            wb_code = "",
                            code_sys_id = code_sys_id,
                            is_std = true,
                            std_class_id = 0,
                            unit_id = 0,
                            unit_name = null,
                            value_type = 0,
                            range = null,
                            range_low = null,
                            range_high = null,
                            note = item.show_name,
                            quote_code_id = 0,
                            state = 1,
                            sort_no = ++sortno,
                            oper_id = 3,
                            oper_time = DateTime.Now,
                            etl_time = DateTime.Now,
                            tenant_id = 0,
                            is_valid = true
                        };
                        conn.Insert<code_set_entity>(codeistm);
                    }
                }

                #region 拼音码、五笔码
                //生成拼音、五笔码
                conn.Execute("update mdm.code_set set spell_code = substring(get_pym_pg(name) from 0 for 20) ,wb_code = substring(get_wbm(name) from 0 for 20) ", commandTimeout: 300);
                conn.Execute("update mdm.code_domain set spell_code = substring(get_pym_pg(domain_name) from 0 for 20) ,wb_code = substring(get_wbm(domain_name) from 0 for 20)", commandTimeout: 300);
                conn.Execute("update mdm.code_system set spell_code = substring(get_pym_pg(code_sys_name) from 0 for 20),wb_code = substring(get_wbm(code_sys_name) from 0 for 20) ", commandTimeout: 300);

                #endregion
            }



            #endregion

        }

        #endregion


        #region 清理重置代码系统
        public void ClearandResetMdmCodeSysCodeset(string connstr)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                //清除数据
                conn.Execute("truncate table mdm.code_domain;");
                conn.Execute("truncate table mdm.code_set;");
                conn.Execute("truncate table mdm.code_system;");

                //重置序列 先执行
                conn.Execute("alter sequence mdm.code_domain_domain_id_seq minvalue 0 start with 1;");
                conn.Execute("alter sequence mdm.code_set_code_id_seq1 minvalue 0 start with 1;");
                conn.Execute("alter sequence mdm.code_system_code_sys_id_seq1 minvalue 0 start with 1;");

                //再执行 从1开始递增
                conn.Execute("SELECT setval('mdm.code_domain_domain_id_seq', 0)");
                conn.Execute("SELECT setval('mdm.code_set_code_id_seq1', 0)");
                conn.Execute("SELECT setval('mdm.code_system_code_sys_id_seq1', 0)");

            }
        }

        #endregion

    }
}
