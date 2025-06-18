using Proyecto_Donacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Proyecto_Donacion.Controllers
{
    [RoutePrefix("api/Beneficiarios")]
    public class BeneficiarioController : ApiController
    {
        // GET api/Beneficiarios  - obtiene todos los beneficiarios
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var lista = Beneficiario.ObtenerTodosBeneficiario();
            return Ok(lista);  // responde con HTTP 200 y la lista en JSON
        }
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Getbyid(int id)
        {
            var cate = Beneficiario.ObtenerPorIdBeneficiario(id);
            return Ok(cate);  // responde con HTTP 200 y la lista en JSON
        }


        // POST api/Donacion  - crea una nueva donacion
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Beneficiario nuevo)
        {
            if (nuevo == null)
                return BadRequest("Datos de Beneficiario inválidos.");
            try
            {
                Beneficiario bene = new Beneficiario();
                bene.Nombre = nuevo.Nombre;
                bene.Email = nuevo.Email;
                int newId = bene.CrearBeneficiario();
                // Devolver el objeto creado con su nuevo ID
                nuevo.ID_Beneficiario = newId;
                return Ok(nuevo);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/beneficiarios/5  - actualiza un beneficiario existente
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Beneficiario editado)
        {
            if (editado == null) return BadRequest();
            if (id != editado.ID_Beneficiario)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");
            Beneficiario bene = new Beneficiario();
            bene.ID_Beneficiario = editado.ID_Beneficiario; // <--- ASIGNA EL ID AQUÍ
            bene.Nombre = editado.Nombre;
            bene.Email = editado.Email;

            bool ok = bene.ActualizarBeneficiario();
            if (ok)
                return Ok("Categoria actualizado correctamente.");
            else
                return NotFound();
        }


        // DELETE api/Beneficiarios/5  - elimina un beneficiario
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            Beneficiario bene = new Beneficiario();
            bool ok = bene.EliminarBeneficiario(id); // Pass the required 'id' parameter
            if (ok)
                return Ok("beneficiario eliminado.");
            else
                return NotFound();
        }
    }
}
