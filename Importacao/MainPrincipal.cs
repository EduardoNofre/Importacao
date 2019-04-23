/**
 * 
 * Sistema refatorado por : Eduardo Nofre Reis de Sá
 * Data refatoração: 20/03/2017
 * Motivo :  Codigo de dificil interpretação e manutenção, varias variavies utilizando o mesmo nome, programa com muitas variaveis estatica e metodos estaticos.
 * 
 * */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using TCIReleaseJucec.configuracao;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Negocio;
using TCIReleaseJucec.Regras;
using TCIReleaseJucec.servico;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec
{
    class MainPrincipal : ServiceBase
    {

        Configuracao config = new Configuracao();
        LogUtil log = new LogUtil();
        ConfiguracaoProperties configApp = null;
        ProjetoProperties projetoObj = null;
        RegrasProtocolo regrasProtocolo = null;
        string NireNunArquivamento = null;
        bool image;
        bool pdfisOk;
        bool documentoSemTiff;

        public MainPrincipal()
        {
            this.InitializeComponent();
            this.configApp = config.ConfiguracaoApp();
        }

        private void InitializeComponent()
        {
            base.ServiceName = "TCIReleaseJucec";
        }


        public void processamento()
        {
            log.escreveLog("Inicio ", configApp);

            while (!Constantes.THREAD_PARADA_SOLICITADO)
            {
                try
                {
                    RegrasProjeto regraProjeto = new RegrasProjeto();

                    RegrasProtocolo regraProtocolo = new RegrasProtocolo();

                    // carrega as informaçoes do projeto
                    projetoObj = regraProjeto.getProjeto(configApp);

                    // Busca protocolo liberados 
                    List<ProtocoloProperties> listaProtocolosLiberado = regraProtocolo.getProtocoloProtocolosLiberado(configApp);

                    log.escreveLog("Quantidade de protocolo disponível " + listaProtocolosLiberado.Count, configApp);

                    foreach (ProtocoloProperties protocolo in listaProtocolosLiberado)
                    {
                        RegrasDocumentos regrasDocumentos = new RegrasDocumentos();

                        // lista de documentos liberados
                        List<DocumentosProperties> listaDocumentosLiberados = regrasDocumentos.getDocumentosLiberado(protocolo);

                        log.escreveLog("Quantidade de documentos disponível " + listaProtocolosLiberado.Count, configApp);

                        foreach (DocumentosProperties documento in listaDocumentosLiberados)
                        {
                            log.escreveLog("Lendo o documento " + documento.idDocumento, configApp);

                            RegrasIndexer regrasIndexer = new RegrasIndexer();

                            // Recupera os indices e valorIndice
                            ReleaseJucecProperties releaseJucec = regrasIndexer.getIndexerProcesso(documento, projetoObj, protocolo, configApp);

                            //Concatena Nire e NumeroArquivamento
                            NireNunArquivamento = regrasIndexer.concatNire_NunArquivamento(releaseJucec, projetoObj);

                            if (NireNunArquivamento != null)
                            {
                                RegrasCaixa regrasCaixa = new RegrasCaixa();
                                RegrasImagem regrasImagem = new RegrasImagem();
                                RegrasParametroProdutoProjeto regrasParametroProdutoProjeto = new RegrasParametroProdutoProjeto();
                                Servicos servicos = new Servicos();

                                // Busca caixa com base no numero arquivamento
                                CaixaProperties caixa = regrasCaixa.getCaixaProcesso(documento);
                                log.escreveLog("Pegou a caixa " + caixa.IdCaixa, configApp);

                                // caminho onde será depositado os arquivos .pdf
                                DirectoryInfo pathDestino = regrasCaixa.trataCaixa(caixa, configApp, releaseJucec, NireNunArquivamento, projetoObj, documento);

                                // retorna um projeto produto 
                                ParametroProdutoProjetoProperties parametroProdutoProjetoProperties = regrasParametroProdutoProjeto.getParametroProdutoProjeto(projetoObj, configApp);

                                // pega todas as imagens referente ao  idDocumento passado como parametro e diferente de excluida.
                                // busca a origem do arquivo .tiff
                                // cria o _.pdf no destino passado como parametro
                                releaseJucec = regrasImagem.getImagem(documento, releaseJucec, configApp, parametroProdutoProjetoProperties, pathDestino, projetoObj);

                                documentoSemTiff = releaseJucec.flagSemImagem;

                                if (documentoSemTiff)
                                {
                                    if (releaseJucec.flag)
                                    {
                                        // Gera o arquivo csv no mesmo diretorio do PDF
                                        log.escreveLog("Gerando arquivo .CSV ", configApp);
                                        servicos.geraArqvuivoCSV(pathDestino.ToString() + "\\" + documento.idProcesso.ToString() + ".csv",releaseJucec, pathDestino, caixa);

                                        log.escreveLog("Gerando arquivo .CSV para o carga generica ", configApp);
                                        // Enviando cvs para o sistema carga Generica
                                        servicos.geraArqvuivoCSV(ConfigurationSettings.AppSettings["diretorioCsvContentProcessar"].ToString() + "\\" + documento.idProcesso.ToString() + ".csv",releaseJucec, pathDestino, caixa);
                                                                            
                                    }
                                    else
                                    {
                                        log.escreveLog("Quantidade de Indices [0] no Processo [" + documento.idProcesso + "] não é o mesmo na estrutura do arquivo", configApp);
                                    }

                                    log.escreveLog("Pesquisando as imagens no diretorio para assinatura do Protocolo [" + protocolo.dsProtocolo + "] na estrutura do arquivo", configApp);

                                    // retorna uma lista de pdf que termine com _.pdf
                                    FileInfo[] filesPdf = servicos.getPdfs(configApp, caixa, pathDestino);

                                    log.escreveLog("Foram encontrados .Pdf's com _.PDF [" + filesPdf.Length + "]", configApp);

                                    // move os pdfs sem assinatura para o diretorio informado  na tag do app.config  <caminhoPdfSemAssinatura>
                                    servicos.moverPdfSemAssinaturaSRM(filesPdf[0], releaseJucec, documento, configApp);

                                    // serviço que assina os pdfs de acordo com a certificado da maquina
                                     pdfisOk = servicos.arquivoParaAssinar(filesPdf[0], releaseJucec, configApp);
                                }
                                if (documentoSemTiff)
                                {
                                    // documento log e status
                                    regrasDocumentos = new RegrasDocumentos();
                                    regrasDocumentos.LogDocumento(documento, Constantes.ENVIADO_FTP);
                                    regrasDocumentos.atualizaSatusDocumento(documento, Constantes.ENVIADO_FTP);
                                }
                                else
                                {
                                    // documento log e status
                                    regrasDocumentos = new RegrasDocumentos();
                                    regrasDocumentos.LogDocumento(documento, Constantes.DOCUMENTO_SEM_TIFF);
                                    regrasDocumentos.atualizaSatusDocumento(documento, Constantes.DOCUMENTO_SEM_TIFF);
                                }
                            }
                        }
                        // processo e status
                        regrasProtocolo = new RegrasProtocolo();
                        regrasProtocolo.atualizaProcesso(protocolo, Constantes.ENVIADO_FTP);
                        regrasProtocolo.logProcesso(protocolo, Constantes.ENVIADO_FTP);
                    }
                }
                catch (Exception ex)
                {
                    log.escreveLog("\n Erro: " + ex.Message + "\n" + ex.ToString() +"\n", configApp);

                   // MessageBox.Show(ex.StackTrace);
                }
            }
        }
    }
}