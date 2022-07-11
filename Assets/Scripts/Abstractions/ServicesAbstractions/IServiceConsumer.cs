

public interface IServiceConsumer<T> where T : IService
{
    public void UseService(T service);
}
