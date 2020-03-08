using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synyi.DBChmCreater.Entity
{
    public class DataTableStructureCollection : List<DataTableStructure>
    {
        public string Name { get; set; }

        public DataTableStructureCollection(string name)
        {
            this.Name = name;
        }


    }

    public class DataTableStructure
    {
        [Description("序号")]
        public int TableNo { get; set; }

        [Description("域")]
        public string Domain { get; set; }

        [Description("表名")]
        public string TableName { get; set; }


        [Description("表说明")]
        public string TableDescription { get; set; }
    }

}
