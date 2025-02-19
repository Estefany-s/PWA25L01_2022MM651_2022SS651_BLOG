using System.ComponentModel.DataAnnotations;

namespace L01_2022MM651_2022SS651.Models
{
    public class Roles
    {
        [Key]
        public int rolId { get; set; }
        public string rol {  get; set; }
    }
}
