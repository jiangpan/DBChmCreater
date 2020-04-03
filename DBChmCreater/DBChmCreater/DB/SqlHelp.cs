using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synyi.DBChmCreater.DB
{
    /// <summary>
    /// Sql数据库访问类
    /// </summary>
    public class SqlHelp : IDisposable
    {
        #region 成员定义
        //定义SqlServer链接字符串
        public string SqlConnectionString;
        //定义存储过程参数列表
        public IList SqlParameterList = new List<SqlParameter>();
        //定义SqlServer连接对象
        private SqlConnection SqlCon;
        #endregion

        #region 构造方法，实例化连接字符串

        /// <summary>
        /// 读取WebConfig链接字符串
        /// <summary>
        public SqlHelp()
        {
            SqlConnectionString = //将AppConfig链接字符串的值给SqlConnectionString变量
                                  //" server = .;database = hwitdb;Integrated security=SSPI;";
            "";
        }

        /// <summary>
        /// 有参构造，实例化连接字符串
        /// </summary>
        /// <param name="str">连接字符串</param>
        public SqlHelp(string connectionString)
        {
            SqlConnectionString = connectionString;
        }

        /// <summary>
        /// 有参构造，实例化连接字符串
        /// </summary>
        /// <param name="server">服务器地址</param>
        /// <param name="intance">数据库名称</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public SqlHelp(string server, string intance, string userName, string password)
        {
            SqlConnectionString = string.Format("server={0};database={1};uid={2};pwd={3}", server, intance, userName, password);
        }
        #endregion

        #region 实现接口IDisposable
        /// <释放资源接口>
        /// 实现接口IDisposable
        /// </释放资源接口>
        public void Dispose()
        {
            if (SqlCon != null)
            {
                if (SqlCon.State == ConnectionState.Open)//判断数据库连接池是否打开
                {
                    SqlCon.Close();
                }

                if (SqlParameterList.Count > 0)//判断参数列表是否清空
                {
                    SqlParameterList.Clear();
                }
                SqlCon.Dispose();//释放连接池资源
                GC.SuppressFinalize(this);//垃圾回收
            }
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 执行Sql文本查询，返回DataTable
        /// <summary>
        /// 执行Sql文本查询，返回DataTable
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public DataTable ExecuteSql(string strSql)
        {
            using (SqlCon = new SqlConnection(SqlConnectionString))
            {
                using (SqlCommand SqlCMD = new SqlCommand())
                {
                    try
                    {
                        SqlCon.Open();//打开数据库连接池
                        SqlCMD.Connection = SqlCon;
                        SqlCMD.CommandTimeout = 0;
                        SqlCMD.CommandType = CommandType.Text;
                        SqlCMD.CommandText = strSql;
                        if (SqlParameterList.Count != 0)
                        {
                            foreach (SqlParameter Para in SqlParameterList)
                            {
                                SqlCMD.Parameters.Add(Para);
                            }
                        }
                        SqlParameterList.Clear();
                        using (SqlDataAdapter Sqladapter = new SqlDataAdapter(SqlCMD))//创建适配器
                        {
                            DataTable SqlDataTable = new DataTable();
                            Sqladapter.SelectCommand.CommandTimeout = 0;
                            Sqladapter.Fill(SqlDataTable);
                            return SqlDataTable;//返回结果集
                        }

                    }
                    catch (Exception ex)
                    {
                        SqlCon.Close();//执行失败则立刻关闭链接
                        throw ex;
                    }
                    finally
                    {
                        Dispose();//释放资源
                    }
                }
            }
        }
        #endregion

        #region 测试连接是否成功
        /// <summary>
        /// 测试连接是否成功
        /// </summary>
        /// <returns></returns>
        public bool HasConnection
        {
            get
            {
                bool flag;
                try
                {
                    SqlCon = new SqlConnection(SqlConnectionString);
                    SqlCon.Open();
                    flag = true;
                }
                catch
                {
                    flag = false;
                    //throw ex;
                }
                finally
                {
                    SqlCon.Close();
                    Dispose();
                }
                return flag;
            }
        }
        #endregion
    }
}
