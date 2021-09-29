using System;
using UnityEngine;

public class InputEvents
    {
        public static InputEvents Current = new InputEvents();
        
        #region TouchBeganEvents
        public Action<Vector2> OnTouchBeganEvent;
        public void TouchBeganEvent(Vector2 position)
        {
            OnTouchBeganEvent?.Invoke(position);
        }
        #endregion
        #region TouchMovedEvents
        public Action<Vector2> OnTouchMovedEvent;
        public void TouchMovedEvent(Vector2 delta)
        {
            OnTouchMovedEvent?.Invoke(delta);        
        }
        #endregion
        #region TouchEndedEvents
        public Action OnTouchEndedEvent;
        public void TouchEndedEvent()
        {        
            OnTouchEndedEvent?.Invoke();        
        }
        #endregion
        #region TouchStationaryEvents
        public Action OnTouchStationaryEvent;
        public void TouchStationaryEvent()
        {
            OnTouchStationaryEvent?.Invoke();
        }
        #endregion
        #region TouchCancelledEvents
        public Action OnTouchCancelledEvent;
        public void TouchCancelledEvent()
        {
            OnTouchCancelledEvent?.Invoke();
        }
        #endregion
        
    }
