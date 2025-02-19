using L01_2022MM651_2022SS651.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace L01_2022MM651_2022SS651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : Controller
    {
        private readonly blogContext _blogContexto;
        public ComentariosController(blogContext blogContext)
        {
            _blogContexto = blogContext;
        }

        [HttpGet]
        [Route("GetAll")]

        // Obtener todos los comentarios.
        public IActionResult Get()
        {
            List<Comentarios> listadoComentarios = (from e in _blogContexto.Comentarios
                                        select e).ToList();

            if (listadoComentarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoComentarios);
        }
        //Crear comentario
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarComentario([FromBody] Comentarios comentarios)
        {
            try
            {
                _blogContexto.Comentarios.Add(comentarios);
                _blogContexto.SaveChanges();
                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // actualizar comentario
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarComentario(int id, [FromBody] Comentarios comentarioModificar)
        {
            Comentarios? comentarioActual = (from e in _blogContexto.Comentarios
                                             where e.cometarioId == id
                                             select e).FirstOrDefault();
            if (comentarioActual == null)
            {
                return NotFound();
            }

            comentarioActual.comentario = comentarioModificar.comentario;

            _blogContexto.Entry(comentarioActual).State = EntityState.Modified;
            _blogContexto.SaveChanges();

            return Ok(comentarioModificar);
        }

        //eliminar un comentario.
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarComentario(int id)
        {
            Comentarios? autor = (from e in _blogContexto.Comentarios
                            where e.cometarioId == id
                            select e).FirstOrDefault();

            if (autor == null) { return NotFound(); }

            _blogContexto.Comentarios.Attach(autor);
            _blogContexto.Comentarios.Remove(autor);
            _blogContexto.SaveChanges();

            return Ok(autor);

        }

        // método que permita retornar el listado de los comentarios filtradas por un usuario en específico.
        [HttpGet]
        [Route("ComentariosDeUsuarioEspecifico/{id}")]
        public IActionResult GetComentariosUsuario(int id)
        {
            var comentario = (from b in _blogContexto.Comentarios
                         join Usuario l in _blogContexto.Usuario
                             on b.usuarioId equals l.usuarioId
                         where b.usuarioId == id
                         select new
                         {
                             b.comentario,
                             l.nombre
                         }).ToList();

            if (comentario == null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        // TOP N de usuarios y sus cantidades de comentarios registardas.
        [HttpGet]
        [Route("usuariosContidadDeComentarios/{cuantos}")]
        public IActionResult usuariosCantidadDeComentarios(int cuantos)
        {
            var usuariosCantidadComentarios = (from l in _blogContexto.Comentarios
                                 join a in _blogContexto.Usuario
                                    on l.usuarioId equals a.usuarioId
                                 group a by a.nombre into user
                                 select new
                                 {
                                     NombreUsuario = user.Key,
                                     CantidadComentarios = user.Count()
                                 })
                                 .OrderByDescending(res => res.CantidadComentarios)
                                 .Take(cuantos)
                                 .ToList();

            if (!usuariosCantidadComentarios.Any())
            {
                return NotFound();
            }

            return Ok(usuariosCantidadComentarios);
        }
    }
}
