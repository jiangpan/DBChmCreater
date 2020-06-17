using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite.mdm
{
    /*
     生成脚本
     select format('public %s %s { get; set; } // %s; %s', ( case udt_name when 'varchar' then 'string' when 'int4' then 'int' when 'int2' then 'int' when 'timestamp' then 'DateTime' when 'bool' then 'bool' else 'string' end),column_name, data_type,is_nullable), * 
from information_schema.columns 
where table_schema = 'mdm' and table_name = 'code_set'
     */
    public class code_set_entity
    {
        public int code_id { get; set; } // integer; NO
        public string code { get; set; } // character varying; NO
        public string name { get; set; } // character varying; NO
        public string show_name { get; set; } // character varying; YES
        public string spell_code { get; set; } // character varying; NO
        public string wb_code { get; set; } // character varying; NO
        public int code_sys_id { get; set; } // integer; YES
        public bool is_std { get; set; } // boolean; YES
        public int std_class_id { get; set; } // integer; YES
        public int unit_id { get; set; } // integer; YES
        public string unit_name { get; set; } // character varying; YES
        public int value_type { get; set; } // smallint; YES
        public string range { get; set; } // character varying; YES
        public string range_low { get; set; } // character varying; YES
        public string range_high { get; set; } // character varying; YES
        public string note { get; set; } // character varying; YES
        public int quote_code_id { get; set; } // integer; YES
        public int state { get; set; } // smallint; NO
        public int sort_no { get; set; } // smallint; YES
        public int oper_id { get; set; } // integer; YES
        public DateTime oper_time { get; set; } // timestamp without time zone; YES
        public DateTime etl_time { get; set; } // timestamp without time zone; NO
        public int tenant_id { get; set; } // integer; YES
        public bool is_valid { get; set; } // boolean; NO
    }
}
