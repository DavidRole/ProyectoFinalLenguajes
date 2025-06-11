using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IDishRepository : IRepository<Dish>
    {
        void Update(Dish dish);
    }
}
