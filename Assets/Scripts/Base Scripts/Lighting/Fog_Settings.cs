using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Settings : MonoBehaviour
{
    [SerializeField]
    float animTime;

    public void ChangeFogDistanceInstantly(float density)
    {
        RenderSettings.fogDensity = density;
    }

    public void ChangeFogDistanceOverTime(float density)
    {
        StartCoroutine(FogAnim(density));
    }

    IEnumerator FogAnim(float density)
    {
        float currentDensity = RenderSettings.fogDensity;

        for (float i = 0; i < animTime; i += Time.deltaTime)
        {
            RenderSettings.fogDensity = Mathf.Lerp(currentDensity, density, i / animTime);
            yield return null;
        }
    }


}
