using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class ProtocoloProperties
    {

        private int idProtocoloProperties;

        private string dsProtocoloProperties;

        private DateTime dtInclusaoProperties;

        private DateTime dtRecebimentoProperties;

        private DateTime dtInicioTransmissaoProperties;

        private DateTime dtFimTransmissaoProperties;

        private DateTime dtInicioDigitalizacaoProperties;

        private DateTime dtFimDigitalizacaoProperties;

        private int idStatusProperties;

        private int idProjetoProperties;

        private int prioridadeProperties;

        private int idTipoProtocoloProperties;


        public int idProtocolo
        {
            get
            {
                return idProtocoloProperties;
            }

            set
            {
                idProtocoloProperties = value;
            }
        }


        public string dsProtocolo
        {
            get
            {
                return dsProtocoloProperties ;
            }

            set
            {
                dsProtocoloProperties = value;
            }
        }

        public DateTime dtInclusao 
        {
            get
            {
                return dtInclusaoProperties;
            }

            set
            {
                dtInclusaoProperties = value;
            }
        }

        public DateTime dtRecebimento 
        {
            get
            {
                return dtRecebimentoProperties;
            }

            set
            {
                dtRecebimentoProperties = value;
            }
        }


        public DateTime dtInicioTransmissao 
        {
            get
            {
                return dtInicioTransmissaoProperties;
            }

            set
            {
                dtInicioTransmissaoProperties = value;
            }
        }

        public DateTime dtFimTransmissao 
        {
            get
            {
                return dtFimTransmissaoProperties;
            }

            set
            {
                dtFimTransmissaoProperties = value;
            }
        }

        public DateTime dtInicioDigitalizacao
        {
            get
            {
                return dtInicioDigitalizacaoProperties;
            }

            set
            {
                 dtInicioDigitalizacaoProperties = value;
            }
        }

        public DateTime dtFimDigitalizacao
        {
            get
            {
                return dtFimDigitalizacaoProperties;
            }

            set
            {
                dtFimDigitalizacaoProperties = value;
            }
        }

        public int idStatus
        {
            get
            {
                return idStatusProperties;
            }

            set
            {
                 idStatusProperties = value;
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

        public int prioridade
        {
            get
            {
                return prioridadeProperties;
            }

            set
            {
                prioridadeProperties = value;
            }
        }

        public int idTipoProtocolo
        {
            get
            {
                return idTipoProtocoloProperties;
            }

            set
            {
                idTipoProtocoloProperties = value;
            }
        }
    }
}
