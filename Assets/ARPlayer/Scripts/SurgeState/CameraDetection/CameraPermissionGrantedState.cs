using ARPlayer.Scripts.Detection;
using Lean.Gui;
using MyBox;
using Pixelplacement;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState.CameraDetection
{
    public class CameraPermissionGrantedState : State
    {
        [SerializeField] private LeanWindow requestCamPermissionGrantedLeaWin;
    
        [Header("Debug Purpose")]
        [SerializeField][ReadOnly] private ARCapabilityDetection arCapabilityDetection;
    
        private void OnEnable()
        {
            Debug.Log("CameraPermissionDeniedState.OnEnable");
            arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();

            requestCamPermissionGrantedLeaWin?.OnOff.AddListener(arCapabilityDetection.GoToARMain);

            //popup modal
            requestCamPermissionGrantedLeaWin?.TurnOn();
        }

        private void OnDisable()
        {
            Debug.Log("CameraPermissionDeniedState.OnDisable");
            requestCamPermissionGrantedLeaWin?.OnOff.RemoveListener(arCapabilityDetection.GoToARMain);
        
            requestCamPermissionGrantedLeaWin?.TurnOff();
        }
    }
}
