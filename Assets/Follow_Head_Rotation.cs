using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Head_Rotation : MonoBehaviour
{
    [SerializeField]
    bool activated;

    [SerializeField]
    float rotationDampSpeed;

    private void Update()
    {
        if (activated)
        {
            Quaternion targetRotation = Quaternion.Euler(0, DeviceManager.Instance.GetHead().eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationDampSpeed);
        }
    }

    public void SetActive(bool active)
    {
        activated = active;
    }
}
