using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject laserPointer;

    [SerializeField]
    private Renderer controllerRenderer;

    [SerializeField]
    private Material opaqueMaterial;

    [SerializeField]
    private Material transparentMaterial;

    [SerializeField]
    private GameObject[] tipps;

    public void Activate()
    {
        laserPointer.SetActive(true);
        controllerRenderer.material = opaqueMaterial;
        foreach (GameObject tipp in tipps)
        {
            tipp.SetActive(true);
        }
    }

    public void Deactivate()
    {
        laserPointer.SetActive(false);
        controllerRenderer.material = transparentMaterial;
        foreach (GameObject tipp in tipps)
        {
            tipp.SetActive(false);
        }
    }

    public void Hide()
    {
        if (controllerRenderer.enabled)
            controllerRenderer.enabled = false;
    }

    public void Show()
    {
        if (!controllerRenderer.enabled)
            controllerRenderer.enabled = true;
    }
}
