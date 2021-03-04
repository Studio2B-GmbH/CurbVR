///
/// Scales a clamped texture up or down, while maintaining the correct centered position
///

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale_Clamped_Texture : MonoBehaviour
{
    [SerializeField]
    float scaleTime;

    [SerializeField]
    float scaleStart = 1;

    [SerializeField]
    float scaleEnd = 0;

    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();

        if (rend == null)
        {
            Debug.LogError("No Mesh Renderer Found on Object: " + gameObject.name);
        }

        ResetScaleToStart();
    }

    public void ScaleToEnd()
    {
        StartCoroutine(scaleAnim(scaleEnd));
    }

    public void ScaleToStart()
    {
        StartCoroutine(scaleAnim(scaleStart));
    }

    public void StopScale()
    {
        StopAllCoroutines();
    }

    public void ResetScaleToEnd()
    {
        float lerpOffset = (scaleEnd - 1) * -0.5f;
        rend.material.SetTextureScale("_MainTex", new Vector2(scaleEnd, scaleEnd));
        rend.material.SetTextureOffset("_MainTex", new Vector2(lerpOffset, lerpOffset));
        rend.enabled = false;
    }

    public void ResetScaleToStart()
    {
        float lerpOffset = (scaleStart - 1) * -0.5f;
        rend.material.SetTextureScale("_MainTex", new Vector2(scaleStart, scaleStart));
        rend.material.SetTextureOffset("_MainTex", new Vector2(lerpOffset, lerpOffset));
        rend.enabled = false;
    }

    private void ResetScale(float scale)
    {
        rend.material.SetTextureScale("_MainTex", new Vector2(scale, scale));
        rend.material.SetTextureOffset("_MainTex", new Vector2((scale - 1) * -0.5f, (scale - 1) * -0.5f));

        if (Mathf.Approximately(scale, scaleStart))
        {
            rend.enabled = false;
        }

        else
        {
            rend.enabled = false;
        }
    }

    IEnumerator scaleAnim(float target)
    {
        if (Mathf.Approximately(target, scaleEnd))
        {
            rend.enabled = true;
        }

        float currentScale = rend.material.GetTextureScale("_MainTex").x;

        for (float i = 0; i < scaleTime; i+=Time.deltaTime)
        {
            float lerpScale = Mathf.SmoothStep(currentScale, target, i / scaleTime);
            float lerpOffset = (lerpScale - 1) * -0.5f;

            rend.material.SetTextureScale("_MainTex", new Vector2(lerpScale, lerpScale));
            rend.material.SetTextureOffset("_MainTex", new Vector2(lerpOffset, lerpOffset));

            yield return null;
        }

        if(Mathf.Approximately(target, scaleStart))
        {
            rend.enabled = false;
        }

        

    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += ResetScaleToStart;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= ResetScaleToStart;
    }
}
