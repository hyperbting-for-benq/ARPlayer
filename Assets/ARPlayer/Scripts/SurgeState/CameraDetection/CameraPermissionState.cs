using ARPlayer.Scripts.Detection;
using Lean.Gui;
using MyBox;
using Pixelplacement;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState.CameraDetection
{
    public class CameraPermissionState : State
    {
        [SerializeField] private LeanWindow requestCamPermissionLeaWin;
        [SerializeField] private LeanWindow requestCamPermissionDeniedLeaWin;
        [Space]
        [SerializeField] private LeanButton requestCamPermissionDeniedLeaBtn;

        [Header("Debug Purpose")] 
        public bool debugAlwaysCameraFail = false;
        [SerializeField][ReadOnly] private ARCapabilityDetection arCapabilityDetection;
        [SerializeField][ReadOnly] private CameraPermissionManager cameraPermissionManager;
        private void OnEnable()
        {
            Debug.Log("CameraPermissionState.OnEnable");
        
            arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();
            cameraPermissionManager = GetComponent<CameraPermissionManager>();

            requestCamPermissionLeaWin?.OnOn.AddListener(OnCheckingWindowOnAction);
        
            requestCamPermissionDeniedLeaBtn?.OnClick.AddListener(CheckCameraPermission);
            requestCamPermissionDeniedLeaBtn.interactable = true;
        
            CheckCameraPermission();
        }

        private void OnDisable()
        {
            Debug.Log("CameraPermissionState.OnDisable");
        
            requestCamPermissionLeaWin?.OnOn.RemoveListener(OnCheckingWindowOnAction);
            requestCamPermissionDeniedLeaBtn?.OnClick.RemoveListener(CheckCameraPermission);
        
            requestCamPermissionLeaWin?.TurnOff();
            requestCamPermissionDeniedLeaWin?.TurnOff();
        }

        private void CheckCameraPermission()
        {
            Debug.Log("CheckCameraPermission");
            //popup modal
            requestCamPermissionDeniedLeaWin?.TurnOff();
            
            requestCamPermissionLeaWin?.TurnOn();
        }

        private void OnCheckingWindowOnAction()
        {
            if (debugAlwaysCameraFail)
            {
                requestCamPermissionLeaWin?.TurnOff();
                requestCamPermissionDeniedLeaWin?.TurnOn();
                return;
            }

            StartCoroutine(cameraPermissionManager.CheckCameraPermission(
                () =>
                {
                    requestCamPermissionLeaWin?.TurnOff();
                    arCapabilityDetection.GoToCameraGranted();
                },
                () =>
                {
                    requestCamPermissionLeaWin?.TurnOff();
                    requestCamPermissionDeniedLeaWin?.TurnOn();
                }
            ));
        }
    }
}
