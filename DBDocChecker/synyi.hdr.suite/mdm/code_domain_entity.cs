using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite.mdm
{
    public class code_domain_entity
    {
        public int domain_id { get; set; } // integer; NO
        public string domain_code { get; set; } // character varying; NO
        public string domain_name { get; set; } // character varying; NO
        public string spell_code { get; set; } // character varying; NO
        public string wb_code { get; set; } // character varying; NO
        public int parent_domain_id { get; set; } // integer; YES
        public string note { get; set; } // character varying; YES
        public int oper_id { get; set; } // integer; YES
        public DateTime oper_time { get; set; } // timestamp without time zone; YES
        public DateTime etl_time { get; set; } // timestamp without time zone; NO
        public int tenant_id { get; set; } // integer; YES
    }
}
