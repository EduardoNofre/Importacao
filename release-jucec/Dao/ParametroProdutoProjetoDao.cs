using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface ParametroProdutoProjetoDao
    {

        ParametroProdutoProjetoProperties parametroProdutoProjeto(ProjetoProperties projetoObj);
    }
}
