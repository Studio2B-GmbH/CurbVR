using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Outline_Highlight : MonoBehaviour
{
    Outline[] Outliners;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] renderer = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer rend in renderer)
        {
             rend.gameObject.AddComponent<Outline>();
        }

        Outliners = GetComponentsInChildren<Outline>();

        DeHighlight();
    }

    void Highlight(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            foreach (Outline outline in Outliners)
            {
                outline.enabled = true;
            }
        }
    }

    public void Highlight()
    {
        foreach (Outline outline in Outliners)
        {
            outline.enabled = true;
        }
    }

    void DeHighlight(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            foreach (Outline outline in Outliners)
            {
                outline.enabled = false;
            }
        }
    }

    public void DeHighlight()
    {
        foreach (Outline outline in Outliners)
        {
            outline.enabled = false;
        }
    }

    private void OnEnable()
    {
        Pointer_Controller.OnEnterGameObject += Highlight;
        Pointer_Controller.OnExitGameObject += DeHighlight;
    }

    private void OnDisable()
    {
        Pointer_Controller.OnEnterGameObject -= Highlight;
        Pointer_Controller.OnExitGameObject -= DeHighlight;
    }
}
