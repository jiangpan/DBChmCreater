using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace synyi.hdr.suite.mdm
{
    [Table("mdm.code_system")]
    public class code_system_entity
    {
        [Key]
        public int code_sys_id { get; set; } // integer; NO
        public string code_sys_code { get; set; } // character varying; NO
        public string code_sys_name { get; set; } // character varying; NO
        public string spell_code { get; set; } // character varying; NO
        public string wb_code { get; set; } // character varying; NO
        public string version { get; set; } // character varying; YES
        public int domain_id { get; set; } // integer; NO
        public string level_code { get; set; } // character varying; NO
        public int source_sys_id { get; set; } // integer; YES
        public string source_note { get; set; } // character varying; YES
        public bool is_value_set { get; set; } // boolean; NO
        public bool is_class { get; set; } // boolean; YES
        public string note { get; set; } // character varying; YES
        public bool is_valid { get; set; } // boolean; NO
        public int oper_id { get; set; } // integer; YES
        public DateTime oper_time { get; set; } // timestamp without time zone; YES
        public DateTime etl_time { get; set; } // timestamp without time zone; NO
        public int tenant_id { get; set; } // integer; YES
    }
}
