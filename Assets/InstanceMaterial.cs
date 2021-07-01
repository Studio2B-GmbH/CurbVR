using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMaterial : MonoBehaviour
{
    [SerializeField]
    Texture2D textureForInstance; 

    // Start is called before the first frame update
    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        Material newInstance = new Material(rend.material);
        rend.material = newInstance;
        rend.material.mainTexture = textureForInstance;
    }
}
