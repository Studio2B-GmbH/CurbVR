using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_Pulse : MonoBehaviour
{
    [SerializeField]
    float pulseTime;

    [SerializeField]
    string ShaderColorProperty;

    [SerializeField]
    Color pulseLowColor;

    [SerializeField]
    Color pulseHighColor;

    MeshRenderer rend;

    bool pulseEnabled;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        SetColor(false);
    }

    public void StartPulse()
    {
        if (rend.material.HasProperty(ShaderColorProperty))
        {
            StartCoroutine(Pulse());
            pulseEnabled = true;
        }
    }

    public void StopPulse()
    {
        pulseEnabled = false;
    }

    /// <summary>
    /// If set to true, sets the color instantly to the pulseHighColor, if false set it to pulseLowColor
    /// </summary>
    /// <param name="high"></param>
    public void SetColor(bool high)
    {
        StopAllCoroutines();

        if (rend.material.HasProperty(ShaderColorProperty))
        {
            rend.material.SetColor(ShaderColorProperty, high ? pulseHighColor : pulseLowColor);
        }
    }

    IEnumerator Pulse()
    {
        for (float i = 0; i < pulseTime; i += Time.deltaTime)
        {
            rend.material.SetColor(ShaderColorProperty, Color.Lerp(pulseLowColor, pulseHighColor, i / pulseTime));
            yield return null;
        }

        for (float i = 0; i < pulseTime; i += Time.deltaTime)
        {
            rend.material.SetColor(ShaderColorProperty, Color.Lerp(pulseHighColor, pulseLowColor, i / pulseTime));
            yield return null;
        }

        rend.material.SetColor(ShaderColorProperty, pulseLowColor);

        if (pulseEnabled)
            StartPulse();
    }
        

   
}
