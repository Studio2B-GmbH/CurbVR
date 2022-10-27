using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToVelocityCurve : MonoBehaviour
{
    [SerializeField]
    AnimationCurve velocityCurve;

    [SerializeField]
    float duration;

    public void SetDuration(float seconds)
    {
        duration = seconds;
    }

    public void MoveTo(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToAnimation(transform.position, target.position));
    }

    IEnumerator MoveToAnimation(Vector3 currentPos, Vector3 targetPos)
    {
        float distance = Vector3.Distance(currentPos, targetPos);

        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            float lerp = velocityCurve.Evaluate(i / duration);
            transform.position = Vector3.Lerp(currentPos, targetPos, lerp);
            yield return null;
        }
    }
}
