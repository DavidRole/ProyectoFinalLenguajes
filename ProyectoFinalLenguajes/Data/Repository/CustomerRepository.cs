using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        AppDbContext _db;
        public CustomerRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
