using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnim : MonoBehaviour
{
    [SerializeField]
    float animTime;

    [SerializeField]
    float startIntensity;

    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        Reset();
    }

    public void Reset()
    {
        StopAllCoroutines();
        SetItensityInstantly(startIntensity);
    }

    public void SetIntensityOverTime(float targetIntensity)
    {
        StartCoroutine(IntensityAnim(targetIntensity));
    }

    public void SetItensityInstantly(float targetIntensity)
    {
        StopAllCoroutines();

        light.intensity = targetIntensity;
    }

    IEnumerator IntensityAnim(float targetIntensity)
    {
        float currentIntensity = light.intensity;

        for (float i = 0; i < animTime; i += Time.deltaTime)
        {
            light.intensity = Mathf.SmoothStep(currentIntensity, targetIntensity, i / animTime);

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
