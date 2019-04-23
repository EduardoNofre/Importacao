using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TCI.Utils.NET2005;
using TCIReleaseJucec.Dao;
using TCIReleaseJucec.Model;
using TCIReleaseJucec.Util;

namespace TCIReleaseJucec.DaoImpl
{
    class ImagemDaoImpl : ImagemDao
    {
        public List<ImagemProperties> ImagemProcessa(Model.DocumentosProperties documento)
        {

            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            List<ImagemProperties> listaImagem = new List<ImagemProperties>();
            StringBuilder sb = new StringBuilder();            

            ImagemProperties imagem = null;

            try
            {
                sb.AppendLine("select idImagem ,NmCaminhoImagem + NmImagem as caminho, NmImagem from tb_imagem with(nolock)");
                sb.AppendLine("where idstatus != " + Constantes.IMAGEM_EXCLUIDA);
                sb.AppendLine("and idDocumento = " + documento.idDocumento);
                sb.AppendLine("order by nmimagem");

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                IDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    imagem = new ImagemProperties();
                    imagem.NmCaminhoImagem = reader.GetString(reader.GetOrdinal("caminho"));
                    imagem.NmImagem = reader.GetString(reader.GetOrdinal("NmImagem"));
                    imagem.IdImagem = reader.GetInt32(reader.GetOrdinal("idImagem"));
                    listaImagem.Add(imagem);
                }

            }catch(Exception ex){

                throw new Exception("Erro Imagem Processa: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
            return listaImagem;
        }


        public void logImagem(ImagemProperties imagem, int idStatus)
        {

            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {
                sb.AppendLine("insert into tb_log_imagem(IdImagem, IdUsuario, IdStatus, DtInclusao, NmDescricao, NmMaquina) values (" + imagem.IdImagem + "," + Constantes.IDUSUARIO + "," + idStatus + " ," + "getdate()" + "," + "'release nesta data'" + ",'" + Environment.MachineName + "')");
                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro log Imagem: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }

        public void atualizaStatus(ImagemProperties imagem, int idStatus)
        {
            SqlConnection con = ConnectionFactory.ConnectionFactory.getConnection();
            StringBuilder sb = new StringBuilder();

            try
            {

                sb.AppendLine("update tb_imagem ");
                sb.AppendLine("set idstatus = " + Constantes.ENVIADO_FTP);
                sb.AppendLine("where idstatus != " + Constantes.IMAGEM_EXCLUIDA);
                sb.AppendLine("and idimagem = " + imagem.IdImagem);

                con.Open();
                SqlCommand cmd = new SqlCommand(sb.ToString(), con);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 120;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro atualiza Status: " + ex.Message + "\n" + ex.ToString());
            }
            finally
            {
                ConnectionFactory.ConnectionFactory.fechaConexão(con);
            }
        }
    }
}
