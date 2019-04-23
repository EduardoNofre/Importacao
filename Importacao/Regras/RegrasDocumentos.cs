using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.Regras
{
    class RegrasDocumentos
    {
        DocumentoDao dao = new DocumentoDaoImpl();

        public List<DocumentosProperties> getDocumentosLiberado(ProtocoloProperties protocolo)
        {
            return dao.documentoLiberados(protocolo);
        }

        public bool tipoIndexDocumento_Jucec_Sesaz(DocumentosProperties documento)
        {
            return dao.tipoIndexDocumento(documento);
        }

        public void LogDocumento(DocumentosProperties documento,int idStatus)
        {
            dao.LogDocumento(documento, idStatus);
        }

        public void atualizaSatusDocumento(DocumentosProperties documento,int idStatus)
        {
            dao.AtualizaSatusDocumento(documento,idStatus);
        }
    }
}