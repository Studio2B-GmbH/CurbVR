using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the Material of this GameObject when the Pointers hits this object
/// </summary>
public class PointerHighlightChangeMat : MonoBehaviour
{
    [SerializeField]
    Material HighlightMaterial;

    Material defaultMaterial;

    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        defaultMaterial = rend.material;
    }

    void Highlight(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            rend.material = HighlightMaterial;
        }
    }

    void DeHighlight(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            rend.material = defaultMaterial;
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
