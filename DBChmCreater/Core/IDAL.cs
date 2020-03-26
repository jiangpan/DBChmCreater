/* ==============================================================================
   * 文 件 名:IDAL
   * 功能描述：
   * Copyright (c) 2012 上海森亿医疗科技有限公司
   * 创 建 人: jiangpan
   * 创建时间: 2020/3/18 15:21:25
   * 修 改 人: 
   * 修改时间: 
   * 修改描述: 
   * 版    本: v1.0.0.0
   * ==============================================================================*/

using Synyi.DBChmCreater.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DBChmCreater.Core
{
    /// <summary>
    /// IDAL
    /// </summary>
    public interface IDAL
    {
        IList<DataTableItem> GetTables();
        IList<DataTableColumnDefCollection> GetTableStruct(List<string> tables);
        List<DataTable> GetTableData(List<string> tables ,int limitRows);
    }
}