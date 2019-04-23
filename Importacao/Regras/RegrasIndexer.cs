using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Negocio;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.Regras
{
    class RegrasIndexer
    {
        IndexerDao dao = new IndexerDaoImpl();

        LogUtil log = new LogUtil();

        ReleaseJucecProperties releaseJucec = null;

        public ReleaseJucecProperties getIndexerProcesso(DocumentosProperties documento, ProjetoProperties projetoObj, ProtocoloProperties protocolo, ConfiguracaoProperties configApp)
        {
            try
            {
                List<ValorIndiceProcessoProperties> listaIndexerProcesso = dao.indexerProcesso(documento);

                if (listaIndexerProcesso.Count > 0)
                {
                    releaseJucec = new ReleaseJucecProperties();

                    foreach (ValorIndiceProcessoProperties indexerProcesso in listaIndexerProcesso)
                    {

                        // siarco consulta para carregar os indices

                        if (indexerProcesso.indiceObject.NmIndiceSistema != null)
                        {
                            if (!(indexerProcesso.indiceObject.NmIndiceSistema == "NUMERO_PROTOCOLO"))
                            {
                                if (!(indexerProcesso.indiceObject.NmIndiceSistema == "NIRE"))
                                {
                                    if (!(indexerProcesso.indiceObject.NmIndiceSistema == "CNPJ"))
                                    {
                                        if (!(indexerProcesso.indiceObject.NmIndiceSistema == "NOME_EMPRESARIAL"))
                                        {
                                            if (!(indexerProcesso.indiceObject.NmIndiceSistema == "ATO"))
                                            {
                                                if (!(indexerProcesso.indiceObject.NmIndiceSistema == "EVENTO"))
                                                {
                                                    if (!(indexerProcesso.indiceObject.NmIndiceSistema == "DESCRICAO"))
                                                    {
                                                        if (!(indexerProcesso.indiceObject.NmIndiceSistema == "MUNICIPIO"))
                                                        {
                                                            if (indexerProcesso.indiceObject.NmIndiceSistema == "NUMERO_ARQUIVAMENTO")
                                                            {
                                                                if (!string.IsNullOrEmpty(indexerProcesso.NmValorIndiceProcesso.ToString().Trim()))
                                                                {
                                                                    releaseJucec.numArquivamento = indexerProcesso.NmValorIndiceProcesso;

                                                                }
                                                                else
                                                                {
                                                                    releaseJucec.numArquivamento = "000000";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            releaseJucec.municipio = indexerProcesso.NmValorIndiceProcesso.ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        releaseJucec.descricao = indexerProcesso.NmValorIndiceProcesso.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    releaseJucec.evento = indexerProcesso.NmValorIndiceProcesso.ToString();
                                                }
                                            }
                                            else{
                                            
                                                releaseJucec.ato = indexerProcesso.NmValorIndiceProcesso.ToString();
                                            }
                                        }
                                        else
                                        {
                                            releaseJucec.nomeempresarial = indexerProcesso.NmValorIndiceProcesso.ToString();
                                        }
                                    }
                                    else
                                    {
                                        releaseJucec.cnpj = indexerProcesso.NmValorIndiceProcesso.ToString();
                                    }
                                }
                                else if (!string.IsNullOrEmpty(indexerProcesso.NmValorIndiceProcesso.ToString().Trim()))
                                {
                                    releaseJucec.nire = indexerProcesso.NmValorIndiceProcesso.ToString();
                                }
                            }
                            else
                            {
                                releaseJucec.numProtocolo = indexerProcesso.NmValorIndiceProcesso.ToString();
                            }
                        }
                    }
                }

                if (projetoObj.nmProjeto.ToUpper() != "JUCEC - CADASTRO SINCRONIZADO")
                {
                    if (projetoObj.nmProjeto != string.Empty)
                    {
                        releaseJucec.nireConcatnumArquivamento = releaseJucec.nireConcatnumArquivamento + "\\" + protocolo.dsProtocolo;
                    }
                    else if (releaseJucec.nire != string.Empty && releaseJucec.numArquivamento != string.Empty)
                    {

                        string concat = releaseJucec.nireConcatnumArquivamento;
                        releaseJucec.nireConcatnumArquivamento = string.Concat(new string[] { concat, "\\", releaseJucec.nire, "\";\"", releaseJucec.numArquivamento });
                    }
                    else
                    {
                        releaseJucec.nireConcatnumArquivamento += "\\000\";\"000000";
                    }
                }
                else
                {
                    string text3 = releaseJucec.nireConcatnumArquivamento;
                    releaseJucec.nireConcatnumArquivamento = string.Concat(new string[] { text3, "\\", releaseJucec.nire, "\";\"", releaseJucec.numArquivamento });
                }


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Quantidade de Indices"))
                {
                    log.escreveLog(ex.Message + Environment.NewLine + ex.StackTrace, configApp);

                    return null;
                }
                throw new Exception("Erro em obterNumProtocolo:" + ex.Message + "\n" + ex.StackTrace);
            }

            releaseJucec.nireConcatnumArquivamento = releaseJucec.nireConcatnumArquivamento.Replace("\\", "").Replace("/", "").Replace("-", "").Replace("_", "");

            return releaseJucec;

        }

        public string concatNire_NunArquivamento(ReleaseJucecProperties releaseJucec, ProjetoProperties projeto)
        {
            string NireNunArquivamento = string.Empty;

            if (projeto.nmProjeto.ToUpper() != "JUCEC - CADASTRO SINCRONIZADO")
            {
                if (releaseJucec.numProtocolo != string.Empty)
                {
                    NireNunArquivamento = NireNunArquivamento + "\\" + releaseJucec.numProtocolo;
                }
                else if (releaseJucec.nire != string.Empty && releaseJucec.numArquivamento != string.Empty)
                {
                    string text2 = NireNunArquivamento;

                    NireNunArquivamento = string.Concat(new string[] { text2, "\\", releaseJucec.nire, "\";\"", releaseJucec.numArquivamento });
                }
                else
                {
                    NireNunArquivamento += "\\000\";\"000000";
                }
            }
            else
            {
                string text3 = NireNunArquivamento;
                NireNunArquivamento = string.Concat(new string[] { text3, "\\", releaseJucec.nire, "\";\"", releaseJucec.numProtocolo });
            }

            return NireNunArquivamento;
        }
    }
}
