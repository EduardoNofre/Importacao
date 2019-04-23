using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using TCIUtility;

namespace TCIReleaseJucec.Util
{
    public class Util
    {
        static DirectoryInfo dirDestino;
        static string dsCaixa = string.Empty;
        static string dsProtocolo = string.Empty;
        static string numProtocolo = string.Empty;
        static string nire = string.Empty;
        static string numArquivamento = string.Empty;
        static string nomeempresarial = string.Empty;
        static string ato = string.Empty;
        static string evento = string.Empty;
        static string descricao = string.Empty;
        static string municipio = string.Empty;
        static string cnpj = string.Empty;

        public static string ajustaDiretorio(string str)
        {
            str = str.Replace("/", "").Replace("\\", "").Replace("*", "").Replace("\"", "").Replace("?", "").Replace(">", "").Replace("<", "").Replace("|", "").Replace(":", "");
            if (string.IsNullOrEmpty(str))
            {
                str = "NAO INFORMADO";
            }
            return str;
        }

        public static string DecryptConnString()
        {
            try
            {
                string[] retorno = new string[2];

                retorno[0] = ConfigurationSettings.AppSettings["stringConexaoBanco"].ToString();

                retorno[1] = ConfigurationSettings.AppSettings["databaseType"].ToString();

                Byte[] b = Convert.FromBase64String(retorno[0]);

                string conexaoDescript = System.Text.ASCIIEncoding.ASCII.GetString(b);

                return conexaoDescript;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void escreveLog(string texto)
        {
            try
            {
                string text = Application.StartupPath + "\\log\\";
                if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["caminhoLog"]))
                {
                    text = ConfigurationSettings.AppSettings["caminhoLog"];
                }
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                using (StreamWriter streamWriter = new StreamWriter(text + "log" + DateTime.Now.ToString("yyyyMMdd_HH") + "00.txt", true, Encoding.Default))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + texto);
                    streamWriter.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public static string carregaXML(FileInfo arquivo, string id)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\"?>");
            stringBuilder.Append("<DeskSignerSDKPDF>");
            stringBuilder.Append("  <Caminho>" + arquivo.FullName + "</Caminho>");
            stringBuilder.Append("  <Certificado>");
            stringBuilder.Append("    <IdCertif>" + id + "</IdCertif>");
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

        public static string pegaCodLicenca()
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

        public static string traducaoErro(string codErro)
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

        private static int QtdPaginasPDF(string FileName)
        {
            int result;
            try
            {
                using (FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    StreamReader streamReader = new StreamReader(fileStream);
                    string input = streamReader.ReadToEnd();
                    fileStream.Close();
                    fileStream.Dispose();
                    Regex regex = new Regex("/Type\\s*/Page[^s]");
                    MatchCollection matchCollection = regex.Matches(input);
                    if (matchCollection.Count > 0)
                    {
                        result = matchCollection.Count;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }


        private static void geraArquivoSefazCSV(int idProcesso, string valor)
        {
            try
            {
                Util.escreveArquivo(string.Concat(new object[]
				{
					dirDestino.Parent.FullName.Replace("\\NUMERO_PROTOCOLO\\", "\\NIRE_ARQUIVAMENTO\\"),"\\",idProcesso,".csv"}), valor);
            }
            catch (Exception ex)
            {
                Util.escreveLog("Erro em geraArquivoSefaz:" + ex.Message + "\n" + ex.StackTrace);
            }
        }


        public static void escreveArquivo(string caminhoArquivo, string texto)
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

        public static string montagemXMLSefaz(int idDocumento)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
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
					dsCaixa,
					dsProtocolo,
					numProtocolo.PadLeft(9, '0'),
					nire,
					numArquivamento,
					DateTime.Now.ToString("dd/MM/yyyy"),
					idDocumento + ".pdf"
				});
            }
            catch (Exception ex)
            {
                Util.escreveLog("Erro em montagemXMLSefaz:" + ex.Message + "\n" + ex.StackTrace);
            }
            return stringBuilder.ToString();
        }

        public static void processaImagemSefaz(FileInfo origem, FileInfo destino, int QtdImgDoc)
        {
            try
            {
                destino = new FileInfo(string.Concat(new string[]
				{                    
					destino.Directory.Parent.FullName.Replace("\\NUMERO_PROTOCOLO\\", "\\NIRE_ARQUIVAMENTO\\"),"\\",destino.Name}));

                if (!destino.Directory.Exists)
                {
                    destino.Directory.Create();
                }
                ClassUtil.Instancia.ConvertTIFtoMultiPage(origem, destino, "pdf");
            }
            catch (Exception ex)
            {
                Util.escreveLog("Erro ao tentar processaImagemSefaz: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static DateTime pesquisaDataExportacao()
        {
            DataTable dataTable = new DataTable();
            DateTime result = default(DateTime);
            try
            {
                dataTable = Persistencia.Instancia.pesquisaDataExportacao();
                result = (DateTime)dataTable.Rows[0]["getdate"];
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static void geraArquivo(int idProcesso, string numProtocolo, string numDoc, string numPenultima, string numUltimaImagem)
        {
            try
            {
                if (numDoc.Length > 10)
                {
                    numDoc.Substring(0, 9);
                }
                numPenultima = numPenultima.ToUpper().Replace(".TIF", "");
                if (numPenultima.Length > 10)
                {
                    numPenultima.Substring(0, 9);
                }
                numUltimaImagem = numUltimaImagem.ToUpper().Replace(".TIF", "");
                if (numUltimaImagem.Length > 10)
                {
                    numUltimaImagem.Substring(0, 9);
                }
                if (numProtocolo.Length > 9)
                {
                    numProtocolo.Substring(0, 8);
                }
                numDoc = numDoc.PadLeft(10, '0');
                numPenultima = numPenultima.PadLeft(10, '0');
                numUltimaImagem = numUltimaImagem.PadLeft(10, '0');
                numProtocolo = numProtocolo.PadLeft(9, '0');

                string texto = string.Concat(new string[]
				{
					"\"I01q\";\"01\";\"",
					numDoc,
					"\";\"",
					numPenultima,
					"\";\"",
					numUltimaImagem,
					"\";\"",
					numProtocolo,
					"\""
				});

                Util.geraArquivoSefaz(idProcesso, Util.montagemXMLSefaz(idProcesso));

                //ReleaseJucec.escreveArquivoCSV(ReleaseJucec._dirDestino.FullName + ".csv", texto);

                Util.escreveArquivoCSV(ConfigurationSettings.AppSettings["pastaPrincipal"] + idProcesso + ".csv", texto);

            }
            catch (Exception ex)
            {
                Util.escreveLog("Erro em geraArquivo:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static void geraArquivoSefaz(int idDocumento, string valor)
        {
            try
            {
                Util.escreveArquivo(string.Concat(new object[]
				{
					dirDestino.Parent.FullName.Replace("\\NUMERO_PROTOCOLO\\", "\\NIRE_ARQUIVAMENTO\\"),"\\",idDocumento,".xml"}), valor);
            }
            catch (Exception ex)
            {
                Util.escreveLog("Erro em geraArquivoSefaz:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public static void escreveArquivoCSV(string caminhoArquivoCSV, string texto)
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
                if (!texto.Trim().Equals(string.Empty))
                {
                    sw.WriteLine("CAIXA;NUMERO_DO_PROTOCOLO;NIRE;NUMERO_DO_ARQUIVAMENTO;CNPJ;NOME_EMPRESARIAL;ATO;EVENTO;DESCRICAO;MUNICIPIO;CAMINHO_IMAGEM");
                    sw.Write(dsCaixa);
                    sw.Write(";");
                    sw.Write(numProtocolo);
                    sw.Write(";");
                    sw.Write(nire);
                    sw.Write(";");
                    sw.Write(numArquivamento);
                    sw.Write(";");
                    sw.Write(cnpj);
                    sw.Write(";");
                    sw.Write(nomeempresarial);
                    sw.Write(";");
                    sw.Write(ato);
                    sw.Write(";");
                    sw.Write(evento);
                    sw.Write(";");
                    sw.Write(descricao);
                    sw.Write(";");
                    sw.Write(municipio);
                    sw.Write(";");
                    sw.Write(dirDestino.FullName + ".pdf", texto);
                }
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

        public static void assinarArquivo(FileInfo arquivo)
        {
            DeskSignerSDKPDF.clsPDF clsPDF = new DeskSignerSDKPDF.clsPDF();
            string xml = string.Empty;
            string text = string.Empty;
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(new DeskSignerSDKPDF.clsCertificados
                {
                    Licenca = Util.pegaCodLicenca()

                }.ListarCertificadosPessoais());

                string text2 = string.Empty;

                string text3 = ConfigurationSettings.AppSettings["nomeCertificado"];

                if (string.IsNullOrEmpty(text3))
                {
                    throw new Exception("Tag [nomeCertificado] não configurada no arquivo .config.");
                }
                Util.escreveLog("Quantidade de Certificados encontrados:..." + xmlDocument.SelectNodes("//Certificado").Count);

                foreach (XmlNode xmlNode in xmlDocument.SelectNodes("//Certificado"))
                {
                    if (xmlNode.InnerText.ToUpper().Contains(text3))
                    {
                        text2 = xmlNode.SelectSingleNode("IDCertificado").FirstChild.InnerText;
                        Util.escreveLog(string.Format("Id do Certificado {0} encontrado para {1}:...", text2.ToString(), text3));
                        break;
                    }
                }
                if (string.IsNullOrEmpty(text2))
                {
                    throw new Exception(string.Format("Certificado não encontrado nesta maquina [{0}]!.{1}É preciso existir um certificado válido para [{2}]", Environment.MachineName, Environment.NewLine, text3));
                }
                clsPDF.Licenca = Util.pegaCodLicenca();

                xml = clsPDF.Assinar(Util.carregaXML(arquivo, text2));

                xmlDocument.LoadXml(xml);

                text = xmlDocument.SelectSingleNode("//DeskSignerSDKPDF/CodErro").FirstChild.InnerText;
                if (text != "0")
                {
                    throw new Exception(string.Format("Erro ao assinar Arquivo ao tentar assinar o arquivo: {0}{1}", Environment.NewLine, Util.traducaoErro(text)));
                }
                arquivo.Delete();
            }
            catch
            {
                throw;
            }
            finally
            {
                clsPDF = null;
            }
        }
    }
}
