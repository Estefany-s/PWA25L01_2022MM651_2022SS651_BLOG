using L01_2022MM651_2022SS651.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
