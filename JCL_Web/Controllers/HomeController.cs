using JCL_Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JCL_Web.Controllers
{
    public class HomeController : Controller
    {
        //Página inicial do sitema
        public ActionResult Index()
        {
            return View();
        }

            //Página para se realizar o login do usuarios
            public ActionResult PaginaLogin() //Fazer login no site
        {
            if (Session["tipo_usuario"] == null)
            {
                return View();
            }
            else
            {
                TempData["msg"] = "Já está logado!";
                return View("Index");
            }

        }

        //Método necessário para realizar o POST e realizar a ação de login
        [HttpPost]
        public ActionResult PaginaLogin(string email, string senha) //HttpPost do Login
        {
            Login login = new Login();
            login.Email = email;
            login.Senha = senha;

            string msg = login.VerificarLogin();
            TempData["msg"] = msg;

            if (msg.Contains("entrou"))
            {
                Session["tipo_usuario"] = login.Tipo;
                Session["id_usuario"] = login.Id;
                Session["nomeUsu"] = login.Nome;
                Session["emailUsu"] = login.Email;

                if (login.Senha.Equals("123456789"))
                {
                    return RedirectToAction("MudarSenha");
                }
                return RedirectToAction("Index");
            }
            else if (msg.Contains("incorreta") || msg.Contains("Erro") || msg.Contains("existe"))
            {
                return View();
            }
            return View();
        }

        //Método para sair da conta, remove todos os dados da Session
        public ActionResult SairLogin()
        {            
            Session.Clear();
            TempData["msg"] = "Saiu da sua conta!";
            return RedirectToAction("Index");
        }

        //Site para mudar a senha
        public ActionResult MudarSenha()
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    return View();
                }
            }
            TempData["msg"] = "Você precisa esta logado!";
            return RedirectToAction("PaginaLogin", "Home");
        }

        //Método para mudar a senha
        [HttpPost]
        public ActionResult MudarSenha(string id, string senha)
        {
            Usuario usuario = new Usuario();
            usuario.Id = id;

            string msg = usuario.MudarSenha(senha, Session["id_usuario"].ToString());

            TempData["msg"] = msg;

            return RedirectToAction("Index", "Home");
        }
    }
}   