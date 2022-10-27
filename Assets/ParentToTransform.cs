using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToTransform : MonoBehaviour
{
    // Start is called before the first frame update
    public void AttachToNewParent(Transform parent)
    {
        transform.parent = parent;
    }

    public void DetachFromParent()
    {
        transform.parent = null;
    }
}
