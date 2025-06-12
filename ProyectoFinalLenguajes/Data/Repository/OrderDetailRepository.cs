using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        AppDbContext _db;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }
    }
}
