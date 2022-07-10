using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenuPanel : MonoBehaviour
{
    #region ProtectedFields

    protected bool IsShow { get; set; }

    #endregion

    #region PublicMethods

    public abstract void Hide();
    public abstract void Show();

    #endregion

    #region ProtectedMethods

    protected void BindListenerToButton(Button button, UnityEngine.Events.UnityAction listener)
    {
        if (button)
        {
            button.onClick.AddListener(listener);
        }
        else
        {
            Debug.Log($"Button Missing {button}", button);
        }
    }

    protected void RemoveListenersFromButton(Button button)
    {
        if (button)
        {
            button.onClick.RemoveAllListeners();
        }
        else
        {
            Debug.Log($"Button Missing {button}", button);
        }
    }

    #endregion

}