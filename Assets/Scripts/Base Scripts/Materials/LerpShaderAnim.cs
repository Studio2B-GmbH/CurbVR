using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpShaderAnim : MonoBehaviour
{
    [SerializeField]
    float fadeTime;

    [SerializeField]
    float startLerp;

    [SerializeField]
    string slotName = "_Lerp";

    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        LerpInstantly(startLerp);
    }

    public void LerpOverTime(float target)
    {
        StartCoroutine(Lerp(target));
    }

    public void LerpInstantly(float amount)
    {
        StopAllCoroutines();

        if (rend.material.HasProperty(slotName))
        {
            rend.material.SetFloat(slotName, amount);
        }
    }

    IEnumerator Lerp(float targetLerp)
    {
        float startLerp = rend.material.HasProperty(slotName) ? rend.material.GetFloat(slotName) : 1;

        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            if (rend.material.HasProperty(slotName))
            {
                rend.material.SetFloat(slotName, Mathf.Lerp(startLerp, targetLerp, i / fadeTime));
            }

            yield return null;
        }

        if (rend.material.HasProperty(slotName))
        {
            rend.material.SetFloat(slotName, targetLerp);
        }

    }
}
