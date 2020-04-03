using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synyi.hdr.suite
{
    public class DatabaseHelper
    {
        private static readonly string dbProviderName = null;

        private static readonly string dbConnectionString = null;

        private DbConnection connection;


        public static string HdrNew { get; set; }
        public static string HdrOld { get; set; }

        public static string HdrCda { get; set; }

        public static string HdrNewLocal { get; set; }

        public static string Exampledb { get; set; }


        public static string HdrV106 { get; set; }


        public static string HdrV108 { get; set; }



        public static string HdrV109 { get; set; }


        public static Dictionary<string, string> sdf = null;

        static DatabaseHelper()
        {
            if (sdf == null)
            {
                sdf = new Dictionary<string, string>();
            }
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                sdf.Add(item.Name, item.ConnectionString);

            } 



            HdrNew = "hdr_new"; //曾用连接
            HdrOld = "hdr_old";  //曾用连接
            HdrNewLocal = "hdr_new_local"; //曾用连接
            HdrCda = "hdr_cda";
            Exampledb = "exampledb";
            HdrV106 = "hdr_v106";

            HdrV108 = "hdr_v108";

            HdrV109 = "hdr_v109";
            //设置当前连接串
            dbConnectionString = ConfigurationManager.ConnectionStrings[HdrNewLocal].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings[HdrNewLocal].ProviderName; //默认连接
        }


        public DatabaseHelper()
        {
            this.connection = CreateConnection(DatabaseHelper.dbConnectionString);
        }

        /// <summary>
        /// 默认连接字符串创建连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection CreateConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DatabaseHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = DatabaseHelper.dbConnectionString;
            return dbconn;
        }

        /// <summary>
        /// 连接字符串名字
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection CreateConnection(string connectionString)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DatabaseHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString; ;
            return dbconn;
        }
    }
}
