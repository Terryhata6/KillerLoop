
using UnityEngine;

public class SaveProgressManager : Singleton<SaveProgressManager>
{
    public SaveProgressManager()
    {
        InitializeFields();
    }

    #region PrivateFields

    private FileManager _fileManager;
    private string _defaultFileName;
    private string _moneyDataFileName;
    private string _levelDataFileName;

    #endregion

    public T LoadData<T>(SaveDataType type) where T : class
    {
        if (_fileManager.LoadFromFile(ChooseFileName(type), out string json))
        {
            Debug.Log($"Load complete from {ChooseFileName(type)}");
            return json.FromJson<T>();
        }
        return null;
    }

    public void SaveData(ISaveable data)
    {
        if (_fileManager.WriteToFile(ChooseFileName(data.Type), data.ToJson()))
        {
            Debug.Log($"Save successful into {ChooseFileName(data.Type)},{data.ToJson()}");
        }
    }

    private void InitializeFields()
    {
        _levelDataFileName = "LevelData.dat";
        _moneyDataFileName = "MoneyData.dat";
        _defaultFileName = "SaveData.dat";
        _fileManager = new FileManager();
        Instance = this;
    }

    private string ChooseFileName(SaveDataType type)
    {
        switch (type)
        {
            case SaveDataType.LevelData:
                {
                    return _levelDataFileName;
                }
            case SaveDataType.MoneyData:
                {
                    return _moneyDataFileName;
                }
            default:
                {
                    return _defaultFileName;
                }
        }
    }

}