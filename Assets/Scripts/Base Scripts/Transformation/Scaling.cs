using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Scaling : MonoBehaviour
{
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

    int scaleCounter;

    bool animationLock;
    // Start is called before the first frame update
    void Start()
    {
        //ScaleUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.All) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(scaleUpAnimated(scaleCounter));
        }

        if (activateHeightCounter)
        {
            heightCounter.text = "Du bist jetzt " + Mathf.CeilToInt(transform.position.y) + " Meter groß";
        }
    }

    //public void ScaleUp()
    //{
    //    transform.position = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

    //    float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
    //    transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

    //}

    public void ScaleToStepAnimated(int step)
    {
        if(step != scaleCounter)
        {
            StartCoroutine(scaleUpAnimated(step));
        }
    }

    public void ScaleUpAnimated()
    {
        scaleCounter++;
        StartCoroutine(scaleUpAnimated(scaleCounter));
    }

    public void SetAnimationTime (float time)
    {
        animationTime = time;
    }

    IEnumerator scaleUpAnimated(int step)
    {
        if (animationLock)
            yield break;

        animationLock = true;

        OnScaleStart.Invoke();


        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, heights[step], transform.position.z);

        Vector3 oldScale = transform.localScale;
        float scaleMultiplier = (1f / 1.8f) * heights[step];
        Vector3 newscale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);


        for (float i = 0; i < animationTime; i += Time.deltaTime)
        {
            float y = scaleCurve.Evaluate(i / animationTime);

            transform.position = Vector3.Lerp(oldPos, newPos, y);
            transform.localScale = Vector3.Lerp(oldScale, newscale, y);
            yield return null;
        }

        scaleCounter = step;

        OnScaleEnd.Invoke();

        animationLock = false;

    }
}
