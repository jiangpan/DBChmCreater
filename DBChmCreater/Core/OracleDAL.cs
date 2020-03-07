///* ==============================================================================
//   * 文 件 名:OracleDAL
//   * 功能描述：
//   * Copyright (c) 2012 武汉经纬视通科技有限公司
//   * 创 建 人: chenbo
//   * 创建时间: 2013/3/18 15:25:39
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
//using System.Data.OracleClient;
//using System.Text;

//using DBChmCreater.Ext;
//using Synyi.DBChmCreater.DB;

//namespace DBChmCreater.Core
//{
//    /// <summary>
//    /// OracleDAL
//    /// </summary>
//    public class OracleDAL : IDAL
//    {
//        private OracleHelp help;
//        private DataTable dtStruct;//表以及字段详情表
//        private DataTable dt;//表以及对应的描述信息

//        public OracleDAL(string conn)
//        {
//            help = new OracleHelp(conn);
//            var strSql = @"select ROWNUM 序号 ,ut.table_name 表名,utc.comments 表说明 from user_tables ut left join user_tab_comments utc on ut.table_name = utc.table_name order by ut.table_name";
//            dt = help.ExecuteSql(strSql);
//            strSql = @" select  row_number()over( partition by utc.table_name order by utc.COLUMN_ID, ROWNUM ) as 序号,
//                utc.table_name as 表名,
//                 utc.column_name as 列名, 
//                 utc.data_type as 数据类型, 
//                 utc.data_length as 长度, 
//                 utc.data_precision as 精度,
//                 utc.data_Scale 小数位数, 
//                 case when  exists ( select   col.column_name   from   user_constraints con,user_cons_columns col 
//                    where  con.constraint_name=col.constraint_name and con.constraint_type='P' and col.table_name=ucc.table_name and col.column_name =  utc.column_name ) 
//                    then '√' else '' end as 主键, 
//                 case when utc.nullable = 'Y' then '√' else '' end as 允许空, 
//                 utc.data_default as 默认值, 
//                 ucc.comments as 列说明 
//                 from 
//                 user_tab_columns utc,user_col_comments ucc 
//                 where  utc.table_name = ucc.table_name and utc.column_name = ucc.column_name  
//                 order by   utc.table_name,序号";
//            dtStruct = help.ExecuteSql(strSql);
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
//                var dt = help.ExecuteSql("select  * from " + table+" where ROWNUM < 100 ");
//                dt.TableName = table;
//                lst.Add(dt);
//            }
//            return lst;
//        }
//    }
//}