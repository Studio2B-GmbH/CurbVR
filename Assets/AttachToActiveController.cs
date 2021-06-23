using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToActiveController : MonoBehaviour
{
    [SerializeField]
    ControllerManager controllerManager;

    private void Start()
    {
        controllerManager = ControllerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(controllerManager.GetController().transform != transform.parent)
        {
            transform.parent = controllerManager.GetController().transform;
        }
    }
}
