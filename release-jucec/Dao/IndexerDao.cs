using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface IndexerDao
    {
        List<ValorIndiceProcessoProperties> indexerProcesso(DocumentosProperties documento);
    }
}
