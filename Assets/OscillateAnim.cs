using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateAnim : MonoBehaviour
{
    [SerializeField]
    Vector3 direction;

    [SerializeField]
    float pointADistance;

    [SerializeField]
    float pointBDistance;

    [SerializeField]
    float moveSpeed;

    Vector3 pointA;

    Vector3 pointB;

    float lerpDist;

    bool goToA = true;

    // Start is called before the first frame update
    void Start()
    {
        pointA = transform.position + (direction * pointADistance);
        pointB = transform.position + (direction * pointBDistance);
        float distanceA = Vector3.Distance(transform.position, pointA);
        float distanceB = Vector3.Distance(transform.position, pointB);
        float totalDistance = distanceA + distanceB;

        lerpDist = distanceA / totalDistance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(pointA, pointB, lerpDist);

        if (goToA)
            lerpDist += moveSpeed * Time.deltaTime;

        else
            lerpDist -= moveSpeed * Time.deltaTime;

        if (lerpDist > 1)
            goToA = false;

        if (lerpDist < 0)
            goToA = true;
            
    }

    private void OnDrawGizmos()
    {
        Vector3 gizmoA = transform.position + (direction * pointADistance);
        Vector3 gizmoB = transform.position + (direction * pointBDistance);

        Gizmos.DrawSphere(gizmoA, 1000f);
        Gizmos.DrawSphere(gizmoB, 1000f);
    }
}
