using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace JCL_Web.Models
{
    public class Historico
    {
        private string acao, idFunc, nomeFunc, emailFunc, dataHisto;
        static DateTime data = DateTime.Now;

        public string Acao { get => acao; set => acao = value; }
        public string IdFunc { get => idFunc; set => idFunc = value; }
        public string NomeFunc { get => nomeFunc; set => nomeFunc = value; }
        public string EmailFunc { get => emailFunc; set => emailFunc = value; }
        public string DataHisto { get => dataHisto; set => dataHisto = value; }

        public static List<Historico> MostrarAtividade()
        {
            List<Historico> historico = new List<Historico>();
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            MySqlConnection con1 = new MySqlConnection(Login.coneccao);

            try
            {
                con.Open();
                MySqlCommand query = new MySqlCommand("SELECT * FROM historico;", con);
                MySqlDataReader reader = query.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Historico hist = new Historico();
                        hist.acao = reader["acao"].ToString();
                        hist.idFunc = reader["funcionario_id"].ToString();
                        hist.dataHisto = reader["data_historio"].ToString();

                        MySqlCommand query1 = new MySqlCommand("SELECT * FROM usuario WHERE id_usuario = " + hist.idFunc + ";", con1);

                        con1.Open();

                        MySqlDataReader leitor = query1.ExecuteReader();

                        if (leitor.Read())
                        {
                            hist.nomeFunc = leitor["nome"].ToString();
                            hist.EmailFunc = leitor["email"].ToString();
                        }

                        con1.Close();

                        historico.Add(hist);
                    }
                }
            }
            catch (Exception e)
            {
                Historico hist = new Historico();
                hist.acao = "Não foi possivel completar a busca! " + e.Message;
                historico.Add(hist);
            }

            return historico;
        }

        public static void MarcarAtividade(string atividade, string funcionario)
        {
            MySqlConnection con = new MySqlConnection(Login.coneccao);
            try
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO historico VALUES(null, @funcionario_id, @acao, @data_historico);", con);
                query.Parameters.AddWithValue("@funcionario_id", funcionario);
                query.Parameters.AddWithValue("@acao", atividade);
                query.Parameters.AddWithValue("@data_historico", data.ToString("yyyy-MM-dd") + " - " + data.Hour + ":" + data.Minute + ":" + data.Second);
                 
                con.Open();

                query.ExecuteNonQuery();

                con.Close();
            } catch(Exception e)
            {

            }
        }
    }
}