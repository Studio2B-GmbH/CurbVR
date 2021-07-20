using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStartingPoint : MonoBehaviour
{
    [SerializeField]
    EventTimer firstStartEventTimer;

    [SerializeField]
    EventTimer otherStartEventTimer;

    string persistentObjectName = "PersistentMainMenuObject";

    //We check with a persistent game object if the menu has been called before
    void Start()
    {
        if(GameObject.Find(persistentObjectName) == null)
        {
            GameObject persistentObject = new GameObject();
            persistentObject = Instantiate(persistentObject);
            persistentObject.name = persistentObjectName;
            DontDestroyOnLoad(persistentObject);

            firstStartEventTimer.StartTimer();
        }

        else
        {
            otherStartEventTimer.StartTimer();
        }
    }
}
