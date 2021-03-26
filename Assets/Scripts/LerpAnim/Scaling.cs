using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scaling : MonoBehaviour
{
    [SerializeField]
    float[] heights;

    [SerializeField]
    bool animationEnabled;

    [SerializeField]
    bool scaleInSteps;

    [SerializeField]
    float animationTime;

    [SerializeField]
    AnimationCurve scaleCurve;

    [SerializeField]
    UnityEvent OnScaleStart;

    [SerializeField]
    UnityEvent OnScaleEnd;

    int scaleCounter;

    bool animationLock;
    // Start is called before the first frame update
    void Start()
    {
        ScaleUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.All) || Input.GetKeyDown(KeyCode.S))
        {
            if (animationEnabled)
            {
                StartCoroutine(scaleUpAnimated());
            }

            else if (scaleInSteps)
            {
                StartCoroutine(scaleUpInSteps());
            }

            else
            {
                ScaleUp();
            }
        }
    }

    public void ScaleUp()
    {
        transform.position = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

        float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

        scaleCounter++;
    }

    public void ScaleUpAnimated()
    {
            StartCoroutine(scaleUpAnimated());
    }

    IEnumerator scaleUpAnimated()
    {
        if (animationLock)
            yield break;

        OnScaleStart.Invoke();

        animationLock = true;

        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

        Vector3 oldScale = transform.localScale;
        float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
        Vector3 newscale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);


        for (float i = 0; i < animationTime; i+= Time.deltaTime)
        {
            float y = scaleCurve.Evaluate(i / animationTime);

            transform.position = Vector3.Lerp(oldPos, newPos, y);
            transform.localScale = Vector3.Lerp(oldScale, newscale, y);
            yield return null;
        }

        scaleCounter++;

        animationLock = false;

        OnScaleEnd.Invoke();
    }

    IEnumerator scaleUpInSteps()
    {
        if (animationLock)
            yield break;

        OnScaleStart.Invoke();

        animationLock = true;

        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

        Vector3 oldScale = transform.localScale;
        float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
        Vector3 newscale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);


        for (float i = 0; i < animationTime; i += Time.deltaTime)
        {
            float y = scaleCurve.Evaluate(i / animationTime);

            transform.position = Vector3.Lerp(oldPos, newPos, y);
            transform.localScale = Vector3.Lerp(oldScale, newscale, y);
            yield return new WaitForSeconds(animationTime / 10f);
            i += animationTime / 10f;
        }

        scaleCounter++;

        animationLock = false;

        OnScaleEnd.Invoke();

    }
}
