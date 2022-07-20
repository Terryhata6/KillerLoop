

using System;
using UnityEngine.EventSystems;

public class StartLevelButtonView: BaseUiView, IPointerDownHandler, ITouchEventsHandler
{
    public Action OnTouch { get; set; }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch?.Invoke();
    }
}
