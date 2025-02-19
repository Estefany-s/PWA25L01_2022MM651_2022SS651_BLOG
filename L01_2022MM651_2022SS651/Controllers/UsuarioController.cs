using Microsoft.AspNetCore.Mvc;
using L01_2022MM651_2022SS651.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2022MM651_2022SS651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly blogContext _blogContexto;

        public UsuarioController(blogContext blogContexto)
        {
            _blogContexto = blogContexto;
        }

        //CRUD de usuario
        [HttpGet]
        [Route("GetAllUser")]
        public IActionResult obtenerUsuario() {
            List<Usuario> listadoUsuarios = (from u in _blogContexto.Usuario select u).ToList();

            if (listadoUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AgregarUsuario([FromBody] Usuario _usuario)
        {
            try
            {
                _blogContexto.Usuario.Add(_usuario);
                _blogContexto.SaveChanges();
                return Ok(_usuario);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error de base de datos: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error general: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("actualizarUser/{id}")]
        public IActionResult actualizarUsuario(int id, [FromBody] Usuario actualizarUsuario)
        {
            Usuario? usuarioActual = (from u in _blogContexto.Usuario where u.usuarioId == id select u).FirstOrDefault();
            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.nombreUsuario = actualizarUsuario.nombreUsuario;
            usuarioActual.clave = actualizarUsuario.clave;
            usuarioActual.nombre = actualizarUsuario.nombre;
            usuarioActual.apellido = actualizarUsuario.apellido;

            _blogContexto.Entry(usuarioActual).State = EntityState.Modified;
            _blogContexto.SaveChanges();

            return Ok(actualizarUsuario);
        }

        [HttpDelete]
        [Route("eliminarUser/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            Usuario? usuario = (from u in _blogContexto.Usuario where u.usuarioId == id select u).FirstOrDefault();

            if (usuario == null) { return NotFound(); }

            _blogContexto.Usuario.Attach(usuario);
            _blogContexto.Usuario.Remove(usuario);
            _blogContexto.SaveChanges();

            return Ok(usuario);
        }

        //Filtrado por nombre
        [HttpGet]
        [Route("busquedaXNombre")]
        public IActionResult busquedaPorNombre(string name)
        {
            List<Usuario> listadoPorNombre = (from u in _blogContexto.Usuario
                                             where u.nombre == name
                                             select u).ToList();

            if (listadoPorNombre.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPorNombre);
        }

        //Filtrado por apellido
        [HttpGet]
        [Route("busquedaXApellido")]
        public IActionResult busquedaPorApellido(string lastName)
        {
            List<Usuario> listadoPorApellido = (from u in _blogContexto.Usuario
                                              where u.apellido == lastName
                                              select u).ToList();

            if (listadoPorApellido.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPorApellido);
        }

        //Filtrado por rol
        [HttpGet]
        [Route("busquedaXRol")]
        public IActionResult busquedaPorRol(string rolName)
        {
            List<Usuario> listadoPorRol = (from u in _blogContexto.Usuario
                                           join r in _blogContexto.Roles
                                            on u.rolId equals r.rolId
                                           where r.rol == rolName
                                                select u).ToList();

            if (listadoPorRol.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPorRol);
        }
    }
}
