using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
