/*******************************************************************************
 * Copyright (c) 2019 Christopher Remde Digital Design
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField]
    bool useDeviceIndependentHMD;

    [SerializeField]
    Transform transformToCopy;

    [SerializeField]
    bool alwaysActive = true;

    [SerializeField]
    bool copyTranslation = true;

    [SerializeField]
    bool copyRotation = true;

    [SerializeField]
    bool copyScale;

    [SerializeField]
    Vector3 translationOffset;

    [SerializeField]
    bool copyWhenOutsideOfBounds;

    [SerializeField]
    float boundSize;

    [SerializeField]
    bool boundMovesWithObject;

    Vector3 boundCenter;


    private void Start()
    {
        if (useDeviceIndependentHMD)
        {
            transformToCopy = DeviceManager.Instance.GetHead();
        }

        boundCenter = transformToCopy.position;
        copyTransform(transformToCopy.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (!alwaysActive) { return; }

        if (copyWhenOutsideOfBounds)
        {
            copyTransformOutsideBounds();
            //copyTransform(translationOffset);
        }

        else
        {
            copyTransform(transformToCopy.position);
        }
    }

    public void copyTransformOutsideBounds()
    {
        Vector3 dir = transformToCopy.position - boundCenter;        
        float distanceFromBoundCenter = dir.magnitude;

        if (distanceFromBoundCenter > boundSize)
        {        
            if (boundMovesWithObject)
            {
                boundCenter += ((dir / distanceFromBoundCenter) * (distanceFromBoundCenter - boundSize));
                copyTransform(boundCenter);
            }
        }
    }

    void copyTransform(Vector3 position)
    {
        if (copyTranslation)
        {
            transform.position  = position + translationOffset;
        }

        if (copyRotation)
        {
            transform.rotation = transformToCopy.rotation;
        }

        if (copyScale)
        {
            transform.localScale = transformToCopy.localScale;
        }
    }

    public void copyTransform()
    {
        copyTransform(transformToCopy.position);
    }
}
