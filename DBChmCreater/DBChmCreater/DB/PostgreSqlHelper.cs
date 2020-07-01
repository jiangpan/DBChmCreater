using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using Dapper;

namespace Synyi.DBChmCreater.DB
{
    public class PostgreSqlHelper : IDisposable
    {
        private NpgsqlConnection connection = null;
        private String connectionString = "dbConnection";
        private NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter();
        public NpgsqlCommand sqlCmd = new NpgsqlCommand();

        // {0} : IP, {1} : DB Name, {2} : userID, {3} : userPW
        private String connectionFormat = "Server={0};Database={1};User Id={2};Password={3};Port={4}";

        public String ConnectionString
        {
            set { connectionString = value; }
        }

        #region Constructors

        public PostgreSqlHelper()
        {
        }

        public PostgreSqlHelper(string pConnectionString)
        {
            if (!string.IsNullOrEmpty(pConnectionString.Trim()))
            {
                connectionString = pConnectionString;
            }
        }

        public PostgreSqlHelper(string pIP, string pDbName, string pUserID, string pUserPW, string pPort)
        {
            connectionString = string.Format(this.connectionFormat, pIP, pDbName, pUserID, pUserPW, pPort);
        }

        public PostgreSqlHelper(string pIP, string pDbName, string pUserID, string pUserPW)
        {
            connectionString = string.Format(this.connectionFormat, pIP, pDbName, pUserID, pUserPW, "5432");
        }

        #endregion

        public IEnumerable<T> Query<T>(string query)
        {
            NpgsqlConnection conn = null;
            IEnumerable<T> result = null;

            using (conn = new NpgsqlConnection(this.connectionString))
            {
                if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
                {
                    conn.Open();
                }

                try
                {
                    result = conn.Query<T>(query);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return result;
        }


        /// <summary>
        /// Sql Script / Stored Procedure Name
        /// </summary>
        public string CmdText
        {
            set { sqlCmd.CommandText = value; }
        }

        /// <summary>
        /// CommandType = CommandType....
        /// </summary>
        public CommandType cmdType
        {

            set { sqlCmd.CommandType = value; }
        }

        /// <summary>
        /// AddParameter(iSql, "return_value", SqlDbType.Int , "", ParameterDirection.ReturnValue , 5); ---- return문
        /// AddParameter(iSql, "@MSG_TEXT", SqlDbType.VarChar, "", ParameterDirection.Output, 100);
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        /// <param name="Param_io"></param>
        /// <param name="nSize"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values, ParameterDirection Param_io, int nSize)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            if (Param_io == ParameterDirection.Input || Param_io == ParameterDirection.InputOutput)
                param1.Value = values;
            param1.Direction = Param_io;
            param1.Size = nSize;
            sqlCmd.Parameters.Add(param1);
        }

        #region Add Parameter TO Query
        /// <summary>
        /// Store_Param(iSql, "@prc", SqlDbType.VarChar,"C");
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            param1.Value = values;
            sqlCmd.Parameters.Add(param1);
        }

        /// <summary>
        /// Store_Param(iSql, "@MSG_CD", SqlDbType.VarChar, "", ParameterDirection.Output);
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="Db_Type"></param>
        /// <param name="values"></param>
        /// <param name="Param_io"></param>
        public void AddParameter(string Param, NpgsqlDbType Db_Type, object values, ParameterDirection Param_io)
        {
            NpgsqlParameter param1 = new NpgsqlParameter(Param, Db_Type);
            if (Param_io == ParameterDirection.Input || Param_io == ParameterDirection.InputOutput)
                param1.Value = values;
            param1.Direction = Param_io;
            sqlCmd.Parameters.Add(param1);
        }

        public object GetParameterValue(NpgsqlCommand command, string parameterName)
        {
            return command.Parameters[parameterName].Value;
        }

        /// <summary>
        /// sp의 OUTPUT 파라미터 값 읽기
        /// int(string) return = (int/string)Return_Param("@return");
        /// </summary>
        /// <param name="param">"@param"</param>
        /// <returns>object</returns>
        public object GetParameter(string param)
        {
            return sqlCmd.Parameters[param].Value;
        }

        #endregion

        #region Generating SqlCommand

        private NpgsqlCommand PrepareCommand(CommandType commandType, string commandText)
        {
            if (connection == null)
            {
                connection = new NpgsqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            NpgsqlCommand command = new NpgsqlCommand(commandText, connection);
            command.CommandType = commandType;
            return command;
        }

        public NpgsqlCommand GetStoreProcedureCommand(string spname)
        {
            return PrepareCommand(CommandType.StoredProcedure, spname);
        }

        public NpgsqlCommand GetSqlQueryCommand(string query)
        {
            return PrepareCommand(CommandType.Text, query);
        }
        #endregion

        #region Direct Quer

        public int DirectNonQuery(string query)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            int iResult = ExecuteNonQuery(sc);
            return iResult;
        }

        /// <summary>
        /// 직접 쿼리결과를 가져오는 함수
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public DataTable DirectQuery(string query)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, string.Empty);
        }

        public DataTable DirectQuery(string query, string tableName)
        {
            NpgsqlCommand sc = GetSqlQueryCommand(query);
            return LoadDataTable(sc, tableName);
        }

        /// <summary>
        /// 쿼리명령어(Update/Insert/Delete)등 실행
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns>Return 1(Success) / 0(Fail)</returns>
        public int StringNonQuery(string sQuery)
        {
            return new PostgreSqlHelper().DirectNonQuery(sQuery);
        }
        #endregion

        #region Database Related Command

        public int ExecuteNonQuery(NpgsqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ExcuteNonQuery()
        {
            return sqlExcuteNonQuery(0);
        }

        /// <summary>
        /// Sql Excute Command
        /// Transaction: 1 begin transaction
        /// Transaction: 0 No transaction
        /// </summary>
        /// <param name="Transaction">Transaction= 1 Or 0</param>
        /// <returns>Return Effective Rows</returns>
        public int sqlExcuteNonQuery(int Transaction)
        {
            try
            {
                sqlData.SelectCommand = sqlCmd;
                sqlCmd.Connection = new NpgsqlConnection(this.connectionString);
                sqlCmd.Connection.Open();
                if (Transaction > 0)
                    sqlCmd.Transaction = sqlCmd.Connection.BeginTransaction(IsolationLevel.ReadCommitted);

                int rtn = sqlCmd.ExecuteNonQuery();

                if (Transaction > 0)
                    sqlCmd.Transaction.Commit();
                sqlCmd.Connection.Close();

                return rtn;
            }
            catch
            {
                if (Transaction > 0)
                    sqlCmd.Transaction.Rollback();

                return -1;
            }
            finally
            {
            }
        }

        public object ExecuteScalar(NpgsqlCommand command)
        {
            return command.ExecuteScalar();
        }

        public NpgsqlDataReader ExecuteReader(NpgsqlCommand command)
        {
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public NpgsqlDataReader ExecuteReader(NpgsqlCommand command, CommandBehavior commandBehavior)
        {
            return command.ExecuteReader(commandBehavior);
        }

        public DataTable LoadDataTable(NpgsqlCommand command, string tableName)
        {
            if (command.CommandText.IndexOf("mdm_drug_property") >= 0)
            {

            }
            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(command))
            {
                using (DataTable dt = new DataTable(tableName))
                {
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public DataSet LoadDataSet(NpgsqlCommand command, string[] tableNames)
        {
            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(command))
            {
                using (DataSet ds = new DataSet())
                {
                    da.Fill(ds);
                    if (tableNames != null)
                    {
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            try
                            {
                                ds.Tables[i].TableName = tableNames[i];
                            }
                            catch
                            {
                            }
                        }
                    }

                    return ds;
                }
            }
        }

        /// <summary>
        /// DataSet 요청
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet LoadDataSet()
        {
            //
            DataSet ds = new DataSet();
            try
            {
                sqlData.SelectCommand = sqlCmd;
                sqlCmd.Connection = new NpgsqlConnection(this.connectionString);

                sqlData.Fill(ds);

                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        private NpgsqlTransaction PrepareTransaction(IsolationLevel isolationLevel)
        {
            if (connection == null)
            {
                connection = new NpgsqlConnection(this.connectionString);
            }
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                connection.Open();
            }
            return connection.BeginTransaction(isolationLevel);
        }

        public NpgsqlTransaction BeginTransaction()
        {
            return PrepareTransaction(IsolationLevel.ReadCommitted);
        }

        public NpgsqlTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return PrepareTransaction(isolationLevel);
        }

        public void Commit(NpgsqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Commit();
        }

        public void RollBack(NpgsqlTransaction transaction)
        {
            if (transaction != null)
                transaction.Rollback();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Destructor

        ~PostgreSqlHelper()
        {
            Dispose();
        }
        #endregion

        void IDisposable.Dispose()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

    }   // end of class
}
