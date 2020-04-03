using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class schemata
    {
        public string catalog_name { set; get; }
        public string schema_name { set; get; }
        public string schema_owner { set; get; }
        public string default_character_set_catalog { set; get; }
        public string default_character_set_schema { set; get; }
        public string default_character_set_name { set; get; }
        public string sql_path { set; get; }
    }
}
