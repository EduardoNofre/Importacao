using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    class DocumentosProperties
    {

        private int idDocumentoProperties;

        private string nmDocumentoProperties;

        private string nmDescricaoProperties;

        private DateTime dtInclusaoProperties;

        private int idUsuarioProperties;

        private int idProcessoProperties;

        private int idStatusProperties;

        private int idTipoDocumentoProperties;

        private int idDocumentoContentProperties;

        public int idDocumento
        {
            get
            {
                return idDocumentoProperties;
            }

            set
            {
                idDocumentoProperties = value;
            }
        }

        public string nmDocumento
        {
            get
            {
                return nmDocumentoProperties;
            }

            set
            {
                nmDocumentoProperties = value;
            }
        }

        public string nmDescricao
        {
            get
            {
                return nmDescricaoProperties;
            }

            set
            {
                nmDescricaoProperties = value;
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

        public int idUsuario
        {
            get
            {
                return idUsuarioProperties;
            }

            set
            {
                idUsuarioProperties = value;
            }
        }

        public int idProcesso
        {
            get
            {
                return idProcessoProperties;
            }

            set
            {
                idProcessoProperties = value;
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

        public int idTipoDocumento
        {
            get
            {
                return idTipoDocumentoProperties;
            }

            set
            {
                idTipoDocumentoProperties = value;
            }
        }

        public int idDocumentoContent
        {
            get
            {
                return idDocumentoContentProperties;
            }

            set
            {
                idDocumentoContentProperties = value;
            }
        }
    }
}
