using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using TCI.Utils.NET2005;
using TCIUtility;

namespace TCIReleaseJucec
{
    internal class Persistencia
    {
        private static Persistencia _instancia;

        public static Persistencia Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new Persistencia();
                }

                return _instancia;
            }
        }

        internal string pesquisaProjeto(int idProjeto)
        {
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idProjeto int");
                sb.AppendLine("set @idProjeto =" + idProjeto);
                sb.AppendLine("select NmProjeto from tb_projeto where idprojeto = @idProjeto");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
                if (dt.Rows.Count == 0)
                    throw new Exception("Projeto não encontrado no banco de dados!");
                return dt.Rows[0]["NmProjeto"].ToString();

            }
            catch
            {
                throw;
            }
        }

        internal string pesquisaProtocolo(int idProtocolo)
        {
            try
            {
                DataTable dt = new DataTable();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select dsprotocolo from tb_protocolo with(nolock) where idprotocolo = " + idProtocolo);
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
                if (dt.Rows.Count == 0)
                    throw new Exception("Protocolo não encontrado no banco de dados!");
                return dt.Rows[0]["dsprotocolo"].ToString();

            }
            catch
            {
                throw;
            }
        }

        internal DataTable pesquisaProtocolosLiberadosCarga(int idProjeto)
        {
            DataTable dt = new DataTable();
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idstatus int, @idstatusDocExcluido int, @idProjeto int");
                sb.AppendLine("set @idProjeto = " + idProjeto);
                sb.AppendLine("set @idstatus = " + (int)ClassUtil.Status.ProntoParaEnvio);
                sb.AppendLine("set @idstatusDocExcluido = " + (int)ClassUtil.Status.DocumentoExcluido);
                sb.AppendLine("select distinct dsprotocolo, idprotocolo from (");
                sb.AppendLine("select a.dsprotocolo, b.idprotocolo, b.idcaixa, count(b.idprocesso)totalProcessos, count(c.idprocesso)totalProcessosAprovados ");
                sb.AppendLine("from tb_protocolo a with(nolock)");
                sb.AppendLine("inner join tb_processo b with(nolock) on a.idprotocolo = b.idprotocolo and a.idprojeto = @idProjeto and b.idstatus not in(24,16,15)");
                sb.AppendLine("left join tb_processo c with(nolock) on b.idprocesso = c.idprocesso and b.idstatus = @idstatus");
                sb.AppendLine("group by a.dsprotocolo, b.idprotocolo, b.idcaixa)a1");
                sb.AppendLine("where totalProcessos = totalProcessosAprovados");
                sb.AppendLine("order by 1");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisaProtocolosLiberadosCarga:" + ex.Message + "\n" + ex.StackTrace);
            }
            return dt;
        }

        internal DataTable pesquisaDocumentosLiberadosCarga(int idProtocolo)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idstatus int, @idProtocolo int");
                sb.AppendLine("set @idProtocolo = " + idProtocolo);
                sb.AppendLine("set @idstatus = " + (int)ClassUtil.Status.ProntoParaEnvio);
                sb.AppendLine("select distinct b.iddocumento, a.idprocesso from tb_processo a with(nolock)");
                sb.AppendLine("inner join tb_documento b with(nolock) on a.idprocesso = b.idprocesso");
                sb.AppendLine("and a.idstatus = b.idstatus and a.idstatus = @idstatus and idProtocolo = @idProtocolo");

                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisaDocumentosLiberadosCarga:" + ex.Message + "\n" + ex.StackTrace);
            }
            return dt;
        }

        internal DataTable pesquisaIndexacaoProcesso(int idProcesso)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idProcesso int");
                sb.AppendLine("set @idProcesso = " + idProcesso);
                sb.AppendLine("select b.nmindiceSistema, a.nmvalorindiceprocesso");
                sb.AppendLine("from tb_valor_indice_processo a with(nolock)");
                sb.AppendLine("inner join tb_indice b with(nolock) on a.idindice = b.idindice");
                sb.AppendLine("where idProcesso = @idProcesso");
                sb.AppendLine("order by cast(nmOrdemApresentacao as int)");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
                if (dt.Rows.Count != 3)
                    throw new Exception(string.Format("Quantidade de Indices [{0}] para o documento do Processo [{1}] não é o mesmo na estrutura do arquivo.",
                        dt.Rows.Count.ToString(),
                        idProcesso.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisaIndexacaoProcesso:" + ex.Message + "\n" + ex.StackTrace);
            }
            return dt;
        }

        internal DataTable pesquisaDataExportacao()
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select getdate() getdate");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
            }
            catch
            {
                throw;
            }
            return dt;
        }

        internal DataTable pesquisaImagensDocumento(int idDocumento)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            try
            {

                sb.AppendLine("declare @idDocumento int");
                sb.AppendLine("set @idDocumento =" + idDocumento);
                sb.AppendLine("select NmCaminhoImagem + NmImagem caminho, NmImagem from tb_imagem");
                sb.AppendLine("where idstatus != " + (int)ClassUtil.Status.ImgExcluida);
                sb.AppendLine("and idDocumento = @idDocumento");
                sb.AppendLine("order by nmimagem");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);

                if (dt.Rows.Count == 0)
                {
                    sb = new StringBuilder();
                    sb.AppendLine("declare @idUsuario int");
                    sb.AppendLine("select @idUsuario = idusuario from tb_usuario where nmlogin  = 'admin'");
                    sb.AppendLine("update tb_documento set idstatus = 24 where iddocumento = " + idDocumento);
                    sb.AppendLine("insert into tb_log_documento (IdDocumento, IdUsuario, IdStatus, DtInclusaoLog, NmDescricao, NmMaquina) ");
                    sb.AppendLine(string.Format("values({0}, @idUsuario, 24, getdate(), 'Documento sem imagem excluido pelo Release Jucec', '{1}')", idDocumento, Environment.MachineName));
                    Database.Instance.ExecuteQuery(sb.ToString());
                    throw new Exception("Não existem imagens para o documento " + idDocumento + "\n Alterado status do Documento para Excluído");
                }
            }
            catch
            {
                throw;
            }
            return dt;
        }

        internal DataTable pesquisaCaminhoImagemProjeto(int _idProjeto)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idParametro int, @idProduto  int, @idProjeto int");
                sb.AppendLine("set @idParametro = " + (int)ClassUtil.DsParametro.CaminhoDaImagem);
                sb.AppendLine("set @idProduto = 3");
                sb.AppendLine("set @idProjeto = " + _idProjeto);
                sb.AppendLine("select nmValorParametro from tb_parametro_produto_projeto");
                sb.AppendLine("where idparametro = @idParametro");
                sb.AppendLine("and idproduto = @idProduto");
                sb.AppendLine("and idProjeto = @idProjeto");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
                if (dt.Rows.Count == 0)
                    throw new Exception("Caminho da Imagem não configurado para o projeto!");

            }
            catch
            {
                throw;
            }
            return dt;
        }

        internal void atualizaStatus(int idProtocolo)
        {
            try
            {
                Database.Instance.BeginTransaction();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idProtocolo int, @idStatusEnviado int, @idStatusDocProntoEnvio int,");
                sb.AppendLine("		@nmDescricao varchar(250), @dtInclusaoLog datetime,  @idUsuario int,");
                sb.AppendLine("		@nmMaquina varchar(50), @idStatusImgExcluida int, @idStatusImgEnviada int");
                sb.AppendLine("declare @tbProcesso table(idprocesso int)");
                sb.AppendLine("set @idProtocolo = " + idProtocolo);
                sb.AppendLine("set @idStatusEnviado = " + (int)ClassUtil.Status.EnviadoFTP);
                sb.AppendLine("set @idStatusDocProntoEnvio = " + (int)ClassUtil.Status.ProntoParaEnvio);
                sb.AppendLine("set @idStatusImgExcluida = " + (int)ClassUtil.Status.ImgExcluida);
                sb.AppendLine("set @idStatusImgEnviada = " + (int)ClassUtil.Status.ImgEnviada);
                sb.AppendLine("set @nmDescricao = 'release nesta data'");
                sb.AppendLine("set @dtInclusaoLog = getdate()");
                sb.AppendLine("set @idUsuario = (select idUsuario from tb_usuario where nmlogin = 'admin')");
                sb.AppendLine("set @nmMaquina = '" + Environment.MachineName + "'");
                sb.AppendLine("insert into @tbProcesso");
                sb.AppendLine("select idprocesso");
                sb.AppendLine("from tb_processo");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio and idprotocolo = @idProtocolo");
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
                sb.AppendLine("insert into tb_log_documento(IdDocumento, IdUsuario, IdStatus, DtInclusaoLog, NmDescricao, NmMaquina)");
                sb.AppendLine("select idDocumento, @idUsuario, @idStatusEnviado, @dtInclusaoLog, @nmDescricao, @nmMaquina");
                sb.AppendLine("from tb_documento");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio");
                sb.AppendLine("and exists (select null from @tbProcesso p where tb_documento.idprocesso = p.idprocesso)");
                sb.AppendLine("update tb_documento");
                sb.AppendLine("set idStatus = @idStatusEnviado");
                sb.AppendLine("where idstatus = @idStatusDocProntoEnvio");
                sb.AppendLine("and exists (select null");
                sb.AppendLine("			from @tbProcesso p");
                sb.AppendLine("			where tb_documento.idprocesso = p.idprocesso)");
                sb.AppendLine("insert into tb_log_processo(IdProcesso, IdStatus, NmDescricao, DtInclusaoLog, IdUsuario, NmMaquina)");
                sb.AppendLine("select idprocesso, @idStatusEnviado, @nmDescricao, @dtInclusaoLog, @idUsuario, @nmMaquina");
                sb.AppendLine("from @tbProcesso");
                sb.AppendLine("update tb_processo");
                sb.AppendLine("set idStatus = @idStatusEnviado");
                sb.AppendLine("where idprocesso in (select idProcesso from @tbProcesso)");
                Database.Instance.ExecuteQuery(sb.ToString());
                Database.Instance.CommitTransaction();

            }
            catch (Exception ex)
            {
                Database.Instance.RollbackTransaction();
                throw new Exception("Erro ao tentar atualizaStatus:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        internal DataTable pesquisaCaixaProcesso(int idProcesso)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idProcesso int");
                sb.AppendLine("set @idProcesso = " + idProcesso);
                sb.AppendLine("select b.dscaixa from tb_processo a with(nolock)");
                sb.AppendLine("inner join tb_caixa b with(nolock) on a.idcaixa = b.idcaixa and a.idprocesso = @idProcesso");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);
                if (dt.Rows.Count == 0)
                    throw new Exception("Caixa não encontrada para o processo " + idProcesso);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        internal string pegarNmProjeto(int idProjeto)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("declare @idProjeto int");

                sb.AppendLine("set @idProjeto = " + idProjeto);

                sb.AppendLine("select nmprojeto  from tb_projeto with(nolock) where idProjeto = @idProjeto");

                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);

                if (dt.Rows.Count == 0)
                {

                    throw new Exception(string.Format("Nome do projeto não encontrado com o id [{0}]!", idProjeto));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt.Rows[0]["nmprojeto"].ToString().ToUpper();
        }

        internal bool primeiroDocumentoDoProcesso(int idDocumento, int idProcesso)
        {
            bool retorno = false;
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("select top 1 idDocumento from tb_documento where idstatus <> 24 and idprocesso = {0} order by 1", idProcesso));
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);

                if (dt.Rows.Count > 0)

                    retorno = idDocumento.Equals(dt.Rows[0]["idDocumento"]);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retorno;
        }
    }
}
