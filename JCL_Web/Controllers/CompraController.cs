using JCL_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JCL_Web.Controllers
{
    public class CompraController : Controller
    {
        //Site para mostrar as compras
        public ActionResult MostraCompras()
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    List<Compra> lista = Compra.MostraCompra();
                    return View(lista);
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

        //Método para mostrar o relátiorio
        public ActionResult MostraRelatorio(string id)
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    Compra compra = Compra.MostraRelatorio(id);
                    return View(compra);
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

        //Página para cadastrar uma compra do cliente
        public ActionResult EfetuaCompra()
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    Session["listaPro"] = Produto.MostraProduto();
                    Session["listaCli"] = Usuario.MostraUsuario("cliente");
                    return View();
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

        //Método que envia os dados inseridos para o banco de dados
        [HttpPost]
        public ActionResult EfetuaCompra(int quantidade, string id_produto, string id_cliente, string relatorio)
        {
            Compra compra = new Compra();
            compra.Id_produto = id_produto;
            string msg = compra.VerificaDisponibilidade(quantidade);
            TempData["msg"] = msg;

            if (msg.Contains("indisponivel") || msg.Contains("necessária"))
            {
                return View();
            }

            compra.ValorCompra = compra.CalculaPreco(quantidade);
            compra.Id_cliente = id_cliente;
            compra.Id_funcionario = Session["id_usuario"].ToString();
            compra.Quantidade = quantidade;
            compra.Relatorio = relatorio;

            TempData["msg"] = compra.EfetuaCompra();
            return RedirectToAction("EfetuaCompra");
        }
    }
}