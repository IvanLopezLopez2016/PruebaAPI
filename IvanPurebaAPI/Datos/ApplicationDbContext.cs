using IvanPurebaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IvanPurebaAPI.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
                
        }
        public DbSet<Prueba> Pruebas { get; set; }
    }
}
