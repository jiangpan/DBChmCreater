/* ==============================================================================
   * 文 件 名:DbCommon
   * 功能描述：
   * Copyright (c) 2012 上海森亿医疗科技有限公司
   * 创 建 人: jiangpan
   * 创建时间: 2020/2/23 15:21:25
   * 修 改 人: 
   * 修改时间: 
   * 修改描述: 
   * 版    本: v1.0.0.0
   * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DBChmCreater.Ext;
using Synyi.DBChmCreater.Entity;

namespace DBChmCreater.DB
{
    /// <summary>
    /// 常用功能类
    /// </summary>
    public class HtmlHelp
    {
        #region 导出表数据为html格式


        #region 导出单个表结构说明
        /// <summary>
        ///  导出表数据为html格式 居中表格样式
        /// </summary>
        /// <param name="dt">DataTable，需要给TableName赋值</param>
        /// <param name="KeepNull">保持Null为Null值，否则为空</param>
        /// <param name="Path">保存路径</param>
        /// <param name="hasReturn">携带返回目录链接</param>
        /// <param name="tableDesc">携带返回目录链接</param>
        public static void CreateHtml(DataTable dt, bool KeepNull, string Path, bool hasReturn = true, string tableDesc = "")
        {
            var code = new StringBuilder();
            code.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            code.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            code.AppendLine("<head>");
            code.AppendLine("    <META http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\"> ");
            code.AppendLine($"    <title>{dt.TableName}</title>");
            code.AppendLine("    <style type=\"text/css\">");
            code.AppendLine("        body");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 9pt;");
            code.AppendLine("        }");
            code.AppendLine("        .styledb");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 14px;");
            code.AppendLine("        }");
            code.AppendLine("        .styletab");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 14px;");
            code.AppendLine("            padding-top: 15px;");
            code.AppendLine("        }");
            code.AppendLine("        a");
            code.AppendLine("        {");
            code.AppendLine("            color: #015FB6;");
            code.AppendLine("        }");
            code.AppendLine("        a:link, a:visited, a:active");
            code.AppendLine("        {");
            code.AppendLine("            color: #015FB6;");
            code.AppendLine("            text-decoration: none;");
            code.AppendLine("        }");
            code.AppendLine("        a:hover");
            code.AppendLine("        {");
            code.AppendLine("            color: #E33E06;");
            code.AppendLine("        }");
            code.AppendLine("    </style>");
            code.AppendLine("</head>");
            code.AppendLine("<body>");
            code.AppendLine("    <div style=\"text-align: center\">");
            code.AppendLine("        <div>");
            code.AppendLine("            <table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" width=\"90%\" style=\"text-align: left\">");
            code.AppendLine("                <tr>");
            code.AppendLine("                    <td bgcolor=\"#FBFBFB\">");
            code.AppendLine("                        <table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" width=\"100%\" bordercolorlight=\"#D7D7E5\" bordercolordark=\"#D3D8E0\">");
            code.AppendLine("                        <caption>");
            code.AppendLine($"        <div class=\"styletab\">{dt.TableName}{(tableDesc.Length == 0 ? string.Empty : "  （" + tableDesc + "） ")}{(hasReturn ? "<a href ='../数据库表目录.html' style = 'float: left; margin-top: 6px;'>返回目录</a>" : string.Empty)}</div>");
            //.FormatString(dt.TableName,
            //tableDesc.Length == 0 ? string.Empty : "  （" + tableDesc + "） ",
            //(hasReturn ? "<a href='../数据库表目录.html' style='float: right; margin-top: 6px;'>返回目录</a>" : string.Empty));
            code.AppendLine("                        </caption>");
            code.AppendLine("                        <tr bgcolor=\"#DEEBF7\">");  //bgcolor="#DEEBF7"
            //构建表头
            foreach (DataColumn dc in dt.Columns)
            {
                code.AppendLine($"            <td>{dc.ColumnName}</td>");//.FormatString(dc.ColumnName));
            }
            code.AppendLine("                         </tr>");
            //构建数据行
            foreach (DataRow dr in dt.Rows)
            {
                code.AppendLine("            <tr>");
                foreach (DataColumn dc in dt.Columns)
                {
                    if (KeepNull && dr[dc.ColumnName] == DBNull.Value)
                    {
                        code.AppendLine("            <td>&nbsp;</td>");
                    }
                    else
                    {
                        code.AppendLine($"            <td>{(dr[dc.ColumnName].ToString().Trim().Length > 0 ? dr[dc.ColumnName].ToString() : "&nbsp;")}</td>");//.FormatString(
                                                                                                                                                              //dr[dc.ColumnName].ToString().Trim().Length > 0 ? dr[dc.ColumnName].ToString() : "&nbsp;"));
                    }
                }
                code.AppendLine("            </tr>");
            }
            code.AppendLine("                        </table>");
            code.AppendLine("                    </td>");
            code.AppendLine("                </tr>");
            code.AppendLine("            </table>");
            code.AppendLine("        </div>");
            code.AppendLine("    </div>");
            code.AppendLine("</body>");
            code.AppendLine("</html>");
            File.WriteAllText(Path, code.ToString(), Encoding.GetEncoding("gb2312"));
            //File.WriteAllText(Path, code.ToString(), Encoding.UTF8);
        }

        #endregion


        #region 导出数据库表目录列表
        /// <summary>
        ///  导出表数据为html格式 居中表格样式
        /// </summary>
        /// <param name="dt">DataTable，需要给TableName赋值</param>
        /// <param name="KeepNull">保持Null为Null值，否则为空</param>
        /// <param name="Path">保存路径</param>
        /// <param name="hasReturn">携带返回目录链接</param>
        /// <param name="tableDesc">携带返回目录链接</param>
        public static void CreateHtml(Synyi.DBChmCreater.Entity.DataTableCollection dt, bool KeepNull, string Path, bool hasReturn = true, string tableDesc = "")
        {
            var code = new StringBuilder();
            code.AppendLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            code.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            code.AppendLine("<head>");
            code.AppendLine("    <META http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\"> ");
            code.AppendLine($"    <title>{dt.Name}</title>");
            code.AppendLine("    <style type=\"text/css\">");
            code.AppendLine("        body");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 9pt;");
            code.AppendLine("        }");
            code.AppendLine("        .styledb");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 14px;");
            code.AppendLine("        }");
            code.AppendLine("        .styletab");
            code.AppendLine("        {");
            code.AppendLine("            font-size: 14px;");
            code.AppendLine("            padding-top: 15px;");
            code.AppendLine("        }");
            code.AppendLine("        a");
            code.AppendLine("        {");
            code.AppendLine("            color: #015FB6;");
            code.AppendLine("        }");
            code.AppendLine("        a:link, a:visited, a:active");
            code.AppendLine("        {");
            code.AppendLine("            color: #015FB6;");
            code.AppendLine("            text-decoration: none;");
            code.AppendLine("        }");
            code.AppendLine("        a:hover");
            code.AppendLine("        {");
            code.AppendLine("            color: #E33E06;");
            code.AppendLine("        }");
            code.AppendLine("    </style>");
            code.AppendLine("</head>");
            code.AppendLine("<body>");
            code.AppendLine("    <div style=\"text-align: center\">");
            code.AppendLine("        <div>");
            code.AppendLine("            <table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" width=\"90%\" style=\"text-align: left\">");
            code.AppendLine("                <tr>");
            code.AppendLine("                    <td bgcolor=\"#FBFBFB\">");
            code.AppendLine("                        <table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" width=\"100%\" bordercolorlight=\"#D7D7E5\" bordercolordark=\"#D3D8E0\">");
            code.AppendLine("                        <caption>");
            code.AppendLine($"        <div class=\"styletab\">{dt.Name}{(tableDesc.Length == 0 ? string.Empty : "  （" + tableDesc + "） ")}{(hasReturn ? "<a href='../数据库表目录.html' style='float: right; margin-top: 6px;'>返回目录</a>" : string.Empty)}</div>");//.FormatString(
            code.AppendLine("                        </caption>");
            code.AppendLine("                        <tr bgcolor=\"#DEEBF7\">");
            //构建表头

            Type itemtype = typeof(DataTableItem);

            var dtStructProps = itemtype.GetProperties();

            foreach (PropertyInfo dc in dtStructProps)
            {
                var prop = dc.GetCustomAttribute<DescriptionAttribute>();
                code.AppendLine($"            <td>{prop.Description}</td>");//属性描述 作为列名
            }
            code.AppendLine("                         </tr>");
            //构建数据行

            var dtgroupByDomain = dt.GroupBy(p => p.Domain).OrderBy(p => p.Key);
            foreach (var domainitem in dtgroupByDomain)
            {
                //一个domain
                int count = domainitem.Count();
                int i = 0;
                var ere = domainitem.OrderBy(p => p.TableName);
                foreach (DataTableItem dritem in ere)
                {
                    code.AppendLine("            <tr>");
                    if (i == 0)
                    {
                        string sfsd = $"rowspan = \"{count}\"";
                        //合并单元格
                        foreach (PropertyInfo dc in dtStructProps)
                        {
                            if (KeepNull && dc.GetValue(dritem) == DBNull.Value)
                            {
                                code.AppendLine("            <td>&nbsp;</td>");
                            }
                            else
                            {
                                if (dc.Name == "Domain")
                                {

                                    code.AppendLine($"            <td {sfsd}>{(dc.GetValue(dritem).ToString().Trim().Length > 0 ? dc.GetValue(dritem).ToString() : "&nbsp;")}</td>");
                                }
                                else
                                {
                                    code.AppendLine($"            <td>{(dc.GetValue(dritem).ToString().Trim().Length > 0 ? dc.GetValue(dritem).ToString() : "&nbsp;")}</td>");
                                }
                            }
                        }

                    }
                    else
                    {
                        foreach (PropertyInfo dc in dtStructProps)
                        {
                            if (dc.Name == "Domain")
                            {
                                continue;
                            }
                            if (KeepNull && dc.GetValue(dritem) == DBNull.Value)
                            {
                                code.AppendLine("            <td>&nbsp;</td>");
                            }
                            else
                            {
                                code.AppendLine($"            <td>{(dc.GetValue(dritem).ToString().Trim().Length > 0 ? dc.GetValue(dritem).ToString() : "&nbsp;")}</td>");
                            }
                        }


                    }
                    i++;
                    code.AppendLine("            </tr>");
                }


            }



            //foreach (DataTableStructure dr in dt)
            //{
            //    code.AppendLine("            <tr>");
            //    foreach (PropertyInfo dc in dtStructProps)
            //    {
            //        if (KeepNull &&  dc.GetValue(dr) == DBNull.Value)
            //        {
            //            code.AppendLine("            <td>&nbsp;</td>");
            //        }
            //        else
            //        {
            //            code.AppendLine($"            <td>{(dc.GetValue(dr).ToString().Trim().Length > 0 ? dc.GetValue(dr).ToString() : "&nbsp;")}</td>");
            //        }
            //    }
            //    code.AppendLine("            </tr>");
            //}
            code.AppendLine("                        </table>");
            code.AppendLine("                    </td>");
            code.AppendLine("                </tr>");
            code.AppendLine("            </table>");
            code.AppendLine("        </div>");
            code.AppendLine("    </div>");
            code.AppendLine("</body>");
            code.AppendLine("</html>");
            File.WriteAllText(Path, code.ToString(), Encoding.GetEncoding("gb2312"));
            //File.WriteAllText(Path, code.ToString(), Encoding.UTF8);
        }


        #endregion


        #region 导出库表中的数据
        /// <summary>
        ///  导出表数据为html格式 Oracle导出格式 带搜索框
        /// </summary>
        /// <param name="dt">DataTable，需要给TableName赋值</param>
        /// <param name="KeepNull">保持Null为Null值，否则为空</param>
        /// <param name="Path">保存路径</param>
        public static void CreateHtml2(DataTable dt, bool KeepNull, string Path)
        {
            var code = new StringBuilder();
            code.AppendLine("<html>");
            code.AppendLine("<head>");
            code.AppendLine("    <META http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\"> ");
            code.AppendLine($"    <title>{dt.TableName}</title>");//.FormatString(dt.TableName));
            code.AppendLine("    <meta http-equiv=\"content-type\" content=\"text/html; charset=GBK\">");
            code.AppendLine("    <style type=\"text/css\">");
            code.AppendLine("        table");
            code.AppendLine("        {");
            code.AppendLine("            background-color: #F2F2F5;");
            code.AppendLine("            border-width: 1px 1px 0px 1px;");
            code.AppendLine("            border-color: #C9CBD3;");
            code.AppendLine("            border-style: solid;");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        td");
            code.AppendLine("        {");
            code.AppendLine("            color: #000000;");
            code.AppendLine("            font-family: Tahoma,Arial,Helvetica,Geneva,sans-serif;");
            code.AppendLine("            font-size: 9pt;");
            code.AppendLine("            background-color: #EAEFF5;");
            code.AppendLine("            padding: 8px;");
            code.AppendLine("            background-color: #F2F2F5;");
            code.AppendLine("            border-color: #ffffff #ffffff #cccccc #ffffff;");
            code.AppendLine("            border-style: solid solid solid solid;");
            code.AppendLine("            border-width: 1px 0px 1px 0px;");
            code.AppendLine("            border-right : #cccccc solid 1px;");
            code.AppendLine("            border-bottom: #cccccc solid 1px;");
            code.AppendLine("            border-top: #cccccc solid 1px;");
            code.AppendLine("            border-left: #cccccc solid 1px;");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        th");
            code.AppendLine("        {");
            code.AppendLine("            font-family: Tahoma,Arial,Helvetica,Geneva,sans-serif;");
            code.AppendLine("            font-size: 9pt;");
            code.AppendLine("            padding: 8px;");
            code.AppendLine("            background-color: #CFE0F1;");
            code.AppendLine("            border-color: #ffffff #ffffff #cccccc #ffffff;");
            code.AppendLine("            border-style: solid solid solid none;");
            code.AppendLine("            border-width: 1px 0px 1px 0px;");
            code.AppendLine("            white-space: nowrap;");
            code.AppendLine("            border-right : #cccccc solid 1px;");
            code.AppendLine("            border-bottom: #cccccc solid 1px;");
            code.AppendLine("            border-top: #cccccc solid 1px;");
            code.AppendLine("            border-left: #cccccc solid 1px;");
            code.AppendLine("        }");
            code.AppendLine("        a:link, a:visited, a:active");
            code.AppendLine("        {");
            code.AppendLine("            color: #015FB6;");
            code.AppendLine("            text-decoration: none;");
            code.AppendLine("        }");
            code.AppendLine("        a:hover");
            code.AppendLine("        {");
            code.AppendLine("            color: #E33E06;");
            code.AppendLine("        }");
            code.AppendLine("    </style>");
            code.AppendLine("    <script type=\"text/javascript\">");
            code.AppendLine("        window.apex_search = {};");
            code.AppendLine("        apex_search.init = function () {");
            code.AppendLine("            this.rows = document.getElementById('data').getElementsByTagName('TR');");
            code.AppendLine("            this.rows_length = apex_search.rows.length;");
            code.AppendLine("            this.rows_text = [];");
            code.AppendLine("            for (var i = 0; i < apex_search.rows_length; i++) {");
            code.AppendLine("                this.rows_text[i] = (apex_search.rows[i].innerText) ? apex_search.rows[i].innerText.toUpperCase() : apex_search.rows[i].textContent.toUpperCase();");
            code.AppendLine("            }");
            code.AppendLine("            this.time = false;");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        apex_search.lsearch = function () {");
            code.AppendLine("            this.term = document.getElementById('S').value.toUpperCase();");
            code.AppendLine("            for (var i = 0, row; row = this.rows[i], row_text = this.rows_text[i]; i++) {");
            code.AppendLine("                row.style.display = ((row_text.indexOf(this.term) != -1) || this.term === '') ? '' : 'none';");
            code.AppendLine("            }");
            code.AppendLine("            this.time = false;");
            code.AppendLine("        }");
            code.AppendLine();
            code.AppendLine("        apex_search.search = function (e) {");
            code.AppendLine("            var keycode;");
            code.AppendLine("            if (window.event) { keycode = window.event.keyCode; }");
            code.AppendLine("            else if (e) { keycode = e.which; }");
            code.AppendLine("            else { return false; }");
            code.AppendLine("            if (keycode == 13) {");
            code.AppendLine("                apex_search.lsearch();");
            code.AppendLine("            }");
            code.AppendLine("            else { return false; }");
            code.AppendLine("        }</script>");
            code.AppendLine("</head>");
            code.AppendLine("<body onload=\"apex_search.init();\">");
            code.AppendLine("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            code.AppendLine("        <tbody>");
            code.AppendLine("            <tr>    ");
            code.AppendLine("                <td>");
            code.AppendLine("                    <input type=\"text\" size=\"30\" maxlength=\"1000\" value=\"\" id=\"S\" onkeyup=\"apex_search.search(event);\" /><input type=\"button\" value=\"Search\" onclick=\"apex_search.lsearch();\" />");
            code.AppendLine("                </td>");
            code.AppendLine("                <td>");
            code.AppendLine("                        " + dt.TableName);
            code.AppendLine("                </td>");
            code.AppendLine("            </tr>");
            code.AppendLine("        </tbody>");
            code.AppendLine("    </table>");
            code.AppendLine("    <br />");
            code.AppendLine("    <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            code.AppendLine("        <tr>");
            foreach (DataColumn dc in dt.Columns)
            {
                code.AppendLine($"            <th>{dc.ColumnName}</th>");//.FormatString(dc.ColumnName));
            }
            code.AppendLine("        </tr>");
            code.AppendLine("        <tbody id=\"data\">");
            foreach (DataRow dr in dt.Rows)
            {
                code.AppendLine("            <tr>");
                foreach (DataColumn dc in dt.Columns)
                {
                    if (KeepNull && dr[dc.ColumnName] == DBNull.Value)
                    {
                        //code.AppendLine("            <td>{0}</td>".FormatString(dr[dc.ColumnName].ToString()));
                        code.AppendLine("            <td>&nbsp;</td>");
                    }
                    else//  align=\"right\"
                    {
                        //处理 array类型
                        if (dc.DataType.Name.IndexOf("Array") >= 0)
                        {//数组类型

                            var strValue = dr[dc.ColumnName].ToString();
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                string renderdValue = string.Empty;
                                if (dr[dc.ColumnName].GetType().Name.IndexOf("Int32[]", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    var arrayValue = dr[dc.ColumnName] as int[];
                                    renderdValue = $"{{{string.Join(",", arrayValue)}}}";

                                }
                                else
                                {
                                    var arrayValue = dr[dc.ColumnName] as int[];
                                    if (arrayValue != null)
                                    {
                                        renderdValue = $"{{{string.Join(",", arrayValue)}}}";

                                    }
                                    else
                                    {
                                        renderdValue = $"{{}}";
                                    }
                                }

                                code.AppendLine($"            <td>{renderdValue}</td>");//.FormatString(
                            }
                            else
                            {
                                code.AppendLine($"            <td>{"&nbsp;"}</td>");//.FormatString(

                            }



                        }
                        else
                        {
                            var strValue = dr[dc.ColumnName].ToString();
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                string renderdValue = strValue.Replace("<", " ").Replace(">", " ");
                                code.AppendLine($"            <td>{renderdValue}</td>");//.FormatString(
                                //code.AppendLine($"            <td>{(dr[dc.ColumnName].ToString().Length > 0 ? dr[dc.ColumnName].ToString() : "&nbsp;")}</td>");//.FormatString(
                            }
                            else
                            {
                                code.AppendLine($"            <td>{"&nbsp;"}</td>");//.FormatString(
                            }
                            //dr[dc.ColumnName].ToString().Length > 0 ? dr[dc.ColumnName].ToString() : "&nbsp;"));

                        }
                    }
                }
                code.AppendLine("            </tr>");
            }
            code.AppendLine("        </tbody>");
            code.AppendLine("    </table>");
            code.AppendLine("</body>");
            code.AppendLine("</html>");
            File.WriteAllText(Path, code.ToString(), Encoding.GetEncoding("gb2312"));
        }

        #endregion


        #endregion
    }
}
