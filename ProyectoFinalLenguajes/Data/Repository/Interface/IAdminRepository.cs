using ProyectoFinalLenguajes.Models;

namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IAdminRepository : IRepository<Admin>
    {
        void Update(Admin admin);
    }
}
