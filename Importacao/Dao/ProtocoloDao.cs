using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface ProtocoloDao
    {
        List<ProtocoloProperties> pesquisaProtocolosLiberadosCarga(ConfiguracaoProperties configApp);

        bool atualizaProtocolo(ProtocoloProperties protocolo);

        void logProcesso(ProtocoloProperties protocolo, int idStatus);

        void atualizarProcesso(ProtocoloProperties protocolo, int idStatus);

    }

}
