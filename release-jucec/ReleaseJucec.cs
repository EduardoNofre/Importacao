
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using TCIUtility;



namespace TCIReleaseJucec
{
    public partial class ReleaseJucec : ServiceBase
    {
        private IContainer components;
        int intervalo;
        int idProjeto;
        string pastaPrincipal;
        string valorFixoArquivo;
        private static Thread thread = null;
        bool pararSolicitado;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.ServiceName = "TCIReleaseJucec";
        }

        public ReleaseJucec()
        {
            this.InitializeComponent();

            try
            {
                if (!int.TryParse(ConfigurationSettings.AppSettings["intervaloExecucaoMinutos"], out intervalo))
                {
                    throw new Exception("intervaloExecucaoMinutos informado no arquivo .config inválido.");
                }

                intervalo = 1800000;

                if (!int.TryParse(intervalo.ToString(), out intervalo))
                {
                    throw new Exception("intervaloExecucaoMinutos informado no arquivo .config inválido.");
                }

                if (!int.TryParse(ConfigurationSettings.AppSettings["idProjeto"], out idProjeto))
                {
                    throw new Exception("IdProjeto informado no arquivo .config inválido.");
                }
                 
                pastaPrincipal = ConfigurationSettings.AppSettings["pastaPrincipal"];

                if (pastaPrincipal == null)
                {
                    throw new Exception("Pasta principal informada no arquivo .config inválida.");
                }

                pastaPrincipal += ((pastaPrincipal.Substring(pastaPrincipal.Length - 1) != "\\") ? "\\" : "");

                valorFixoArquivo = ConfigurationSettings.AppSettings["valorFixoArquivo"];

                if (valorFixoArquivo == null)
                {
                    throw new Exception("valorFixoArquivo informado no arquivo .config inválido.");
                }
            }
            catch (Exception ex)
            {
                Util.Util.escreveLog(string.Concat(new string[]
				{
					"Ocorreu o seguinte erro:",
					Environment.NewLine,
					ex.Message,
					Environment.NewLine,
					ex.StackTrace
				}));
            }
        }

        public void iniciar()
        {
            thread = new Thread(new ThreadStart(ReleaseJucec.processamento));
            thread.Start();
        }

        protected override void OnStart(string[] args)
        {
            Util.Util.escreveLog("Serviço Iniciado");
            thread = new Thread(new ThreadStart(ReleaseJucec.processamento));
            thread.Start();
        }

        protected override void OnStop()
        {
            pararSolicitado = true;
            Util.Util.escreveLog("Serviço Parado");
        }

        private static void processamento()
        {

            MainPrincipal principal = new MainPrincipal();
            principal.processamento();
            Util.Util.escreveLog("Executado o comando Thread.CurrentThread.Abort()");
            Thread.CurrentThread.Abort();
        }
    }
}
