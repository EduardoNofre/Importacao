using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.model
{
    class ConfiguracaoProperties
    {
        private int intervaloProperties;
        private int idProjetoProperties;
        private string pastaPrincipalProperties;
        private string valorFixoArquivoProperties;
        private string caminhoLogProperties;
        private string nomeCertificadoProperties;
        private string caminhoPdfSemAssinaturaProperties;



        public int intervalo
        {
            get
            {
                return intervaloProperties;
            }

            set
            {
                intervaloProperties = value;
            }
        }

        public int idProjeto
        {
            get
            {
                return idProjetoProperties;
            }

            set
            {
                idProjetoProperties = value;
            }
        }
        public String pastaPrincipal
        {
            get
            {
                return pastaPrincipalProperties;
            }

            set
            {
                pastaPrincipalProperties = value;
            }
        }

        public String valorFixoArquivo
        {
            get
            {
                return valorFixoArquivoProperties;
            }

            set
            {
                valorFixoArquivoProperties = value;
            }
        }
        public String caminhoLog
        {
            get
            {
                return caminhoLogProperties;
            }
            set
            {
                caminhoLogProperties = value;
            }
        }

        public string NomeCertificado
        {
            get { return nomeCertificadoProperties; }
            set { nomeCertificadoProperties = value; }
        }

        public string caminhoPdfSemAssinatura
        {
            get { return caminhoPdfSemAssinaturaProperties; }
            set { caminhoPdfSemAssinaturaProperties = value; }
        }
    }
}
