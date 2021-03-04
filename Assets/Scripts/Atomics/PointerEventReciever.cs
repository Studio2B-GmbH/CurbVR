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
    [SerializeField]
    bool recieverEnabled = true;

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

    Collider col;

    private void Start()
    {
        if(transform.GetComponent<Collider>() == null)
        {
            Debug.LogWarning(gameObject.name + " doesn't have a collider attached and will not recieve Pointer Events");
            return;
        }

        col = transform.GetComponent<Collider>();

        col.enabled = recieverEnabled;
    }

    void PointerEnter(RaycastHit hit)
    {
        if(hit.transform.gameObject == this.gameObject)
        {
            OnPointerEnter.Invoke(hit);
        }
    }

    void PointerExit(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            OnPointerExit.Invoke(hit);
        }
    }
    void PointerStay(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            OnPointerStay.Invoke(hit);
        }
    }

    void PointerPressDown(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            OnPointerPressDown.Invoke(hit);
        }
    }

    void PointerPressStay(RaycastHit hit)
    {
        if(hit.transform.gameObject == this.gameObject)
        {
            OnPointerStayPressed.Invoke(hit);
        }
    }

    void PointerPressUp(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            OnPointerPressUp.Invoke(hit);
        }
    }

    public void DisableReciever()
    {
        col.enabled = false;
    }

    public void EnableReciever()
    {
        col.enabled = true;
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
