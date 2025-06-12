using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        AppDbContext _db;
        public AdminRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Admin admin)
        {
           _db.Update(admin);
        }
    }
}
