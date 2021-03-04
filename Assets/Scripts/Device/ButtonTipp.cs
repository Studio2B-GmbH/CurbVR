using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ButtonTipp : MonoBehaviour
{
    [SerializeField]
    TextMeshPro tippText;

    [SerializeField]
    Renderer arrowRend;

    [SerializeField]
    float minTippDistance;

    [SerializeField]
    float maxTippDistance;

    [SerializeField]
    float lookAtMin = 0.8f;

    Transform hmd;

    // Start is called before the first frame update
    void Start()
    {
        tippText.alpha = 0;
        arrowRend.material.SetColor("_Color", new Color(1, 1, 1, 0));
        hmd = DeviceManager.Instance.GetHead();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 hmdToControllerDir = transform.parent.position - hmd.position;
        float dot = Vector3.Dot(hmdToControllerDir.normalized, hmd.forward.normalized);
        dot = Mathf.Clamp(map(dot, lookAtMin, 1, 0, 1), 0, 1);
        float distance = Vector3.Distance(hmd.position, transform.parent.position);
        float alphaValue = Mathf.Clamp(map(distance, minTippDistance, maxTippDistance, 0, 1), 0, 1) * dot;

        tippText.alpha = alphaValue;
        arrowRend.material.SetColor("_Color", new Color(1, 1, 1, alphaValue));

    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
