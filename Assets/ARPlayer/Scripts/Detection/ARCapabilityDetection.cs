using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using Pixelplacement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Detection
{
    public class ARCapabilityDetection : MonoBehaviour
    {
        [SerializeField] [MyBox.ReadOnly] private StateMachine stateMachine;
        [Space] 
        //[SerializeField] private GameObject CheckerLoaded;
        [SerializeField] private GameObject CapabilityDisabled;
        [SerializeField] private GameObject CapabilityEnabled;
        [SerializeField] private GameObject CameraPermissionCheck;
        [SerializeField] private GameObject EmptyState;

        [Space] 
        [SerializeField][ReadOnly] private MainManager _mainManager; 
        private void OnEnable()
        {
            _mainManager = FindObjectOfType<MainManager>();
            stateMachine = GetComponent<StateMachine>();
        }

        private void OnDisable()
        {
            _mainManager = null;
        }

        private void Start()
        {
            StartCoroutine(DetectARCapability(
                    () =>
                    {
                        if (CheckPlaneDetectionCapability())
                        {
                            stateMachine.ChangeState(CapabilityEnabled);
                        }
                        else
                        {
                            Debug.LogError("Cannot DetectPlane!");
                            stateMachine.ChangeState(CapabilityDisabled);
                        }
                    },
                    () =>
                    {
                        stateMachine.ChangeState(CapabilityDisabled);
                    },
                () =>
                    {
                        Debug.LogError("Unexpected State Detected!");
                        stateMachine.ChangeState(CapabilityDisabled);
                    }
                ) 
            );
        }

        public void GoToCameraCheck()
        {
            stateMachine.ChangeState(CameraPermissionCheck);
        }
        
        public void GoToARMain()
        {
            stateMachine.ChangeState(EmptyState);
            
            if (_mainManager == null)
            {
                Debug.LogError("MainManager Missing!");
                return;
            }

            _mainManager.LoadScene_ARMain();
        }

        private IEnumerator DetectARCapability(Action arSesStateEnabled, Action arSesStateDisabled, Action unexpected)
        {

            yield return new WaitForSeconds(1f);
            
            // immediate meet correct state or unexpected
            if (ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability)
            {
                Debug.Log("CheckingAvailability");

                yield return ARSession.CheckAvailability();
            }
            else
            {
                yield return new WaitForSeconds(1f);
                
                Debug.LogWarning("Unexpected State Occurred");
                unexpected?.Invoke();
            }

            if (ARSession.state == ARSessionState.Unsupported)
            {
                yield return new WaitForSeconds(1f);
                
                // Start some fallback experience for unsupported devices
                Debug.Log("Unsupported End");
                arSesStateDisabled?.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(1f);
                
                Debug.Log("Successful End");
                arSesStateEnabled?.Invoke();
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

        private bool CheckPlaneDetectionCapability()
        {
            var planeDescriptors = new List<XRPlaneSubsystemDescriptor>();
            SubsystemManager.GetSubsystemDescriptors(planeDescriptors);

            if (planeDescriptors.Count <= 0) 
                return false;
            
            Debug.Log("PlaneDetection Supported");
            foreach(var planeDescriptor in planeDescriptors)
            {
                if (!planeDescriptor.supportsClassification) 
                    continue;
                
                Debug.Log("Plane Classification Supported");
                break;
            }

            return true;
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
}