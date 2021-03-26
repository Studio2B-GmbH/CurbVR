using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbs_Appear_Randomly : MonoBehaviour
{
    [SerializeField]
    AnimationCurve appearTime;

    Scale_Animation[] orbs;

    private void Start()
    {
        orbs = GetComponentsInChildren<Scale_Animation>();
    }

    public void OrbsAppear()
    {
        StartCoroutine(AppearAnimation());
    }

    IEnumerator AppearAnimation()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            orbs[i].ScaleOverTimeToOriginalSize();

            yield return new WaitForSeconds(appearTime.Evaluate((float)i / orbs.Length));
        }
    }
}
