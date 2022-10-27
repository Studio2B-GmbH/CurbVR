using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStartingPoint : MonoBehaviour
{
    [SerializeField]
    EventTimer firstStartEventTimer;

    [SerializeField]
    EventTimer concurrentStartsEventTimer;

    [SerializeField]
    GameObject co2Bubble;

    //We check with a persistent game object if the menu has been called before
    void Start()
    {
        co2Bubble.SetActive(false);

        GameObject sceneProgressGO = GameObject.Find("SceneProgressTracker");

        if (sceneProgressGO != null)
        {
            SceneProgress sceneProgress = sceneProgressGO.GetComponent<SceneProgress>();

            if (!sceneProgress.startSceneActivated)
            {
                firstStartEventTimer.StartTimer();
            }

            else
            {
                concurrentStartsEventTimer.StartTimer();
            }

            if (sceneProgress.skalenSpielSceneActivated)
            {
                co2Bubble.SetActive(true);
            }

        }

        else
        {
            concurrentStartsEventTimer.StartTimer();
        }
    }
}
