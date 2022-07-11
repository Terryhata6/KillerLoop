using System.Collections.Generic;

public interface IContainConsumers
{
    public List<IServiceConsumer<IService>> GetConsumers();
}