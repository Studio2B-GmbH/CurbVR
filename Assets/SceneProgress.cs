using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneProgress : MonoBehaviour
{
    public bool startSceneActivated;

    public bool skalenSpielSceneActivated;

    public bool fireSceneActivated;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");

        if (scene.name == "Skalenspiel")
        {
            skalenSpielSceneActivated = true;
            startSceneActivated = true;
        }             

        if (scene.name == "FireScene")
        {
            startSceneActivated = true;
            fireSceneActivated = true;
        }
    }

    private void OnEnable()
    {
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
