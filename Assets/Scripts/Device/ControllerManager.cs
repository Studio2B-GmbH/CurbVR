using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerManager : MonoBehaviour
{
    public static ControllerManager Instance { get; set; }

    [SerializeField]
    ControllerActivator oculusControllerLeft;

    [SerializeField]
    ControllerActivator oculusControllerRight;

    GameObject activeController;
    GameObject inactiveController;

    bool rightSideActivated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest || DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCRift)
        {
            rightSideActivated = true;
            oculusControllerLeft.Deactivate();
            oculusControllerRight.Activate();
            activeController = oculusControllerRight.gameObject;
        }

        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCVRDisabled)
        {
            activeController = oculusControllerRight.gameObject;
            inactiveController = oculusControllerLeft.gameObject;
            oculusControllerLeft.Deactivate();
            oculusControllerRight.Activate();
        }

        Debug.Log("Active Controller: " + activeController.name + ", Parent: " + activeController.transform.parent.name);
    }

    private void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
        {
            oculusControllerLeft.Show();
        }

        else
        {
            oculusControllerLeft.Hide();
        }

        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            oculusControllerRight.Show();
        }

        else
        {
            oculusControllerRight.Hide();
        }

        //Left Controller on Quest activated
        if (DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest && OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.LTouch))
        {
            rightSideActivated = false;
            oculusControllerLeft.Activate();
            oculusControllerRight.Deactivate();
            activeController = oculusControllerLeft.gameObject;
            inactiveController = oculusControllerRight.gameObject;

        }

        //Right Controller on Quest activated
        if (DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest && OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.RTouch))
        {
            rightSideActivated = true;
            oculusControllerLeft.Deactivate();
            oculusControllerRight.Activate();
            activeController = oculusControllerRight.gameObject;
            inactiveController = oculusControllerLeft.gameObject;
        }

        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCVRDisabled)
        {
            oculusControllerRight.transform.position = DeviceManager.Instance.GetHead().position;
            oculusControllerRight.transform.rotation = DeviceManager.Instance.GetHead().rotation;
        }
    }

    public GameObject GetActiveController()
    {
        return activeController;
    }

    public GameObject GetInactiveController()
    {
        return inactiveController;
    }

    public bool RightSideActivated()
    {
        return rightSideActivated;
    }

}
