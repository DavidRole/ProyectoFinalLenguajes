using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class CookRepository : Repository<Cook>, ICookRepository
    {
        AppDbContext _db;
        public CookRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Cook cook)
        {
            _db.Update(cook);
        }
    }
}
