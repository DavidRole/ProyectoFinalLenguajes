using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalLenguajes.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options) { }


    }
}
