using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        AppDbContext _db;
        public OrderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order order)
        {
           _db.Update(order);
        }
    }
}
