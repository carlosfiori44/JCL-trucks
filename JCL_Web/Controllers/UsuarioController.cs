using JCL_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JCL_Web.Controllers
{
    public class UsuarioController : Controller
    {
        //Página para cadastrar um usuário no banco
        public ActionResult CadastraUsuario()
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
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
                TempData["msg"] = "Entre para poder utilizar os recursos!";
                return RedirectToAction("PaginaLogin");
            }
        }

        //Método para enviar os dados inseridos para o banco de dados
        [HttpPost]
        public ActionResult CadastraUsuario(string nome, string email,
            string telefone, string endereco, string regiao)
        {
            if (Session["tipo_usuario"] != null)
            {
                Usuario usuario = new Usuario();
                usuario.Nome = nome;
                usuario.Email = email;
                usuario.Telefone = telefone;
                usuario.Endereco = endereco;
                usuario.Regiao = regiao;

                if (Session["tipo_usuario"].Equals("gerente"))
                {
                    usuario.TipoUsuario = "funcionario";
                    usuario.Senha = "123456789";
                }
                else
                {
                    usuario.TipoUsuario = "cliente";
                    usuario.Senha = null;
                }

                string msg = usuario.CadastrarCliente();

                if (msg.Contains("sucesso"))
                {
                    Models.Historico.MarcarAtividade("Realizou cadastro de cliente", Session["id_usuario"].ToString());
                }
                TempData["msg"] = msg;
            }
            return View();
        }

        //Página para mostrar as informações dos usuários no banco de dados
        public ActionResult MostraUsuario(string nome)
        {
            List<Usuario> usuario;
            TempData["pesquisaN"] = "J";

            if (Session["tipo_usuario"] != null)
            {
                if (nome == null || nome.Equals(""))
                {
                    if (Session["tipo_usuario"].Equals("gerente"))
                    {
                        usuario = Usuario.MostraUsuario("*");
                    }
                    else
                    {
                        usuario = Usuario.MostraUsuario("cliente");
                    }
                }
                else
                {
                    if (Session["tipo_usuario"].Equals("gerente"))
                    {
                        usuario = Usuario.PesquisaUsuarioNome(nome, "todos");
                    }
                    else
                    {
                        usuario = Usuario.PesquisaUsuarioNome(nome, "cliente");
                    }
                }
            }
            else
            {
                TempData["msg"] = "Você precisa estar logado!";
                return RedirectToAction("Index", "Home");
            }
            return View(usuario);
        }

        //Site para mudar dados do usuario
        public ActionResult EditarUsuario(string id)
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    Usuario usuario = Usuario.PesquisaUsuario(id);
                    return View(usuario);
                }
            }
            TempData["msg"] = "Você precisa esta logado!";
            return RedirectToAction("PaginaLogin", "Home");
        }

        //Método para alterar dados do usuario
        [HttpPost]
        public ActionResult EditarUsuario(string id, string endereco, string email, string regiao, string telefone)
        {
            Usuario usuario = new Usuario();
            usuario.Id = id;
            usuario.Endereco = endereco;
            usuario.Email = email;
            usuario.Regiao = regiao;
            usuario.Telefone = telefone;

            string msg = usuario.EditarUsuario();

            TempData["msg"] = msg;

            if (msg.Contains("feita"))
            {
                Models.Historico.MarcarAtividade("Realizou alteração no usuario", Session["id_usuario"].ToString());
            }
            return RedirectToAction("MostraUsuario");
        }

        //Página para mostra o histórico das ações dos funcionarios
        public ActionResult Historico()
        {
            if (Session["tipo_usuario"] != null) //Verificação para ver o usuario logado
            {
                if (Session["tipo_usuario"].Equals("gerente") || Session["tipo_usuario"].Equals("funcionario"))
                {
                    List<Models.Historico> historico = Models.Historico.MostrarAtividade();
                    return View(historico);
                }
            }

            TempData["msg"] = "Você precisa esta logado!";
            return RedirectToAction("PaginaLogin", "Home");
        }
    }
}