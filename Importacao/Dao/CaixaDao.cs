using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface CaixaDao
    {
        CaixaProperties caixaProcesso(DocumentosProperties documento);
    }
}
