namespace ProyectoFinalLenguajes.Data.Repository.Interface
{
    public interface IUnitOfWork
    {
        
        IDishRepository Dish { get; }
        IOrderDetailRepository OrderDetail { get; }

        IOrderRepository Order { get; }

        IOrderMinutesRepository OrderMinutes { get; }

        void Save();
    }
}
