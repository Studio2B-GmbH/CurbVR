using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering_Light : MonoBehaviour
{
    [SerializeField]
    float minBrightness;

    [SerializeField]
    float maxBrightness;

    [SerializeField]
    float minBurstTime;

    [SerializeField]
    float maxBurstTime;

    [SerializeField]
    float minCoolDownTime;

    [SerializeField]
    float maxCoolDownTime;

    [SerializeField]
    string shaderPropertyName;

    MeshRenderer rend;

    Color defaultBrightness;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        defaultBrightness = rend.material.GetColor(shaderPropertyName);

        StartFlickerBurst();
    }

    void StartFlickerBurst()
    {
        StartCoroutine(FlickerBurst());
    }

    IEnumerator FlickerBurst()
    {
        float brightness = Random.Range(minBrightness, maxBrightness);
        Color brightnessC = new Color(brightness, brightness, brightness);

        float durationHigh = Random.Range(minBurstTime, maxBurstTime);
        float durationLow = Random.Range(minBurstTime, maxBurstTime);

        for (float i = 0; i < durationHigh; i+= Time.deltaTime)
        {
            rend.material.SetColor(shaderPropertyName, Color.Lerp(defaultBrightness, brightnessC, i / durationHigh));
            yield return null;
        }

        for (float i = 0; i < durationLow; i += Time.deltaTime)
        {
            rend.material.SetColor(shaderPropertyName, Color.Lerp(brightnessC, defaultBrightness, i / durationLow));
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(minCoolDownTime, maxCoolDownTime));

        StartFlickerBurst();
    }

}
