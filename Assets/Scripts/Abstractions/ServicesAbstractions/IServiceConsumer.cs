

public interface IServiceConsumer<T> : IConsumer where T : IService
{
    public void UseService(T service);
}
