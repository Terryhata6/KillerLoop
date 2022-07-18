

public interface IEnemiesLevelInfoUpdater : IService
{
    public EnemiesLevelInfo EnemiesInfo { get; }

    public RoadRunSave RoadRunWay { get; }
}