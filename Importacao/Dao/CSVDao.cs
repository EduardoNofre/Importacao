using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface CSVDao
    {
        List<CSVProperties> documentoLiberados();
    }
}
