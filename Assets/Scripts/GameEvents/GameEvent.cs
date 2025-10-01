using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Events/Game Event")]
public class GameEvent : ScriptableObject
{
    public event Action OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
