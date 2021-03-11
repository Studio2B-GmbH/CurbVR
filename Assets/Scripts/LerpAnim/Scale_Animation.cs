using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Animation : MonoBehaviour
{
    [SerializeField]
    float animTime;

    [SerializeField]
    float startSize;

    [SerializeField]
    float delay;

    float originalSize;

    private void Start()
    {
        originalSize = transform.localScale.x;
        ScaleInstantly(startSize);
    }

    private void Reset()
    {
        ScaleInstantly(startSize);
    }

    public void ScaleOverTime(float amount)
    {
        StartCoroutine(ScaleAnim(amount));
    }

    public void ScaleOverTimeToOriginalSize()
    {
        StartCoroutine(ScaleAnim(originalSize));
    }

    public void ScaleInstantly(float amount)
    {
        StopAllCoroutines();

        transform.localScale = new Vector3(amount, amount, amount);
    }

    IEnumerator ScaleAnim(float scaleAmount)
    {
        yield return new WaitForSeconds(delay);

        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(scaleAmount, scaleAmount, scaleAmount);

        for (float i = 0; i < animTime; i+=Time.deltaTime)
        {
            float dampedLerp = Mathf.SmoothStep(0, 1, i / animTime);

            transform.localScale = Vector3.Lerp(startScale, targetScale, dampedLerp);

            yield return null;
        }
    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += Reset;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= Reset;
    }
}
