using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class ValorIndiceProperties
    {
        private int idValorIndiceProperties;
        private string nmValorIndiceProperties;
        private IndiceProperties indiceObjectProperties;
        private int idDocumentoProperties;
        private int conferidoDupDigProperties;

        public int IdValorIndice
        {
            get { return idValorIndiceProperties; }
            set { idValorIndiceProperties = value; }
        }
        
        public string NmValorIndice
        {
            get { return nmValorIndiceProperties; }
            set { nmValorIndiceProperties = value; }
        }

        internal IndiceProperties indiceObject
        {
            get { return indiceObjectProperties; }
            set { indiceObjectProperties = value; }
        }

        public int IdDocumento
        {
            get { return idDocumentoProperties; }
            set { idDocumentoProperties = value; }
        }

        public int ConferidoDupDig
        {
            get { return conferidoDupDigProperties; }
            set { conferidoDupDigProperties = value; }
        }
    }
}
