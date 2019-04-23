using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;
using TCIUtility;

namespace TCIReleaseJucec.Regras
{
    class RegrasImagem
    {
        ImagemDao dao = new ImagemDaoImpl();
        LogUtil log = new LogUtil();

        public ReleaseJucecProperties getImagem(DocumentosProperties documento, ReleaseJucecProperties releaseJucec, ConfiguracaoProperties configApp, ParametroProdutoProjetoProperties parametroProdutoProjetoPropertie, DirectoryInfo pathDestino, ProjetoProperties projetoObj)
        {
            ImagemDao dao = new ImagemDaoImpl();

            try
            {

                List<ImagemProperties> listaImagems = dao.ImagemProcessa(documento);

                log.escreveLog("Quantidade de imagem para o documento " + documento.idDocumento.ToString() + listaImagems.Count.ToString(), configApp);

                if (listaImagems.Count > 0)
                {
                    //releaseJucec.penultimaImg = listaImagems[listaImagems.Count - 2].NmImagem;

                    //log.escreveLog("PenultimaImg " + releaseJucec.penultimaImg, configApp);

                    releaseJucec.flagSemImagem = true;
                }
                else
                {
                    //releaseJucec.penultimaImg = string.Empty;

                    //log.escreveLog("PenultimaImg " + releaseJucec.penultimaImg, configApp);

                    releaseJucec.flagSemImagem = true;
                }
                if (releaseJucec.flagSemImagem)
                {

                    releaseJucec.ultimaImg = listaImagems[listaImagems.Count - 1].NmImagem;

                    log.escreveLog("UltimaImg " + releaseJucec.ultimaImg, configApp);

                    FileInfo fileDestinoPdf = null;

                    foreach (ImagemProperties imagem in listaImagems)
                    {

                        log.escreveLog("lendo a imagem  id " + imagem.IdImagem + " Nome imagem " + imagem.NmImagem, configApp);

                        log.escreveLog("Caminho da imagem no banco " + imagem.NmCaminhoImagem, configApp);

                        FileInfo fileOrigem = new FileInfo(parametroProdutoProjetoPropertie.NmValorParametro + imagem.NmCaminhoImagem);

                        log.escreveLog("Origem da imagem .Tiff " + fileOrigem.FullName.ToString(), configApp);

                        log.escreveLog("Destino do .CSV, .PDF sem assinatura, .PDF assinado " + pathDestino.FullName, configApp);

                        fileDestinoPdf = new FileInfo(pathDestino.FullName + "\\");

                        log.escreveLog("Destino com PDF " + fileDestinoPdf.FullName.ToString(), configApp);

                        if (!fileDestinoPdf.Directory.Exists)
                        {
                            fileDestinoPdf.Directory.Create();
                        }
                        if (new FileInfo(fileOrigem.FullName).Exists)
                        {
                            if (projetoObj.nmProjeto.ToUpper() != Constantes.DESCRICSO_PROJETO)
                            {
                                fileOrigem.CopyTo(fileDestinoPdf.FullName, true);
                            }
                            else
                            {

                                fileDestinoPdf = new FileInfo(string.Concat(new object[] { fileDestinoPdf.FullName, "\\", releaseJucec.numProtocolo, "_.pdf" }));

                                this.processaImagem(fileOrigem, fileDestinoPdf, listaImagems.Count, configApp);

                                dao.logImagem(imagem, Constantes.ENVIADO_FTP);
                            }

                            log.escreveLog("A Imagem [" + fileOrigem.FullName + "] copiada para o diretorio " + fileDestinoPdf.FullName, configApp);
                        }
                        else
                        {
                            log.escreveLog("A Imagem [" + fileOrigem.FullName + "] não existe no diretorio", configApp);
                        }
                    }

                }

                // processo
                //dao.atualizaStatus(imagem, Constantes.PDF_GERADO);

                releaseJucec.flag = true;

            }
            catch (Exception ex)
            {
                log.escreveLog("Erro ao tentar processaImagem:" + ex.Message + "\n" + ex.StackTrace, configApp);
                throw new Exception(ex.Message + "\n" + ex.StackTrace);
            }

            return releaseJucec;
        }

        private void processaImagem(FileInfo origem, FileInfo destino, int QtdImgDoc, ConfiguracaoProperties configApp)
        {


            try
            {
                destino = new FileInfo(string.Concat(new string[] { 
                    
                destino.Directory.FullName.Replace("\\NUMERO_PROTOCOLO\\", "\\NIRE_ARQUIVAMENTO\\"), "\\", destino.Name }));

                if (!destino.Directory.Exists)
                {
                    destino.Directory.Create();
                }

                ClassUtil.Instancia.ConvertTIFtoMultiPage(origem, destino, "pdf");

            }
            catch (Exception ex)
            {
                log.escreveLog("Erro ao tentar processaImagemSefaz: " + "\n" + ex.Message + "\n" + ex.StackTrace, configApp);
                throw new Exception(ex.Message + "\n" + ex.StackTrace);
            }
        }

    }
}

