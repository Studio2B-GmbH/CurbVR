using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer_Controller : MonoBehaviour
{
    [SerializeField]
    float scaleByDistance = 1f;

    [SerializeField]
    float minLaserDistance = 2f;

    [SerializeField]
    float maxLaserDistance = 3f;

    [SerializeField]
    GameObject Hitsphere;

    [SerializeField]
    GameObject HitRing;

    [SerializeField]
    GameObject RayCaster;

    [SerializeField]
    AudioClip hightlight;

    [SerializeField]
    AudioClip select;

    [SerializeField]
    LineRenderer laserLineRend;

    [SerializeField]
    Transform lineRendTargetScaleTransform;

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

        SetLaserDistance(1f, false);

    }

    // Update is called once per frame
    void Update()
    {
        didRayHit = false;

        controllerAnchor = ControllerManager.Instance.GetActiveController();
        hmd = DeviceManager.Instance.GetHead();

        transform.position = controllerAnchor.transform.position;
        transform.rotation = controllerAnchor.transform.rotation;

        if (lineRendTargetScaleTransform != null)
        {
            laserLineRend.widthMultiplier = lineRendTargetScaleTransform.localScale.y;
        }

        RaycastHit hit;
        if (Physics.Raycast(RayCaster.transform.position, transform.TransformDirection(Vector3.forward), out hit, 99999999, noHMDHitLayerMask))
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
                Hitsphere.transform.localScale = RingStartSize * (dist / dist) * scaleByDistance;
                HitRing.transform.localScale = SphereStartSize * (dist / dist) * scaleByDistance;
                Hitsphere.SetActive(true);

                SetLaserDistance(Vector3.Distance(hit.point, transform.position), true);

                if (DeviceManager.Instance.GetSelectionButtonDown() && hit.collider.tag != "VisualOnly")
                {
                    if (OnPressedGameObjectDown != null)
                    {
                        OnPressedGameObjectDown.Invoke(hit);
                    }
                    HitRing.SetActive(true);
                    Audio.PlayOneShot(select);
                }

                if (DeviceManager.Instance.GetSelectionButton() && !DeviceManager.Instance.GetSelectionButtonDown())
                {
                    if(OnPressedStayGameObject != null)
                    {
                        OnPressedStayGameObject.Invoke(hit);
                    }
                }

                if (DeviceManager.Instance.GetSelectionButtonUp())
                {
                    if(OnPressedGameObjectUp != null)
                    {
                        OnPressedGameObjectUp.Invoke(hit);
                    }
                }
            }

            OnConstanstantRayCastHitUpdatePointer?.Invoke(hit);
        }

        if (didRayHit)
        {
            if (!hit.collider.isTrigger)
            {
                if (oldhitGameObject != null)
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

                        //If Pointer hovers over selectable object, play sound

                        if (hit.transform.CompareTag("Selectable"))
                            Audio.PlayOneShot(hightlight);
                    }
                }

                //Pointer Enters new Gameobject
                if (oldhitGameObject == null)
                {
                    OnEnterGameObject?.Invoke(hit);

                    if (hit.transform.CompareTag("Selectable"))
                        Audio.PlayOneShot(hightlight);
                }

                oldhitGameObject = hit.transform.gameObject;
                oldHit = hit;

            }

        }

        else
        {
            //Pointer leaves GO
            if (oldhitGameObject != null)
            {
                OnExitGameObject?.Invoke(oldHit);
            }

            oldHit = new RaycastHit();
            oldhitGameObject = null;
            Hitsphere.SetActive(false);
            HitRing.SetActive(false);

            SetLaserDistance(0, false);

        }
    }


    private void SetLaserDistance(float distance, bool hit)
    {
        float targetScale = 1f;

        if (lineRendTargetScaleTransform != null)
        {
            targetScale = lineRendTargetScaleTransform.localScale.y;
        }

        Vector3 position1 = laserLineRend.transform.position;
        Vector3 position2 = Vector3.zero;

        if (hit)
        {
            position2 = transform.position + ((transform.forward * Mathf.Clamp(distance, 0, maxLaserDistance * targetScale)));
        }

        else
        {
            position2 = transform.position + (transform.forward * minLaserDistance * targetScale);
        }

        laserLineRend.SetPosition(0, position1);
        laserLineRend.SetPosition(1, position2);
    }
}
