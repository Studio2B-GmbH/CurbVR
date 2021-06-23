using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceHMD : MonoBehaviour
{
    private Transform hmd;

    private void Start()
    {
        hmd = DeviceManager.Instance.GetHead();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2.0f * transform.position - hmd.position);
    }
}
