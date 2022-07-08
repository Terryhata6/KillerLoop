
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetIconView : MonoBehaviour
{
    #region PrivateFileds

    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshPro _text;

    #endregion

    #region AccessFields

    public Image Icon => _icon;
    public TextMeshPro Text => _text;

    #endregion
}