using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerSwitcher : MonoBehaviour
{
    [SerializeField]
    ControllerActivator questControllerLeft;

    [SerializeField]
    ControllerActivator questControllerRight;

    GameObject activeController;

    bool rightSideActivated;

    // Start is called before the first frame update
    void Awake()
    {
        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest || DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCRift)
        {
            rightSideActivated = true;
            questControllerLeft.Deactivate();
            questControllerRight.Activate();
            activeController = questControllerRight.gameObject;
        }

        else if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCVRDisabled)
        {
            activeController = questControllerRight.gameObject;
            questControllerLeft.Deactivate();
            questControllerRight.Activate();
        }

        Debug.Log("Active Controller: " + activeController.name + ", Parent: " + activeController.transform.parent.name);
    }

    private void Update()
    {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
        {
            questControllerLeft.Show();
        }

        else
        {
            questControllerLeft.Hide();
        }

        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            questControllerRight.Show();
        }

        else
        {
            questControllerRight.Hide();
        }

        //Left Controller on Quest activated
        if (DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest && OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.LTouch))
        {
            rightSideActivated = false;
            questControllerLeft.Activate();
            questControllerRight.Deactivate();
            activeController = questControllerLeft.gameObject;

        }

        //Right Controller on Quest activated
        if (DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.Quest && OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.RTouch))
        {
            rightSideActivated = true;
            questControllerLeft.Deactivate();
            questControllerRight.Activate();
            activeController = questControllerRight.gameObject;
        }

        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCVRDisabled)
        {
            questControllerRight.transform.position = DeviceManager.Instance.GetHead().position;
            questControllerRight.transform.rotation = DeviceManager.Instance.GetHead().rotation;
        }
    }

    public GameObject GetController()
    {
        return activeController;
    }

    public bool RightSideActivated()
    {
        return rightSideActivated;
    }

}
