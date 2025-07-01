using ProyectoFinalLenguajes.Data.Repository.Interface;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;

        public IDishRepository Dish { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public IOrderRepository Order { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Dish = new DishRepository(_db);
            Order = new OrderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
