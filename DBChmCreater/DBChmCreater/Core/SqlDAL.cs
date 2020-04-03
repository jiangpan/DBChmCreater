///* ==============================================================================
//   * 文 件 名:SqlDAL
//   * 功能描述：
//   * Copyright (c) 2012 上海森亿医疗科技有限公司
//   * 创 建 人: jiangpan
//   * 创建时间: 2020/2/23 15:22:18
//   * 修 改 人: 
//   * 修改时间: 
//   * 修改描述: 
//   * 版    本: v1.0.0.0
//   * ==============================================================================*/
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;

//using DBChmCreater.Ext;
//using Synyi.DBChmCreater.DB;

//namespace DBChmCreater.Core
//{
//    /// <summary>
//    /// SqlDAL
//    /// </summary>
//    public class SqlDAL : IDAL
//    {
//        private SqlHelp help;
//        private DataTable dtStruct;//表以及字段详情表
//        private DataTable dt;//表以及对应的描述信息


//        public SqlDAL(string conn, int type = 2012)
//        {
//            help = new SqlHelp(conn);
//            var strSql = @" SELECT TOP 100 PERCENT 
//					d.name as 表名,
//                    a.colorder AS 序号,  
//                    a.name AS 列名,  
//                    b.name AS 数据类型,  
//                    a.length AS 长度, 
//                    ISNULL(COLUMNPROPERTY(a.id, a.name, 'Scale'), 0) AS 小数位数,  
//                    CASE WHEN COLUMNPROPERTY(a.id,a.name, 'IsIdentity') = 1 THEN '是' ELSE '' END AS 标识,  
//                    CASE WHEN EXISTS 
//                        (SELECT 1 FROM dbo.sysindexes si  
//                          INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id AND si.indid = sik.indid   
//                          INNER JOIN dbo.syscolumns sc ON sc.id = sik.id AND sc.colid = sik.colid   
//                          INNER JOIN dbo.sysobjects so ON so.name = si.name AND so.xtype = 'PK'  
//                       WHERE sc.id = a.id AND sc.colid = a.colid) THEN '√' ELSE '' END AS 主键,   
//                    CASE WHEN a.isnullable = 1 THEN '√' ELSE '' END AS 允许空,  
//                     ISNULL(e.text, '') AS 默认值,  
//                        ISNULL(g.[value], '') AS 列说明 
//              FROM dbo.syscolumns a  
//                    LEFT OUTER JOIN dbo.systypes b ON a.xtype = b.xusertype  
//                    INNER JOIN dbo.sysobjects d ON a.id = d.id AND d.xtype = 'U' AND d.status >= 0 
//                    LEFT OUTER JOIN dbo.syscomments e ON a.cdefault = e.id  
//                    LEFT OUTER JOIN sys.extended_properties g ON a.id = g.major_id AND a.colid = g.minor_id AND g.name = 'MS_Description'  
//                    LEFT OUTER JOIN sys.extended_properties f ON d.id = f.major_id AND f.minor_id = 0 AND f.name = 'MS_Description'  
//              ORDER BY d.name, 序号 ";
//            dtStruct = help.ExecuteSql(strSql);
//            //            if (type == 2012)
//            //                strSql = @"select Row_Number() over ( order by getdate() )  as 序号, t1.name as 表名,
//            // case when t2.minor_id = 0 then isnull(t2.value, '') else '' end as 表说明
//            //from sysobjects t1 
//            //left join sys.extended_properties t2 on t1.id=t2.major_id
//            //where type='u'  and ( minor_id=0 or minor_id is null )";
//            //            else if (type == 2008 || type == 2005)
//            strSql = @"SELECT  Row_Number() over ( order by getdate() )  as 序号, case when a.colorder = 1 then d.name 
//                   else '' end as 表名, 
//        case when a.colorder = 1 then isnull(f.value, '') 
//                     else '' end as 表说明
//FROM syscolumns a 
//       inner join sysobjects d 
//          on a.id = d.id 
//             and d.xtype = 'U' 
//             and d.name <> 'sys.extended_properties'
//       left join sys.extended_properties   f 
//         on a.id = f.major_id 
//            and f.minor_id = 0
// where a.colorder = 1 and d.name<>'sysdiagrams'";
//            dt = help.ExecuteSql(strSql);
//        }

//        public DataTable GetTables()
//        {
//            return dt;
//        }

//        public List<DataTable> GetTableStruct(List<string> tables)
//        {
//            List<DataTable> lst = new List<DataTable>();
//            foreach (var table in tables)
//            {
//                var dtData = dtStruct.GetNewDataTable("表名='" + table + "'");
//                dtData.TableName = table;
//                dtData.Columns.Remove("表名");
//                lst.Add(dtData);
//            }
//            return lst;
//        }

//        public List<DataTable> GetTableData(List<string> tables)
//        {
//            List<DataTable> lst = new List<DataTable>();
//            foreach (var table in tables)
//            {
//                //避免取出来的数据过大
//                var dt = help.ExecuteSql("select top 100 * from " + table);
//                dt.TableName = table;
//                lst.Add(dt);
//            }
//            return lst;
//        }
//    }
//}