using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    public class ValorIndiceProcessoProperties
    {
        private int idValorIndiceProcessoProperties;
        private string nmValorIndiceProcessoProperties;
        private int idProcessoProperties;
        private IndiceProperties indiceObjectProperties;
        private int conferidoDupDigProperties;

        public int IdValorIndiceProcesso
        {
            get { return idValorIndiceProcessoProperties; }
            set { idValorIndiceProcessoProperties = value; }
        }
        

        public string NmValorIndiceProcesso
        {
            get { return nmValorIndiceProcessoProperties; }
            set { nmValorIndiceProcessoProperties = value; }
        }

        public int IdProcesso
        {
            get { return idProcessoProperties; }
            set { idProcessoProperties = value; }
        }

        public IndiceProperties indiceObject
        {
            get { return indiceObjectProperties; }
            set { indiceObjectProperties = value; }
        }

        public int ConferidoDupDig
        {
            get { return conferidoDupDigProperties; }
            set { conferidoDupDigProperties = value; }
        }
    }
}
