using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


[System.Serializable]
public class GameObjectList
{
    public List<GameObject> list;
}

[System.Serializable]
public class GameObjectListList
{
    public List<GameObjectList> list;
}

public class Scaling : MonoBehaviour
{
    [Header("Scaling")]

    [SerializeField]
    float[] heights;

    [SerializeField]
    float animationTime;

    [SerializeField]
    AnimationCurve scaleCurve;

    [SerializeField]
    bool activateHeightCounter;

    [SerializeField]
    TextMeshPro heightCounter;

    [SerializeField]
    UnityEvent OnScaleStart;

    [SerializeField]
    UnityEvent OnScaleEnd;

    [SerializeField]
    GameObjectListList scaleLODAppearingOnScaleStart;

    [SerializeField]
    GameObjectListList scaleLODImposters;


    int scaleCounter;

    bool animationLock;

    [Header("Vignette")]

    [SerializeField]
    float vignetteFadeTime;

    [SerializeField]
    float vignetteNormalFOV;

    [SerializeField]
    float vignetteFadeFOV;

    OVRVignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        vignette = DeviceManager.Instance.GetHead().GetComponent<OVRVignette>();
        vignette.VignetteFieldOfView = vignetteNormalFOV;

        //Some very basic LOD that disables object that are not assigned to the current level

        GameObjectList objectsToActivate = scaleLODAppearingOnScaleStart.list[scaleCounter];

        foreach (GameObject go in objectsToActivate.list)
        {
            go.SetActive(true);
        }

        for (int i = 0; i < scaleLODAppearingOnScaleStart.list.Count; i++)
        {
            if (i != scaleCounter)
            {
                foreach (GameObject go in scaleLODAppearingOnScaleStart.list[i].list)
                {
                    if (!objectsToActivate.list.Contains(go))
                        go.SetActive(false);
                }
            }
        }

        for (int i = 0; i < scaleLODImposters.list.Count; i++)
        {
            if (i != scaleCounter)
            {
                foreach (GameObject go in scaleLODImposters.list[i].list)
                {
                        go.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.All) || Input.GetKeyDown(KeyCode.S))
        {
            scaleCounter++;

            if (scaleCounter == heights.Length)
                scaleCounter = 0;
            StartCoroutine(ScaleAnimated(scaleCounter));
        }

        if (activateHeightCounter)
        {
            heightCounter.text = "Du bist jetzt " + Mathf.CeilToInt(transform.position.y) + " Meter groß";
        }
    }

    public void ScaleToStepAnimated(int step)
    {
        if(step != scaleCounter)
        {
            StartCoroutine(ScaleAnimated(step));
        }
    }

    public void ScaleUpAnimated()
    {
        scaleCounter++;
        StartCoroutine(ScaleAnimated(scaleCounter));
    }

    public void SetAnimationTime (float time)
    {
        animationTime = time;
    }

    IEnumerator ScaleAnimated(int step)
    {
        if (animationLock)
            yield break;

        animationLock = true;

        yield return FadeFOV(vignetteFadeFOV);

        //Activating LODs

        GameObjectList objectsToActivate = scaleLODAppearingOnScaleStart.list[step];

        foreach (GameObject go in objectsToActivate.list)
        {
            go.SetActive(true);
        }

        //Deactivating Imposters

        for (int i = 0; i < scaleLODImposters.list.Count; i++)
        {
            if (i != step)
            {
                foreach (GameObject go in scaleLODImposters.list[i].list)
                {
                    go.SetActive(false);
                }
            }
        }

        OnScaleStart.Invoke();

        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, heights[step], transform.position.z);

        Vector3 oldScale = transform.localScale;
        float scaleMultiplier = (1f / 1.8f) * heights[step];
        Vector3 newscale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);


        //Deactivating LODs
        for (float i = 0; i < animationTime; i += Time.deltaTime)
        {
            float y = scaleCurve.Evaluate(i / animationTime);

            transform.position = Vector3.Lerp(oldPos, newPos, y);
            transform.localScale = Vector3.Lerp(oldScale, newscale, y);
            yield return null;
        }

        scaleCounter = step;

        for (int i = 0; i < scaleLODAppearingOnScaleStart.list.Count; i++)
        {
            if(i != step)
            {
                foreach (GameObject go in scaleLODAppearingOnScaleStart.list[i].list)
                {
                    if(!objectsToActivate.list.Contains(go))
                        go.SetActive(false);
                }
            }            
        }

        //Activating Imposters
        for (int i = 0; i < scaleLODImposters.list[step].list.Count; i++)
        {
            scaleLODImposters.list[step].list[i].SetActive(true);
        }

        OnScaleEnd.Invoke();

        yield return FadeFOV(vignetteNormalFOV);

        animationLock = false;

    }

    IEnumerator FadeFOV(float targetFOV)
    {
        float oldFOV = vignette.VignetteFieldOfView;

        for (float i = 0; i < vignetteFadeTime; i += Time.deltaTime)
        {
            vignette.VignetteFieldOfView = Mathf.Lerp(oldFOV, targetFOV, i / vignetteFadeTime);
            yield return null;
        }
    }


}
