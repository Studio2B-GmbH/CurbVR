using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapTexture : MonoBehaviour
{
    [SerializeField]
    Texture newTexture;

    [SerializeField]
    string textureSlotInShader;

    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void Swap()
    {
        Swap(newTexture, textureSlotInShader);
    }

    public void Swap(Texture tex)
    {
        Swap(tex, textureSlotInShader);
    }

    public void Swap(Texture tex, string slot)
    {
        rend.material.SetTexture(slot, tex);
    }

    public void SwapMainTexture(Texture tex)
    {
        Swap(tex, "_MainTex");
    }

    public void SwapSecondaryTexture(Texture tex)
    {
        Swap(tex, "_SecondTex");
    }

    public void SwapDissolveTexture(Texture tex)
    {
        Swap(tex, "_Dissolve");

    }
}
