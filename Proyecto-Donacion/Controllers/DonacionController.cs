using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Proyecto_Donacion.Models;

namespace Proyecto_Donacion.Controllers
{
    [RoutePrefix("api/Donaciones")]

    public class DonacionController : ApiController
    {
        // GET api/Donacione  - obtiene todas las donaciones
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var lista = Donacion.ObtenerTodos();
            return Ok(lista);  // responde con HTTP 200 y la lista en JSON
        }

        // GET api/donaciones/5  - obtiene donacion por ID
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var prod = Donacion.ObtenerPorID(id);
            if (prod != null)
                return Ok(prod);
            else
                return NotFound(); // HTTP 404 si no se encontró
        }

        // POST api/donaciones  - crea un nuevo producto
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Donacion nuevo)
        {
            if (nuevo == null)
                return BadRequest("Datos de donacion inválidos.");
            try
            {
                int newId = Donacion.CrearDonacion(nuevo.Monto, nuevo.Fecha, nuevo.UsuarioID);
                // Devolver el objeto creado con su nuevo ID
                nuevo.ID_donaciones = newId;
                return Ok(nuevo);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/productos/5  - actualiza un donacion existente
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Donacion editado)
        {
            if (editado == null) return BadRequest();
            if (id != editado.ID_donaciones)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            bool ok = Donacion.ActualizarDonacion(id, editado.Monto, editado.Fecha, editado.ID_BENEFICIARIOS);
            if (ok)
                return Ok("Donacion actualizada correctamente.");
            else
                return NotFound(); // si no se actualizó, podría ser que no existía
        }

        // DELETE api/productos/5  - elimina un producto
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            bool ok = Donacion.EliminarDonacion(id);
            if (ok)
                return Ok("Donacion eliminada.");
            else
                return NotFound();
        }
    }
}
