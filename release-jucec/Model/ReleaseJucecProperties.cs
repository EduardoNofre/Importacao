using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class ReleaseJucecProperties
    {


        private string municipioProperties;
        private string descricaoProperties;
        private string eventoProperties;
        private string atoProperties;
        private string nomeempresarialProperties;
        private string cnpjProperties;
        private string nireProperties;
        private string numProtocoloProperties;
        private string numArquivamentoProperties;
        private string penultimaImgProperties;
        private string ultimaImgProperties;
        private bool flagProperties;
        private bool flagSemImagemProperties;
        private string nireConcatnumArquivamentoProperties;
        private string pathOrigemProperties;

        public string ultimaImg
        {
            get { return ultimaImgProperties; }
            set { ultimaImgProperties = value; }
        }

        public string penultimaImg
        {
            get { return penultimaImgProperties; }
            set { penultimaImgProperties = value; }
        }
        

        public string municipio
        {
            get { return municipioProperties; }
            set { municipioProperties = value; }
        }

        public string descricao
        {
            get { return descricaoProperties; }
            set { descricaoProperties = value; }
        }

        public string evento
        {
            get { return eventoProperties; }
            set { eventoProperties = value; }
        }

        public string ato
        {
            get { return atoProperties; }
            set { atoProperties = value; }
        }

        public string nomeempresarial
        {
            get { return nomeempresarialProperties; }
            set { nomeempresarialProperties = value; }
        }

        public string cnpj
        {
            get { return cnpjProperties; }
            set { cnpjProperties = value; }
        }


        public string nire
        {
            get { return nireProperties; }
            set { nireProperties = value; }
        }

        public string numProtocolo
        {
            get { return numProtocoloProperties; }
            set { numProtocoloProperties = value; }
        }

        public string numArquivamento
        {
            get { return numArquivamentoProperties; }
            set { numArquivamentoProperties = value; }
        }
        public bool flag
        {
            get { return flagProperties; }
            set { flagProperties = value; }
        }


        public bool flagSemImagem
        {
            get { return flagSemImagemProperties; }
            set { flagSemImagemProperties = value; }
        }

        public string nireConcatnumArquivamento
        {
            get { return nireConcatnumArquivamentoProperties; }
            set { nireConcatnumArquivamentoProperties = value; }
        }

        public string pathOrigem
        {
            get { return pathOrigemProperties; }
            set { pathOrigemProperties = value; }
        }
    }
}
