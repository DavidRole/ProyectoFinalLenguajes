using ProyectoFinalLenguajes.Data.Repository.Interface;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        public IAdminRepository Admin { get; private set; }

        public ICookRepository Cook { get; private set; }

        public ICustomerRepository Customer { get; private set; }

        public IDishRepository Dish { get; private set; }

        public IOrderDetailRepository OrderDetail { get; private set; }

        public IOrderRepository Order { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Admin = new AdminRepository(_db);
            Cook = new CookRepository(_db);
            Customer = new CustomerRepository(_db);
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
