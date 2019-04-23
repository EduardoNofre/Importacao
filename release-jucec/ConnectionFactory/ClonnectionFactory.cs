using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCIReleaseJucec.ConnectionFactory
{
    public class ConnectionFactory
    {
        public static SqlConnection getConnection()
        {
            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = getDataSourceConfigApp();

            return conn;
        }

        public static void fechaConexão(SqlConnection con)
        {
            try
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            catch (SqlException sqle)
            {
                throw sqle;
            }
            finally
            {
                con.Close();
            }
        }

        public static String getDataSourceConfigApp()
        {
            return "Data Source=10.254.100.12; Initial Catalog=EasyCapture_JUCEC; User Id=user_jucec_release; Password=pass_jucec_release; Connect Timeout=300";
        }
    }
}
