using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

public class ARCababilityDetection : MonoBehaviour
{
    //[SerializeField] private GameObject arSession;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetectARCabability(
        () => { TestPlaneDetection(); },
            () => { }
        ));
    }
    
    IEnumerator DetectARCabability(Action success, Action fail)
    {
        switch (ARSession.state)
        {
            case ARSessionState.None:
            case ARSessionState.CheckingAvailability:
                Debug.Log("CheckAvailability");
                yield return ARSession.CheckAvailability();
                break;
            default:
                Debug.LogWarning("Unexpected States");
                break;
        }
        
        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
            Debug.Log("Unsupported");
            fail?.Invoke();
        }
        else
        {
            Debug.Log("End");
            success?.Invoke();
        }
    }
    
    // #region XR Enable
    // public IEnumerator StartXR() {
    //     yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
    //
    //     if (XRGeneralSettings.Instance.Manager.activeLoader == null) {
    //         Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
    //     } else {
    //         Debug.Log("Starting XR...");
    //         XRGeneralSettings.Instance.Manager.StartSubsystems();
    //         yield return null;
    //     }
    // }
    //
    // void StopXR() {
    //     Debug.Log("Stopping XR...");
    //     XRGeneralSettings.Instance.Manager.StopSubsystems();
    //     XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    //     Debug.Log("XR stopped completely.");
    // }
    // #endregion

    void TestPlaneDetection()
    {
        var planeDescriptors = new List<XRPlaneSubsystemDescriptor>();
        SubsystemManager.GetSubsystemDescriptors(planeDescriptors);
        
        if(planeDescriptors.Count > 0)
        {
            Debug.Log("PlaneDetection Supported");
            foreach(var planeDescriptor in planeDescriptors)
            {
                if(planeDescriptor.supportsClassification)
                {
                    Debug.Log("Plane Classification Supported");
                    break;
                }
            }
        }
    }

    void test()
    {
        // var planeDescriptors = new List<XRPlaneSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(planeDescriptors);
        //
        // var rayCastDescriptors = new List<XRRaycastSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(rayCastDescriptors);
        //
        // var faceDescriptors = new List<XRFaceSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(faceDescriptors);
        //
        // var imageDescriptors = new List<XRImageTrackingSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(imageDescriptors);
        //
        // var envDescriptors = new List<XREnvironmentProbeSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(envDescriptors);
        //
        // var anchorDescriptors = new List<XRAnchorSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(anchorDescriptors);
        //
        // var objectDescriptors = new List<XRObjectTrackingSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(objectDescriptors);
        //
        // var participantDescriptors = new List<XRParticipantSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(participantDescriptors);
        //
        // var depthDescriptors = new List<XRDepthSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(depthDescriptors);
        //
        // var occlusionDescriptors = new List<XROcclusionSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(occlusionDescriptors);
        //
        // var cameraDescriptors = new List<XRCameraSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(cameraDescriptors);
        //
        // var sessionDescriptors = new List<XRSessionSubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(sessionDescriptors);
        //
        // var bodyTrackingDescriptors = new List<XRHumanBodySubsystemDescriptor>();
        // SubsystemManager.GetSubsystemDescriptors(bodyTrackingDescriptors);
    }
}