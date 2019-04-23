using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCIReleaseJucec.Dao;

namespace TCIReleaseJucec.Model
{
    class CSVProperties
    {
        string caixaProperties;
        string numeroProtocoloProperties;
        string nireProperties;
        string numeroArquivamentoProperties;
        string caminhoImagemProperties;

        public String caixa
        {
            get
            {
                return caixaProperties;
            }

            set
            {
                caixaProperties = value;
            }
        }

        public String numeroProtocolo
        {
            get
            {
                return numeroProtocoloProperties;
            }

            set
            {
                numeroProtocoloProperties = value;
            }
        }

        public String nire
        {
            get
            {
                return nireProperties;
            }

            set
            {
                nireProperties = value;
            }
        }

        public String numeroArquivamento
        {
            get
            {
                return numeroArquivamentoProperties;
            }

            set
            {
                numeroArquivamentoProperties = value;
            }
        }

        public String caminhoImagem
        {
            get
            {
                return caminhoImagemProperties;
            }

            set
            {
                caminhoImagemProperties = value;
            }
        }

    }
}
