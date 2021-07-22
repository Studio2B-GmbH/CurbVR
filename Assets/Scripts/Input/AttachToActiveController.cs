using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToActiveController : MonoBehaviour
{
    [SerializeField]
    ControllerManager controllerManager;

    [SerializeField]
    bool attachToInactiveController;

    private void Start()
    {
        controllerManager = ControllerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (attachToInactiveController)
        {
            if (controllerManager.GetActiveController().transform != transform.parent)
            {
                transform.parent = controllerManager.GetInactiveController().transform;
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.identity;
            }
        }

        else
        {
            if (controllerManager.GetInactiveController().transform != transform.parent)
            {
                transform.parent = controllerManager.GetActiveController().transform;
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.identity;
            }
        }

       

    }
}
