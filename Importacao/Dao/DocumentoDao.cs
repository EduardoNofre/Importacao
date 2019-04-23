using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface DocumentoDao
    {
        List<DocumentosProperties> documentoLiberados(ProtocoloProperties protocolo);

        bool primeiroDocumentoDoProcesso(int idDocumento, int idProcesso);

        bool tipoIndexDocumento(DocumentosProperties documento);

        void LogDocumento(DocumentosProperties documento, int idstatus);

        void AtualizaSatusDocumento(DocumentosProperties documento, int idStatus);

    }
}
