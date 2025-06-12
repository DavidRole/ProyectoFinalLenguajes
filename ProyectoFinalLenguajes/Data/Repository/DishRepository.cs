using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        AppDbContext _db;
        public DishRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Dish dish)
        {
            _db.Update(dish);
        }
    }
}
