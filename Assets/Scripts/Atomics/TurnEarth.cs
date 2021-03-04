using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEarth : MonoBehaviour
{
    [SerializeField]
    ControllerSwitcher controllerSwitcher;

    [SerializeField]
    float maxTurnSpeed = 0.1f;

    [SerializeField]
    float speedMultiplier;

    [SerializeField]
    int stopFramesMin;

    [SerializeField]
    float dampStrength;

    [SerializeField]
    int detectionAngleTolerance;

    [SerializeField]
    float detectionMaxSpeed;

    [SerializeField]
    int fallBackTime = 15;

    int stopFramesCounter;

    Transform pointer;

    Vector3 oldPointerForwardVector;

    float targetVelocity;

    float lerper = 0;

    bool pointerPressed;

    bool detectAngle;

    Quaternion angleToDetect;

    bool rotationLocked;

    Quaternion oldRotation;

    float fallbackCounter;

    EventTimer.ContinueTimerCallback timerCallback;

    private void Start()
    {
        pointer = controllerSwitcher.GetController().transform;
    }

    public void OnPressDown(RaycastHit hit)
    {
        oldPointerForwardVector = pointer.forward;
        pointerPressed = true;
    }

    public void OnPressUp(RaycastHit hit)
    {
        pointerPressed = false;
    }

    public void UnlockRotation()
    {
        rotationLocked = false;
    }

    /// <summary>
    /// Detects a specific rotation of the earth (With some to tolerance)
    /// Important: Needs a callback function supplied via SupplyCallback
    /// </summary>
    /// <param name="degrees"></param>
    public void DetectRotation(int degrees)
    {
        detectAngle = true;
        angleToDetect = Quaternion.identity * Quaternion.AngleAxis(degrees, transform.up);
    }

    public void SupplyCallback(EventTimer.ContinueTimerCallback callback)
    {
        timerCallback = callback;
    }

    // Update is called once per frame
    void Update()
    {
        if (detectAngle)
        {
            float rotationalVelocity = Quaternion.Angle(oldRotation, transform.rotation);
            oldRotation = transform.rotation;

            float angle = Quaternion.Angle(transform.rotation, angleToDetect);
            if((angle < detectionAngleTolerance && rotationalVelocity < detectionMaxSpeed) || fallbackCounter > fallBackTime)
            {
                rotationLocked = true;
                detectAngle = false;
                timerCallback.Invoke();
            }



            fallbackCounter += Time.deltaTime;
        }

        if (rotationLocked)
        {
            fallbackCounter = 0;
            pointerPressed = false;

            transform.rotation = Quaternion.Slerp(transform.rotation, angleToDetect, maxTurnSpeed);
        }

        else
        {
            if (DeviceManager.Instance.GetSelectionButtonUp())
            {
                pointerPressed = false;
            }

            if (pointerPressed && !rotationLocked)
            {
                lerper = 0;
                float newVelocity = Vector3.SignedAngle(pointer.forward, oldPointerForwardVector, pointer.up) * speedMultiplier;

                //Sometimes, the velocity will report as 0, even when it actually is still in motion.
                //So we wait for a few frames before allowing the velocity to fall to zero
                if (Mathf.Abs(newVelocity) > 0 || stopFramesCounter > stopFramesMin)
                {
                    targetVelocity = newVelocity;
                    stopFramesCounter = 0;
                }

                else
                {
                    stopFramesCounter++;
                }


                oldPointerForwardVector = pointer.forward;
            }

            else
            {
                lerper += dampStrength * Time.deltaTime;
            }

            targetVelocity = Mathf.Clamp(Mathf.SmoothStep(targetVelocity, 0, lerper), maxTurnSpeed * -1, maxTurnSpeed);
            transform.RotateAround(Vector3.up, targetVelocity);
        }
        

    }
}
