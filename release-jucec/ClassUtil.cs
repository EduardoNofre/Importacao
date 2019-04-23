using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Collections;
using System.Drawing;
using System.Web;
using Accusoft.ImagXpressSdk;

namespace TCIUtility
{
    public class ClassUtil
    {
        #region Variáveis
        private static ClassUtil instancia;
        #endregion Variáveis

        #region Contrutores
        private ClassUtil()
        {
        }

        public static ClassUtil Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ClassUtil();
                }
                return instancia;
            }

        }
        #endregion Contrutores

        #region Variáveis Publicas
        public static int idUsuario = 0;
        public static int idProjeto = 0;
        public static int idProduto = 0;
        #endregion

        #region Enumeracao Status
        public enum Status : int
        {
            Disponível = 1,
            Aberto = 2,
            Fechado = 3,
            AIndexar = 4,
            Indexando = 5,
            Indexado = 6,
            EmConferência = 7,
            ReprovadoConferência = 8,
            AprovadoConferência = 9,
            ProntoParaCQ = 10,
            ReprovadoCQ = 11,
            AprovadoCQ = 12,
            ProntoParaEnvio = 13,
            Enviando = 14,
            EnviadoFTP = 15,
            EnviadoSISDOC = 16,
            ImgDisponivel = 17,
            ImgEnviada = 18,
            ImgExcluida = 19,
            ImgInexistente = 20,
            Vazio = 21,
            Abandonado = 22,
            ReprovadoIndexer = 23,
            DocumentoExcluido = 24,
            EmTratamento = 25,
            Auditando = 26,
            ServicoExecutando = 27,
            ServicoParado = 28,
            AprovadoComIrr = 29,
            ProntoParaEnvioComIrr = 30,
            ProntoParaCarga = 31,
            Cancelado = 32,
            ImagensExcluidasFullFileDelete = 33,
            ProtocoloCaixaExcluidoPeloUsuário = 34,
            ProntoParaRemessa = 35,
            EnviadoParaRedigitalizacaoCapturaDistribuida = 36,
            AguardandoRetornoCritica = 101

        }
        #endregion

        #region Enumeracao Descrição de Parametro
        public enum DsParametro : int
        {
            CaminhoProduto = 1,
            DiretorioImagemCapture = 2,
            DiretorioImagemTratada = 3,
            Anomalia = 4,
            CaminhoDaImagem = 5,
            PercentualCQ = 6,
            PercentualReprovacao = 7,
            IndexacaoTipificada = 8,
            LogoTipo = 9,
            TipoDeAuditoria = 10,
            PossuiConference = 11,
            ManterValoresAvancar = 12,
            PossuiCarimbo = 13,
            Send2FTPDestinatário = 14,
            Send2FTPBackup = 15,
            Send2FTPSMTP = 16,
            Send2FTPSisDoc = 17,
            AprovarIrregularidade = 18,
            UtilizarWebService = 19,
            HabilitarCapture = 20,
            HabilitarBotãoIncluirImg = 21,
            FullFileDeleteApagaImagensRepositório = 22,
            ProjetoSomenteLocal = 23,
            DllNameValidationScript = 24
        }
        #endregion

        #region Validação de Data
        public static bool ValidacaoData(string Valor)
        {
            try
            {
                CultureInfo culture = new CultureInfo("pt-BR");

                Convert.ToDateTime(Valor, culture);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Formatar Data
        public static string FormataData(string Valor)
        {
            if (Valor == "")
                return "null";
            else
                return "'" + Convert.ToDateTime(Valor).ToString("yyyy-MM-dd") + "'";
        }
        #endregion

        #region Validação de Número
        public static bool IsNumeric(string valor, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(valor, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        #endregion

        #region MD5
        /// <summary>
        /// Função para Geracao de Criptografia MD5
        /// Utilizado para trafegar dados de login do usuario
        /// </summary>
        /// <param name="texto">string a ser criptografada</param>
        /// <returns>string criptografada</returns>
        public static string CryptografaSenha_MD5(string texto)
        {
            byte[] oBytes = Encoding.Default.GetBytes(texto);
            try
            {
                MD5CryptoServiceProvider oCrypt = new MD5CryptoServiceProvider();
                byte[] hash = oCrypt.ComputeHash(oBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw new Exception("Erro ao criptografar dados");
            }
        }
        #endregion

        #region TrataApostrofo
        public static string TrataApostrofo(string Texto)
        {
            if (Texto != null && Texto.Length > 0)
            {
                string texto = Texto;
                texto = texto.Replace("'", "''");
                texto = texto.Replace("\"", "\\");

                return texto;
            }

            return string.Empty;
        }
        #endregion

        #region Faz a Quebra do texto
        public static string[] QuebraTexto(string Texto, char Separador)
        {
            string[] texto = null;

            if (Texto != null && Texto.Length > 0)
            {
                texto = Texto.Split(new char[] { Separador });
                return texto;
            }

            return texto;
        }

        public static string[] QuebraTexto(string Texto, int Posicao, char Separador)
        {
            string[] texto = new string[2];

            if (Texto != null && Texto.Length > 0)
            {
                texto[0] = Texto.Substring(Posicao, Texto.IndexOf(Separador)).Trim();
                texto[1] = Texto.Substring(Texto.IndexOf(Separador) + 1).Trim();

                return texto;
            }

            return texto;
        }
        #endregion

        #region Adiciona linha Selecione
        private void adicionaLinhaSelecione(DataTable datatable, string NomeIdCampo, string NomeCampo)
        {
            //datatable.Columns.Add(new DataColumn(NomeCampo, System.Type.GetType("System.String")));
            //datatable.Columns.Add(new DataColumn(NomeIdCampo, System.Type.GetType("System.Int32")));

            DataRow dr = datatable.NewRow();
            dr[NomeCampo] = "--Selecione--";
            dr[NomeIdCampo] = 0;
            datatable.Rows.Add(dr);
        }
        #endregion

        #region Preeche Combo
        public void PreencheCombo(System.Windows.Forms.ComboBox combo, DataTable dt, string displayMember, string valueMember, string valorSelecionado)
        {
            adicionaLinhaSelecione(dt, valueMember, displayMember);

            try
            {
                this.PreencherCombo(combo, dt, displayMember, valueMember, valorSelecionado);
            }
            catch (Exception exp)
            {
                throw new Exception("Erro ao preencher Combo de Lotes:\n" + exp.Message);
            }
        }

        private void PreencherCombo(System.Windows.Forms.ComboBox combo, DataTable dtTable, string displayMember, string valueMember, string valorSelecionado)
        {
            try
            {
                combo.DataSource = dtTable;
                combo.DisplayMember = displayMember;
                combo.ValueMember = valueMember;

                if (valorSelecionado != null)
                {
                    int i = 0;
                    while (i < dtTable.Rows.Count && dtTable.Rows[i][valueMember].ToString() != valorSelecionado)
                        ++i;

                    combo.SelectedIndex = (i < combo.Items.Count) ? i : -1;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message, ex);
            }
        }
        #endregion

        #region Executa DLL
        public static object ExecutaDll(string dllName, string className)
        {
            String libraryPath = null;

            if (File.Exists(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\")) + "\\" + dllName))
                libraryPath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\")) + "\\" + dllName;
            else if (File.Exists(Directory.GetCurrentDirectory() + @"\" + dllName))
                libraryPath = Directory.GetCurrentDirectory() + @"\" + dllName;
            else
                throw new Exception(dllName + " NÃO ENCONTRADO EM\n" +
                    Directory.GetCurrentDirectory() + " E\n" +
                    Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\")));

            Assembly assembly = Assembly.LoadFrom(libraryPath);

            String libName = dllName.Substring(0, dllName.Length - 4);
            return assembly.CreateInstance(libName + "." + className, true);
        }
        #endregion

        #region Convert Imagem em byte

        internal static byte[] ConvertImageToByte(Stream stream)
        {
            Int64 lenght = 0;
            byte[] buffer = null;
            try
            {
                if (stream.Length < 60000000)
                    lenght = stream.Length;
                else
                    lenght = 60000000;

                buffer = new byte[lenght];

                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Converter image to byte..:\n" + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                buffer = null;
                stream.Dispose();
                stream = null;
            }
        }
        //public static byte[] ConvertImageToByte(Stream stream)
        //{
        //    MemoryStream ms = null;

        //    try
        //    {
        //        byte[] buffer = new byte[stream.Length];
        //        ms = new MemoryStream();

        //        while (true)
        //        {
        //            int read = stream.Read(buffer, 0, buffer.Length);
        //            if (read <= 0)
        //                return ms.ToArray();

        //            ms.Write(buffer, 0, read);
        //        }

        //        ms.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ms != null)
        //            ms.Close();

        //        throw new Exception("Erro ao Converter image to byte..:\n" + ex.Message + "\n" + ex.StackTrace);
        //    }
        //}
        #endregion

        #region Convert Byte em Imagem
        public static void ConvertByteToImage(byte[] mArrayImage, string mCaminhoImagem, string mFileName)
        {
            if (mArrayImage != null)
            {
                if (!Directory.Exists(mCaminhoImagem))
                    Directory.CreateDirectory(mCaminhoImagem);

                using (FileStream fsOut = new FileStream(mCaminhoImagem + @"\" + mFileName, FileMode.OpenOrCreate))
                {
                    BinaryWriter w = new BinaryWriter(fsOut);
                    w.Write(mArrayImage);
                    w.Close();
                }
            }
        }
        #endregion

        #region Convert TIF para MultiPage
        public void ConvertTIFtoMultiPage(FileInfo mCaminhoOrigem, FileInfo mCaminhoDestino, string mTipo)
        {

            try
            {
                using (ImagXpress _imageX10 = new ImagXpress())
                {
                    _imageX10.Licensing.UnlockRuntime(1908225079, 373669040, 1341647942, 30454);
                    ImageX image = ImageX.FromFile(_imageX10, mCaminhoOrigem.FullName);
                    try
                    {
                        SaveOptions so = new SaveOptions();
                        so.Format = ImageXFormat.Tiff;
                        if (mTipo.ToUpper() == "PDF")
                            so.Format = ImageXFormat.Pdf;

                        so.Tiff.MultiPage = true;
                        so.Pdf.MultiPage = true;
                        if (image.BitsPerPixel != 1)
                        {
                            so.Tiff.Compression = Compression.Jpeg;
                            so.Pdf.Compression = Compression.Jpeg;
                        }
                        else
                        {
                            so.Tiff.Compression = Compression.Group4;
                            so.Pdf.Compression = Compression.Group4;
                        }

                        image.Save(mCaminhoDestino.FullName, so);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        image.Dispose();
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception("Erro ao processar imagem:\t" + exp.Message + Environment.NewLine + exp.StackTrace);
            }
        }

        //public void ConvertTIFtoMultiPage(FileInfo mCaminhoOrigem, FileInfo mCaminhoDestino, string mTipo)
        //{
        //    PICImagXpress imageX = new PICImagXpress();

        //    try
        //    {
        //        imageX.FileName = mCaminhoOrigem.FullName;
        //        if (mTipo.ToUpper() == "PDF")
        //            imageX.SaveFileType = enumSaveFileType.FT_PDF;
        //        else
        //            switch (imageX.IBPP)
        //            {
        //                case 1:
        //                    imageX.SaveFileType = enumSaveFileType.FT_TIFF_G4;
        //                    break;
        //                default:
        //                    imageX.SaveFileType = enumSaveFileType.FT_JPEG;
        //                    break;
        //            }
        //        //imageX.SaveFileType = (mTipo.ToUpper() == "PDF") ? enumSaveFileType.FT_PDF : enumSaveFileType.FT_TIFF_G4;
        //        imageX.SaveFileName = mCaminhoDestino.FullName.ToUpper().Replace(mCaminhoDestino.Extension, (mTipo.ToUpper() == "PDF") ? ".pdf" : ".tif");
        //        imageX.SaveMultiPage = true;
        //        imageX.SaveFile();
        //    }
        //    catch (Exception exp)
        //    {
        //        throw new Exception("Erro ao processar imagem:\t" + exp.Message);
        //    }
        //    finally
        //    {
        //        if (imageX != null)
        //            imageX.Dispose();

        //        imageX = null;
        //    }
        //}

        #endregion

    }
}
