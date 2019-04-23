using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.model
{
    public class ProjetoProperties
    {

        int idProjetoProperties;

        string nmProjetoProperties;

        string nmProjetoCompletoProperties;

        bool ativoProperties;

        bool temCaptureDistribuidaProperties;

        int QHoraExpurgoProperties;

        DateTime dtAtualizacaoProperties;

        int qtDiaExpurgoBaseLocalProperties;

        string msRefPCPProperties;

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

        public string nmProjeto
        {
            get
            {
                return nmProjetoProperties;
            }

            set
            {
                nmProjetoProperties = value;
            }
        }

        public string nmProjetoCompleto
        {
            get
            {
                return nmProjetoCompletoProperties;
            }

            set
            {
                nmProjetoCompletoProperties = value;
            }
        }

        public bool ativo
        {
            get
            {
                return ativoProperties;
            }

            set
            {
                ativoProperties = value;
            }
        }

        public bool temCaptureDistribuida
        {
            get
            {
                return temCaptureDistribuidaProperties;
            }

            set
            {
                temCaptureDistribuidaProperties = value;
            }
        }

        public int QHoraExpurgo
        {
            get
            {
                return QHoraExpurgoProperties;
            }

            set
            {
                QHoraExpurgoProperties = value;
            }
        }

        public DateTime dtAtualizacao
        {
            get
            {
                return dtAtualizacaoProperties;
            }

            set
            {
                dtAtualizacaoProperties = value;
            }
        }

        public int qtDiaExpurgoBaseLocal
        {
            get
            {
                return qtDiaExpurgoBaseLocalProperties;
            }

            set
            {
                qtDiaExpurgoBaseLocalProperties = value;
            }
        }

        public string msRefPCP
        {
            get
            {
                return msRefPCPProperties;
            }

            set
            {
                msRefPCPProperties = value;
            }
        }
    }
}
