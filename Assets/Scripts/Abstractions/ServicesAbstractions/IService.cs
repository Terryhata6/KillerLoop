
public interface IService
{
    public void AddConsumer<T>(IServiceConsumer<T> consumer) where T:IService;
}