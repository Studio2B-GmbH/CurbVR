using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Send time-based events, easily customizable in the Editor
/// </summary>

public class EventTimer: MonoBehaviour
{
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

    bool timerPaused;

    private void Start()
    {
        timerCallback = StartTimer;

        foreach (TimeableObject tO in objects)
        {
            if (counter >= tO.eventStart && !tO.activated)
            {
                tO.activated = true;
            }

            if (counter >= tO.eventEnd && tO.eventEnd > 0.1 && tO.activated && !tO.ended)
            {
                tO.ended = true;
            }
        }
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
            tO.ended = false;
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
                    //is hooked up is done.
                    if(tO.WaitForCallback.GetPersistentEventCount() > 0)
                    {
                        PauseTimer();
                        tO.WaitForCallback.Invoke(timerCallback);
                    }
                }

                if(counter >= tO.eventEnd && tO.eventEnd > 0.1 && tO.activated && !tO.ended)
                {
                    tO.EndEvent();
                    tO.ended = true;
                }
            }

            counter += Time.deltaTime;
        }

    }

}
