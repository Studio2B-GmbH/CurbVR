using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Send time-based events, easily customizable in the Editor
/// </summary>

public class EventTimer: MonoBehaviour
{
    //If the event timer should start itself on Monobehaviour Start
    [SerializeField]
    bool selfInit;

    [SerializeField]
    float startCounter = 0;

    [SerializeField]
    TimeableObject[] objects;

    //This delegate should be used when the timer needs to wait for a function to end
    //It is called at the same time as timeableObject.StartEvent()
    public delegate void ContinueTimerCallback();
    ContinueTimerCallback timerCallback;

    public delegate void ResetLerpAnimations();
    public static event ResetLerpAnimations OnResetLerpAnimations;

    float counter;

    bool timerEnabled;

    private void Start()
    {
        timerCallback = StartTimer;

        foreach (TimeableObject tO in objects)
        {
            if (counter > tO.eventStart && !tO.activated)
            {
                tO.activated = true;
            }
        }

        if (selfInit)
            StartTimer();
    }

    public void StartTimer()
    {       
        timerEnabled = true;
    }

    public void PauseTimer()
    {
        timerEnabled = false;
    }

    public void ResetTimer()
    {
        counter = startCounter;
        timerEnabled = false;

        foreach (TimeableObject tO in objects)
        {
            tO.activated = false;
        }
    }

    public void ResetAllLerpers()
    {
        OnResetLerpAnimations.Invoke();
    }

    private void Update()
    {
        if (timerEnabled)
        {
            foreach (TimeableObject tO in objects)
            {
                if(counter >= tO.eventStart && !tO.activated)
                {
                    tO.activated = true;
                    tO.StartEvent();

                    //If there is a listener on the callback function, we expect that the user
                    //intented to pause the event timer here and continue when the function that
                    //is hooked up has called StartTimer() on this script.
                    if(tO.WaitForCallback.GetPersistentEventCount() > 0)
                    {
                        PauseTimer();
                        tO.WaitForCallback.Invoke(timerCallback);
                    }
                }
            }

            counter += Time.deltaTime;
        }

    }

}
