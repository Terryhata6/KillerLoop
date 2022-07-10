public interface ITargetInfo : IService
{
    public int CurrentTargetNumber { get; }
    public TargetsUIInfo GetTargetInfo(int targetNumber);
}