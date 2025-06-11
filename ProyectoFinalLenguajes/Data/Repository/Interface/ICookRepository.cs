using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface ICookRepository : IRepository<Cook>
    {
        void Update(Cook cook);
    }
}
