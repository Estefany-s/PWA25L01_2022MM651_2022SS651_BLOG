using L01_2022MM651_2022SS651.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022MM651_2022SS651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : Controller
    {
        private readonly blogContext _blogContexto;
        public CalificacionesController(blogContext blogContext)
        {
            _blogContexto = blogContext;
        }

        [HttpGet]
        [Route("GetAll")]

        // Obtener todos las calificaciones.
        public IActionResult Get()
        {
            List<Calificaciones> listadoCalificaciones = (from e in _blogContexto.Calificaciones
                                                    select e).ToList();

            if (listadoCalificaciones.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoCalificaciones);
        }

        //Crear calificacion
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCalificacion([FromBody] Calificaciones calificaciones)
        {
            try
            {
                _blogContexto.Calificaciones.Add(calificaciones);
                _blogContexto.SaveChanges();
                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Actualizar Calificación
        [HttpPut]
        [Route("actualizarCalification/{id}")]
        public IActionResult actualizarCalificacion(int id, [FromBody] Calificaciones actualizarCalificacion)
        {
            Calificaciones? calificacionActual = (from c in _blogContexto.Calificaciones where c.calificacionId == id select c).FirstOrDefault();
            if (calificacionActual == null)
            {
                return NotFound();
            }

            calificacionActual.calificacion = actualizarCalificacion.calificacion;

            _blogContexto.Entry(calificacionActual).State = EntityState.Modified;
            _blogContexto.SaveChanges();

            return Ok(actualizarCalificacion);
        }

        //Eliminar calificación
        [HttpDelete]
        [Route("eliminarCalification/{id}")]
        public IActionResult EliminarCalificacion(int id)
        {
            Calificaciones? calificacion = (from c in _blogContexto.Calificaciones where c.calificacionId == id select c).FirstOrDefault();

            if (calificacion == null) { return NotFound(); }

            _blogContexto.Calificaciones.Attach(calificacion);
            _blogContexto.Calificaciones.Remove(calificacion);
            _blogContexto.SaveChanges();

            return Ok(calificacion);
        }

        //Filtrar listado de calificaciones por publicacion
        [HttpGet]
        [Route("obtenerCalificacionXPubli/{id}")]
        public IActionResult obtenerCalificacionXPublicacion(int id)
        {
            var calificacionXPubli = (from c in _blogContexto.Calificaciones
                                 join p in _blogContexto.Publicaciones
                                    on c.publicacionId equals p.publicacionId
                                 where p.publicacionId == id
                                 select new
                                 {
                                     p.titulo,
                                     c.calificacion
                                 }).ToList();

            if (calificacionXPubli == null)
            {
                return NotFound();
            }
            return Ok(calificacionXPubli);
        }
    }
}
