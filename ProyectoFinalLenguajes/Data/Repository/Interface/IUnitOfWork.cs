namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; }
        ICookRepository CookRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IDishRepository IDishRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }

        IOrderRepository OrderRepository { get; }

        void Save();
    }
}
