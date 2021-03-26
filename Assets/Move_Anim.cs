using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Anim : MonoBehaviour
{
    [SerializeField]
    Vector3[] targetPositions;

    [SerializeField]
    float animTime;

    int counter;

    public void MoveToNextPos()
    {
        StartCoroutine(Move(targetPositions[counter]));
        counter++;
    } 

    IEnumerator Move(Vector3 pos)
    {
        Vector3 currentPos = transform.localPosition;

        for (float i = 0; i < animTime; i+= Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(currentPos, pos, i / animTime);
            yield return null;
        }

        transform.localPosition = pos;
    }
}
