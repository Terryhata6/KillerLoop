using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : BaseController, IExecute
{
    public InputController(MainController main) : base(main) { }
    private bool _countQueue = true;
    private Queue<Vector2> _queue = new Queue<Vector2>();
    private float _temporalMagnitude = 0;


    private Vector2 _mouseStartPosition;
    private Vector2 _mouseOldPosition;
    private Vector2 _mousePosition;
    

    /// <summary>
    /// 
    /// </summary>
    public float TemporalMagnitude = 0;
    private int counter = 0;
    private Touch _touch;

    public override void Initialize()
    {
        base.Initialize();


    }

    public void Execute()
    {
        if (!IsActive)
        {
            return;
        }
        if (!_main.UseMouse)
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);
                switch (_touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            InputEvents.current.TouchBeganEvent(_touch.position);
                            break;
                        }
                    case TouchPhase.Canceled:
                        {
                            InputEvents.current.TouchCancelledEvent();
                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            InputEvents.current.TouchMovedEvent(_touch.position);
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            InputEvents.current.TouchEndedEvent();
                            break;
                        }
                    case TouchPhase.Stationary:
                        {
                            InputEvents.current.TouchStationaryEvent();
                            break;
                        }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition =_mouseOldPosition = _mouseStartPosition = Input.mousePosition * .01f;             
                InputEvents.current.TouchBeganEvent(_mouseStartPosition);
            }
            if (Input.GetMouseButton(0))
            {
                _mousePosition = Input.mousePosition * .01f;
                if (_mousePosition == _mouseOldPosition)
                {
                    InputEvents.current.TouchStationaryEvent();
                }
                else
                {
                    _mousePosition = _mousePosition - _mouseOldPosition;
                    InputEvents.current.TouchMovedEvent(_mousePosition);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                InputEvents.current.TouchEndedEvent();
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