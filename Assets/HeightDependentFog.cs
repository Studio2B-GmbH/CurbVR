using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDependentFog : MonoBehaviour
{
    [SerializeField]
    float minHeight = 0;
    [SerializeField]
    float maxHeight = 50000;
    [SerializeField]
    float minFog = 0.00002f;
    [SerializeField]
    float maxFog = 0.00008f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity = Mathf.Clamp(Remap(this.transform.position.y, minHeight, maxHeight, maxFog, minFog), minFog, maxFog);
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
