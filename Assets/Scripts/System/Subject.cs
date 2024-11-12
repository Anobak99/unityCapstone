using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> _observers = new();

    public void AddObsrver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        for(int i = _observers.Count - 1; i >= 0; i--)
        {
            _observers[i].OnNotify();
        }
    }
}
