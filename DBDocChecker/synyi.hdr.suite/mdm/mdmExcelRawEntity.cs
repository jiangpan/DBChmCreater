using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite.mdm
{
    public class mdmExcelRawEntity
    {
        public string 一级域代码 { get; set; }   //	一级域代码
        public string 一级域 { get; set; } //	一级域
        public string 二级域代码 { get; set; }   //	二级域代码
        public string 二级域 { get; set; } //	二级域
        public string 三级域代码 { get; set; }   //	三级域代码
        public string 三级域 { get; set; } //	三级域
        public string 代码系统 { get; set; }    //	代码系统
        public string 代码系统名称 { get; set; }  //	代码系统名称
        public string 标准来源 { get; set; }    //	标准来源
        public string 标准级别 { get; set; }    //	标准级别
        public string 标准完整性 { get; set; }   //	标准完整性
        public string 值集合 { get; set; }	//	"值集合"
        public string ETL转换 { get; set; }	//	"ETL转换"
        public string 应用转换 { get; set; }	//	"应用转换"
        public string 示例数据 { get; set; }	//	示例数据


    }
}
