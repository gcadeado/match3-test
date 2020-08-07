using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

//Classic GameEvent that takes any Event without arguments

public class GameEventListener : MonoBehaviour
{
    public List<GameEvent> events = new List<GameEvent>();
    public UnityEvent Response;

    private void OnEnable()
    {
        for (int i = events.Count - 1; i >= 0; i--)
        {
            events[i].RegisterListener(this);
        }

    }
    private void OnDisable()
    {
        for (int i = events.Count - 1; i >= 0; i--)
        {
            events[i].UnregisterListener(this);
        }
    }

    public virtual void OnEventRaised()
    {
        Response.Invoke();
    }
}