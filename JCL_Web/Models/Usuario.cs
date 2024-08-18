using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace JCL_Web.Models
{
    public class Usuario
    {
        private string nome, email, telefone, endereco, tipoUsuario, senha, regiao, id;

        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string TipoUsuario { get => tipoUsuario; set => tipoUsuario = value; }
        public string Id { get => id; set => id = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Regiao { get => regiao; set => regiao = value; }

        public string CadastrarCliente()
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();

                MySqlCommand query = new MySqlCommand("INSERT INTO usuario VALUES(null, @nome, @endereco, @email, @senha, @regiao, @tipoUsuario);", con);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@email", email);
                query.Parameters.AddWithValue("@endereco", endereco);
                query.Parameters.AddWithValue("@senha", senha);
                query.Parameters.AddWithValue("@regiao", regiao);
                query.Parameters.AddWithValue("@tipoUsuario", tipoUsuario);
                query.ExecuteNonQuery();

                con.Close();
                con.Open();

                MySqlCommand query1 = new MySqlCommand("SELECT id_usuario, nome FROM usuario WHERE email = @email", con);
                query1.Parameters.AddWithValue("@email", email);
                MySqlDataReader leitor = query1.ExecuteReader();
                if (leitor.Read())
                {
                    id = leitor["id_usuario"].ToString();
                }

                con.Close();
                con.Open();

                MySqlCommand query2 = new MySqlCommand("INSERT INTO telefoneDoUsuario VALUES(null, @id_usuario, @telefoneUsuario);", con);
                query2.Parameters.AddWithValue("@id_usuario", id);
                query2.Parameters.AddWithValue("@telefoneUsuario", telefone);
                query2.ExecuteNonQuery();

                con.Close();

                return "Cadastrado com sucesso!";
            }
            catch (Exception ex)
            {
                return "E-mail já cadastrado! " + ex.Message;
            }
        }

        public static List<Usuario> MostraUsuario(string tipo)
        {
            List<Usuario> lista = new List<Usuario>();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            MySqlConnection con1 = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = null;

                if (tipo.Equals("*"))
                {
                    query = new MySqlCommand("SELECT * FROM usuario;", con);
                }
                else if (tipo.Equals("cliente"))
                {
                    query = new MySqlCommand("SELECT * FROM usuario WHERE tipoUsuario = 'cliente';", con);
                }

                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Usuario objUsuario = new Usuario();

                        MySqlCommand query1 = new MySqlCommand("SELECT telefone_usuario FROM telefoneDoUsuario WHERE id_usuario = " + leitor["id_usuario"].ToString() + ";", con1);

                        //Abrindo conexão com o banco de dados para resgar o telefone do usuario
                        con1.Open();

                        objUsuario.telefone = query1.ExecuteScalar().ToString();

                        con1.Close();

                        objUsuario.nome = leitor["nome"].ToString();
                        objUsuario.senha = leitor["senha"].ToString();
                        objUsuario.email = leitor["email"].ToString();
                        objUsuario.endereco = leitor["endereco"].ToString();
                        objUsuario.tipoUsuario = leitor["tipoUsuario"].ToString();
                        objUsuario.regiao = leitor["regiao"].ToString();
                        objUsuario.id = leitor["id_usuario"].ToString();

                        if(objUsuario.regiao.Equals("")) { objUsuario.regiao = "N/A"; } 
                        if(objUsuario.endereco.Equals("")) { objUsuario.endereco = "N/A"; }

                        lista.Add(objUsuario);
                    }
                }
                else
                {
                    Usuario objUsuario = new Usuario();
                    objUsuario.nome = "Nenhum dado encontrado!";
                    objUsuario.senha = "N/A";
                    objUsuario.email = "N/A";
                    objUsuario.endereco = "N/A";
                    objUsuario.tipoUsuario = "N/A";
                    objUsuario.regiao = "N/A";
                    objUsuario.id = "N/A";
                    lista.Add(objUsuario);

                    return lista;
                }
            }
            catch (Exception e)
            {
                Usuario objUsuario = new Usuario();
                objUsuario.nome = "Não foi possível realizar sua busca!";
                lista.Add(objUsuario);
            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public static string VerificarRegiao(string id)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            string regiao = null;

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT regiao FROM usuario WHERE id_usuario = " + id + ";", con);
                regiao = query.ExecuteScalar().ToString();

                con.Close();

                return regiao;

            } 
            catch(Exception e)
            {
                return regiao;
            }
        }

        public string MudarSenha(string senha, string id)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("UPDATE usuario SET senha = @senha WHERE id_usuario = " + id + ";", con);
                query.Parameters.AddWithValue("@senha", senha);
                query.ExecuteNonQuery();

                con.Close();

                return "Senha alterada!";

            }
            catch (Exception e)
            {
                return "Não foi possivel alterar sua senha!" + e.Message;
            }
        }

        public static Usuario PesquisaUsuario(string id)
        {
            Usuario usuario = new Usuario();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM usuario WHERE id_usuario = " + id + ";", con);
                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuario.nome = leitor["nome"].ToString();
                        usuario.regiao = leitor["regiao"].ToString();
                        usuario.endereco = leitor["endereco"].ToString();
                        usuario.email = leitor["email"].ToString();
                    }
                }
                else
                {
                    usuario.nome = "Não foi possivel completar a ação!";
                }

                con.Close();
                con.Open();

                query = new MySqlCommand("SELECT telefone_usuario FROM telefoneDoUsuario WHERE id_usuario = " + id + ";", con);

                usuario.telefone = query.ExecuteScalar().ToString();

                con.Close();

                return usuario;
            }
            catch (Exception e)
            {
                usuario.Nome = "Não foi possivel completar a ação!" + e.Message;
                return usuario;
            }
        }

        public static List<Usuario> PesquisaUsuarioNome(string nome, string tipo)
        {
            List<Usuario> lista = new List<Usuario>();
            Usuario usuario = new Usuario();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            MySqlConnection con1 = new MySqlConnection(Login.coneccao);
            MySqlCommand query;

            if (tipo.Equals("todos"))
            {
                query = new MySqlCommand("SELECT * From usuario Where nome like '" + nome + "%';", con);
            }
            else
            {
                query = new MySqlCommand("SELECT * FROM usuario WHERE nome LIKE '" + nome + "%' AND tipoUsuario = 'cliente' ;", con);
            }

            MySqlCommand query1;

            try
            {
                con.Open();

                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuario.nome = leitor["nome"].ToString();
                        usuario.regiao = leitor["regiao"].ToString();
                        usuario.endereco = leitor["endereco"].ToString();
                        usuario.email = leitor["email"].ToString();
                        usuario.id = leitor["id_usuario"].ToString();
                        con1.Open();
                        query1 = new MySqlCommand("SELECT telefone_usuario FROM telefoneDoUsuario WHERE id_usuario = " + usuario.id + ";", con1);
                        usuario.telefone = query1.ExecuteScalar().ToString();
                        con1.Close();
                        lista.Add(usuario);
                    }
                }
                else
                {
                    usuario.nome = "Ninguem encontrado!";
                    lista.Add(usuario);
                }

                con.Close();
                con1.Close();

                return lista;
            }
            catch (Exception e)
            {
                usuario.Nome = "Não foi possivel completar a ação!" + e.Message;
                lista.Add(usuario);
                return lista;
            }
        }

        public string EditarUsuario()
        {

            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open(); 
  
                MySqlCommand query = new MySqlCommand("UPDATE usuario SET email = coalesce(@email, email), endereco = coalesce(@endereco, endereco), regiao = coalesce(@regiao, regiao) WHERE id_usuario = " + id + ";", con);
                query.Parameters.AddWithValue("@email", email);
                query.Parameters.AddWithValue("@endereco", endereco);
                query.Parameters.AddWithValue("@regiao", regiao);
                query.ExecuteNonQuery();

                con.Close();
                con.Open();

                query = new MySqlCommand("UPDATE telefoneDoUsuario SET telefone_usuario = coalesce(@telefone, telefone_usuario) WHERE id_usuario = " + id + "; ", con);
                query.Parameters.AddWithValue("@telefone", telefone);
                query.ExecuteNonQuery();

                con.Close();

                return "Alteração feita!";

            }
            catch (Exception e)
            {
                return "Não foi possivel alterar!" + e.Message;
            }
        }
    }
}