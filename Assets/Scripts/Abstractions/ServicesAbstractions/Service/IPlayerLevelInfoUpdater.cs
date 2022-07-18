

public interface IPlayerLevelInfoUpdater : IService
{
    public PlayerLevelInfo PlayerInfo { get; }

    public WriterLevelInfo WriterInfo { get; }
}