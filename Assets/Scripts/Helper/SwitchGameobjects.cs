using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGameobjects : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;

    int indexer;

    private void Start()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.A))
        {
            gameObjects[indexer]?.SetActive(true);
            print(gameObjects[indexer].name);

            if(indexer - 1 >= 0)
            {
                gameObjects[indexer - 1]?.SetActive(false);
            }

            else
            {
                gameObjects[gameObjects.Length - 1]?.SetActive(false);
            }

            indexer++;

            if (indexer == gameObjects.Length)
            {
                indexer = 0;
            }

        }
    }
}
