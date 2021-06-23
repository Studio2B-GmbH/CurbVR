using UnityEngine;
using UnityEditor;

//This script is used to estimated the time it will approximately take to show all orbs 
//and show it directly in the editor

[CustomEditor(typeof(Orbs_Appear_Randomly))]
public class Orb_Appear_Time_Estimation : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Orbs_Appear_Randomly orbsAppear = (Orbs_Appear_Randomly)target;

        int orbs = orbsAppear.transform.childCount;

        AnimationCurve animationCurve = orbsAppear.appearTime;

        float animationTime = 0;

        for (int i = 0; i < orbs; i++)
        {
            animationTime += animationCurve.Evaluate((float)i / orbs);
        }

        if(orbs > 0)
        {
            animationTime += orbsAppear.transform.GetChild(0).GetComponent<Scale_Animation>().animTime;
        }


        GUILayout.Label("Appear Animation will take " + Mathf.Round(animationTime) + " seconds");
    }
}

