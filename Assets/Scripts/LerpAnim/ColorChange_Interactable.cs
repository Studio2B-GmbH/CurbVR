using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Changes the object to multiple different colors
/// </summary>
public class ColorChange_Interactable : MonoBehaviour
{
    [SerializeField]
    Color defaultColor;

    [SerializeField]
    Color highlightColor;

    [SerializeField]
    Color selectColor;

    [SerializeField]
    MeshRenderer rend;

    bool isSelected;


    // Start is called before the first frame update
    void Start()
    {
        if(rend == null)
        {
            rend = GetComponent<MeshRenderer>();

            if(rend == null)
            {
                Debug.LogError("No meshrenderer found on " + gameObject.name);
                return;
            }
        }

        SwitchToDefaultColor();
    }

    public void SwitchToDefaultColor()
    {
        SwitchColor(defaultColor);
    }

    public void SwitchToHighlightColor()
    {
        SwitchColor(highlightColor);
    }

    public void SwitchToSelectColor()
    {
        SwitchColor(selectColor);
        isSelected = true;
    }

    public void ResetSelection()
    {
        isSelected = false;
        SwitchColor(defaultColor);
    }

    void SwitchColor(Color col)
    {
        if(!isSelected)
            rend.material.color = col;
    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += ResetSelection;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= ResetSelection;
    }
}
