using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.configuracao
{

    /**
     * 
     * Esta classe faz a leitura do arquivo app.config
     * e algumas verificaçoes.
     * 
     **/
    class Configuracao
    {
        ConfiguracaoProperties configObj = null;

        LogUtil log = null;

        public ConfiguracaoProperties ConfiguracaoApp()
        {
            configObj = new ConfiguracaoProperties();

            try
            {
                if (ConfigurationSettings.AppSettings["intervaloExecucaoMinutos"] != null)
                {
                    configObj.intervalo *= 60000;
                }

                if (ConfigurationSettings.AppSettings["idProjeto"] != null)
                {

                    configObj.idProjeto = int.Parse(ConfigurationSettings.AppSettings["idProjeto"]);

                    if (ConfigurationSettings.AppSettings["pastaPrincipal"] != null)
                    {
                        configObj.pastaPrincipal = ConfigurationSettings.AppSettings["pastaPrincipal"];

                        configObj.pastaPrincipal += ((configObj.pastaPrincipal.Substring(configObj.pastaPrincipal.Length - 1) != "\\") ? "\\" : "");
                    }
                }

                if (ConfigurationSettings.AppSettings["valorFixoArquivo"] != null)
                {
                    configObj.valorFixoArquivo = ConfigurationSettings.AppSettings["valorFixoArquivo"];
                }

                if (ConfigurationSettings.AppSettings["caminhoLog"] != null)
                {
                    configObj.caminhoLog = ConfigurationSettings.AppSettings["caminhoLog"];
                }


                if (ConfigurationSettings.AppSettings["nomeCertificado"] != null)
                {
                    configObj.NomeCertificado = ConfigurationSettings.AppSettings["nomeCertificado"];
                }

                if (ConfigurationSettings.AppSettings["caminhoPdfSemAssinatura"] != null)
                {
                    configObj.caminhoPdfSemAssinatura = ConfigurationSettings.AppSettings["caminhoPdfSemAssinatura"];
                }
            }

            catch (Exception ex)
            {
                log = new LogUtil();

                log.escreveLog(string.Concat(new string[] { "Ocorreu o seguinte erro:", Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace }), configObj);
            }

            return configObj;
        }
    }
}
