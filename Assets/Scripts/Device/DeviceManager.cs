using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OVR;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Provides device-independent ways of checking for controller/HMD input. Currently supports Oculus Go, Oculus Quest, PC VR Emulation and Oculus Rift
/// </summary>
public class DeviceManager : MonoBehaviour {

    public static DeviceManager Instance { get; set; }

    public enum Platform { Unknown, Quest, PCVRDisabled, PCRift };

    [SerializeField]
    OVRHeadsetEmulator headsetEmulator;

    [SerializeField]
    Transform oculusHead;

    private Transform head;

    private Platform platform;

    private bool sixDOFenabled;


    private void Awake() {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }   
        
        platform = Platform.Unknown;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (!XRDevice.isPresent || OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Link_Quest)
        {
            platform = Platform.PCVRDisabled;
            head = oculusHead; //We use the Oculus HMD Emulation for the Non-VR PC-Version
            Camera.main.fieldOfView = 60;
            headsetEmulator.opMode = OVRHeadsetEmulator.OpMode.AlwaysOn;

        }

        else if(OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Rift_CV1)
        {
            platform = Platform.PCRift;
            head = oculusHead;
            sixDOFenabled = true;
            headsetEmulator.opMode = OVRHeadsetEmulator.OpMode.Off;

        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR


        if (OVRPlugin.GetSystemHeadsetType() == OVRPlugin.SystemHeadset.Oculus_Quest)
        {
            platform = Platform.Quest;
            UnityEngine.XR.XRSettings.eyeTextureResolutionScale = 1.2f;
            OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Low;
            head = oculusHead;
            sixDOFenabled = true;
        }

#endif

        if (!platform.Equals(Platform.Unknown)) 
        {
            Debug.Log("Chosen platform for this run: " + platform);
        } 
        
        else 
        {
            Debug.LogError("Unknown platform! RuntimePlatform is " + Application.platform + "; loadedDeviceName is " + XRSettings.loadedDeviceName);
        }
    }

    /// <summary>
    /// This function provides a device-independent way of checking if the "Selection Button" (Mouse Click on PC, Controller Trigger
    /// on VR Devices) was used
    /// </summary>
    /// <returns>Returns true if the selection button has been pressed down</returns>
    public bool GetSelectionButtonDown()
    {
        switch (platform)
        {
            case Platform.Unknown:
                return false;
            case Platform.Quest:
            case Platform.PCRift:
                return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.All) ||
                       OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.All);
            case Platform.PCVRDisabled:
                return Input.GetMouseButtonDown(0);
            default:
                return false;
        }
    }


    /// <summary>
    /// This function provides a device-independent way of checking if the "Selection Button" (Mouse Click on PC, Controller Trigger
    /// on VR Devices) was released
    /// </summary>
    /// <returns>Returns true if the selection button has been released</returns>
    public bool GetSelectionButtonUp()
    {
        switch (platform)
        {
            case Platform.Unknown:
                return false;
            case Platform.Quest:
            case Platform.PCRift:
                return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.All) ||
                       OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.All);
            case Platform.PCVRDisabled:
                return Input.GetMouseButtonUp(0);
            default:
                return false;
        }
    }

    public bool GetSelectionButton()
    {
        switch (platform)
        {
            case Platform.Unknown:
                return false;
            case Platform.Quest:
            case Platform.PCRift:
                return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.All) ||
                       OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.All);
            case Platform.PCVRDisabled:
                return Input.GetMouseButton(0);
            default:
                return false;
        }
    }

    /// <summary>
    /// Checks device-independently if the back-Button is being pressed
    /// Go: Back Button
    /// Rift & Quest: A / X Buttons
    /// PC: Backspace / Fifth mouse button
    /// </summary>
    /// <returns>Returns true if the buttons has been pressed down</returns>
    public bool GetBackButtonDown()
    {
        switch (platform)
        {
            case Platform.Unknown:
                return false;
            case Platform.Quest:
            case Platform.PCRift:
                return OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three);
            case Platform.PCVRDisabled:
                return Input.GetKeyDown(KeyCode.Backspace) || Input.GetMouseButton(4);
            default:
                return false;
        }
    }

    public Platform GetPlatform()
    {
        return platform;
    }

    public Transform GetHead() //hihi
    {
        return head;
    }

    public bool Get6DOFEnabled()
    {
        return sixDOFenabled;
    }
}
