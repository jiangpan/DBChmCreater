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
using Pinyin4net;
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
        public void BuildMdmCodeSystem(string excelPath, string sheetName, string dbConnName)
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

            InsertCodeSystem(codeSyslist, connstr);

        }

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
                        //spell_code = "yyfs",
                        //wb_code = "yyfs",
                        parent_domain_id = 0,
                        note = "1",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };
                    List<string> aaa = new List<string>();
                    for (int i = 0; i < item.levelname.Length; i++)
                    {
                        var pinyin = Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(item.levelname[i])?.FirstOrDefault();
                        aaa.Add(pinyin);
                    }
                    codedomain.spell_code = string.Join("", aaa.Select(p => p?.ToUpper()?.Substring(0, 1)));
                    codedomain.wb_code = codedomain.spell_code;
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
                        //spell_code = "yyfs",
                        //wb_code = "yyfs",
                        parent_domain_id = 0,
                        note = "2",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };
                    List<string> aaa = new List<string>();
                    for (int i = 0; i < item.levelname.Length; i++)
                    {
                        var pinyin = Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(item.levelname[i])?.FirstOrDefault();
                        aaa.Add(pinyin);
                    }

                    var parentdomid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"1"}' and domain_code = '{item.levl1code}'").FirstOrDefault();

                    codedomain.parent_domain_id = parentdomid;

                    codedomain.spell_code = string.Join("", aaa.Select(p => p?.ToUpper()?.Substring(0, 1)));
                    codedomain.wb_code = codedomain.spell_code;
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
                        //spell_code = "yyfs",
                        //wb_code = "yyfs",
                        parent_domain_id = 0,
                        note = "3",
                        oper_id = 2,
                        oper_time = DateTime.Now,
                        etl_time = DateTime.Now,
                        tenant_id = 0
                    };
                    List<string> aaa = new List<string>();
                    for (int i = 0; i < item.levelname.Length; i++)
                    {
                        var pinyin = Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(item.levelname[i])?.FirstOrDefault();
                        aaa.Add(pinyin);
                    }
                    codedomain.spell_code = string.Join("", aaa.Select(p => p?.ToUpper()?.Substring(0, 1)));
                    codedomain.wb_code = codedomain.spell_code;

                    var parentdomid = conn.Query<int>($"select domain_id from mdm.code_domain where  note = '{"2"}' and domain_code = '{item.levl2code}'").FirstOrDefault();

                    codedomain.parent_domain_id = parentdomid;


                    var result = conn.Insert<code_domain_entity>(codedomain, commandTimeout: 15);

                }
            }
        }

        #endregion


        #region 生成代码系统

        public void InsertCodeSystem(IList<mdmExcelRawEntity> codesyslist, string connstr)
        {
            var domains = codesyslist.Where(p => !string.IsNullOrEmpty(p.代码系统)).Distinct();
            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                foreach (var item in domains)
                {
                    var codedomain = new code_system_entity
                    {
                        //code_sys_id = "779",
                        code_sys_code = item.代码系统,
                        code_sys_name = item.代码系统名称,
                        //spell_code = "ZYBLWSLXYJML",
                        //wb_code = "WBUDYNOG_GXHV",
                        version = "1.0.9",
                        domain_id = 1,
                        level_code = "5",
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
                    List<string> aaa = new List<string>();
                    for (int i = 0; i < item.代码系统名称.Length; i++)
                    {
                        var pinyin = Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(item.代码系统名称[i])?.FirstOrDefault();
                        aaa.Add(pinyin);
                    }
                    codedomain.spell_code = string.Join("", aaa.Select(p => p?.ToUpper()?.Substring(0, 1)));
                    codedomain.wb_code = codedomain.spell_code;

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
                if (row == null || row.FirstDataCell == null)
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
                    var codeistm = new code_set_entity {
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

                    IList<string> aaa = new List<string>();
                    for (int i = 0; i < item.name.Length; i++)
                    {
                        var pinyin = Pinyin4net.PinyinHelper.ToHanyuPinyinStringArray(item.name[i])?.FirstOrDefault();
                        aaa.Add(pinyin);
                    }
                    codeistm.spell_code = string.Join("", aaa.Select(p => p?.ToUpper()?.Substring(0, 1)));
                    codeistm.wb_code = codeistm.spell_code;

                    conn.Insert<code_set_entity>(codeistm);
                }


            }





            }
        #endregion


        #region 导出code system code set为excel文件

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

            using (NpgsqlConnection conn = new NpgsqlConnection(connstr))
            {
                //where b1.etl_time < current_TIMESTAMP - interval '1 days'
                var result = conn.Query<export_code_sys_code_set>(@"with a as (select code_sys_id,jsonb_agg(jsonb_build_object('code_id',code_id , 'code',code , 'name',name , 'show_name',show_name , 'spell_code',spell_code , 'wb_code',wb_code , 'code_sys_id',code_sys_id , 'is_std',is_std , 'std_class_id',std_class_id , 'unit_id',unit_id , 'unit_name',unit_name , 'value_type',value_type , 'range',range , 'range_low',range_low , 'range_high',range_high , 'note',note , 'quote_code_id',quote_code_id , 'state',state , 'sort_no',sort_no , 'oper_id',oper_id , 'oper_time',oper_time , 'etl_time',etl_time , 'tenant_id',tenant_id , 'is_valid',is_valid ))  codeset from mdm.code_set group by code_sys_id )
,b as (select b1.*,a.codeset from mdm.code_system b1 inner join a on a.code_sys_id = b1.code_sys_id  where b1.etl_time > current_TIMESTAMP - interval '1 days')
select * from b");
                ExportCodeSysCodeSetToExcel(result);


            }

        }
        #endregion

        #region 导出为Excel文件
        public void ExportCodeSysCodeSetToExcel(IEnumerable<export_code_sys_code_set> lst)
        {
            AsposeHelper helper = new AsposeHelper();


            string exportPath = Path.Combine(AppContext.BaseDirectory, $"mdm_export_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            Workbook wb = new Workbook();
            wb.Worksheets.Clear();
            IList<Style> styles = helper.CheckTest(wb);

            var sheetlist = wb.Worksheets.Add("代码系统清单");
            var cellindex = sheetlist.Cells;
            cellindex.Merge(0, CellsHelper.ColumnNameToIndex("A"), 1, 3);
            cellindex[CellsHelper.CellIndexToName(0, CellsHelper.ColumnNameToIndex("A"))].Value = "代码系统索引";
            cellindex[CellsHelper.CellIndexToName(0, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("A"))].Value = "代码系统代码";
            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("B"))].Value = "代码系统名称";
            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("B"))].SetStyle(styles[2]);


            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("C"))].Value = "操作";
            cellindex[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("C"))].SetStyle(styles[2]);


            int codesysoffset = 2;


            for (int j = 0; j < lst.Count(); j++)
            {
                var item = lst.ElementAt(j);


                #region 生成目录
                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("A"))].Value = item.code_sys_code;
                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("B"))].Value = item.code_sys_name;
                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("B"))].SetStyle(styles[2]);

                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C"))].Value = "查看";
                cellindex[CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C"))].SetStyle(styles[2]);


                #endregion

                string sheetname = item.code_sys_code;
                if (item.code_sys_code.Length > 31)
                {
                    sheetname = sheetname.Substring(0, 31);
                }
                var sheet = wb.Worksheets.Add(sheetname);
                var cells = sheet.Cells;



                IList<code_set_entity> codesets = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<code_set_entity>>(item.codeset);
                var cel = cells[CellsHelper.CellIndexToName(0, CellsHelper.ColumnNameToIndex("A"))];
                cel.Value = "返回";

                cells.Merge(1, CellsHelper.ColumnNameToIndex("A"), 1, 2);
                cells[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("A"))].Value = item.code_sys_name;
                cells[CellsHelper.CellIndexToName(1, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("A"))].Value = "代码";
                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("A"))].SetStyle(styles[2]);

                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("B"))].Value = "名称";
                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("B"))].SetStyle(styles[2]);

                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("c"))].Value = "显示名";
                cells[CellsHelper.CellIndexToName(2, CellsHelper.ColumnNameToIndex("c"))].SetStyle(styles[2]);


                #region 创建链接
                sheetlist.Hyperlinks.Add(CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C")), 1, 1, $"{sheet.Name}!{cel.Name}");
                sheet.Hyperlinks.Add(cel.Name, 1, 1, $"{sheetlist.Name}!{ CellsHelper.CellIndexToName(j + codesysoffset, CellsHelper.ColumnNameToIndex("C")) }");
                #endregion

                int offset = 3;
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
                sheet.AutoFitColumns();
            }
            sheetlist.IsGridlinesVisible = false;
            sheetlist.AutoFitColumns();
            wb.Save(exportPath, SaveFormat.Xlsx);

        }

        #endregion




    }
}
