using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile_Orbs : MonoBehaviour
{
    [SerializeField]
    GameObject[] orbs;

    [SerializeField]
    AnimationCurve pileTimeCurve;

    [SerializeField]
    float pauseBetweenPile;

    [SerializeField]
    float stackHeightStart;

    [SerializeField]
    AnimationCurve stackHeightAnimCurve;

    public void StartPiling()
    {
        StartCoroutine(Pile(orbs));
    }

    IEnumerator Pile(GameObject[] orbs)
    {
        Vector3 endPos;
        float stackHeight = stackHeightStart;

        for (int i = 0; i < orbs.Length; i++)
        {
            float pileTime = pileTimeCurve.Evaluate((float)i / orbs.Length);

            float lastOrbHeight = 0;

            if((i - 1) >= 0)
            {
                lastOrbHeight = orbs[i - 1].transform.localScale.x;
            }

            print(stackHeight);
            print(orbs[i].transform.localScale.x / 2);
            print(lastOrbHeight / 2);

            stackHeight += ((orbs[i].transform.localScale.x / 2) + (lastOrbHeight / 2));

            print(stackHeight);

            Vector3 startPos = orbs[i].transform.position;
            endPos = new Vector3(transform.position.x, stackHeight, transform.position.z);

            for (float j = 0; j < pileTime; j+= Time.deltaTime)
            {
                Vector3 pos = Vector3.Lerp(startPos, endPos, j/pileTime);
                pos = new Vector3(pos.x, pos.y + stackHeightAnimCurve.Evaluate(j / pileTime), pos.z);
                orbs[i].transform.position = pos;
                yield return null;
            }

            orbs[i].transform.position = endPos;

        }
    }
}
