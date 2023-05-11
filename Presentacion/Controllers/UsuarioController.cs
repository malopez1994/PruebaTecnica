using Microsoft.AspNetCore.Mvc;
using Presentacion.Models;
using System.Diagnostics;
using Presentacion.Metodos;
using Newtonsoft.Json.Linq;

namespace Presentacion.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        public IConfiguration _Config;
        private MethodPersona funcion;
        public UsuarioController(ILogger<UsuarioController> logger, IConfiguration config)
        {
            _logger = logger;
            _Config = config;
        }

        public IActionResult Index()
        {
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success) 
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
            List<UsuarioModel> List = funcion.ListaUsuario(HttpContext.Session.GetString("Token").ToString());
            return View(List);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UsuarioModel usuario = new UsuarioModel();
            usuario.FechaNacimiento = DateTime.Now;
            return PartialView("_Create",usuario);
        }

        [HttpPost]
        public IActionResult Create(UsuarioModel usuario)
        {
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success)
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
           bool valido = funcion.CrearUsuario(HttpContext.Session.GetString("Token").ToString(), usuario);
           return Redirect("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            UsuarioModel usuario = new UsuarioModel();
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success)
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
            usuario = funcion.UsuarioxId(HttpContext.Session.GetString("Token").ToString(), id);
            return PartialView("_Update", usuario);
        }

        [HttpPost]
        public IActionResult Update(UsuarioModel usuario)
        {
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success)
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
            bool valido = funcion.ActualizarUsuario(HttpContext.Session.GetString("Token").ToString(), usuario);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            UsuarioModel usuario = new UsuarioModel();
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success)
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
            usuario = funcion.UsuarioxId(HttpContext.Session.GetString("Token").ToString(), id);
            return PartialView("_Delete", usuario);
        }

        [HttpPost]
        public IActionResult Delete(UsuarioModel usuario)
        {
            funcion = new MethodPersona(_Config);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                var obj = funcion.AccesoToken();
                if ((bool)obj.success)
                {
                    string Tkn = obj.token;
                    HttpContext.Session.SetString("Token", Tkn);
                }
            }
            bool valido = funcion.EliminarUsuario(HttpContext.Session.GetString("Token").ToString(), usuario.Id);
            return RedirectToAction("Index");
        }
    }
}