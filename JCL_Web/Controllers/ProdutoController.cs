using JCL_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JCL_Web.Controllers
{
    public class ProdutoController : Controller
    {
        //Página para mostrar os produtos cadastrados no banco de dados
        public ActionResult MostraProdutos()
        {
            List<Produto> produtos = Produto.MostraProduto();
            return View(produtos);
        }

        //Página para cadastrar produto
        public ActionResult CadastraProduto()
        {
            if (Session["tipo_usuario"] != null)
            {
                if (Session["tipo_usuario"].Equals("gerente"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("PaginaLogin", "Home");
            }
        }

        //Método para enviar as informações para o banco de dados
        [HttpPost]
        public ActionResult CadastraProduto(string nome, string descricao, float preco, int quantidade)
        {
            Produto produto = new Produto();
            produto.Nome = nome;
            produto.Descricao = descricao;
            produto.Preco = preco;
            produto.Quantidade = quantidade;

            string msg = produto.CadastrarProduto();

            TempData["msg"] = msg;
            return View();
        }

        //Página para inserir os dados atualizados dos produtos
        public ActionResult EditarProduto(string id)
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    Produto produto = Produto.PesquisaProduto(id);
                    return View(produto);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["msg"] = "Entre para poder utilizar os recursos!";
                return RedirectToAction("PaginaLogin", "Home");
            }

        }    

        //Método que altera os dados no banco de dados dos produtos
        [HttpPost]
        public ActionResult EditarProduto(string id, string nome, string descricao, float preco, int quantidade, float desconto)
        {
            Produto produto = new Produto();
            produto.Id = id;
            produto.Nome = nome;
            produto.Descricao = descricao;
            produto.Preco = preco;
            produto.Quantidade = quantidade;
            produto.Desconto = desconto;

            TempData["msg"] = produto.EditarProduto();
            return RedirectToAction("MostraProdutos");
        }
    }
}