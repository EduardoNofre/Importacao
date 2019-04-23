using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class CaixaProperties
    {

        private int IdCaixaProperties;
        private string DsCaixaProperties;
        private DateTime dtInclusaoProperties;
        private DateTime dtAberturaProperties;
        private DateTime DtFechamentoProperties;
        private int idStatusProperties;
        private int qtDocumentosProperties;
        private int idAreaProperties;
        private int idUnidadeOrganizacionalProperties;

        public int IdUnidadeOrganizacional
        {
            get { return idUnidadeOrganizacionalProperties; }
            set { idUnidadeOrganizacionalProperties = value; }
        }

        public int IdArea
        {
            get { return idAreaProperties; }
            set { idAreaProperties = value; }
        }

        public int QtDocumentos
        {
            get { return qtDocumentosProperties; }
            set { qtDocumentosProperties = value; }
        }

        public int IdStatus
        {
            get { return idStatusProperties; }
            set { idStatusProperties = value; }
        }

        public DateTime DtFechamento
        {
            get { return DtFechamentoProperties; }
            set { DtFechamentoProperties = value; }
        }

        public DateTime DtAbertura
        {
            get { return dtAberturaProperties; }
            set { dtAberturaProperties = value; }
        }

        public DateTime DtInclusao
        {
            get { return dtInclusaoProperties; }
            set { dtInclusaoProperties = value; }
        }

        public string DsCaixa
        {
            get { return DsCaixaProperties; }
            set { DsCaixaProperties = value; }
        }

        public int IdCaixa
        {
            get { return IdCaixaProperties; }
            set { IdCaixaProperties = value; }
        }
    }
}
