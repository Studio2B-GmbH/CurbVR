using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow_Distance : MonoBehaviour
{
    public void ChangeShadowDistance(float distance)
    {
        QualitySettings.shadowDistance = distance;
    }
}
