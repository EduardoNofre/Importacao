using DeskSignerSDKPDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Regras;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.servico
{
    class Servicos
    {
        LogUtil log = new LogUtil();

        public bool arquivoParaAssinar(FileInfo filePdf, ReleaseJucecProperties releaseJucec, ConfiguracaoProperties configApp)
        {
            bool pdfisOk = true;

            try
            {
                if (filePdf.Exists)
                {
                    log.escreveLog("inicio da assinatura do PDF: " + filePdf.FullName, configApp);

                    this.assinarArquivo(filePdf, releaseJucec, configApp);
                }
            }
            catch (Exception ex)
            {
                pdfisOk = false;

                log.escreveLog("Erro : " + ex.Message + Environment.NewLine + ex.StackTrace, configApp);

                filePdf.Delete();
            }

            return pdfisOk;
        }

        private void assinarArquivo(FileInfo arquivoAssinar, ReleaseJucecProperties releaseJucec, ConfiguracaoProperties configApp)
        {

            clsPDF clsPDF = new clsPDF();
            string xml = string.Empty;
            string idCertificadoNOde = string.Empty;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(new clsCertificados
                {
                    Licenca = this.pegaCodLicenca()
                }.ListarCertificadosPessoais());

                if (string.IsNullOrEmpty(configApp.NomeCertificado))
                {
                    throw new Exception("Tag [nomeCertificado] não configurada no arquivo .config.");
                }

                log.escreveLog("Buscando Certificado" + "Nome do Certificado " + configApp.NomeCertificado, configApp);

                foreach (XmlNode xmlNode in xmlDocument.SelectNodes("//Certificado"))
                {
                    log.escreveLog("XML Node: " + xmlNode.InnerText.ToUpper(), configApp);

                    if (xmlNode.InnerText.ToUpper().Contains(configApp.NomeCertificado.ToUpper()))
                    {
                        idCertificadoNOde = xmlNode.SelectSingleNode("IDCertificado").FirstChild.InnerText;

                        log.escreveLog(string.Format("Id do Certificado {0} encontrado para {1}:...", configApp.NomeCertificado.ToString(), idCertificadoNOde), configApp);

                        break;
                    }
                }

                if (string.IsNullOrEmpty(idCertificadoNOde))
                {
                    throw new Exception(string.Format("Certificado não encontrado nesta maquina [{0}]!.{1}É preciso existir um certificado válido para [{2}]", Environment.MachineName, Environment.NewLine, idCertificadoNOde));
                }

                clsPDF.Licenca = this.pegaCodLicenca();

                xml = clsPDF.Assinar(this.carregaXML(arquivoAssinar, idCertificadoNOde));

                log.escreveLog("Aviso: "+ xml.ToString(), configApp);
                log.escreveLog("Aviso: Caminho: " + arquivoAssinar.FullName, configApp);

                xmlDocument.LoadXml(xml);

                string xmlErro = xmlDocument.SelectSingleNode("//DeskSignerSDKPDF/CodErro").FirstChild.InnerText;

                if (xmlErro != "0")
                {
                    log.escreveLog("Erro ao assinar o Arquivo "+ this.traducaoErro(xmlErro).ToString(), configApp);
                }

            }
            catch (Exception ex)
            {

                log.escreveLog("Erro ao assinar o Arquivo " + ex.ToString(), configApp);
                throw ex;
            }
            finally
            {
                clsPDF = null;
            }
        }


         // Autor:Eduardo Nofre 
         // Date: 04/04/2017
         // Motivação: solicitação do cliente para o Rafael Lira que esteve no cliente em Recifie
         // action: move os arquivos pdf sem assinatura para um diretorio definido pelo cliente 
         // essa configuração fica no appConfig ou melhor appConfig.ini

        public Boolean moverPdfSemAssinaturaSRM(FileInfo arquivoMover, ReleaseJucecProperties releaseJucec,DocumentosProperties documento,ConfiguracaoProperties configApp)
        {
            RegrasDocumentos regrasDocumentos = new RegrasDocumentos();
            
            bool isJucec =  regrasDocumentos.tipoIndexDocumento_Jucec_Sesaz(documento);

            if (isJucec)
            {

                if (arquivoMover.FullName != "")
                {
                    FileInfo dirOrigemPdf = new FileInfo(arquivoMover.FullName);

                    if (Directory.Exists(configApp.caminhoPdfSemAssinatura))
                    {
                        if (!File.Exists(configApp.caminhoPdfSemAssinatura + "\\" + releaseJucec.numProtocolo + ".pdf"))
                        {
                            System.IO.File.Copy(arquivoMover.FullName, configApp.caminhoPdfSemAssinatura + "\\" + releaseJucec.numProtocolo + ".pdf");

                            log.escreveLog(".PDF sem assinatura enviado para o SRM " + arquivoMover.Name, configApp);

                            regrasDocumentos.LogDocumento(documento, Constantes.ENVIADO_SRM);

                            return true;
                        }else{

                            log.escreveLog("Aviso: Não foi possivel enviar para o SRM porque talvez o arquivo pdf ja exista neste diretorio " + configApp.caminhoPdfSemAssinatura + "\\" + releaseJucec.numProtocolo + ".pdf", configApp);
                            return true;
                        }
                    }
                    else
                    {
                        log.escreveLog("Aviso: Diretorio não existe " + configApp.caminhoPdfSemAssinatura, configApp);

                        System.IO.Directory.CreateDirectory(configApp.caminhoPdfSemAssinatura);

                        if (Directory.Exists(configApp.caminhoPdfSemAssinatura))
                        {
                            log.escreveLog("Aviso: Diretorio criado comsucesso " + configApp.caminhoPdfSemAssinatura, configApp);

                            System.IO.File.Copy(arquivoMover.FullName, configApp.caminhoPdfSemAssinatura + "\\" + releaseJucec.numProtocolo + ".pdf");

                            log.escreveLog("Aviso: Enviado SRM " + arquivoMover.Name, configApp);

                            regrasDocumentos.LogDocumento(documento, Constantes.ENVIADO_SRM);

                            return true;
                        }
                        else
                        {
                            log.escreveLog("Não foi possivel criar o diretorio verifique a permissão na pasta protocolo SRM" + releaseJucec.numProtocolo, configApp);
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool processaImagem(int idDocumento, int idProcesso)
        {

            return false;
        }

        public string pegaCodLicenca()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("I48cdAeQI2C6Nr11wIRdjymp2hCRumkl44C3+5XZ+RP27+giIQrcOs8CaUDBVuwIueoIZXhXmG1T3wKfUtWby");
            stringBuilder.Append("iYjeN+CR9OrQpfnKL2PCmBetzHBRKeLuVANHBvA+V1jvaAHjGRYYObbD6dOT/sxvxBlPCekeUIQ4pKGjgmsb9");
            stringBuilder.Append("/bvGDtXWM9D/wBiPlyE80R5whjKh95oBHSFbo67sF7aLDZZJCMg8sXJNj1gijH8+blnhgrW8aZ9MvGelSJ2be");
            stringBuilder.Append("BTzIe32hPdgcrMaNiaZA0h+9G19auJGvaCp7dA420BCvwV6WLMMSiExq7N59YaG9RStojY2lgMVI4YT9Bk/8T");
            stringBuilder.Append("ypQlQbRQUFNJHowH4Ues9NqYRRcH9TRPso0Wp7SjFxzh6Wqm16SSe5ihgzdL5qC9NR86LZ8m9r8gro6Zqol1F");
            stringBuilder.Append("kc1qrltP6+U+fB7q5WnkB90JgxBWpNgMZD+VtvTm+Jc/KdTqEKcOkk1wUscCwsE8QHtoNSCYUH9lNDetewAMJ");
            stringBuilder.Append("qEbFuVX4+YoCECapVA5GClnEkO2npKHsAZFxzYHJPjThwcb4BbZWj039+dN0HZQiy6hFND+MU3jC2nHyl6W28");
            stringBuilder.Append("/njw2+07LE+jLO9frIuUBWC8yiXqzio4TmQLROKXQdwUCuzI06V1JIZ6FZZsQ3kL2MTRJ1GAqvIDqd30udIs9");
            stringBuilder.Append("UIzk8g4GGcCYqoeAhORBnJtkbLpyT1i9WT5RGhCz2+OI37PrMlUKUgJk3fBUyRLd961ylL0u31oCfdX2+wQzL");
            stringBuilder.Append("el5DTcl5Se+lIM/kcPb53Dje8zNfWvqHo3k6mM0ofXZuQFX1LwVL863w7gEKyjn+g32gndCniIVj7cYe8wJOA");
            stringBuilder.Append("z/fNDupAH0t+OCH9Q2jDOW5emnghm439S4h3/UH6VcsXm737iuuUx5eurm9oZw7Ky7ME+hKzXm6vyDyEom/n3");
            stringBuilder.Append("J+rJUOyZt6ULzM7Aq90CfloJpxtCVhnxEbWHIaOoV8jUOBqLbb3tBc5bMQhuUimUkK3u9q9lBfFw3i1Pkvf6e");
            stringBuilder.Append("Wk1zZsRKAQRTJuORisThHKB0Tm0Lt5O63hrAbSBXRWwlZTXeS57QheuD2zudwC/HGDhk");
            return stringBuilder.ToString();
        }
        public string carregaXML(FileInfo arquivo, string idCertificado)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\"?>");
            stringBuilder.Append("<DeskSignerSDKPDF>");
            stringBuilder.Append("  <Caminho>" + arquivo.FullName + "</Caminho>");
            stringBuilder.Append("  <Certificado>");
            stringBuilder.Append("    <IdCertif>" + idCertificado + "</IdCertif>");
            stringBuilder.Append("    <CaminhoPKCS12></CaminhoPKCS12> ");
            stringBuilder.Append("    <SenhaPKCS12></SenhaPKCS12> ");
            stringBuilder.Append("  </Certificado>");
            stringBuilder.Append("  <CaminhoSaida>" + arquivo.FullName.Replace("_.pdf", arquivo.Extension) + "</CaminhoSaida>");
            stringBuilder.Append("  <bPrimeiraPagina>N</bPrimeiraPagina>");
            stringBuilder.Append("  <bUltimaPagina>N</bUltimaPagina>");
            stringBuilder.Append("  <bTodasPaginas>S</bTodasPaginas>");
            stringBuilder.Append("  <Descricao></Descricao>");
            stringBuilder.Append("  <Pagina></Pagina>");
            stringBuilder.Append("  <bVisivel>S");
            stringBuilder.Append("    <bImagem>N");
            stringBuilder.Append("      <CaminhoImagem></CaminhoImagem>");
            stringBuilder.Append("      <TamImagem></TamImagem>");
            stringBuilder.Append("      <ImagemAltura></ImagemAltura>");
            stringBuilder.Append("      <ImagemLargura></ImagemLargura>");
            stringBuilder.Append("    </bImagem>");
            stringBuilder.Append("    <bTexto>N");
            stringBuilder.Append("      <bTextoCompleto>N</bTextoCompleto>");
            stringBuilder.Append("      <TamFonteTitulo></TamFonteTitulo>");
            stringBuilder.Append("      <TamFonteTexto></TamFonteTexto>");
            stringBuilder.Append("    </bTexto>");
            stringBuilder.Append("    <AssinaturaPosX>0</AssinaturaPosX>");
            stringBuilder.Append("    <AssinaturaPosY>0</AssinaturaPosY>");
            stringBuilder.Append("  </bVisivel>");
            stringBuilder.Append("  <Detached>S</Detached>");
            stringBuilder.Append("</DeskSignerSDKPDF>");
            return stringBuilder.ToString();
        }

        public string traducaoErro(string codErro)
        {
            switch (codErro)
            {
                case "1":
                    return "Parâmetros inválidos (nulo). - CodErro: " + codErro;
                case "2":
                    return "Parâmetro caminho deve apontar para um arquivo valido. - CodErro: " + codErro;
                case "3":
                    return "Parametro Caminho Saida deve apontar para um caminho existente, mas o arquivo de saída não deve existir. - CodErro: " + codErro;
                case "4":
                    return "IDCertif ou CaminhoPKCS12 devem estar com tamanho maior que ZERO (apenas uma delas). - CodErro: " + codErro;
                case "5":
                    return "Se CaminhoPKCS12 estiver com tamanho maior que zero, deve apontar para um arquivo válido. - CodErro: " + codErro;
                case "6":
                    return "As TAGs bPrimeiraPagina, bUltimaPagina, bTodasPaginas e Pagina admitem somente as seguintes combinações:  -  -  - Somente uma delas pode conter S e neste caso Pagina deve ser ZERO  -  - Se Pagina é maior que ZERO, todas as outras TAGS devem conter N.  -  - CodErro: " + codErro;
                case "7":
                    return "TAG bVisivel deve conter apenas S ou N. - CodErro: " + codErro;
                case "8":
                    return "TAG bImagem deve conter apenas S ou N. O valor S somente pode ser aceito se bVisivel estiver com S também. - CodErro: " + codErro;
                case "9":
                    return "TAG Caminhoimagem deve ser validada somente se bImagem contiver S. Neste caso esta TAG deve apontar para um caminho arquivo válido. - CodErro: " + codErro;
                case "10":
                    return "TAG TamImagem aceita valores entre 10 e 500 inclusive somente se CaminhoImagem estiver com valor. - CodErro: " + codErro;
                case "11":
                    return "TAG ImagemAltura aceita valores maiores que ZERO somente se CaminhoImagem estiver com valor. - CodErro: " + codErro;
                case "12":
                    return "TAG ImagemLargura aceita valores maiores que ZERO somente se CaminhoImagem estiver com valor. - CodErro: " + codErro;
                case "13":
                    return "TAG bTexto deve conter apenas S ou N. O valor S somente pode ser aceito se bVisivel estiver com S também e bImagem estiver com S. - CodErro: " + codErro;
                case "14":
                    return "TAG bTextCompleto deve conter apenas S ou N. O valor S somente pode ser aceito se bTexto estiver com S também. - CodErro: " + codErro;
                case "15":
                    return "TAG TamFonteTitulo aceita valores entre 1 e 100 inclusive somente se bTexto estiver com S. - CodErro: " + codErro;
                case "16":
                    return "TAG TamFonteTexto aceita valores entre 1 e 100 inclusive somente se bTexto estiver com S. - CodErro: " + codErro;
                case "17":
                    return "TAG AssinaturaPosX aceita valores entre 0 e 95 inclusive somente se bVisivel estiver com S. - CodErro: " + codErro;
                case "18":
                    return "TAG AssinaturaPosY aceita valores entre 0 e 95 inclusive somente se bVisivel estiver com S. - CodErro: " + codErro;
                case "19":
                    return "Erro na inicialização. - CodErro: " + codErro;
                case "20":
                    return "Erro ao abrir arquivo PDF de entrada. - CodErro: " + codErro;
                case "21":
                    return "Erro ao abrir arquivo temporario. - CodErro: " + codErro;
                case "22":
                    return "PDF criptografado não pode ser assinado. - CodErro: " + codErro;
                case "23":
                    return "Erro ao verificar criptografia do PDF. - CodErro: " + codErro;
                case "24":
                    return "Arquivo PKCS12 não existe. - CodErro: " + codErro;
                case "25":
                    return "Senha do arquivo PKCS12 invalida. - CodErro: " + codErro;
                case "26":
                    return "Erro ao ler arquivo PKCS12. - CodErro: " + codErro;
                case "27":
                    return "Arquivo PKCS12 invalido. - CodErro: " + codErro;
                case "28":
                    return "Erro ao procurar/obter certificado. - CodErro: " + codErro;
                case "29":
                    return "Certificado não encontrado. - CodErro: " + codErro;
                case "30":
                    return "Erro ao ler imagem. - CodErro: " + codErro;
                case "31":
                    return "Erro ao calcular posição da assinatura. - CodErro: " + codErro;
                case "32":
                    return "Pagina solicitada não existe. - CodErro: " + codErro;
                case "33":
                    return "Erro ao associar certificado na assinatura. - CodErro: " + codErro;
                case "34":
                    return "Erro ao assinar o PDF. - CodErro: " + codErro;
                case "35":
                    return "Erro ao copiar arquivo assinado para o destino. - CodErro: " + codErro;
                case "36":
                    return "Erro interno ao assinar PDF. - CodErro: " + codErro;
                case "37":
                    return "Licença inválida. - CodErro: " + codErro;
                case "38":
                    return "Licença vencida. - CodErro: " + codErro;
                case "39":
                    return "Licença especificada não é deste produto. - CodErro: " + codErro;
                case "40":
                    return "Não há certificados disponíveis na maquina. - CodErro: " + codErro;
                case "41":
                    return "Certificado inválido. - CodErro: " + codErro;
                case "42":
                    return "Erro ao validar certificado. - CodErro: " + codErro;
                case "43":
                    return "Computador não contem certificados digitais. - CodErro: " + codErro;
                case "44":
                    return "Erro ao listar certificado digitais. - CodErro: " + codErro;
                case "45":
                    return "Imagem não está no formato JPEG2000 - CodErro: " + codErro;
                case "46":
                    return "Erro ao validar formato da imagem - CodErro: " + codErro;
            }
            return "0";
        }

        public string montagemXML(CaixaProperties caixa, DocumentosProperties documento, ReleaseJucecProperties releaseJucec, ProtocoloProperties protocolo)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string arg_0B_0 = string.Empty;
            string arg_11_0 = string.Empty;
            string arg_17_0 = string.Empty;
            string arg_1D_0 = string.Empty;
            string arg_23_0 = string.Empty;
            string arg_29_0 = string.Empty;
            string arg_2F_0 = string.Empty;

            stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            stringBuilder.AppendLine("<ARQUIVOINDEX>");
            stringBuilder.AppendLine("\t<CAIXA>{0}</CAIXA>");
            stringBuilder.AppendLine("\t<PROTOCOLO>{1}</PROTOCOLO>");
            stringBuilder.AppendLine("\t<NR_PROTOCOLO>{2}</NR_PROTOCOLO>");
            stringBuilder.AppendLine("\t<NR_NIRE>{3}</NR_NIRE>");
            stringBuilder.AppendLine("\t<NR_ARQUIVAMENTO>{4}</NR_ARQUIVAMENTO>");
            stringBuilder.AppendLine("\t<DATA_GERACAO>{5}</DATA_GERACAO>");
            stringBuilder.AppendLine("\t<IMAGEM>{6}</IMAGEM>");
            stringBuilder.AppendLine("</ARQUIVOINDEX>");

            return string.Format(stringBuilder.ToString(), new object[]
				{
					caixa.DsCaixa,
					protocolo.dsProtocolo,
					releaseJucec.numProtocolo.PadLeft(9, '0'),
					releaseJucec.nire,
					releaseJucec.numArquivamento,
					DateTime.Now.ToString("dd/MM/yyyy"),
					documento.idProcesso + ".pdf"
				});

            return null;
        }

        public void gerarArquivoXML(DocumentosProperties documento, string xml, ConfiguracaoProperties configApp, DirectoryInfo pathDestino)
        {
            try
            {
                this.escreveArquivo(string.Concat(new object[]
				{
					pathDestino.Parent.FullName.Replace("\\NUMERO_PROTOCOLO\\", "\\NIRE_ARQUIVAMENTO\\"),"\\",documento.idProcesso,".xml"}), xml);
            }
            catch (Exception ex)
            {
                log.escreveLog("Erro em geraArquivoSefaz XML :" + ex.Message + "\n" + ex.StackTrace, configApp);
            }
        }

        private void escreveArquivo(string caminhoArquivo, string texto)
        {
            StreamWriter streamWriter = null;
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(caminhoArquivo.Substring(0, caminhoArquivo.LastIndexOf("\\")));
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                streamWriter = new StreamWriter(caminhoArquivo, false, Encoding.Default);
                if (!texto.Trim().Equals(string.Empty))
                {
                    streamWriter.WriteLine(texto);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
        }

        public void geraArqvuivoCSV(string caminhoArquivoCSV, ReleaseJucecProperties releaseJucec, DirectoryInfo pathDestino, CaixaProperties caixa)
        {
            StreamWriter sw = null;

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(caminhoArquivoCSV.Substring(0, caminhoArquivoCSV.LastIndexOf("\\")));

                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                sw = new StreamWriter(caminhoArquivoCSV, false, Encoding.Default);

                sw.WriteLine("CAIXA;NUMERO_DO_PROTOCOLO;CAMINHO_IMAGEM");
                sw.Write(caixa.DsCaixa);
                sw.Write(";");
                sw.Write(releaseJucec.numProtocolo);
                sw.Write(";");
                sw.Write(pathDestino.FullName + "\\" + releaseJucec.numProtocolo + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }


        public FileInfo[] getPdfs(ConfiguracaoProperties configApp, CaixaProperties caixa, DirectoryInfo pathDestino)
        {

            FileInfo[] filesPdf = new DirectoryInfo(pathDestino.FullName).GetFiles("*_.*pdf", SearchOption.AllDirectories);

            log.escreveLog( "Arquivos para assinar no diretorio: ", configApp);

            return filesPdf;
        }
    }
}
