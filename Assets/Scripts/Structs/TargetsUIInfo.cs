public struct TargetsUIInfo
{
    #region PrivateFields
    private int _number;
    private LevelType _type;
    #endregion
    public TargetsUIInfo(int Number, LevelType Type)
    {
        _number = Number;
        _type = Type;
    }
    #region AccessFields
    public int LevelNumber => _number;
    public LevelType LevelType => _type;
    #endregion
}
