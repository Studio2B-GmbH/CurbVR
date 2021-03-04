using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{
    [SerializeField]
    float[] heights;

    [SerializeField]
    bool animationEnabled;

    [SerializeField]
    float animationTime;

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
        if (DeviceManager.Instance.GetBackButtonDown() || Input.GetKeyDown(KeyCode.S))
        {
            if (animationEnabled)
            {
                StartCoroutine(scaleUpAnimated());
            }

            else
            {
                ScaleUp();
            }
        }
    }

    void ScaleUp()
    {
        transform.position = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

        float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

        scaleCounter++;

        if (scaleCounter >= heights.Length)
            scaleCounter = 0;
    }

    IEnumerator scaleUpAnimated()
    {
        if (animationLock)
            yield break;

        animationLock = true;

        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(transform.position.x, heights[scaleCounter], transform.position.z);

        Vector3 oldScale = transform.localScale;
        float scaleMultiplier = (1f / 1.8f) * heights[scaleCounter];
        Vector3 newscale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);


        for (float i = 0; i < animationTime; i+= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(oldPos, newPos, i / animationTime);
            transform.localScale = Vector3.Lerp(oldScale, newscale, i / animationTime);
            yield return null;
        }

        scaleCounter++;

        if (scaleCounter >= heights.Length)
            scaleCounter = 0;

        animationLock = false;
    }
}
