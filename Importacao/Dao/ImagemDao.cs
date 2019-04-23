using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Model;

namespace TCIReleaseJucec.Dao
{
    interface ImagemDao
    {
        List<ImagemProperties> ImagemProcessa(DocumentosProperties documento);

        void logImagem(ImagemProperties imagem, int idStatus);

        void atualizaStatus(ImagemProperties imagem, int idStatus);
    }
}
