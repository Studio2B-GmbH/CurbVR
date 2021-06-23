using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCounter : MonoBehaviour
{
    [SerializeField]
    int eventFireCount;

    [SerializeField]
    UnityEvent OnPointerEnter = new UnityEvent();

    int counter;

    public void Count()
    {
        counter++;

        if(counter == eventFireCount)
        {
            OnPointerEnter.Invoke();
        }
    }

    public void ResetCounter()
    {
        counter = 0;
    }
}
