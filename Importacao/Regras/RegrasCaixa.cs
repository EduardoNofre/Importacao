using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.DaoImpl;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.Regras
{
    class RegrasCaixa
    {
        CaixaDao dao = new CaixaDaoImpl();
        LogUtil log = new LogUtil();

        public CaixaProperties getCaixaProcesso(DocumentosProperties documento)
        {
            return dao.caixaProcesso(documento);
        }

        public DirectoryInfo trataCaixa(CaixaProperties caixa, ConfiguracaoProperties configApp, ReleaseJucecProperties releaseJucec, string path, ProjetoProperties projetoObj, DocumentosProperties documento)
        {
            String subStringCaixa = string.Empty;
            String destino = string.Empty;
            DirectoryInfo dirDestino;

            if (caixa.DsCaixa.Length >= 9)
            {
                subStringCaixa = caixa.DsCaixa.Substring(caixa.DsCaixa.Length - 9);
            }

            subStringCaixa = subStringCaixa.Replace(".", "");

            DateTime dataAtual = DateTime.Now;

            string dataAtualFormatada = dataAtual.ToString("yyyyMMdd");

            string pathDestino = string.Concat(new string[] { configApp.pastaPrincipal, "TCIReleaseJucec_2017\\", "Data_"+dataAtualFormatada, "\\", "Caixa_"+subStringCaixa, "\\","Protocolo_"+Util.Util.ajustaDiretorio(releaseJucec.numProtocolo),"\\", "Processo_"+documento.idProcesso.ToString(), "\\", "Documento_"+documento.idDocumento.ToString()});

            if (path.Contains(";") && projetoObj.nmProjeto.ToUpper() != "JUCEC - CADASTRO SINCRONIZADO")
            {
                pathDestino = pathDestino.Replace("NUMERO_PROTOCOLO\\", "NIRE_ARQUIVAMENTO\\");
            }

            dirDestino = new DirectoryInfo(pathDestino);

            return dirDestino;
        }
    }
}
