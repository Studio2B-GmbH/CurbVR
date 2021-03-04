/*******************************************************************************
 * Copyright (c) 2019 Christopher Remde Digital Design
******************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer_HitPoint : MonoBehaviour
{
    [SerializeField]
    GameObject hitPoint;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f))
        {
            hitPoint.SetActive(true);
            hitPoint.transform.position = hit.point;
            hitPoint.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
        }

        else
        {
            hitPoint.SetActive(false);
        }
    }
}
