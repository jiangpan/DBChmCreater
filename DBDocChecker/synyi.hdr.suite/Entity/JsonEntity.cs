using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite.Entity
{
    public class JsonItemEntity
    {

        public string schema { set; get; } // mdm,
        public string table { set; get; } // mdm_drug_map,
        public string table_name { set; get; } // mdm.mdm_drug_map,
        public string column { set; get; } // source_code,
        public string distrubtion { set; get; } // ,
        public string format { set; get; } // mdm.mdm_drug_map,
        public string tablenamech { set; get; } // 药品标准化对照表,
        public string tablecomment { set; get; } // 药品标准化对照信息,
        public string serialnumber { set; get; } // 3,
        public string columnname { set; get; } // source_code,
        public string columncomment { set; get; } // 源代码,
        public string datatype { set; get; } // varchar(50),
        public string isnull { set; get; } // 否,
        public string codesystem { set; get; } // ,
        public string isstandard { set; get; } // 

        public string foreignkey { get; set; }

        public string description { get; set; }

    }

    public class JsonEntity
    {

        public IList<JsonItemEntity> RECORDS { get; set; }
    }
}
