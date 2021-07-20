using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Fades objects with transparent materials or a text mesh pro object in and out. The user can specify which sprites to fade and when they should fade (relativ to an arbitrary Start point in time)
/// </summary>
/// 

[System.Serializable]
class FadeObject
{
    [SerializeField]
    public GameObject gO;

    [SerializeField]
    public float fadeStart;

    [SerializeField]
    public float fadeEnd;

    [HideInInspector]
    public bool activated;

    [HideInInspector]
    public MeshRenderer rend;
}

public class Material_Fader : MonoBehaviour
{
    [SerializeField]
    FadeObject[] objects;

    [SerializeField]
    float fadeTime;

    bool fadeEnabled;

    float counter;

    private void Start()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(objects[i].gO != null)
            {
                MeshRenderer render = objects[i].gO.GetComponentInChildren<MeshRenderer>();
                if (render != null)
                {
                    objects[i].rend = render;
                }
            }
           
        }

        ResetFade();
    }

    public void StartFade()
    {
        fadeEnabled = true;
    }

    public void PauseFade()
    {
        fadeEnabled = false;
    }

    public void ResetFade()
    {
        counter = 0;
        fadeEnabled = false;
        StopAllCoroutines();

        foreach (FadeObject fO in objects)
        {
            if(fO.rend != null)
            {
                Color col = fO.rend.material.color;
                fO.rend.material.color = new Color(col.r, col.g, col.b, 0);
            }
           
        }
    }

    private void Update()
    {
        if (fadeEnabled)
        {
            foreach (FadeObject fO in objects)
            {
                if(counter >= fO.fadeStart && !fO.activated)
                {
                    fO.activated = true;
                    StartCoroutine(fadeIn(fO));
                }

                if(counter >= fO.fadeEnd && fO.activated)
                {
                    fO.activated = false;
                    StartCoroutine(fadeOut(fO));
                }
            }

            counter += Time.deltaTime;
        }

        
    }

    IEnumerator fadeIn(FadeObject fObject)
    {
        if(fObject.rend != null)
        {
            Color startCol = fObject.rend.material.color;
            Color targetCol = new Color(startCol.r, startCol.g, startCol.b, 1);

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {
                fObject.rend.material.color = Color.Lerp(startCol, targetCol, i / fadeTime);
                yield return null;
            }

            fObject.rend.material.color = targetCol;
        }        
    }

    IEnumerator fadeOut(FadeObject fObject)
    {
        if (fObject.rend != null)
        {
            Color startCol = fObject.rend.material.color;
            Color targetCol = new Color(startCol.r, startCol.g, startCol.b, 0);

            for (float i = 0; i < fadeTime; i += Time.deltaTime)
            {
                fObject.rend.material.color = Color.Lerp(startCol, targetCol, i / fadeTime);
                yield return null;
            }

            fObject.rend.material.color = targetCol;
        }
    }
}
