using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

/// <summary>
/// This class just forwards Settings for the outliner to the main Camera
/// This is needed to stay camera-indipendent, if we want to switch between
/// different VR-SDKs for example.
/// </summary>
public class Outliner_Settings : MonoBehaviour
{
    [SerializeField]
    Color OutlineColor1;

    [SerializeField]
    Color OutlineColor2;

    [SerializeField]
    Color OutlineColor3;

    [SerializeField]
    [Range(1.0f, 6.0f)]
    float lineThickness = 1.25f;

    [SerializeField]
    [Range(0, 10)]
    float lineIntensity = .5f;

    [SerializeField]
    [Range(0, 1)]
    float fillAmount = 0.2f;


    private void Start()
    {
        GameObject mainCamera = DeviceManager.Instance.GetHead().gameObject;

        mainCamera.AddComponent<OutlineEffect>();

        OutlineEffect outlineEffect = mainCamera.GetComponent<OutlineEffect>();

        outlineEffect.lineColor0 = OutlineColor1;
        outlineEffect.lineColor1 = OutlineColor2;
        outlineEffect.lineColor2 = OutlineColor3;

        outlineEffect.lineThickness = lineThickness;
        outlineEffect.lineIntensity = lineIntensity;
        outlineEffect.fillAmount = fillAmount;
    }
}
