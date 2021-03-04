using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEmulation : MonoBehaviour
{
    Transform headTransform;

    bool emulationEnabled;


    // Start is called before the first frame update
    void Start()
    {
        if(DeviceManager.Instance.GetPlatform() == DeviceManager.Platform.PCVRDisabled)
        {
            emulationEnabled = true;
            headTransform = DeviceManager.Instance.GetHead();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (emulationEnabled)
        {
            transform.position = headTransform.position;
            transform.rotation = headTransform.rotation;
        }
    }
}
