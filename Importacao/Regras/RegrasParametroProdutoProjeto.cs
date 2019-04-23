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
    class RegrasParametroProdutoProjeto
    {
        LogUtil log = new LogUtil();

        ParametroProdutoProjetoDao dao = new ParametroProdutoProjetoDaoImpl();

        public ParametroProdutoProjetoProperties getParametroProdutoProjeto(ProjetoProperties projetoObj, ConfiguracaoProperties configApp)
        {
            ParametroProdutoProjetoProperties parametroProdutoProjetoProperties = dao.parametroProdutoProjeto(projetoObj);

            log.escreveLog(string.Concat(new object[] { "Parametro produto projeto ", parametroProdutoProjetoProperties.NmValorParametro, "para o id projeto", projetoObj.idProjeto }), configApp);

            return parametroProdutoProjetoProperties;
        }
    }
}
