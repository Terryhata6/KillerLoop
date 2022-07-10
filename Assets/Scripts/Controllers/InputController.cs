
using UnityEngine;

public class InputController : BaseController, IExecute
{
    public InputController()  { }

    private Touch _firstTouch;
    private bool _mouseCLickedPreviousFrame;
    private Vector2 _mousePosition;
    private Vector2 _mouseOldPosition;


    public override void Initialize()
    {
        base.Initialize();

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;
    }

    public void Execute()
    {
        if (!IsActive)
        {
            return;
        }
#if UNITY_EDITOR
        {
            if (_mouseCLickedPreviousFrame)
            {
                if (Input.GetMouseButton(0))
                {
                    _mousePosition = Input.mousePosition;
                    if (_mousePosition == _mouseOldPosition)
                    {
                        InputEvents.Current.TouchStationaryEvent(_mousePosition);
                        //Debug.Log("Cтационарно");
                    }
                    else
                    {
                        InputEvents.Current.TouchMovedEvent(_mousePosition);
                        //Debug.Log("Мувд");
                    }
                }
                else
                {
                    _mousePosition = Input.mousePosition;
                    InputEvents.Current.TouchEndedEvent(_mousePosition);
                    //Debug.Log("Енд");
                    _mouseCLickedPreviousFrame = false;
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    _mouseCLickedPreviousFrame = true;
                    InputEvents.Current.TouchBeganEvent(Input.mousePosition);
                    //Debug.Log("Старт");
                }
            }
            _mouseOldPosition = _mousePosition;

            return;
        }
#endif
        if (Input.touchCount > 0)
        {
            _firstTouch = Input.GetTouch(0);
            switch (_firstTouch.phase)
            {
                case TouchPhase.Began:
                    {
                        InputEvents.Current.TouchBeganEvent(_firstTouch.position);
                        break;
                    }
                case TouchPhase.Canceled:
                    {
                        InputEvents.Current.TouchCancelledEvent();
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        InputEvents.Current.TouchMovedEvent(_firstTouch.position);
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        InputEvents.Current.TouchEndedEvent(_firstTouch.position);
                        break;
                    }
                case TouchPhase.Stationary:
                    {
                        InputEvents.Current.TouchStationaryEvent(_firstTouch.position);
                        break;
                    }
            }
        }
    }

}