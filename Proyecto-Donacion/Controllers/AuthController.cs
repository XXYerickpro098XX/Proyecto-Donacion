using Proyecto_Donacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Proyecto_Donacion.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        // POST api/auth/login
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest creds)
        {
            if (creds == null) return BadRequest("Se requiere correo y contraseña.");
            string correo = creds.correo;
            string pass = creds.Password;
            // Obtener usuario por correo
            Usuario usr = Usuario.ObtenerPorCorreo(correo);
            if (usr == null)
                return Unauthorized(); // No autorizado (401) si usuario no existe
                                       // Verificar contraseña
            string hashIngresado = SeguridadUtil.HashSHA256(pass);
            if (usr.PasswordHash1 != hashIngresado)
            {
                return Unauthorized(); // contraseña incorrecta
            }
            // Si llegamos aquí, autenticación exitosa
            // Preparar objeto de respuesta (sin incluir la PasswordHash por seguridad)
            var userData = new { UsuarioID = usr.UsuarioID1, Nombre = usr.Nombre1, correo = usr.Correo1, Rol = usr.Rol1 };
            // Convertir a JSON y luego cifrar 
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(userData);
            return Ok(userData); // Devuelve el objeto JSON normal, sin cifrar

        }
    }

    // Podemos definir una clase auxiliar para recibir la solicitud de login:
    public class LoginRequest
    {
        public string correo { get; set; }
        public string Password { get; set; }
    }
}
