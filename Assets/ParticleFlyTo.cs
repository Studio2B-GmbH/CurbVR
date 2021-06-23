using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFlyTo : MonoBehaviour
{
    [SerializeField]
    GameObject particlePrefab;

    [SerializeField]
    Transform flyTo;

    [SerializeField]
    float animTime;

    public void StartFlyTo()
    {
        StartCoroutine(FlyToAnimation());
    }


    IEnumerator FlyToAnimation()
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);

        for (float i = 0; i < animTime; i+=Time.deltaTime)
        {
            particle.transform.position = Vector3.Lerp(transform.position, flyTo.transform.position, i / animTime);
            yield return null;
        }

        Destroy(particle);
        
    }
}
