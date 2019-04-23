using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.Negocio
{
    class RegrasProjeto
    {
        ProjetoDao dao = new ProjetoDaoImpl();

        LogUtil log = new LogUtil();

        public ProjetoProperties getProjeto(ConfiguracaoProperties configApp)
        {
            log.escreveLog("Pesquisando projeto", configApp);

            return dao.getProjeto(configApp);
        }
    }
}
