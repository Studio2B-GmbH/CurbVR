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

    [HideInInspector]
    public bool activated;

    [SerializeField]
    public UnityEvent OnEventStart = new UnityEvent();

    [SerializeField]
    public WaitForCallBack WaitForCallback = new WaitForCallBack();

    public void StartEvent()
    {
        OnEventStart.Invoke();
        Debug.Log("EventStart");
    }
}
