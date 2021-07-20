using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsBeingLookedAt : MonoBehaviour
{
    [SerializeField]
    float angleTolerance;

    public UnityEvent lookedAtStartEvent;

    bool wasLookedAt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirVector = transform.position - DeviceManager.Instance.GetHead().position;
        Vector3 lookAtVector = DeviceManager.Instance.GetHead().forward;

        if(Vector3.Angle(dirVector, lookAtVector) < angleTolerance)
        {
            if (!wasLookedAt)
            {
                lookedAtStartEvent.Invoke();
                wasLookedAt = true;
            }
        }

        else
        {
            wasLookedAt = false;
        }

    }
}
