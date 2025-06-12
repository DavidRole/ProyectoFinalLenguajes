namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IUnitOfWork
    {
        IAdminRepository Admin { get; }
        ICookRepository Cook { get; }
        ICustomerRepository Customer { get; }
        IDishRepository Dish { get; }
        IOrderDetailRepository OrderDetail { get; }

        IOrderRepository Order { get; }

        void Save();
    }
}
