using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fade_Anim : MonoBehaviour
{
    [SerializeField]
    float fadeTime;

    [SerializeField]
    float startAlpha;

    [SerializeField]
    float delay;

    MeshRenderer[] rends;

    TextMeshPro[] tmps;

    private void Awake()
    {
        rends = GetComponentsInChildren<MeshRenderer>();
        tmps = GetComponentsInChildren<TextMeshPro>();
        FadeInstantly(startAlpha);
    }

    public void ResetFade()
    {
        StopAllCoroutines();
        FadeInstantly(startAlpha);
    }

    public void FadeOverTime(float amount)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(amount));
    }

    public void FadeInstantly(float amount)
    {
        StopAllCoroutines();

        foreach (MeshRenderer rend in rends)
        {
            if (rend.material.HasProperty("_Color"))
            {
                Color col = rend.material.GetColor("_Color");
                rend.material.SetColor("_Color", new Color(col.r, col.g, col.b, amount));

                if (amount < 0.1)
                    rend.enabled = false;

                else
                    rend.enabled = true;
            }            
        }

        foreach (TextMeshPro tmp in tmps)
        {
            tmp.alpha = amount;

            if (amount < 0.1)
                tmp.enabled = false;

            else
                tmp.enabled = true;
        }
    }

    IEnumerator Fade(float fadeAmount)
    {
        yield return new WaitForSeconds(delay);

        Color[] startCols = new Color[rends.Length];
        Color[] targetCols = new Color[rends.Length];
        float[] startTMPAlphas = new float[tmps.Length];

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i].material.HasProperty("_Color"))
            {
                startCols[i] = rends[i].material.GetColor("_Color");
                targetCols[i] = new Color(startCols[i].r, startCols[i].g, startCols[i].b, fadeAmount);
                rends[i].enabled = true;
            }
        }

        for (int i = 0; i < tmps.Length; i++)
        {
            startTMPAlphas[i] = tmps[i].alpha;
            tmps[i].enabled = true;
        }

        for (float i = 0; i < fadeTime; i += Time.deltaTime)
        {
            for (int j = 0; j < rends.Length; j++)
            {
                if (rends[j].material.HasProperty("_Color"))
                {
                    rends[j].material.SetColor("_Color", Color.Lerp(startCols[j], targetCols[j], i / fadeTime));
                }
            }

            for (int k = 0; k < tmps.Length; k++)
            {
                tmps[k].alpha = Mathf.Lerp(startTMPAlphas[k], fadeAmount, i / fadeTime);
            }

            yield return null;
        }

        for (int i = 0; i < rends.Length; i++)
        {
            if (rends[i].material.HasProperty("_Color"))
            {
                rends[i].material.SetColor("_Color", targetCols[i]);

                if (fadeAmount < 0.1)
                {
                    rends[i].enabled = false;
                }
            }
        }

        for (int i = 0; i < tmps.Length; i++)
        {
            tmps[i].alpha = fadeAmount;

            if (fadeAmount < 0.1)
            {
                tmps[i].enabled = false;
            }
        }

    }

    public float GetAnimTime()
    {
        return fadeTime;
    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += ResetFade;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= ResetFade;
    }

}
