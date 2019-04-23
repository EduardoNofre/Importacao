using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("TCIReleaseJucec")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("GRUPO TCI")]
[assembly: AssemblyProduct("TCIReleaseJucec")]
[assembly: AssemblyCopyright("Copyright © GRUPO TCI 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("74407656-55b8-4a83-9211-8f5a7c8d070b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
///Thiago Fonseca 30/03/2010
/// Implementação da aplicação
/// Diogo Cezari - 16/12/2010 - 2.2.0
///     Colocando aspas duplas nos dados que vão dentro do TXT
/// Jose Miguel - 26/01/2010 - 2.2.1
///      Retirado o traço e colocado ponto e virgula no metodo de obter numero do protocolo.//
///      colocado também mais um nivel de pasta Numero_protocolo e Nire_arquivamento dependo do indice preenchido.
///      retorno += @"\" + nire + ";" + numArquivamento;
/// 2.2.2 - Miguel - 26/01
///         - Melhorado os logs do aplicativo para identificar erro.
///2.2.4 - Miguel - 27/01
///         - Colocado em Nire = 000 quando o mesmo vier fazio da base de dados
///         e pro Arquivamento = 00000; Conforme Solicitação de Womel.
///2.3.1 - Bertuzzi - 10/10
///        - Alterado para obter apenas os ultimos 9 digitos da caixa.
///2.3.2 - Fonseca - 16/04/2012 - Correcao SDM 20646
///2.3.3 - Fonseca - 23/04/2012 - Em pesquisaProtocolosLiberadosCarga, levando em consideração protocolo e caixa ao invés de somente protocolo.
///2.3.4 - Fonseca - 08/05/2012 - Alteração de escopo - Mantis 0045564. Solicitado colocar YYYYMMDD (getdate) antes da pasta da caixa.
///2.3.5 - Fonseca - 09/05/2012 - Corrigindo erro bug dos modulos do Easy No release. Existem documentos com status 13 sem imagens
///no método Persistencia.Instancia.pesquisaImagensDocumento estamos validando se o documento possui imagem. Caso não possua colocar status 24 nele e gravar logDocumento.
///2.4.0 - Fonseca - 21/05/2012 - Implementando demanda 0045642: Alteração do projeto JUCEC CE do BUREAU
///2.4.1 - Fonseca - 22/05/2012 - Criado Persistencia.Instancia.primeiroDocumentoDoProcesso para validar se o documento do 
///         processo é o primeiro (projeto JUCEC - CADASTRO SINCRONIZADO)
///2.5.0 - Fonseca - 01/05/2012 - Implementada assinatura digital para os arquivos pdf multipaginados da Jucec.
/*
 * 2.2.6 - Miguel - Gerando os arquivos assinados a cada Protocolo gerado.
 * 2.2.7 - Fonseca - Alterado processaImagemSefaz para criar o ultimo diretorio com nire + numArquivamento
 * 2.2.8 - Fonseca - 26/06/2012 - Correcao caminho das imagens em processaImagem
 */


[assembly: AssemblyVersion("2.3.1")]
[assembly: AssemblyFileVersion("2.3.1")]