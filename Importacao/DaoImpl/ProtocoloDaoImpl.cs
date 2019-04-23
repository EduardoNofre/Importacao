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
using TCIUtility;

namespace TCIReleaseJucec.DaoImpl
{
    class ProtocoloDaoImpl : ProtocoloDao
    {

        LogUtil log = new LogUtil();

        public List<ProtocoloProperties> pesquisaProtocolosLiberadosCarga(ConfiguracaoProperties configApp)
        {

            ProtocoloProperties protocoloProperties = null;
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            List<ProtocoloProperties> listaProtocoloProperties = new List<ProtocoloProperties>();

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select distinct dsprotocolo, idprotocolo from (");
                sb.AppendLine("select a.dsprotocolo, b.idprotocolo, b.idcaixa, count(b.idprocesso)totalProcessos, count(c.idprocesso)totalProcessosAprovados ");
                sb.AppendLine("from tb_protocolo a with(nolock)");
                sb.AppendLine("inner join tb_processo b with(nolock) on a.idprotocolo = b.idprotocolo and a.idprojeto = " + configApp.idProjeto + " and b.idstatus not in(24,16,15)");
                sb.AppendLine("left join tb_processo c with(nolock) on b.idprocesso = c.idprocesso and b.idstatus = " + Constantes.PRONTO_PARA_ENVIO);
                sb.AppendLine("group by a.dsprotocolo, b.idprotocolo, b.idcaixa)a1");
                sb.AppendLine("where totalProcessos = totalProcessosAprovados");
                sb.AppendLine("order by 1");

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    protocoloProperties = new ProtocoloProperties();
                    protocoloProperties.idProtocolo = reader.GetInt32(reader.GetOrdinal("idprotocolo"));
                    protocoloProperties.dsProtocolo = reader.GetString(reader.GetOrdinal("dsprotocolo"));
                    listaProtocoloProperties.Add(protocoloProperties);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisaDocumentosLiberadosCarga: " + ex.ToString() + "\n" + ex.StackTrace);
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }

            return listaProtocoloProperties;
        }


        public bool atualizaProtocolo(ProtocoloProperties protocolo)
        {
            bool statusOk = true;

            Database.Instance.BeginTransaction();

            StringBuilder sb = new StringBuilder();

            try
            {

                sb.AppendLine("declare @idProtocolo int, @idStatusEnviado int, @idStatusDocProntoEnvio int,");
                sb.AppendLine("		@nmDescricao varchar(250), @dtInclusaoLog datetime,  @idUsuario int,");
                sb.AppendLine("		@nmMaquina varchar(50), @idStatusImgExcluida int, @idStatusImgEnviada int");
                sb.AppendLine("declare @tbProcesso table(idprocesso int)");
                sb.AppendLine("set @idProtocolo = " + protocolo.idProtocolo);
                sb.AppendLine("set @idStatusEnviado = " + Constantes.ENVIADO_FTP);
                sb.AppendLine("set @idStatusDocProntoEnvio = " + Constantes.PRONTO_PARA_ENVIO);
                sb.AppendLine("set @idStatusImgExcluida = " + Constantes.IMAGEM_EXCLUIDA);
                sb.AppendLine("set @idStatusImgEnviada = " + Constantes.IMAGEM_ENVIADA);
                sb.AppendLine("set @nmDescricao = 'release nesta data'");
                sb.AppendLine("set @dtInclusaoLog = getdate()");
                sb.AppendLine("set @idUsuario = (select idUsuario from tb_usuario where nmlogin = 'admin')");
                sb.AppendLine("set @nmMaquina = '" + Environment.MachineName + "'");


                // insere id processo na tbProcesso e com status pronto para envio

                sb.AppendLine("insert into @tbProcesso");
                sb.AppendLine("select idprocesso");
                sb.AppendLine("from tb_processo");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio and idprotocolo = @idProtocolo");

                // insere log na tabela tb_log_imagem status imagem enviada e data inclusão select diferente de excluida select igual pronto para envio select idprocesso igual idprocesso

                sb.AppendLine("insert into tb_log_imagem(IdImagem, IdUsuario, IdStatus, DtInclusao, NmDescricao, NmMaquina)");
                sb.AppendLine("select IdImagem, @idUsuario,  @idStatusImgEnviada, @dtInclusaoLog, @nmDescricao, @nmMaquina");
                sb.AppendLine("from tb_imagem");
                sb.AppendLine("where idstatus != @idStatusImgExcluida");
                sb.AppendLine("and exists(select null ");
                sb.AppendLine("			from tb_documento d");
                sb.AppendLine("			where idstatus = @idStatusDocProntoEnvio");
                sb.AppendLine("			and exists (select null");
                sb.AppendLine("						from @tbProcesso p ");
                sb.AppendLine("						where d.idprocesso = p.idprocesso)");
                sb.AppendLine("			and tb_imagem.iddocumento = d.iddocumento)");

                // atualiza o stauts para  idStatusImgEnviada diferente de idStatusImgExcluida e iddocumento = iddocumento select idstatus pronto para envio select  idprocesso = idprocesso

                sb.AppendLine("update tb_imagem");
                sb.AppendLine("set idstatus = @idStatusImgEnviada");
                sb.AppendLine("where idstatus != @idStatusImgExcluida");
                sb.AppendLine("and exists(select null");
                sb.AppendLine("			from tb_documento d");
                sb.AppendLine("			where idstatus = @idStatusDocProntoEnvio ");
                sb.AppendLine("			and exists (select null");
                sb.AppendLine("						from @tbProcesso p");
                sb.AppendLine("						where d.idprocesso = p.idprocesso)");
                sb.AppendLine("			and tb_imagem.iddocumento = d.iddocumento)");

                // insere na tabela tb_log_documento select idstatus pronto para envio select null idprocesso = idprocesso

                sb.AppendLine("insert into tb_log_documento(IdDocumento, IdUsuario, IdStatus, DtInclusaoLog, NmDescricao, NmMaquina)");                
                sb.AppendLine("select idDocumento, @idUsuario, @idStatusEnviado, @dtInclusaoLog, @nmDescricao, @nmMaquina");
                sb.AppendLine("from tb_documento");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio");
                sb.AppendLine("and exists (select null from @tbProcesso p where tb_documento.idprocesso = p.idprocesso)");

                // atualiza  tb_documento idstatus enviado

                sb.AppendLine("update tb_documento");
                sb.AppendLine("set idStatus = @idStatusEnviado");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio");
                sb.AppendLine("and exists (select null");
                sb.AppendLine("			from @tbProcesso p");
                sb.AppendLine("			where tb_documento.idprocesso = p.idprocesso)");

                // insere na tabela  tb_log_processo pega todos os e grava com id status enviado

                sb.AppendLine("insert into tb_log_processo(IdProcesso, IdStatus, NmDescricao, DtInclusaoLog, IdUsuario, NmMaquina)");
                sb.AppendLine("select idprocesso, @idStatusEnviado, @nmDescricao, @dtInclusaoLog, @idUsuario, @nmMaquina");
                sb.AppendLine("from @tbProcesso");

                // atualiza idadusto para enviado	

                sb.AppendLine("update tb_processo");
                sb.AppendLine("set idStatus = @idStatusEnviado");
                sb.AppendLine("where idprocesso in (select idProcesso from @tbProcesso)");

                Database.Instance.ExecuteQuery(sb.ToString());
                Database.Instance.CommitTransaction();

            }
            catch (Exception ex)
            {
                statusOk = false;
                Database.Instance.RollbackTransaction();
                throw new Exception("Erro ao tentar atualizaStatus: " + ex.Message + "\n" + ex.ToString() + "Idprotocolo  " + protocolo.idProtocolo);
            }

            return statusOk;
        }


        public void atualizarProcesso(ProtocoloProperties protocolo, int idStatus)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {                
                sb.AppendLine("update tb_processo");
                sb.AppendLine("set idStatus = " + idStatus);
                sb.AppendLine("where idprotocolo = " + protocolo.idProtocolo);

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro atualizar Processo: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }

        public void logProcesso(ProtocoloProperties protocolo, int idStatus)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {

                sb.AppendLine("insert into tb_log_processo(IdProcesso, IdStatus, NmDescricao, DtInclusaoLog, IdUsuario, NmMaquina) values(" + protocolo.idProtocolo + "," + idStatus + "," + "'Release log gravado'" + "," + "getdate()" + "," + Constantes.IDUSUARIO + ",'" + Environment.MachineName + "')");
                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao log Processo: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }


        public void atualizarProtocolo(ProtocoloProperties protocolo, int idStatus)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {

                sb.AppendLine("update tb_protocolo");
                sb.AppendLine("set idStatus =" + idStatus);
                sb.AppendLine("where idprotocolo = " + protocolo.idProtocolo);

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro atualizar Protocolo: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }
        public void  logProtocolo(ProtocoloProperties protocolo, int idStatus)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine("insert into tb_log_processo(IdProcesso, IdStatus, NmDescricao, DtInclusaoLog, IdUsuario, NmMaquina) values(" + protocolo.idProtocolo + ","+idStatus+","+ "'Log release protocolo'"+","+ "getdate()"+","+ "(select idUsuario from tb_usuario where nmlogin = 'admin')"+","+ Environment.MachineName+")");

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro log Protocolo: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }

    }
}
