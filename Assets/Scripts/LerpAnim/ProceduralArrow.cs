using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProceduralArrow : MonoBehaviour
{
    [SerializeField]
    [Range(0f, -1f)]
    float arrowEndOffset;

    [SerializeField]
    float animTime = 2f;

    [SerializeField]
    float arrowStartLerp;

    [SerializeField]
    bool editEnabled = false;


    int offsetVertice1 = 1;

    int offsetVertice2 = 3;

    int offsetUVVert1 = 0;

    int offsetUVVert2 = 2;

    int offsetUVVert3 = 6;

    int offsetUVVert4 = 7;

    Mesh mesh;

    Vector3[] vertices;

    Vector3[] offsetVertices;

    Vector2[] uvs;

    Vector2[] offsetUVs;

    float tailSegmentOriginalLength;
    float arrowSegmentLengthUVSpace;

    MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        uvs = mesh.uv;

        offsetVertices = new Vector3[vertices.Length];
        offsetUVs = new Vector2[uvs.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            offsetVertices[i] = vertices[i];
            offsetUVs[i] = uvs[i];
        }

        tailSegmentOriginalLength = Mathf.Abs(vertices[1].y - vertices[0].y);
        arrowSegmentLengthUVSpace = Mathf.Abs(uvs[6].x - uvs[5].x);

        UpdateMesh();
        SetAnimationInstant(arrowStartLerp);
    }

    void Update()
    {
        if (editEnabled)
        {
            UpdateMesh();
        }
        
    }

    void UpdateMesh()
    {
        offsetVertices[offsetVertice1] = new Vector3(vertices[offsetVertice1].x, vertices[offsetVertice1].y + arrowEndOffset, vertices[offsetVertice1].z);
        offsetVertices[offsetVertice2] = new Vector3(vertices[offsetVertice2].x, vertices[offsetVertice2].y + arrowEndOffset, vertices[offsetVertice2].z);

        float tailSegmentCurrentLength = Mathf.Abs(offsetVertices[1].y - offsetVertices[0].y);
        float scaleFactor = tailSegmentCurrentLength / tailSegmentOriginalLength;
        float offSetForUV = Mathf.Abs((arrowSegmentLengthUVSpace / scaleFactor) - arrowSegmentLengthUVSpace);

        offsetUVs[offsetUVVert1] = new Vector2(uvs[offsetUVVert1].x + offSetForUV, uvs[offsetUVVert1].y);
        offsetUVs[offsetUVVert2] = new Vector2(uvs[offsetUVVert2].x + offSetForUV, uvs[offsetUVVert2].y);
        offsetUVs[offsetUVVert3] = new Vector2(uvs[offsetUVVert3].x + offSetForUV, uvs[offsetUVVert3].y);
        offsetUVs[offsetUVVert4] = new Vector2(uvs[offsetUVVert4].x + offSetForUV, uvs[offsetUVVert4].y);


        mesh.vertices = offsetVertices;
        mesh.uv = offsetUVs;
    }

    public void Reset()
    {
        StopAnimation();
        SetAnimationInstant(arrowStartLerp);
    }

    public void PlayAnimation(float target)
    {
        StopAnimation();
        StartCoroutine(ArrowAnim(target));
    }

    public void SetAnimationInstant(float target)
    {
        StopAnimation();
        rend.material.SetFloat("_Lerp", target);
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
    }

    IEnumerator ArrowAnim(float target)
    {
        float start = rend.material.GetFloat("_Lerp");

        for (float i = 0; i < animTime; i+= Time.deltaTime)
        {
            rend.material.SetFloat("_Lerp", Mathf.Lerp(start, target, i / animTime) );
            yield return null;
        }
    }

    private void OnEnable()
    {
        EventTimer.OnResetLerpAnimations += Reset;
    }

    private void OnDisable()
    {
        EventTimer.OnResetLerpAnimations -= Reset;
    }
}
