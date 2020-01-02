using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class ColumnEntity
    {
        public string table_catalog { get; set; }
        public string table_schema { get; set; }
        public string table_name { get; set; }
        public string ordinal_position { get; set; }
        public string column_name { get; set; }
        public string chinese_name { get; set; }
        public string data_typ { get; set; }

    
        public string is_nullable { get; set; }
        public string ref_key { get; set; }
        public string vale_range { get; set; }
        public string is_standard { get; set; }
        public string col_memo { get; set; }
    }
}
