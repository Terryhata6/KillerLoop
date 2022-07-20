using System;

public interface ITouchEventsHandler
{
    public Action OnTouch { get; set; }
}