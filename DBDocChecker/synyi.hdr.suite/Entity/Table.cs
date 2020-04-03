using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class Table
    {
        public string table_catalog { set; get; }
        public string table_schema { set; get; }
        public string table_name { set; get; }
        public string table_type { set; get; }
        public string self_referencing_column_name { set; get; }
        public string reference_generation { set; get; }
        public string user_defined_type_catalog { set; get; }
        public string user_defined_type_schema { set; get; }
        public string user_defined_type_name { set; get; }
        public string is_insertable_into { set; get; }
        public string is_typed { set; get; }
        public string commit_action { set; get; }
    }
}
