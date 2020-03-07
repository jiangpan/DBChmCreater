//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//using System.Data.Common;
//using System.Data.OracleClient;

//namespace Synyi.DBChmCreater.DB
//{
//    /// <summary>
//    /// Oracle数据库访问类
//    /// </summary>
//    public class OracleHelp : IDisposable
//    {
//        #region 成员定义
//        //连接字符串
//        public string OracleConnectionString;
//        //连接对象
//        private OracleConnection OracleConn;
//        //存储过程参数列表
//        public IList OracleParameterList = new List<OracleParameter>();
//        #endregion

//        public OracleHelp(string str)
//        {
//            OracleConnectionString = str;
//        }

//        #region 测试是否能否建立连接
//        /// <summary>
//        /// 测试是否能否建立连接
//        /// </summary>
//        /// <returns></returns>
//        public bool HasConnection
//        {
//            get
//            {

//                bool flag;
//                try
//                {
//                    //打开数据库 
//                    OracleConn = new OracleConnection(OracleConnectionString);
//                    OracleConn.Open();
//                    flag = true;

//                }
//                catch
//                {
//                    //打开不成功 则连接不成功 
//                    flag = false;
//                    //throw ex;
//                }
//                finally
//                {
//                    //关闭数据库连接 
//                    OracleConn.Close();
//                    Dispose();
//                }
//                return flag;
//            }
//        }
//        #endregion

//        #region 执行Sql，返回 DataTable
//        /// <summary>
//        /// 执行Sql，返回 DataTable
//        /// </summary>
//        /// <param name="strSql">sql语句</param>
//        /// <returns>返回DataTable</returns>
//        public DataTable ExecuteSql(string strSql)
//        {
//            using (OracleConn = new OracleConnection(OracleConnectionString))
//            {
//                using (OracleCommand OraCMD = new OracleCommand())
//                {
//                    try
//                    {
//                        OracleConn.Open();
//                        OraCMD.CommandText = strSql;
//                        OraCMD.CommandType = CommandType.Text;
//                        OraCMD.Connection = OracleConn;
//                        if (OracleParameterList.Count != 0)
//                        {
//                            foreach (OracleParameter Para in OracleParameterList)//循环添加数据到SqlCommand对象里面
//                            {
//                                OraCMD.Parameters.Add(Para);
//                            }
//                        }
//                        OracleParameterList.Clear();
//                        using (OracleDataAdapter Oraadapter = new OracleDataAdapter(OraCMD))
//                        {
//                            DataTable dt = new DataTable();
//                            Oraadapter.Fill(dt);
//                            return dt;
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        OracleConn.Close();
//                        throw ex;
//                    }
//                    finally
//                    {
//                        Dispose();
//                    }
//                }
//            }
//        }
//        #endregion

//        #region 释放资源
//        /// <summary>
//        /// 释放资源接口
//        /// </summary>
//        public void Dispose()
//        {
//            if (OracleConn != null)
//            {
//                if (OracleConn.State == ConnectionState.Open)//判断数据库连接池是否打开
//                {
//                    OracleConn.Close();
//                }

//                if (OracleParameterList.Count > 0)//判断参数列表是否清空
//                {
//                    OracleParameterList.Clear();
//                }
//                OracleConn.Dispose();//释放连接池资源
//                GC.SuppressFinalize(this);//垃圾回收
//            }
//        }

//        void IDisposable.Dispose()
//        {
//            GC.SuppressFinalize(this);
//        }

//        ~OracleHelp()
//        {
//            Dispose();
//        }
//        #endregion
//    }
//}
