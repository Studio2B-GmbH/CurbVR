using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    bool buttonEnabled = true;

    enum startState { active, inactive}

    [SerializeField]
    bool startHidden;

    [SerializeField]
    startState buttonStartState = startState.active;

    [SerializeField]
    Texture2D defaultTex;

    [SerializeField]
    Texture2D highlightTex;

    Collider col;

    Renderer rend;

    Fade_Anim fadeAnim;

    [SerializeField]
    UnityEvent OnStart = new UnityEvent();
    [SerializeField]
    PointerEvent OnPointerEnter = new PointerEvent();
    [SerializeField]
    PointerEvent OnPointerExit = new PointerEvent();
    [SerializeField]
    PointerEvent OnPointerStay = new PointerEvent();
    [SerializeField]
    PointerEvent OnPointerPressDown = new PointerEvent();
    [SerializeField]
    PointerEvent OnPointerStayPressed = new PointerEvent();
    [SerializeField]
    PointerEvent OnPointerPressUp = new PointerEvent();

    Vector3 originalSize;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        fadeAnim = GetComponent<Fade_Anim>();

        Material newInstance = new Material(rend.material);
        rend.material = newInstance;
        rend.material.mainTexture = defaultTex;

        switch (buttonStartState)
        {
            case startState.active:
                rend.material.color = new Color(1, 1, 1, 1f);
                break;
            case startState.inactive:
                buttonEnabled = false;
                rend.material.color = new Color(1, 1, 1, 0.5f);
                break;
            default:
                break;
        }

        if (startHidden)
        {
            fadeAnim.FadeInstantly(0);
            buttonEnabled = false;
            col.enabled = false;
        }
           

        else
            fadeAnim.FadeInstantly(1);

        originalSize = transform.localScale;

        OnStart.Invoke();

    }


    void PointerEnter(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerEnter.Invoke(hit);

                transform.localScale = originalSize * 1.1f;

                if (highlightTex != null)
                    rend.material.mainTexture = highlightTex;
            }
        }
        
    }

    void PointerExit(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerExit.Invoke(hit);

                transform.localScale = originalSize;

                rend.material.mainTexture = defaultTex;
            }
        }
        
    }
    void PointerStay(RaycastHit hit)
    {
        if(this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerStay.Invoke(hit);
            }
        }
        
    }

    void PointerPressDown(RaycastHit hit)
    {
        if(this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerPressDown.Invoke(hit);
            }
        }       
    }

    void PointerPressStay(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerStayPressed.Invoke(hit);
            }
        }
            
    }

    void PointerPressUp(RaycastHit hit)
    {
        if(this != null)
        {
            if (hit.transform.gameObject == this.gameObject && buttonEnabled)
            {
                OnPointerPressUp.Invoke(hit);
            }
        }
        
    }

    //Inactive means the button is grayed out
    public void SetInactive()
    {
        buttonEnabled = false;

        rend.material.color = new Color(1,1,1,0.5f);
    }


    public void SetActive()
    {
        buttonEnabled = true;

        rend.material.color = Color.white;
    }

    public void SetCollidersActive(bool active)
    {
        col.enabled = active;
    }

    public void Show()
    {
        fadeAnim.FadeOverTime(1);
        buttonEnabled = true;
        col.enabled = true;
    }

    public void Hide()
    {
        fadeAnim.FadeOverTime(0);
        buttonEnabled = false;
        StartCoroutine(SetColliderActiveDelayed(false, fadeAnim.GetAnimTime()));
    }

    IEnumerator SetColliderActiveDelayed(bool active, float time)
    {
        yield return new WaitForSeconds(time);
        col.enabled = active;
    }

    private void OnEnable()
    {
        Pointer_Controller.OnEnterGameObject += PointerEnter;
        Pointer_Controller.OnExitGameObject += PointerExit;
        Pointer_Controller.OnStayGameObject += PointerStay;
        Pointer_Controller.OnPressedGameObjectDown += PointerPressDown;
        //Pointer_Controller.OnPressedStayGameObject += PointerPressStay;
        Pointer_Controller.OnPressedGameObjectUp += PointerPressUp;
    }

    private void OnDisable()
    {
        Pointer_Controller.OnEnterGameObject -= PointerEnter;
        Pointer_Controller.OnExitGameObject -= PointerExit;
        Pointer_Controller.OnStayGameObject -= PointerStay;
        Pointer_Controller.OnPressedGameObjectDown -= PointerPressDown;
        //Pointer_Controller.OnPressedStayGameObject += PointerPressStay;
    }
}
