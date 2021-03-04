using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WaitForCallBack : UnityEvent<EventTimer.ContinueTimerCallback>
{
}

[System.Serializable]
class TimeableObject
{

    [SerializeField]
    public float eventStart;

    [SerializeField]
    public float eventEnd;

    [HideInInspector]
    public bool activated;

    [HideInInspector]
    public bool ended;

    [SerializeField]
    public UnityEvent OnEventStart = new UnityEvent();

    [SerializeField]
    public UnityEvent OnEventEnd = new UnityEvent();

    [SerializeField]
    public WaitForCallBack WaitForCallback = new WaitForCallBack();

    public void StartEvent()
    {
        OnEventStart.Invoke();
        Debug.Log("EventStart");
    }

    public void EndEvent()
    {
        OnEventEnd.Invoke();
    }
}
