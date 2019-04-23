using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TCIReleaseJucec.model;

namespace TCIReleaseJucec.Dao
{
    interface ProjetoDao
    {
        ProjetoProperties getProjeto(ConfiguracaoProperties configApp);
    }
}
