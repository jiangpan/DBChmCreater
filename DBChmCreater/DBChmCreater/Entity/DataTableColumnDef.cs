using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;

namespace Synyi.DBChmCreater.Entity
{
    public class DataTableColumnDefCollection : List<DataTableColumnDef>
    {
        public string TableName { get; set; }
    }



    public class DataTableColumnDef
    {
        //[Description("业务域")]
        public string tableschema { get; set; }//业务域
        //[Description("表名")]
        public string tablename { get; set; }//表名
        [Description("序号")]
        public int ordinal { get; set; }//序号
        [Description("列名")]
        public string colname { get; set; }//列名
        [Description("中文名")]
        public string description { get; set; }//中文名
        [Description("数据类型")]
        public string datatype { get; set; }//数据类型
        [Description("长度")]
        public int? length { get; set; }//长度
        [Description("小数位数")]
        public int? precision { get; set; }//小数位数
        [Description("自增标识")]
        public string identity { get; set; }//标识
        [Description("主键")]
        public string primaykey { get; set; }//主键
        [Description("允许空")]
        public string isnull { get; set; }//允许空
        [Description("默认值")]
        public string coldefault { get; set; }//默认值
        //[Description("列说明")]
        public string memo { get; set; }//列说明

        //[Description("中文表名")]
        public string tablenamech { get; set; }//列说明

        //[Description("数据域")]
        public string schema { get; set; }//列说明

        //[Description("数据域中文名")]
        public string schemach { get; set; }//列说明
        //[Description("表说明")]
        public string tablecomment { get; set; }//列说明
        //[Description("列序号")]
        public string serialnumber { get; set; }//列说明
        //[Description("列名称")]
        public string columnname { get; set; }//列说明
        //[Description("列说明")]
        public string columncomment { get; set; }//列说明
        //[Description("完整数据类型")]
        public string datatypeex { get; set; }//列说明
        //[Description("是否可空")]
        public string isnulled { get; set; }//列说明
        [Description("外键约束")]
        public string foreignkey { get; set; }//列说明
        [Description("字典来源（值域）")]
        public string codesystem { get; set; }//列说明
        [Description("字典标准化")]
        public string isstandard { get; set; }//列说明
        [Description("备注说明")]
        public string descriptionex { get; set; }//列说明

        public string opertime { get; set; }//列说明


    }
}
