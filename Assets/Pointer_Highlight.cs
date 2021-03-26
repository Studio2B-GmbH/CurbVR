using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Highlight : MonoBehaviour
{
    [SerializeField]
    bool highlightChilds = true;

    [SerializeField]
    float highlightBrightness = 0.2f;

    List<MeshRenderer> meshRenderers;

    List<Color> originalColors = new List<Color>();
    List<Color> highlightColors = new List<Color>();

    private void Start()
    {
        if (highlightChilds)
        {
            meshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        }

        else
        {
            meshRenderers = new List<MeshRenderer>();
            meshRenderers.Add(GetComponent<MeshRenderer>());
        }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                if (meshRenderers[i].materials[j].HasProperty("_Color"))
                {
                    Color newColor = (meshRenderers[i].materials[j].color);
                    originalColors.Add(newColor);
                    highlightColors.Add(new Color(newColor.r + highlightBrightness, newColor.g + highlightBrightness, newColor.b + highlightBrightness));
                }              
                
            }
        }
    }

    void PointerEnter(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            ChangeColors(true);
        }
    }

    void PointerExit(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            ChangeColors(false);
        }
    }

    void ChangeColors(bool highlight)
    {
        int indexer = 0;

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                if (meshRenderers[i].materials[j].HasProperty("_Color"))
                {
                    meshRenderers[i].materials[j].color = highlight ? highlightColors[indexer] : originalColors[indexer];
                    indexer++;
                }
            }
        }
    }

    private void OnEnable()
    {
        Pointer_Controller.OnEnterGameObject += PointerEnter;
        Pointer_Controller.OnExitGameObject += PointerExit;
    }

    private void OnDisable()
    {
        Pointer_Controller.OnEnterGameObject -= PointerEnter;
        Pointer_Controller.OnExitGameObject -= PointerExit;
    }
}
