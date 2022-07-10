
using UnityEngine;
using UnityEngine.UI;

public class ShopIconView : BaseUiView
{
    #region PrivateFileds

    [SerializeField] private Animation _attentionAnimation;
    [SerializeField] private Image _attentionImage;

    #endregion

    private void Awake()
    {
        AttentionDisable();
    }

    #region PublicMethods

    public void SetAttention(bool newGoodsAvaible)
    {
       if (newGoodsAvaible)
        {
            AttentionActive();
        }
       else
        {
            AttentionDisable();
        }
    }

    #endregion 

    #region PrivateMethods

    private bool CheckForComponents()
    {
        return _attentionAnimation && _attentionImage;
    }

    private void AttentionActive()
    {
        if (CheckForComponents())
        {
            _attentionImage.enabled = true;
            _attentionAnimation.Play();
        }
    }

    private void AttentionDisable()
    {
        if (CheckForComponents())
        {
            _attentionImage.enabled = false;
            _attentionAnimation.Stop();
        }
    }

    #endregion
}