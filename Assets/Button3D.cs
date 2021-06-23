using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{
    bool buttonEnabled = true;

    bool activated;

    enum startState { active, inactive, activated}

    [SerializeField]
    bool startHidden;

    [SerializeField]
    startState buttonStartState = startState.active;

    [SerializeField]
    Texture2D defaultTex;

    [SerializeField]
    Texture2D highlightTex;

    [SerializeField]
    Texture2D pressedTex;

    [SerializeField]
    Texture2D inactiveTex;

    [SerializeField]
    Texture2D activatedTex;

    [SerializeField]
    Texture2D activatedHighlightTex;

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

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
        fadeAnim = GetComponent<Fade_Anim>();

        Material newInstance = new Material(rend.material);
        rend.material = newInstance;

        switch (buttonStartState)
        {
            case startState.active:
                rend.material.mainTexture = defaultTex;
                break;
            case startState.inactive:
                buttonEnabled = false;
                rend.material.mainTexture = inactiveTex;
                break;
            case startState.activated:
                activated = true;
                rend.material.mainTexture = activatedTex;
                break;
            default:
                break;
        }

        if (startHidden)
            fadeAnim.FadeInstantly(0);

        else
            fadeAnim.FadeInstantly(1);


        OnStart.Invoke();

        Debug.Log("StartInvoked");
    }


    void PointerEnter(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerEnter.Invoke(hit);

            Debug.Log("PointerEnter");

            if (activated)
                rend.material.mainTexture = activatedHighlightTex;

            else
                rend.material.mainTexture = highlightTex;
        }
    }

    void PointerExit(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerExit.Invoke(hit);

            if (activated)         
                rend.material.mainTexture = activatedTex;            

            else
                rend.material.mainTexture = defaultTex;
        }
    }
    void PointerStay(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerStay.Invoke(hit);
        }
    }

    void PointerPressDown(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerPressDown.Invoke(hit);

            rend.material.mainTexture = pressedTex;
        }
    }

    void PointerPressStay(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerStayPressed.Invoke(hit);

            rend.material.mainTexture = pressedTex;
        }
    }

    void PointerPressUp(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && buttonEnabled)
        {
            OnPointerPressUp.Invoke(hit);

            if (activated)
                rend.material.mainTexture = activatedHighlightTex;
            else
                rend.material.mainTexture = highlightTex;
        }
    }

    //Inactive means the button is grayed out
    public void SetInactive()
    {
        buttonEnabled = false;
        activated = false;

        rend.material.mainTexture = inactiveTex;
    }


    public void SetActive()
    {
        buttonEnabled = true;
        activated = false;

        rend.material.mainTexture = defaultTex;

    }

    //Activated means that the content which the buttons points to has already been seen
    public void SetActivated()
    {
        buttonEnabled = false;
        activated = true;

        rend.material.mainTexture = activatedTex;
    }

    public void SetCollidersActive(bool active)
    {
        col.enabled = active;
    }

    public void Show()
    {
        fadeAnim.FadeOverTime(1);
        buttonEnabled = true;

    }

    public void Hide()
    {
        fadeAnim.FadeOverTime(0);
        buttonEnabled = false;
    }

    private void OnEnable()
    {
        Pointer_Controller.OnEnterGameObject += PointerEnter;
        Pointer_Controller.OnExitGameObject += PointerExit;
        Pointer_Controller.OnStayGameObject += PointerStay;
        Pointer_Controller.OnPressedGameObjectDown += PointerPressDown;
        Pointer_Controller.OnPressedStayGameObject += PointerPressStay;
        Pointer_Controller.OnPressedGameObjectUp += PointerPressUp;
    }

    private void OnDisable()
    {
        Pointer_Controller.OnEnterGameObject -= PointerEnter;
        Pointer_Controller.OnExitGameObject -= PointerExit;
        Pointer_Controller.OnStayGameObject -= PointerStay;
        Pointer_Controller.OnPressedGameObjectDown -= PointerPressDown;
        Pointer_Controller.OnPressedStayGameObject += PointerPressStay;
    }
}
