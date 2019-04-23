using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.DaoImpl
{
    class ProjetoDaoImpl : ProjetoDao
    {
        public ProjetoProperties getProjeto(ConfiguracaoProperties configApp)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            ProjetoProperties projeto = new ProjetoProperties();

            try
            {

                String strSql = "select idProjeto,NmProjeto,NmProjetoCompleto,Ativo,TemCapturaDistribuida,QtHoraExpurgo,DtAtualizacao,QtDiaExpurgoBaseLocal,MsRelPCP  from tb_projeto with(nolock) where idProjeto = " + configApp.idProjeto;
                
                con.Open();
                SqlCommand cmd = new SqlCommand(strSql, con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {

                    projeto.idProjeto = reader.GetInt32(reader.GetOrdinal("IdProjeto"));
                    projeto.nmProjeto = reader.GetString(reader.GetOrdinal("NmProjeto"));
                    projeto.nmProjetoCompleto = reader.GetString(reader.GetOrdinal("NmProjetoCompleto"));
                    projeto.ativo = reader.GetBoolean(reader.GetOrdinal("Ativo"));
                    projeto.temCaptureDistribuida = reader.GetBoolean(reader.GetOrdinal("TemCapturaDistribuida"));
                    projeto.QHoraExpurgo = reader.GetInt32(reader.GetOrdinal("QtHoraExpurgo"));
                    projeto.dtAtualizacao = reader.GetDateTime(reader.GetOrdinal("DtAtualizacao"));
                    projeto.qtDiaExpurgoBaseLocal = reader.GetInt32(reader.GetOrdinal("QtDiaExpurgoBaseLocal"));
                    projeto.msRefPCP = reader.GetString(reader.GetOrdinal("MsRelPCP"));

                }
            }
            catch (Exception ex)
            {

                throw new Exception("Erro getProjeto: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return projeto;
        }
    }
}
