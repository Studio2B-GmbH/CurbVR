using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit_Animation : MonoBehaviour
{

    [SerializeField]
    GameObject orbiter;

    [SerializeField]
    float distanceToCenter;

    [SerializeField]
    float orbitSpeed;

    // Start is called before the first frame update
    void Start()
    {
        orbiter.transform.position = transform.position + transform.forward * distanceToCenter;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, orbitSpeed * Time.deltaTime);
    }
}
