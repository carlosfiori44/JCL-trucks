using JCL_Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace JCL_Web
{
    [WebService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Olá, Mundo";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void VerificaLogin(string email, string senha) //HttpPost do Login
        {
            Login login = new Login();
            login.Email = email;
            login.Senha = senha;

            string msg = login.VerificarLogin();
            Context.Response.Write(new JavaScriptSerializer().Serialize(msg));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CadastraProduto(string nome, string descricao, float preco, int quantidade)
        {
            Produto produto = new Produto();
            produto.Nome = nome;
            produto.Descricao = descricao;
            produto.Preco = preco;
            produto.Quantidade = quantidade;

            string msg = produto.CadastrarProduto();
            Context.Response.Write(new JavaScriptSerializer().Serialize(msg));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CadastraUsuario(string nome, string email, string telefone, string endereco, string tipoUsuario, string senha, string regiao)
        {
            Usuario usuario = new Usuario();
            usuario.Nome = nome;
            usuario.Email = email;
            usuario.Telefone = telefone;
            usuario.Endereco = endereco;
            usuario.TipoUsuario = tipoUsuario;
            usuario.Senha = senha;
            usuario.Regiao = regiao;

            string msg = usuario.CadastrarCliente();
            Context.Response.Write(new JavaScriptSerializer().Serialize(msg));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Historico()
        {
            List<Models.Historico> historico = Models.Historico.MostrarAtividade();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void MostraProdutos()
        {
            List<Produto> produtos = Produto.MostraProduto();
            Context.Response.Write(new JavaScriptSerializer().Serialize(produtos));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void EfetuaCompra(int quantidade, string id_produto, string id_cliente)
        {
            Compra compra = new Compra();
            //compra.ValorCompra = compra.calculaPreco(quantidade);
            compra.Id_cliente = id_cliente;
            compra.Id_funcionario = Session["id_usuario"].ToString();
            compra.Quantidade = quantidade;
            compra.Id_produto = id_produto;
            string msg = compra.EfetuaCompra();

            Context.Response.Write(new JavaScriptSerializer().Serialize(msg));
        }
    }
}
        
    
