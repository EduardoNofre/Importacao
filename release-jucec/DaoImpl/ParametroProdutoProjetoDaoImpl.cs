using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.model;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.DaoImpl
{
    class ParametroProdutoProjetoDaoImpl : ParametroProdutoProjetoDao
    {

        public ParametroProdutoProjetoProperties parametroProdutoProjeto(ProjetoProperties projetoObj)
        {


            DataTable dt = new DataTable();
            ParametroProdutoProjetoProperties parametroProdutoProjetoProperties = new ParametroProdutoProjetoProperties();


            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("declare @idParametro int, @idProduto  int, @idProjeto int");
                sb.AppendLine("set @idParametro = " + Constantes.CAMINHO_IMAGEM);
                sb.AppendLine("set @idProduto = 3");
                sb.AppendLine("set @idProjeto = " + projetoObj.idProjeto);
                sb.AppendLine("select IdProduto,IdProjeto, nmValorParametro,IdParametro from tb_parametro_produto_projeto with(nolock)");
                sb.AppendLine("where idparametro = @idParametro");
                sb.AppendLine("and idproduto = @idProduto");
                sb.AppendLine("and idProjeto = @idProjeto");
                dt = Database.Instance.ExecuteQuery(sb.ToString(), dt);


                foreach (DataRow objetoParametroProdutoProjeto in dt.Rows)
                {
                    parametroProdutoProjetoProperties.IdProduto = (int)objetoParametroProdutoProjeto["IdProduto"];
                    parametroProdutoProjetoProperties.IdProjeto = (int)objetoParametroProdutoProjeto["IdProjeto"];
                    parametroProdutoProjetoProperties.NmValorParametro = (string)objetoParametroProdutoProjeto["nmValorParametro"];
                    parametroProdutoProjetoProperties.IdParametro = (int)objetoParametroProdutoProjeto["IdParametro"];
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Erro Parametro Produto Projeto Properties: " + ex.Message + "\n" + ex.ToString());
            }
            return parametroProdutoProjetoProperties;
        }
    }
}
