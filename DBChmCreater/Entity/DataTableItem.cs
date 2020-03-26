using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synyi.DBChmCreater.Entity
{
    public class DataTableCollection : List<DataTableItem>
    {
        public string Name { get; set; }

        public DataTableCollection(string name)
        {
            this.Name = name;
        }


    }

    public class DataTableItem
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
