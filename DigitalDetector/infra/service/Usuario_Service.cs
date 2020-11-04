using DigitalDetector.models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DigitalDetector.infra.service
{
    public class Usuario_Service
    {

        private string conn = runtime.url;

        public List<Usuario> CarregarTodosUsuario()
        {
            try
            {
                List<Usuario> users = new List<Usuario>();

                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Id, Nome,Digital from Usuarios", con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var u = new Usuario();
                        u.Id = (Guid)rdr["Id"];
                        u.Nome = rdr["Nome"].ToString();
                        u.Digital = rdr["Digital"].ToString();

                        users.Add(u);
                    }

                    con.Close();

                    return users;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }

        public Usuario CarregarUsuario(string base64, ENivelUsuario? nivel = null)
        {
            try
            {
                Usuario u = new Usuario();

                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter($"SELECT top(1) Id, Nome,Digital,Nivel from Usuarios where Digital = @digital {(nivel != null ? " and Nivel <= @nivel" : "")} order by Nivel desc", con);
                    cmd.SelectCommand.CommandType = CommandType.Text;
                    cmd.SelectCommand.Parameters.AddWithValue("@digital", base64);
                    cmd.SelectCommand.Parameters.AddWithValue("@nivel", (nivel != null ? (int)nivel : 0));

                    con.Open();
                    DataSet ds = new DataSet();
                    cmd.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        u.Id = (Guid)ds.Tables[0].Rows[0]["Id"];
                        u.Nome = ds.Tables[0].Rows[0]["Nome"].ToString();
                        u.Digital = ds.Tables[0].Rows[0]["Digital"].ToString();
                        u.Nivel = (ENivelUsuario)Int32.Parse(ds.Tables[0].Rows[0]["Nivel"].ToString());
                    }
                    else
                    {
                        return null;
                    }
                    con.Close();

                    return u;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }
        public Usuario CarregarUsuario(string nome)
        {
            try
            {
                Usuario u = new Usuario();

                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlDataAdapter cmd = new SqlDataAdapter($"SELECT top(1) Id, Nome,Digital,Nivel from Usuarios where Nome = @nome order by Nivel desc", con);
                    cmd.SelectCommand.CommandType = CommandType.Text;
                    cmd.SelectCommand.Parameters.AddWithValue("@nome", nome);

                    con.Open();
                    DataSet ds = new DataSet();
                    cmd.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        u.Id = (Guid)ds.Tables[0].Rows[0]["Id"];
                        u.Nome = ds.Tables[0].Rows[0]["Nome"].ToString();
                        u.Digital = ds.Tables[0].Rows[0]["Digital"].ToString();
                        u.Nivel = (ENivelUsuario)Int32.Parse(ds.Tables[0].Rows[0]["Nivel"].ToString());
                    }
                    else
                    {
                        return null;
                    }
                    con.Close();

                    return u;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return null;
            }
        }
        public bool CriarUsuario(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Insert into Usuarios(Id,Nome,Digital,Nivel) values (@id,@nome,@digital,@nivel)", con);

                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@digital", usuario.Digital);
                    cmd.Parameters.AddWithValue("@nivel", (int)usuario.Nivel);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
        public bool AlterarUsuario(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Update Usuarios set Nome = @nome, Digital = @digital where Id = @id", con);

                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", usuario.Id);
                    cmd.Parameters.AddWithValue("@nome", usuario.Nome);
                    cmd.Parameters.AddWithValue("@digital", usuario.Digital);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
        public bool Remover(Guid id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Delete from Usuarios where Id = @id", con);

                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
    }
}
