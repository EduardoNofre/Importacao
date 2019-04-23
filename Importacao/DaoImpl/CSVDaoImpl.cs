using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.DaoImpl
{
    class CSVDaoImpl : CSVDao
    {
        public List<CSVProperties> documentoLiberados()
        {
            CSVProperties csv = null;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            List<CSVProperties> listaDocumentos = new List<CSVProperties>();

            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception("Erro sql csv: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }


            throw new NotImplementedException();
        }
    }
}
