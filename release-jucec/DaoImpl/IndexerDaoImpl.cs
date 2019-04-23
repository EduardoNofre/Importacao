using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.DaoImpl
{
    class IndexerDaoImpl : IndexerDao
    {
        public List<ValorIndiceProcessoProperties> indexerProcesso(DocumentosProperties documento)
        {
           
            ValorIndiceProcessoProperties valorIndiceProcesso = null;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            List<ValorIndiceProcessoProperties> listaValorIndiceProperties = new List<ValorIndiceProcessoProperties>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select b.nmindiceSistema, a.nmvalorindiceprocesso");
                sb.AppendLine("from tb_valor_indice_processo a with(nolock)");
                sb.AppendLine("inner join tb_indice b with(nolock) on a.idindice = b.idindice");
                sb.AppendLine("where idProcesso = " + documento.idProcesso);
                sb.AppendLine("order by cast(nmOrdemApresentacao as int)");

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                if (reader.FieldCount >= 1)
                {
                    while (reader.Read())
                    {
                        valorIndiceProcesso = new ValorIndiceProcessoProperties();

                        valorIndiceProcesso.indiceObject = new IndiceProperties();

                        valorIndiceProcesso.NmValorIndiceProcesso = reader.GetString(reader.GetOrdinal("nmvalorindiceprocesso"));

                        valorIndiceProcesso.indiceObject.NmIndiceSistema = reader.GetString(reader.GetOrdinal("nmindiceSistema"));

                        listaValorIndiceProperties.Add(valorIndiceProcesso);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Quantidade de Indices [{0}] para o documento do Processo [{1}] não é o mesmo na estrutura do arquivo.", reader.FieldCount.ToString(), documento.idProcesso.ToString()));
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao pesquisaIndexacaoProcesso: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return listaValorIndiceProperties;
        }
    }
}
