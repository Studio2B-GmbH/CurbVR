using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract_Animation : MonoBehaviour
{
    [SerializeField]
    GameObject orbsParent;

    [SerializeField]
    float animTime;

    [SerializeField]
    AnimationCurve velocity;

    [SerializeField]
    GameObject orbToGrow;

    [SerializeField]
    AnimationCurve mainOrbScale;

    [SerializeField]
    AnimationCurve attractOrbsScaleMultiplicator;

    private void Start()
    {
        orbToGrow.transform.localScale = Vector3.zero;
    }

    public void StartAttractAnimation()
    {
        List<GameObjectDistance> goWithDistance = new List<GameObjectDistance>();

        foreach (Transform child in orbsParent.transform.GetChild(0))
        {
            foreach (Transform grandChild in child)
            {
                goWithDistance.Add(new GameObjectDistance(grandChild.gameObject, Vector3.Distance(transform.position, grandChild.position)));
            }
        }


        goWithDistance.Sort((x, y) => x.distance.CompareTo(y.distance));

        StartCoroutine(AttractAnimation(goWithDistance));
    }

    IEnumerator AttractAnimation(List<GameObjectDistance> gameObjectWithDistances)
    {

        for (float i = 0; i < animTime; i += Time.deltaTime)
        {
            for (int j = 0; j < gameObjectWithDistances.Count; j++)
            {
                Vector3 dirToCenter = transform.position - gameObjectWithDistances[j].go.transform.position;
                float normalizedDistance = Mathf.InverseLerp(0, 7000, Vector3.Distance(gameObjectWithDistances[j].go.transform.position, transform.position));
                gameObjectWithDistances[j].go.transform.position += (dirToCenter.normalized * velocity.Evaluate(i / animTime));
                float scale = gameObjectWithDistances[j].originalScale * attractOrbsScaleMultiplicator.Evaluate(normalizedDistance);

                if (j == 1)
                {
                    print(normalizedDistance);
                    print(scale);
                }

                gameObjectWithDistances[j].go.transform.localScale = new Vector3(scale, scale, scale);
            }

            float orbToGrowScale = mainOrbScale.Evaluate(i / animTime);
            orbToGrow.transform.localScale = new Vector3(orbToGrowScale, orbToGrowScale, orbToGrowScale);

            yield return null;

        }
    }



}

public struct GameObjectDistance
{
    public GameObject go { get; }
    public float distance { get; }

    public float originalScale;

    public GameObjectDistance(GameObject gameObject, float distanceTo)
    {
        go = gameObject;
        distance = distanceTo;
        originalScale = go.transform.localScale.x;
    }
}
