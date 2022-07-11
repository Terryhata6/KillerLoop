using System.Collections.Generic;

public interface IProgressValuesUpdater : IService
{
    public List<ProgressValue> ProgressValues { get; }
}