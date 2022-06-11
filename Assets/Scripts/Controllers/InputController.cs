using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : BaseController, IExecute
{
    public InputController()  { }
    
    private bool                 _countQueue = true;
    private float                _temporalMagnitude = 0;
    private Vector2              _mouseStartPosition;
    private bool                 _mouseCLickedPreviousFrame = false;
    private Vector2              _mouseOldPosition;
    private Vector2              _mousePosition = Vector2.zero;
    private int                  _counter = 0;
    private Touch                _firstTouch;

    
    public float                 TemporalMagnitude = 0;

    public override void Initialize()
    {
        base.Initialize();

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;
        
        UIEvents.Current.OnButtonPause += Disable;
        UIEvents.Current.OnButtonResume += Enable;
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


// Slide Mechanics start
/*
    private void CountSlide(Vector2 deltaPosition)
    {
        //Debug.Log($"Count: {_queue.Count}");
        _queue.Dequeue();
        if (_touch.phase != TouchPhase.Began)
        {
            _queue.Enqueue(deltaPosition);
        }
        else
        {
            _queue.Enqueue(Vector2.zero);
        }

        foreach (Vector2 _vector in _queue)
        {
            _temporalMagnitude += _vector.magnitude;
        }

        //Debug.Log($"{_temporalMagnitude} before");
        _temporalMagnitude /= 4.0f;

        if (_temporalMagnitude >= Screen.width * 0.05f)
        {
            InputEvents.current.SlideEvent(deltaPosition);
            /*
            foreach (Vector2 _vector in _queue)
            {
                Debug.LogWarning($"Vector.magnitude {counter} in queue: {_vector.magnitude} , vector: {_vector}");
            }
            *//*
            counter++;
        }
        TemporalMagnitude = _temporalMagnitude;
        _temporalMagnitude = 0;
    }*/
    // Slide Mechanics end
}