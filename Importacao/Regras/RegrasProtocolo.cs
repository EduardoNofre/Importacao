using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.Regras
{
    class RegrasProtocolo
    {
        ProtocoloDao dao = new ProtocoloDaoImpl();

        LogUtil log = new LogUtil();

        public List<ProtocoloProperties> getProtocoloProtocolosLiberado(ConfiguracaoProperties configApp)
        {
            log.escreveLog("Pesquisando por protocolo liberado ", configApp);

            return dao.pesquisaProtocolosLiberadosCarga(configApp);

        }

        public void atualizarStatusProtocolo(ProtocoloProperties protocolo, ConfiguracaoProperties configApp)
        {

            log.escreveLog("atualizarStatusProtocoloDao: ", configApp);

            bool statusOk = dao.atualizaProtocolo(protocolo);

            if (statusOk)
            {
                log.escreveLog("Status alterado com seucesso IdProtocolo" + protocolo.idProtocolo, configApp);

            }
            else
            {

                log.escreveLog("Erro ao alterar o status IdProtocolo" + protocolo.idProtocolo, configApp);
            }
        }

        public void logProcesso(ProtocoloProperties protocolo, int idStatus)
        {
            dao.logProcesso(protocolo, idStatus);
        }

        public void atualizaProcesso(ProtocoloProperties protocolo, int idStatus)
        {
            dao.atualizarProcesso(protocolo, idStatus);
        }
    }
}
