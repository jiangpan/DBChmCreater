/* ==============================================================================
   * 文 件 名:DALFacotry
   * 功能描述：
   * Copyright (c) 2092 上海森亿医疗科技有限公司
   * 创 建 人: jiangpan
   * 创建时间: 2020/2/19 15:27:47
   * 修 改 人: 
   * 修改时间: 
   * 修改描述: 
   * 版    本: v1.0.0.0
   * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Text;


namespace DBChmCreater.Core
{
    /// <summary>
    /// DALFacotry
    /// </summary>
    public class DALFacotry
    {
        public static IDAL Create(string dbType, string Conn)
        {
            switch (dbType)
            {
                //case "SQL2005及以上": return new SqlDAL(Conn);
                //case "Oracle": return new OracleDAL(Conn);
                case "Postgres":return new PostgresDAL(Conn);
                default: return null;
            }
        }
    }
}