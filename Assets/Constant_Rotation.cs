using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant_Rotation : MonoBehaviour
{
    [SerializeField]
    float rotation;

    [SerializeField]
    Vector3 rotationAxis;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationAxis, rotation * Time.deltaTime);
    }
}
