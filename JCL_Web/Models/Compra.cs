using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JCL_Web.Models
{
    public class Compra
    {
        private float valorCompra, desconto;
        private DateTime dataCompra = DateTime.Now;
        private string id_funcionario, id_compra, id_produto, id_cliente, relatorio, data, status, nomeProd, nomeFunc, nomeClien;
        private int quantidade;

        public float ValorCompra { get => valorCompra; set => valorCompra = value; }
        public DateTime DataCompra { get => dataCompra; set => dataCompra = value; }
        public string Id_funcionario { get => id_funcionario; set => id_funcionario = value; }
        public string Id_compra { get => id_compra; set => id_compra = value; }
        public string Id_produto { get => id_produto; set => id_produto = value; }
        public string Id_cliente { get => id_cliente; set => id_cliente = value; }
        public int Quantidade { get => quantidade; set => quantidade = value; }
        public string Relatorio { get => relatorio; set => relatorio = value; }
        public float Desconto { get => desconto; set => desconto = value; }
        public string Data { get => data; set => data = value; }
        public string Status { get => status; set => status = value; }
        public string NomeProd { get => nomeProd; set => nomeProd = value; }
        public string NomeFunc { get => nomeFunc; set => nomeFunc = value; }
        public string NomeClien { get => nomeClien; set => nomeClien = value; }
        
        public static void AlteraStatus(string id)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("UPDATE compra SET status = 'Ok!' WHERE id_compra = " + id + ";", con);
                query.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e)
            {
            }
        }

        public static Compra MostraRelatorio(string id)
        {
            Compra compra = new Compra();
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT relatorio FROM compra WHERE id_compra = " + id + ";", con);
                compra.relatorio = query.ExecuteScalar().ToString();
                con.Close();
                return compra;
            }
            catch (Exception e)
            {
                compra.relatorio = "Não foi possivel completar a ação!" + e.Message;
                return compra;
            }
        }

        public static List<Compra> MostraCompra()
        {
            List<Compra> lista = new List<Compra>();
            MySqlConnection con = new MySqlConnection(Login.coneccao); 

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM compra;", con);
                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Compra compra = new Compra();
                        compra.id_produto = leitor["id_produto"].ToString();
                        compra.nomeProd = Produto.PesquisaProduto(leitor["id_produto"].ToString()).Nome;

                        compra.id_cliente = leitor["id_cliente"].ToString();
                        compra.nomeClien = Usuario.PesquisaUsuario(leitor["id_cliente"].ToString()).Nome ;

                        compra.id_funcionario = leitor["idFuncionario"].ToString();
                        compra.nomeFunc = Usuario.PesquisaUsuario(leitor["idFuncionario"].ToString()).Nome;

                        compra.valorCompra = float.Parse(leitor["valor_compra"].ToString());
                        compra.data = leitor["data_compra"].ToString();
                        compra.quantidade = int.Parse(leitor["quantidadeItens"].ToString());
                        compra.desconto = float.Parse(leitor["desconto"].ToString());
                        compra.status = leitor["status"].ToString();
                        compra.id_compra = leitor["id_compra"].ToString();

                        lista.Add(compra);
                    }
                }
            }
            catch (Exception e)
            {
                Compra compra = new Compra();
                compra.status = "Não foi possivel completar sua busca" + e.Message;
                lista.Add(compra);
            }

            return lista;
        }

        public string EfetuaCompra()
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();

                MySqlCommand query = new MySqlCommand("INSERT INTO compra VALUES(null, @id_produto, @id_cliente, @id_funcionario, @valor, @data, @quantidade, @desconto, @status, @relatorio);", con);
                query.Parameters.AddWithValue("@valor", valorCompra);
                query.Parameters.AddWithValue("@data", dataCompra.ToString("yyyy/MM/dd HH:mm:ss"));
                query.Parameters.AddWithValue("@quantidade", quantidade);
                query.Parameters.AddWithValue("@id_produto", id_produto);
                query.Parameters.AddWithValue("@id_cliente", id_cliente);
                query.Parameters.AddWithValue("@id_funcionario", id_funcionario);
                query.Parameters.AddWithValue("@desconto", desconto);
                query.Parameters.AddWithValue("@status", "Ok");
                query.Parameters.AddWithValue("@relatorio", relatorio);

                query.ExecuteNonQuery();

                con.Close();

                return "Compra efetuada!";
            }
            catch (Exception e)
            {
                return "Não foi possivel efetuar a compra!" + e.Message;
            }
        }

        public string VerificaDisponibilidade(int quantidade)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();

                MySqlCommand query = new MySqlCommand("SELECT quantidade FROM produto WHERE id_produto = " + id_produto + ";", con);
                int quantProd = int.Parse(query.ExecuteScalar().ToString());

                if (quantProd == 0)
                {
                    con.Close();
                    return "Produto indisponivel!";
                }

                if(quantidade > quantProd)
                {
                    con.Close();
                    return "Não há a quantidade necessária";
                }

                con.Close();

                return "Produto disponivel!";
            }
            catch (Exception e)
            {
                return "Erro inesperado!" + e.Message;
            }
            finally
            {
                con.Close();
            }
        }

        public float CalculaPreco(int quantidades)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            float preco = 0;

            try
            {
                con.Open();

                MySqlCommand query = new MySqlCommand("SELECT preco, desconto FROM produto WHERE id_produto = " + id_produto + ";", con);
                MySqlDataReader leitor = query.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (decimal.Parse(leitor["desconto"].ToString()) > 0)
                        {
                            preco = (float.Parse(leitor["preco"].ToString()) * (float.Parse(leitor["desconto"].ToString())/100) * quantidades);
                        }
                        else
                        {
                            preco = float.Parse(leitor["preco"].ToString()) * quantidades;
                        }
                    }
                }

                con.Close();

                con.Open();
                MySqlCommand query1 = new MySqlCommand("UPDATE produto SET quantidade = quantidade - " + quantidades + " WHERE id_produto = " + id_produto + ";", con);
                query1.ExecuteNonQuery();
                con.Close();

                return preco;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}