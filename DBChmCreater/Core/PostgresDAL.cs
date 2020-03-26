using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DBChmCreater.Ext;
using Synyi.DBChmCreater.DB;
using Synyi.DBChmCreater.Entity;

namespace DBChmCreater.Core
{
    public class PostgresDAL : IDAL
    {
        private PostgreSqlHelper help;
        private DataTable dtStruct;//表以及字段详情表
        private DataTable dt;//表以及对应的描述信息


        public PostgresDAL()
        {

        }

        public PostgresDAL(string conn)
        {
            help = new PostgreSqlHelper(conn);
            var strSql = @" select format('%s.%s',cols.table_schema,quote_ident(cols.table_name))  表名
,cols.ordinal_position 序号
,cols.column_name  列名
,col_description((cols.table_schema || '.' ||cols.table_name)::regclass::oid,cols.ordinal_position ) as 中文名
,case when position('_' in cols.udt_name) > 0 then regexp_replace(cols.udt_name,'(_)(.*)','\2[]') else cols.udt_name end  数据类型
,case cols.data_type when 'character varying' then cols.character_maximum_length when 'numeric' then cols.numeric_precision else null end 长度
,cols.numeric_scale 小数位数
, CASE WHEN position( 'extval(' in cols.column_default)  > 1 THEN '√' ELSE '' END  标识
, case when EXISTS ( select a.table_schema,a.table_name,b.constraint_name,a.ordinal_position as position,a.column_name as key_column
from information_schema.table_constraints b inner join information_schema.key_column_usage a on a.constraint_name = b.constraint_name 
     and a.constraint_schema = b.constraint_schema  and a.constraint_name = b.constraint_name
where b.constraint_type = 'PRIMARY KEY' and a.table_schema = cols.table_schema and a.table_name = cols.table_name and a.column_name = cols.column_name) then '√' ELSE '' END 主键
,case when cols.is_nullable = 'YES' THEN '√' ELSE '' END   允许空
,cols.column_default 默认值
,'' as 列说明
from 
information_schema.columns cols inner join information_schema.tables tbs  on cols.TABLE_NAME = tbs.TABLE_NAME
where tbs.table_type = 'BASE TABLE'
              ORDER BY 1, 2 ";
            dtStruct = help.DirectQuery(strSql);

           
            //            if (type == 2012)
            //                strSql = @"select Row_Number() over ( order by getdate() )  as 序号, t1.name as 表名,
            // case when t2.minor_id = 0 then isnull(t2.value, '') else '' end as 表说明
            //from sysobjects t1 
            //left join sys.extended_properties t2 on t1.id=t2.major_id
            //where type='u'  and ( minor_id=0 or minor_id is null )";
            //            else if (type == 2008 || type == 2005)
            //strSql = @" select ROW_NUMBER () OVER (ORDER BY table_schema) 序号, table_schema as 域, format('%s',quote_ident(table_name))  表名, obj_description(to_regclass(table_schema || '.'|| quote_ident(table_name))) 表说明 from information_schema.tables  where table_type = 'BASE TABLE' and table_schema not in ('pg_catalog','information_schema') ";
            //dt = help.DirectQuery(strSql);
        }
        public IList<DataTableItem> GetTables()
        {
            var strSql1 = @" select ROW_NUMBER () OVER (ORDER BY table_schema) tableno, table_schema as domain, format('%s',quote_ident(table_name))  tablename, obj_description(to_regclass(table_schema || '.'|| quote_ident(table_name))) tabledescription from information_schema.tables  where table_type = 'BASE TABLE' and table_schema not in ('pg_catalog','information_schema') ";

            var result = help.Query<DataTableItem>(strSql1).ToList();


            return result;
        }
        public IList<DataTableColumnDefCollection> GetTableStruct(List<string> tables)
        {

            var strSql = @"  select cols.table_schema tableschema
 ,quote_ident(cols.table_name)  tablename
,cols.ordinal_position ordinal
,cols.column_name  colname
,col_description((cols.table_schema || '.' ||cols.table_name)::regclass::oid,cols.ordinal_position ) as description
,case when position('_' in cols.udt_name) > 0 then regexp_replace(cols.udt_name,'(_)(.*)','\2[]') else cols.udt_name end  datatype
,case cols.data_type when 'character varying' then cols.character_maximum_length when 'numeric' then cols.numeric_precision else null end length
,cols.numeric_scale as precision
, CASE WHEN position( 'extval(' in cols.column_default)  > 1 THEN '√' ELSE '' END as  identity
, case when EXISTS ( select a.table_schema,a.table_name,b.constraint_name,a.ordinal_position as position,a.column_name as key_column
from information_schema.table_constraints b inner join information_schema.key_column_usage a on a.constraint_name = b.constraint_name 
     and a.constraint_schema = b.constraint_schema  and a.constraint_name = b.constraint_name
where b.constraint_type = 'PRIMARY KEY' and a.table_schema = cols.table_schema and a.table_name = cols.table_name and a.column_name = cols.column_name) then '√' ELSE '' END primaykey
,case when cols.is_nullable = 'YES' THEN '√' ELSE '' END as isnull
,cols.column_default coldefault
,'' as memo
from 
information_schema.columns cols inner join information_schema.tables tbs  on cols.TABLE_NAME = tbs.TABLE_NAME
where tbs.table_type = 'BASE TABLE'
              ORDER BY 1, 2 ";
            var result  = help.Query< DataTableColumnDef>(strSql).ToList();

            IList<DataTableColumnDefCollection> result11 = new List<DataTableColumnDefCollection>();

            List<DataTable> lst = new List<DataTable>();

            var result11grp = result.GroupBy(p => new { p.tableschema, p.tablename });
            foreach (var table in tables)
            {
                var dtdefs = result11grp.Where(p => table == $"{p.Key.tableschema}.{p.Key.tablename}").FirstOrDefault();
                if (dtdefs != null)
                {
                    DataTableColumnDefCollection col = new DataTableColumnDefCollection();
                    col.TableName = table;                    
                    col.AddRange(dtdefs.AsEnumerable());
                    result11.Add(col);
                }
            }
            return result11;
        }

        public List<DataTable> GetTableData(List<string> tables,int limitRows)
        {
            List<DataTable> lst = new List<DataTable>();
            foreach (var table in tables)
            {
                string limitstr = string.Empty;
                if (limitRows >=1 )
                {
                    limitstr = $" limit {limitRows} ";
                }
                else
                {
                    limitstr = string.Empty;
                }

                //避免取出来的数据过大
                var dt = help.DirectQuery($"select * from {table} {limitstr}");
                dt.TableName = table;
                lst.Add(dt);
            }
            return lst;
        }
       
    }
}

