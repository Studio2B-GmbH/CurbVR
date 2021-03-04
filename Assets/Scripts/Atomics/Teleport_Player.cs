using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport_Player : MonoBehaviour
{
    [SerializeField]
    Transform vrParent;

    public void Teleport(Transform newPosition)
    {
        vrParent.position = new Vector3(newPosition.position.x, newPosition.position.y, newPosition.position.z);
    }
}
