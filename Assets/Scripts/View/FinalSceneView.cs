
using UnityEngine;

public class FinalSceneView : BaseObjectView, ICameraTargetSpawner
{
    #region PrivateFields

    #region Serialized

    [Header("Win scene info")]
    [SerializeField] private GameObject _winScene;
    [SerializeField] private DancePlayerView _dancingPlayer;


    #endregion

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _winScene.SetActive(false);
        _dancingPlayer.gameObject.SetActive(false);
        InitializeService();
    }

    public void Enable()
    {
        if (!_winScene)
        {
            return;
        }
        _winScene.SetActive(true);
        _dancingPlayer.gameObject.SetActive(true);
        _dancingPlayer.Dance();
        _dancingPlayer.MoneyFountain();
        UpdateConsumersInfo();
    }

    #region IService

    private BaseService<ICameraTargetSpawner> _servicerHelper;

    public Transform CameraTarget => _dancingPlayer.transform;

    public void AddConsumer(IConsumer consumer)
    {
        _servicerHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _servicerHelper = new BaseService<ICameraTargetSpawner>(this);
        _servicerHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _servicerHelper?.ServeConsumers();
    }

    #endregion
}