using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Proyecto_Donacion.Models;

namespace Proyecto_Donacion.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ApiController
    {
        // GET api/usuario
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var lista = Usuario.ObtenerTodos();
            return Ok(lista);
        }

        // POST api/usuario
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Usuario nuevo)
        {
            if (nuevo == null)
                return BadRequest("Datos de usuario inválidos.");

            try
            {
                Usuario us = new Usuario();
                us.Nombre1 = nuevo.Nombre1;
                us.Correo1 = nuevo.Correo1;
                us.PasswordHash1 = nuevo.PasswordHash1;
                us.Rol1 = nuevo.Rol1;
                int newId = us.CrearUsuario();

                nuevo.UsuarioID1 = newId;
                return Ok(nuevo);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/usuario/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Usuario editado)
        {
            if (editado == null)
                return BadRequest();
            if (id != editado.UsuarioID1)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            Usuario us = new Usuario();
            us.Nombre1 = editado.Nombre1;
            us.Correo1 = editado.Correo1;
            us.PasswordHash1 = editado.PasswordHash1;
            us.Rol1 = editado.Rol1;

            int newId = us.CrearUsuario();   // igual que tu ejemplo, aunque no sea correcto
            bool ok = us.ActualizarUsuario();

            if (ok)
                return Ok("Usuario actualizado correctamente.");
            else
                return NotFound(); // Si no se actualizó, quizás no existía
        }

        // DELETE api/usuario/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            Usuario us = new Usuario();
            bool ok = us.EliminarUsuario(id);
            if (ok)
                return Ok("Usuario eliminado.");
            else
                return NotFound();
        }
    }
}
