using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class BizHelper
    {
        public string BuildQueryTableColumnsSQL(IList<string> schema = null,IList<string> tables = null,IList<string> types = null)
        {

            StringBuilder sb = new StringBuilder();
            string sql = @"select table_catalog,table_schema,table_name, ordinal_position, column_name, '' as chinese_name,
       case when  data_type = 'character varying' then(udt_name || '(' || coalesce(character_maximum_length::text, '') || ')')
            when  data_type = 'integer' then(udt_name)
           when  data_type = 'timestamp without time zone' then(udt_name || '(' || datetime_precision::text || ')')
           when  data_type = 'jsonb' then(udt_name)
           when  data_type = 'numeric' then(udt_name || '(' || coalesce(numeric_precision::text, '') || ',' || coalesce(numeric_scale::text, '') || ')')
           when  data_type = 'boolean' then(data_type)
           when  data_type = 'smallint' then(udt_name)
           when  data_type = 'text' then(udt_name)
           when  data_type = 'date' then(udt_name)
           when  data_type = 'ARRAY' then(replace(udt_name, '_', '') || '[' || ']')
           when  data_type = 'xml' then(udt_name)
           when  data_type = 'character' then(replace(udt_name, 'bp', '') || '(' || coalesce(character_maximum_length::text, '') || ')')
           when  data_type = 'bigint' then(udt_name)
           when  data_type = 'point' then(udt_name)
           when  data_type = 'json' then(udt_name)
           else ''
       end as data_typ
       --, data_type, character_maximum_length, datetime_precision, udt_name
       ,case  is_nullable when 'YES' then '是' when 'NO' then '否' else '' end as is_nullable
, '' as ref_key
, '' as vale_range
, '' as is_standard
, '' as col_memo
from information_schema.columns " ;

            sb.Append(sql);
            sb.Append(" where 1=1 ");

            if (schema != null && schema.Count > 0)
            {
                string schemastring = string.Join(",", schema.Select(p => "'" + p + "'"));
                sb.Append($" and table_schema in ({schemastring}) ");
            }

            if (tables != null && tables.Count > 0)
            {
                string tableString = string.Join(",", tables.Select(p => "'" + p + "'"));
                sb.Append($" and  table_name in ({tableString}) ");
            }


            if (types != null && types.Count > 0)
            {
                string typestring = string.Join(",", types.Select(p => "'" + p + "'"));
                sb.Append($" and  data_type in ({typestring}) ");
            }

            //sb.Append(" order by table_schema asc, table_name asc,ordinal_position asc ");

            return sb.ToString();


        }


    }
}
