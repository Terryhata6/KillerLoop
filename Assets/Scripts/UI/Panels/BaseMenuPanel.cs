using UnityEngine;

public abstract class BaseMenuPanel : MonoBehaviour
{
    protected bool IsShow { get; set; }

    public abstract void Hide();
    public abstract void Show();
}