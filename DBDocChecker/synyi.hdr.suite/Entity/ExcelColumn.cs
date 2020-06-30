using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite.Entity
{
    public class ExcelColumn
    {
        public string TableName { get; set; } //表名 （organization（机构信息））

        public string TableNameCh { get; set; } //表名 （organization（机构信息））

        public string Schema { get; set; } //业务域（mdm（主数据））

        public string SchemaCh { get; set; } //业务域（mdm（主数据））


        public string TableComment { get; set; } //表说明 （机构信息，如：医疗机构、科研机构等） 
        public string SerialNumber { get; set; } //序号	
        public string ColumnName { get; set; } //列名	
        public string ColumnComment { get; set; } //中文名	
        public string DataType { get; set; } //字段类型	
        public string IsNulled { get; set; } //允许空	
        public string ForeignKey { get; set; } //外键（关联字段）	
        public string CodeSystem { get; set; } //字典系统（值域）	
        public string IsStandard { get; set; } //字典表标准化	
        public string Description { get; set; } //说明
    }
}
