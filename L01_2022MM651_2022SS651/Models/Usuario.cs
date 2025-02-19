using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace L01_2022MM651_2022SS651.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        public int usuarioId {  get; set; }
        public int rolId { get; set; }
        public string nombreUsuario { get; set; }
        public string clave {  get; set; }
        public string nombre {  get; set; }
        public string apellido { get; set; }
    }
}
