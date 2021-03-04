using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Controller : MonoBehaviour
{
    [SerializeField]
    GameObject Hitsphere;

    [SerializeField]
    GameObject HitRing;

    [SerializeField]
    GameObject RayCaster;

    [SerializeField]
    float scaleByDistance = 1f;

    [SerializeField]
    AudioClip hightlight;

    [SerializeField]
    AudioClip select;

    public delegate void enterGameObject(RaycastHit hit);
    public static event enterGameObject OnEnterGameObject;

    public delegate void stayGameObject(RaycastHit hit);
    public static event stayGameObject OnStayGameObject;

    public delegate void exitGameObject(RaycastHit hit);
    public static event exitGameObject OnExitGameObject;

    public delegate void pressedGameObjectDown(RaycastHit hit);
    public static event pressedGameObjectDown OnPressedGameObjectDown;

    public delegate void pressedStayGameObject(RaycastHit hit);
    public static event pressedStayGameObject OnPressedStayGameObject;

    public delegate void pressedGameObjectUp(RaycastHit hit);
    public static event pressedGameObjectUp OnPressedGameObjectUp;

    public delegate void constantRayCastHitUpdatePointer(RaycastHit hit);
    public static event constantRayCastHitUpdatePointer OnConstanstantRayCastHitUpdatePointer;

    public delegate void constantRayCastHitUpdateHMD(RaycastHit hit);
    public static event constantRayCastHitUpdateHMD OnConstantRayCastHitUpdateHMD;

    [SerializeField]
    ControllerSwitcher controllerSwitcher;

    GameObject controllerAnchor;
    Transform hmd;
    bool didRayHit;
    AudioSource Audio;

    GameObject oldhitGameObject;
    RaycastHit oldHit;

    Vector3 SphereStartSize;
    Vector3 RingStartSize;

    int noHMDHitLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        oldhitGameObject = null;
        SphereStartSize = Hitsphere.transform.localScale;
        RingStartSize = HitRing.transform.localScale;

        noHMDHitLayerMask = 1 << 10;
        noHMDHitLayerMask = ~noHMDHitLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        didRayHit = false;

        controllerAnchor = controllerSwitcher.GetController();
        hmd = DeviceManager.Instance.GetHead();

        transform.position = controllerAnchor.transform.position;
        transform.rotation = controllerAnchor.transform.rotation;

        RaycastHit hit;
        if (Physics.Raycast(RayCaster.transform.position, transform.TransformDirection(Vector3.forward), out hit, 10000f, noHMDHitLayerMask))
        {
            if (!hit.collider.isTrigger)
            {
                didRayHit = true;
                Hitsphere.transform.position = hit.point;
                HitRing.transform.position = hit.point;
                HitRing.transform.rotation = Quaternion.LookRotation(hit.normal);
                Hitsphere.transform.rotation = Quaternion.LookRotation(hit.normal);

                Plane plane = new Plane(Camera.main.transform.forward, Camera.main.transform.position);
                float dist = plane.GetDistanceToPoint(hit.point);
                Hitsphere.transform.localScale = RingStartSize * dist * scaleByDistance;
                HitRing.transform.localScale = SphereStartSize * dist * scaleByDistance;
                Hitsphere.SetActive(true);

                if (DeviceManager.Instance.GetSelectionButtonDown() && hit.collider.tag != "VisualOnly")
                {
                    OnPressedGameObjectDown.Invoke(hit);
                    HitRing.SetActive(true);
                    Audio.PlayOneShot(select);
                }

                if (DeviceManager.Instance.GetSelectionButton() && !DeviceManager.Instance.GetSelectionButtonDown())
                {
                    OnPressedStayGameObject.Invoke(hit);
                }

                if (DeviceManager.Instance.GetSelectionButtonUp())
                {
                    OnPressedGameObjectUp.Invoke(hit);
                }
            }
        
            OnConstanstantRayCastHitUpdatePointer?.Invoke(hit);
        }

        if (didRayHit)
        {
            if (!hit.collider.isTrigger)
            {
                if(oldhitGameObject != null)
                {
                    //Pointer Stays on Object
                    if (oldhitGameObject == hit.transform.gameObject)
                    {
                        OnStayGameObject?.Invoke(hit);
                    }

                    //Pointer Leaves and Enters new object at the same time
                    if (oldhitGameObject != hit.transform.gameObject)
                    {
                        OnEnterGameObject?.Invoke(hit);
                        OnExitGameObject?.Invoke(oldHit);
                        Audio.PlayOneShot(hightlight);
                    }
                }       
                
                //Pointer Enters new Gameobject
                if(oldhitGameObject == null)
                {
                    OnEnterGameObject?.Invoke(hit);
                    Audio.PlayOneShot(hightlight);
                }

                oldhitGameObject = hit.transform.gameObject;
                oldHit = hit;

            }

        }

        else
        {
            //Pointer leaves GO
            if(oldhitGameObject != null)
            {
                OnExitGameObject?.Invoke(oldHit);
            }

            oldHit = new RaycastHit();
            oldhitGameObject = null;
            Hitsphere.SetActive(false);
            HitRing.SetActive(false);
        }

        //if (Physics.Raycast(hmd.position, hmd.TransformDirection(Vector3.forward), out hit, 100f))
        //{
        //    OnConstantRayCastHitUpdateHMD?.Invoke(hit);
        //}        
    }
}
