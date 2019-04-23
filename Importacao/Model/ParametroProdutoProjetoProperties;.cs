using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class ParametroProdutoProjetoProperties
    {

        private int idProdutoProperties;
        private int idProjetoProperties;
        private string nmValorParametroProperties;
        private int idParametroProperties;

        public int IdParametro
        {
            get { return idParametroProperties; }
            set { idParametroProperties = value; }
        }

        public string NmValorParametro
        {
            get { return nmValorParametroProperties; }
            set { nmValorParametroProperties = value; }
        }

        public int IdProjeto
        {
            get { return idProjetoProperties; }
            set { idProjetoProperties = value; }
        }

        public int IdProduto
        {
            get { return idProdutoProperties; }
            set { idProdutoProperties = value; }
        }
    }
}
