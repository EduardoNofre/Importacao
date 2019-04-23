using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCIReleaseJucec.Model
{
    public class IndiceProperties
    {
        private int idIndiceProperties;
        private string nmIndiceTelaProperties;
        private int nmOrdemApresentacaoProperties;
        private string nmIndiceSistemaProperties;
        private bool fgSubirParaContentProperties;
        private bool fgPossiveisValoresIndiceProperties;
        private int idMascaraProperties;
        private int idTipoObjetoProperties;
        private int numMaximoCaracteresProperties;
        private int tamanhoCampoTelaProperties;
        private int idTipoCampoProperties;
        private int fgCapturaDistribuidaProperties;
        private int posBaixoProperties;
        private int posDirProperties;
        private int posEsqProperties;
        private int idIndicePaiProperties;
        private int fgObrigatorioProperties;
        private int fgVisivelProperties;
        private int numMinimoCaracteresProperties;
        private string nmMascaraProperties;
        private int duplaDigitacaoProperties;
        private int posTopProperties;


        public int IdIndice
        {
            get { return idIndiceProperties; }
            set { idIndiceProperties = value; }
        }

        public string NmIndiceTela
        {
            get { return nmIndiceTelaProperties; }
            set { nmIndiceTelaProperties = value; }
        }
       
        public int NmOrdemApresentacao
        {
            get { return nmOrdemApresentacaoProperties; }
            set { nmOrdemApresentacaoProperties = value; }
        }
        
        public string NmIndiceSistema
        {
            get { return nmIndiceSistemaProperties; }
            set { nmIndiceSistemaProperties = value; }
        }

        public bool FgSubirParaContent
        {
            get { return fgSubirParaContentProperties; }
            set { fgSubirParaContentProperties = value; }
        }

        public bool FgPossiveisValoresIndice
        {
            get { return fgPossiveisValoresIndiceProperties; }
            set { fgPossiveisValoresIndiceProperties = value; }
        }
    
        public int IdMascara
        {
            get { return idMascaraProperties; }
            set { idMascaraProperties = value; }
        }

        public int IdTipoObjeto
        {
            get { return idTipoObjetoProperties; }
            set { idTipoObjetoProperties = value; }
        }
        
        public int NumMaximoCaracteres
        {
            get { return numMaximoCaracteresProperties; }
            set { numMaximoCaracteresProperties = value; }
        }
        
        public int TamanhoCampoTela
        {
            get { return tamanhoCampoTelaProperties; }
            set { tamanhoCampoTelaProperties = value; }
        }

        public int IdTipoCampo
        {
            get { return idTipoCampoProperties; }
            set { idTipoCampoProperties = value; }
        }

        public int FgCapturaDistribuida
        {
            get { return fgCapturaDistribuidaProperties; }
            set { fgCapturaDistribuidaProperties = value; }
        }

        public int FgObrigatorio
        {
            get { return fgObrigatorioProperties; }
            set { fgObrigatorioProperties = value; }
        }

        public int IdIndicePai
        {
            get { return idIndicePaiProperties; }
            set { idIndicePaiProperties = value; }
        }

        public int PosEsq
        {
            get { return posEsqProperties; }
            set { posEsqProperties = value; }
        }

        public int PosTop
        {
            get { return posTopProperties; }
            set { posTopProperties = value; }
        }

        public int PosDir
        {
            get { return posDirProperties; }
            set { posDirProperties = value; }
        }

        public int PosBaixo
        {
            get { return posBaixoProperties; }
            set { posBaixoProperties = value; }
        }

        public int FgVisivel
        {
            get { return fgVisivelProperties; }
            set { fgVisivelProperties = value; }
        }

        public int NumMinimoCaracteres
        {
            get { return numMinimoCaracteresProperties; }
            set { numMinimoCaracteresProperties = value; }
        }

        public string NmMascara
        {
            get { return nmMascaraProperties; }
            set { nmMascaraProperties = value; }
        }

        public int DuplaDigitacao
        {
            get { return duplaDigitacaoProperties; }
            set { duplaDigitacaoProperties = value; }
        }
    }
}
