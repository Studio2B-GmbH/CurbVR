using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vignette_Fade : MonoBehaviour
{
    [SerializeField]
    float fadeTime;

    [SerializeField]
    float minFOV;

    [SerializeField]
    float maxFOV;

    OVRVignette vignette;

    private void Start()
    {
        vignette = DeviceManager.Instance.GetHead().GetComponent<OVRVignette>();
        vignette.VignetteFieldOfView = maxFOV;
    }

    public void FadeToFOV(float targetFOV)
    {
        StartCoroutine(FadeFOV(targetFOV));
    }

    IEnumerator FadeFOV(float targetFOV)
    {
        float oldFOV = vignette.VignetteFieldOfView;

        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            vignette.VignetteFieldOfView = Mathf.Clamp(Mathf.Lerp(oldFOV, targetFOV, i / fadeTime), minFOV, maxFOV);
            yield return null;
        }
    }
}
