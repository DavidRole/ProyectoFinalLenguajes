using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}
