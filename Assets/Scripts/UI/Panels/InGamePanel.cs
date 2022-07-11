using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BaseMenuPanel,
    IServiceConsumer<IProgressValuesUpdater>
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    #endregion

    #endregion

    private void Awake()
    {
        Dictionary<ProgressValueType, int> dict = new Dictionary<ProgressValueType, int>();
        dict.Add(ProgressValueType.EnemyBeaten, 22);
        dict.Add(ProgressValueType.EnemyBeaten, 22);
    }

    #region PublicMethods

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
    }

    #region IServiceConsumer

    public void UseService(IProgressValuesUpdater service)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void OnDestroy()
    {

    }

    #endregion

}