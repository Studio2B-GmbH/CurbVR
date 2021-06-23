using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Highlight : MonoBehaviour
{
    public bool highlightActivated;

    [SerializeField]
    bool highlightChilds = true;

    [SerializeField]
    float highlightBrightness = 0.2f;

    [SerializeField]
    float highlightTime = 0.5f;

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
        if (hit.transform.gameObject == this.gameObject && highlightActivated)
        {
            ChangeBrightness(1);
        }
    }

    void PointerExit(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            ChangeBrightness(0);
        }
    }

    public void Highlight()
    {
        ChangeBrightness(1);
    }

    public void DeHighlight()
    {
        ChangeBrightness(0);
    }

    void ChangeBrightness(float brightness)
    {
        int indexer = 0;

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            for (int j = 0; j < meshRenderers[i].materials.Length; j++)
            {
                if (meshRenderers[i].materials[j].HasProperty("_Color"))
                {
                    meshRenderers[i].materials[j].color = Color.Lerp(originalColors[indexer], highlightColors[indexer], brightness);
                    indexer++;
                }
            }
        }
    }

    public void Blink(int repetitions)
    {
        StartCoroutine(BlinkAnim(repetitions));
    }

    IEnumerator BlinkAnim(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            for (float t = 0; t < highlightTime; t += Time.deltaTime)
            {
                ChangeBrightness(highlightTime / t);
                yield return null;
            }

            for (float t = 0; t < highlightTime; t += Time.deltaTime)
            {
                ChangeBrightness(1 - (highlightTime / t));
                yield return null;
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
