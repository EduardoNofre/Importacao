using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.DaoImpl
{
    class CaixaDaoImpl : CaixaDao
    {

        public CaixaProperties caixaProcesso(DocumentosProperties documento)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            CaixaProperties caixa = new CaixaProperties();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select b.IdCaixa,b.dscaixa,b.IdStatus from tb_processo a with(nolock)");
                sb.AppendLine("inner join tb_caixa b with(nolock) on a.idcaixa = b.idcaixa and a.idprocesso = " + documento.idProcesso);

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    caixa.IdCaixa = reader.GetInt32(reader.GetOrdinal("IdCaixa"));
                    caixa.DsCaixa = reader.GetString(reader.GetOrdinal("dscaixa"));
                    caixa.IdStatus = reader.GetInt32(reader.GetOrdinal("IdStatus"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Caixa não encontrada para o processo " + documento.idProcesso + ex.ToString());
            }
            finally
            {

                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
            return caixa;
        }
    }
}
