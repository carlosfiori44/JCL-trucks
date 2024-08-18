using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace JCL_Web.Models
{
    public class Produto
    {
        private string nome, descricao;
        private float preco, desconto;
        private int quantidade;
        private string id;

        public string Nome { get => nome; set => nome = value; }
        public string Descricao { get => descricao; set => descricao = value; }
        public float Preco { get => preco; set => preco = value; }
        public int Quantidade { get => quantidade; set => quantidade = value; }
        public string Id { get => id; set => id = value; }
        public float Desconto { get => desconto; set => desconto = value; }

        public string CadastrarProduto()
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("INSERT INTO produto VALUES(null, @nome, @preco, @desconto, @quantidade, @descricao);", con);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@descricao", descricao);
                query.Parameters.AddWithValue("@preco", preco);
                query.Parameters.AddWithValue("@desconto", desconto);
                query.Parameters.AddWithValue("@quantidade", quantidade);
                query.ExecuteNonQuery();

                con.Close();

                return "Produto cadastrado com sucesso!";
            }
            catch (Exception ex)
            {
                return ex.Message + "Não foi possível cadastrar!";
            }
        }

        public static List<Produto> MostraProduto()
        {
            List<Produto> lista = new List<Produto>();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM produto;", con);
                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Produto produto = new Produto();
                        produto.nome = leitor["nome"].ToString();
                        produto.descricao = leitor["descricao"].ToString();
                        produto.preco = float.Parse(leitor["preco"].ToString());
                        produto.quantidade = int.Parse(leitor["quantidade"].ToString());
                        produto.id = leitor["id_produto"].ToString();
                        produto.desconto = float.Parse(leitor["desconto"].ToString());

                        lista.Add(produto);
                    }
                }
            }
            catch (Exception e)
            {
                Produto produto = new Produto();
                produto.nome = "Não foi possivel completar sua busca" + e.Message;
                lista.Add(produto);
            }

            return lista;
        }

        public static Produto PesquisaProduto(string id)
        {
            Produto produto = new Produto();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM produto WHERE id_produto = " + id + ";", con);
                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        produto.Descricao = leitor["descricao"].ToString();
                        produto.Nome = leitor["nome"].ToString();
                        produto.Quantidade = int.Parse(leitor["quantidade"].ToString());
                        produto.Preco = float.Parse(leitor["preco"].ToString());
                        produto.desconto = float.Parse(leitor["desconto"].ToString());
                    }
                }
                else
                {
                    produto.Nome = "Não foi possivel completar a ação!";
                }

                return produto;
            } 
            catch(Exception e)
            {
                produto.Nome = "Não foi possivel completar a ação!" + e.Message;
                return produto;
            }
        }

        public string EditarProduto()
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("UPDATE produto SET nome = coalesce(@nome, nome), preco = coalesce(@preco, preco), desconto = coalesce(@desconto, desconto), quantidade = coalesce(@quantidade, quantidade), descricao = coalesce(@descricao, descricao) WHERE id_produto = " + id + ";", con);
                query.Parameters.AddWithValue("@nome", nome);
                query.Parameters.AddWithValue("@preco", preco);
                query.Parameters.AddWithValue("@descricao", descricao);
                query.Parameters.AddWithValue("@quantidade", quantidade);
                query.Parameters.AddWithValue("@desconto", desconto);
                query.ExecuteNonQuery();
                con.Close();

                return "Modificações aplicadas!";
            }
            catch (Exception e)
            {
                return "Erro ao aplicar modificações!" + e.Message;
            }
        }   
    }
}