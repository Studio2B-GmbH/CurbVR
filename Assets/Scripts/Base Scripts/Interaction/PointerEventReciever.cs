using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class PointerEvent : UnityEvent<RaycastHit>
{
}

/// <summary>
/// This script recieves events from the 3D Pointer, and makes them easily digestable by turning them into Unity Events. 
/// The gameobject which this script is attached to also needs a collider to be able to recieve Pointer Events 
/// </summary>
public class PointerEventReciever : MonoBehaviour
{
    public bool recieverEnabled = true;

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

    Collider[] cols;
    Pointer_Highlight highlight;

    private void Start()
    {
        if (transform.GetComponent<Collider>() == null)
        {
            Debug.LogWarning(gameObject.name + " doesn't have a collider attached and will not recieve Pointer Events");
            return;
        }

        highlight = GetComponent<Pointer_Highlight>();
        cols = transform.GetComponentsInChildren<Collider>();

        if (recieverEnabled)
        {
            EnableReciever();
        }

        else
        {
            DisableReciever();
        }

    }

    void PointerEnter(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && recieverEnabled)
        {
            OnPointerEnter.Invoke(hit);
        }
    }

    void PointerExit(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && recieverEnabled)
        {
            OnPointerExit.Invoke(hit);
        }
    }
    void PointerStay(RaycastHit hit)
    {

        if (hit.transform.gameObject == this.gameObject && recieverEnabled)
        {
            OnPointerStay.Invoke(hit);
        }
    }

    void PointerPressDown(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && recieverEnabled)
            {
                OnPointerPressDown.Invoke(hit);
            }
        }

    }

    void PointerPressStay(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && recieverEnabled)
            {
                OnPointerStayPressed.Invoke(hit);
            }
        }
    }

    void PointerPressUp(RaycastHit hit)
    {
        if (this != null)
        {
            if (hit.transform.gameObject == this.gameObject && recieverEnabled)
            {
                OnPointerPressUp.Invoke(hit);
            }
        }
    }

    public void DisableReciever()
    {
        recieverEnabled = false;

        if (highlight != null)
        {
            highlight.highlightActivated = false;
        }
    }

    public void EnableReciever()
    {
        recieverEnabled = true;

        if (highlight != null)
        {
            highlight.highlightActivated = false;
        }
    }

    public void SetCollidersActive(bool active)
    {
        UpdateColliders(active);
    }

    void UpdateColliders(bool active)
    {
        foreach (Collider col in cols)
        {
            col.enabled = recieverEnabled;
        }
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
