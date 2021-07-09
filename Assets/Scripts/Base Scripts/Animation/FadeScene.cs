using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour
{
    public static FadeScene Instance { get; private set; }

    [SerializeField]
    bool fadeInOnSceneStart = true;
    public float defaultFadeSpeed = 1.5f;
    public UnityEngine.Events.UnityEvent OnFadedInComplete;

    GameObject fadeSphere;
    float deltaTimeForThisScript;
    bool isInFade;

    
    

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasChanged;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        fadeSphere = this.transform.gameObject;       

    }

    private void Start()
    {
        if (fadeInOnSceneStart)
        {
            Material fadeSphereMat = fadeSphere.GetComponent<Renderer>().material;
            fadeSphere.GetComponent<MeshRenderer>().enabled = true;
            fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, 1));
        }

    }

    private void Update()
    {
        deltaTimeForThisScript = Time.deltaTime;
    }

    private void SceneWasChanged(Scene scene, LoadSceneMode lsMode)
    {
        if (fadeInOnSceneStart)
        {
            fadeInGlobal();
        }
    }

    public void fadeInGlobal()
    {
        StartCoroutine(fadeGlobal(false, false, 0, "null"));
    }

    public void fadeOutGlobal()
    {
        StartCoroutine(fadeGlobal(true, false, 0, "null"));
    }

    public void loadSceneWithFade(string scene)
    {
        StartCoroutine(fadeGlobal(true, false, 0, scene));
    }

    IEnumerator fadeGlobal(bool fadeToBlack, bool fadeBackIn, float waitWhileBlack, string scene) //fadeToBlack: Should the scene fade to black, or from black to scene? fadeBackIn: Should the app first fade to black and then directly in reverse?
    {
        if(isInFade) { yield break; }

        isInFade = true;
        Material fadeSphereMat = fadeSphere.GetComponent<Renderer>().material;
        fadeSphere.GetComponent<MeshRenderer>().enabled = true;

        if (fadeToBlack)
        {
            fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, 0));

            for (float i = 0; i <= 1; i += defaultFadeSpeed * deltaTimeForThisScript)
            {
                fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, i));
                yield return null;
            }

            fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, 1));

            if (scene != "null")
            {
                print("Changing scene");
                SceneManager.LoadScene(scene);
                
            }

            if (fadeBackIn)
            {
                yield return new WaitForSeconds(waitWhileBlack);
                fadeToBlack = false;
            }

            else
            {
                isInFade = false;
                yield break;
            }
        }

        if (!fadeToBlack)
        {
            fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, 1));

            for (float i = 1; i >= 0; i -= defaultFadeSpeed * deltaTimeForThisScript)
            {
                fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, i));
                yield return null;
            }

            fadeSphereMat.SetColor("_Color", new Color(0, 0, 0, 0));

            fadeSphere.GetComponent<MeshRenderer>().enabled = false;

            OnFadedInComplete.Invoke();
        }

        isInFade = false;
    }
}
