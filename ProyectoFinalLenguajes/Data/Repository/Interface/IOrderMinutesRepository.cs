using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IOrderMinutesRepository: IRepository<OrderMinutes>
    {
        void Update(OrderMinutes orderMinutes);
    }
}
