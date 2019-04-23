using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class ImagemProperties
    {
        private int idImagemProperties;
        private string nmImagemProperties;
        private string nmCaminhoImagemProperties;
        private string nmTamanhoImagemProperties;
        private int idDocumentoProperties;
        private int idStatusProperties;
        private int idImagemProcessoProperties;
        private int visitadaProperties;

        public int Visitada
        {
            get { return visitadaProperties; }
            set { visitadaProperties = value; }
        }

        public int IdImagemProcesso
        {
            get { return idImagemProcessoProperties; }
            set { idImagemProcessoProperties = value; }
        }

        public int IdStatus
        {
            get { return idStatusProperties; }
            set { idStatusProperties = value; }
        }

        public int IdDocumento
        {
            get { return idDocumentoProperties; }
            set { idDocumentoProperties = value; }
        }

        public string NmTamanhoImagem
        {
            get { return nmTamanhoImagemProperties; }
            set { nmTamanhoImagemProperties = value; }
        }

        public string NmCaminhoImagem
        {
            get { return nmCaminhoImagemProperties; }
            set { nmCaminhoImagemProperties = value; }
        }

        public string NmImagem
        {
            get { return nmImagemProperties; }
            set { nmImagemProperties = value; }
        }

        public int IdImagem
        {
            get { return idImagemProperties; }
            set { idImagemProperties = value; }
        }
    }
}
