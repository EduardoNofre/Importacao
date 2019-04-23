using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.DaoImpl
{
    class DocumentoDaoImpl : DocumentoDao
    {
        public List<DocumentosProperties> documentoLiberados(ProtocoloProperties protocolo)
        {
            
            DocumentosProperties documento = null;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            List<DocumentosProperties> listaDocumentos = new List<DocumentosProperties>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select distinct b.iddocumento, a.idprocesso from tb_processo a with(nolock)");
                sb.AppendLine("inner join tb_documento b with(nolock) on a.idprocesso = b.idprocesso");
                sb.AppendLine("and a.idstatus = b.idstatus and a.idstatus = "+Constantes.PRONTO_PARA_ENVIO + " and idProtocolo = "+ protocolo.idProtocolo);

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    documento = new DocumentosProperties();
                    documento.idDocumento = reader.GetInt32(reader.GetOrdinal("iddocumento"));
                    documento.idProcesso = reader.GetInt32(reader.GetOrdinal("idprocesso"));
                    listaDocumentos.Add(documento);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisaDocumentosLiberadosCarga: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return listaDocumentos;
        }


        public bool primeiroDocumentoDoProcesso(int idDocumento, int idProcesso)
        {
            bool retorno = false;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("select top 1 idDocumento from tb_documento with(nolock) where idstatus <> 24 and idprocesso = {0} order by 1", idProcesso));

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                if (reader.FieldCount > 0)
                {
                    if (idDocumento == reader.GetInt32(reader.GetOrdinal("idDocumento")))
                    {
                        return true;
                    }
                }                   

            }
            catch (Exception ex)
            {
                throw new Exception("Erro primeiro Documento Do Processo: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return retorno;
        }


        public bool tipoIndexDocumento(DocumentosProperties documento)
        {

            bool retorno = false;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {

                sb.AppendLine("select * from tb_documento a with(nolock), tb_valor_indice b with(nolock)" +
                               "where a.iddocumento=b.iddocumento " +
                               "and b.idindice= " + Constantes.INDICE +
                               "and b.nmvalorindice = '" + Constantes.TIPO_INDEX_DOCUMENTO +
                               "' and a.idprocesso in (select idprocesso from tb_documento where idprocesso= " + documento.idProcesso + ") and a.iddocumento = " + documento.idDocumento + "");

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = true;
                    break;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao tipo Index Documento: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return retorno;
        }


        public void LogDocumento(DocumentosProperties documento, int idStatus)
        {

            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();
            con.Open();

            try
            {
                sb.AppendLine("insert into tb_log_documento(IdDocumento, IdUsuario, IdStatus, DtInclusaoLog, NmDescricao, NmMaquina) values (" + documento.idDocumento + ","+Constantes.IDUSUARIO+"," +idStatus +","+"getdate()"+","+ "'Release Enviado SRM'"+",'"+Environment.MachineName+"')");
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro Log Documento: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }


        public void AtualizaSatusDocumento(DocumentosProperties documento,int idStatus)
        {

            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();
            con.Open();

            try {

                sb.AppendLine("update tb_documento");
                sb.AppendLine("set idStatus = " + idStatus);
                sb.AppendLine("where iddocumento = " + documento.idDocumento);

                
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro Log Atualiza Satus Documento: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }
    }
}
