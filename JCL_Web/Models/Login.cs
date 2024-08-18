using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCL_Web.Models
{
    public class Login
    {
        private string email, senha, id, tipo, endereco, nome;
        //public static string coneccao = "Server=10.87.100.6; Database=jclDataBase; Uid=aluno; Password=Senai1234";
        public static string coneccao = "Server=localhost; Database=jclDataBase; Uid=root; Password=sqlSen@i5e5i";

        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Id { get => id; set => id = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Nome { get => nome; set => nome = value; }

        public string VerificarLogin()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(coneccao);
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM usuario WHERE email = @email", con);
                query.Parameters.AddWithValue("@email", email);
                MySqlDataReader leitor = query.ExecuteReader();
                if (leitor.Read())
                {
                    if (leitor["senha"].Equals(senha))
                    {
                        id = leitor["id_usuario"].ToString();
                        nome = leitor["nome"].ToString();
                        tipo = leitor["tipoUsuario"].ToString();                 

                        return "Você entrou na sua conta!";
                    }
                    else
                    {
                        return "Senha incorreta!";
                    }
                }
                else
                {
                    return "Essa conta não existe!";
                }
            }
            catch (Exception e)
            {
                return "Erro: " + e.Message;
            }
        }
    }
}