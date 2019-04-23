using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.configuracao
{
    class ThreadTci : ServiceBase
    {
        private Thread thread = null;
        MainPrincipal mainPrincipal = new MainPrincipal();
        LogUtil log = new LogUtil();

        ConfiguracaoProperties configObj = null;

        public ThreadTci()
        {
            configObj = new ConfiguracaoProperties();
            this.InitializeComponent();
        }

        public void iniciar()
        {
            thread = new Thread(new ThreadStart(mainPrincipal.processamento));
            thread.Start();
        }

        protected override void OnStart(string[] args)
        {
            log.escreveLog("Serviço Iniciado", configObj);
            thread = new Thread(new ThreadStart(mainPrincipal.processamento));
            thread.Start();
        }

        protected override void OnStop()
        {
            Constantes.THREAD_PARADA_SOLICITADO = true;
            log.escreveLog("Serviço Parado", configObj);
        }

        private void InitializeComponent()
        {
            base.ServiceName = "TCIReleaseJucec";
        }

    }
}
