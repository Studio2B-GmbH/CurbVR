using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTilingAnim : MonoBehaviour
{
    [SerializeField]
    float xOffsetSpeed, yOffsetSpeed;
    [SerializeField]
    float tileXMin, tileXMax;
    [SerializeField]
    float tileYMin, tileYMax;
    [SerializeField]
    float tileSpeed;

    Renderer renderer;
    float xOffset;
    float yOffset;
    float tileSin;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        xOffset += xOffsetSpeed * Time.deltaTime;
        yOffset += yOffsetSpeed * Time.deltaTime;

        tileSin = Mathf.Sin(Time.time * tileSpeed);
        tileSin = (tileSin + 1) / 2;

        float tileX = Mathf.Lerp(tileXMin, tileXMax, tileSin);
        float tileY = Mathf.Lerp(tileYMin, tileYMax, tileSin);

        renderer.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
        renderer.material.SetTextureScale("_MainTex", new Vector2(tileX, tileY));
    }
}
