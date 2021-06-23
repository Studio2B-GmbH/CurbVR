using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Wind_Bow : MonoBehaviour
{
    [SerializeField]
    Vector3 minOffset;

    [SerializeField]
    Vector3 maxOffset;

    [SerializeField]
    float minBurstTime;

    [SerializeField]
    float maxBurstTime;

    [SerializeField]
    float minCoolDownTime;

    [SerializeField]
    float maxCoolDownTime;

    [SerializeField]
    int verticeIndex1;

    [SerializeField]
    int verticeIndex2;

    MeshRenderer rend;

    Mesh mesh;

    Vector3[] vertices;

    Vector2[] uvs;

    Vector3 verticeIndex1Start;

    Vector3 verticeIndex2Start;


    void Start()
    {
        rend = GetComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        uvs = mesh.uv;

        verticeIndex1Start = vertices[verticeIndex1];
        verticeIndex2Start = vertices[verticeIndex2];

        StartBow();
    }

    void StartBow()
    {
        StartCoroutine(Bow());
    }

    IEnumerator Bow()
    {
        verticeIndex1Start = vertices[verticeIndex1];
        verticeIndex2Start = vertices[verticeIndex2];

        Vector3 offset = new Vector3(Random.Range(minOffset.x, maxOffset.x), Random.Range(minOffset.y, maxOffset.y), Random.Range(minOffset.y, maxOffset.y));

        float durationHigh = Random.Range(minBurstTime, maxBurstTime);
        float durationLow = Random.Range(minBurstTime, maxBurstTime);

        for (float i = 0; i < durationHigh; i += Time.deltaTime)
        {
            vertices[verticeIndex1] = Vector3.Lerp(verticeIndex1Start, verticeIndex1Start + offset, Mathf.SmoothStep(0, 1, i / durationHigh));
            vertices[verticeIndex2] = Vector3.Lerp(verticeIndex2Start, verticeIndex2Start + offset, Mathf.SmoothStep(0, 1, i / durationHigh));
            mesh.vertices = vertices;
            yield return null;
        }

        for (float i = 0; i < durationLow; i += Time.deltaTime)
        {
            vertices[verticeIndex1] = Vector3.Lerp(verticeIndex1Start + offset, verticeIndex1Start , Mathf.SmoothStep(0, 1, i / durationLow));
            vertices[verticeIndex2] = Vector3.Lerp(verticeIndex2Start + offset, verticeIndex2Start, Mathf.SmoothStep(0, 1, i / durationLow));
            mesh.vertices = vertices;
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(minCoolDownTime, maxCoolDownTime));

        StartBow();
    }
}
