using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Billboards the object to the HMD Head, optionally restricted to a single plane
/// </summary>

public class Sprite_Billboard : MonoBehaviour
{
    enum billBoardAxis { X, Y, Z, XYZ };

    [SerializeField]
    billBoardAxis axisLimitations = billBoardAxis.Y;

    [SerializeField]
    Vector3 rotationOffset;

    Transform lookAt;

    Plane plane;

    private void Start()
    {
        Vector3 planeNormal = Vector3.zero;

        switch (axisLimitations)
        {
            case billBoardAxis.X:
                planeNormal = transform.right.normalized;
                break;
            case billBoardAxis.Y:
                planeNormal = transform.up.normalized;
                break;
            case billBoardAxis.Z:
                planeNormal = transform.forward.normalized;
                break;
            default:
                break;
        }

        plane = new Plane(planeNormal, transform.position);

        lookAt = DeviceManager.Instance.GetHead();
    }

    // Update is called once per frame
    void Update()
    {
        if(axisLimitations == billBoardAxis.XYZ)
        {
            transform.LookAt(lookAt);
        }

        else
        {
            //We project the position of the HMD onto a plane, which is orthogonal to the choosen axis. This effectivly gives us a 2D point which is locked to a local Axis of this object.        
            Vector3 pointOnPlane = plane.ClosestPointOnPlane(lookAt.transform.position);

            //Because we input a 2D Position, the rotation is restricted to a single axis
            transform.LookAt(pointOnPlane, plane.normal);
        }

        

        transform.rotation *= Quaternion.Euler(rotationOffset);
    }

}
