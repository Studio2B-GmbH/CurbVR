using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Animation : MonoBehaviour
{
    [SerializeField]
    public float animTime;

    [SerializeField]
    float startSize;

    [SerializeField]
    float delay;

    [SerializeField]
    bool playSpawnOrbAnimation;

    [SerializeField]
    GameObject spawnOrbPrefab;

    float originalSize;

    GameObject spawnOrbObject;

    private void Start()
    {
        originalSize = transform.localScale.x;
        ScaleInstantly(startSize);
    }

    private void ResetScale()
    {
        ScaleInstantly(startSize);
    }

    public void ScaleOverTime(float amount)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ScaleAnim(amount));
        }
    }

    public void ScaleOverTimeToOriginalSize()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ScaleAnim(originalSize));
        }

        if (playSpawnOrbAnimation)
        {
            spawnOrbObject = Instantiate(spawnOrbPrefab, transform.position, Quaternion.identity);
            spawnOrbObject.transform.localScale = new Vector3(originalSize, originalSize, originalSize);
        }
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

        if(spawnOrbObject != null)
        {
            Destroy(spawnOrbObject);
        }
    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += ResetScale;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= ResetScale;
    }
}
