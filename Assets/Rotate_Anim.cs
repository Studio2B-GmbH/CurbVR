using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Anim : MonoBehaviour
{
    [SerializeField]
    float rotationTime;

    enum billBoardAxis { X, Y, Z};

    [SerializeField]
    billBoardAxis axisLimitations = billBoardAxis.Y;

    public void RotateOverTime(float angle)
    {
        StartCoroutine(RotationAnim(angle));
    }

    public void RotateInstantly(float angle)
    {
        Vector3 dirAxis = Vector3.zero;

        switch (axisLimitations)
        {
            case billBoardAxis.X:
                dirAxis = transform.right;
                break;
            case billBoardAxis.Y:
                dirAxis = transform.up;
                break;
            case billBoardAxis.Z:
                dirAxis = transform.forward;
                break;
            default:
                break;
        }

        transform.Rotate(dirAxis, angle / rotationTime);
    }

    IEnumerator RotationAnim(float angle)
    {
        Vector3 dirAxis = Vector3.zero;

        switch (axisLimitations)
        {
            case billBoardAxis.X:
                dirAxis = transform.right;
                break;
            case billBoardAxis.Y:
                dirAxis = transform.up;
                break;
            case billBoardAxis.Z:
                dirAxis = transform.forward;
                break;
            default:
                break;
        }


        for (float i = 0; i < rotationTime; i+= Time.deltaTime)
        {
            transform.Rotate(dirAxis, (angle / rotationTime) * Time.deltaTime );
            yield return null;
        }
    }
}
