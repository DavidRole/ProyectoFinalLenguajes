using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class OrderMinutesRepository : Repository<OrderMinutes>, IOrderMinutesRepository
    {
        AppDbContext _db;
        public OrderMinutesRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderMinutes orderMinutes)
        {
            _db.Update(orderMinutes);
        }
    }
}
