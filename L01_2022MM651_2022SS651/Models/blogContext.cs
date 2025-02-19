using Microsoft.EntityFrameworkCore;

namespace L01_2022MM651_2022SS651.Models
{
    public class blogContext : DbContext
    {
        public blogContext(DbContextOptions<blogContext> options) : base(options)
        {
        }

        public DbSet<Calificaciones> Calificaciones { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Publicaciones> Publicaciones { get; set; }

    }
}
