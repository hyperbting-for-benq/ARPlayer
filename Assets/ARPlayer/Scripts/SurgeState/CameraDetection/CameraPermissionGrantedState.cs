using System.Collections;
using System.Collections.Generic;
using ARPlayer.Scripts.Detection;
using Lean.Gui;
using MyBox;
using Pixelplacement;
using UnityEngine;

public class CameraPermissionGrantedState : State
{
    [SerializeField] private LeanWindow requestCamPermissionGrantedLeaWin;
    
    [Header("Debug Purpose")]
    [SerializeField][ReadOnly] private ARCapabilityDetection arCapabilityDetection;
    [SerializeField][ReadOnly] private CameraPermissionManager cameraPermissionManager;
    
    private void OnEnable()
    {
        Debug.Log("CameraPermissionDeniedState.OnEnable");
        arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();
        cameraPermissionManager = GetComponent<CameraPermissionManager>();

        requestCamPermissionGrantedLeaWin?.OnOff.AddListener(arCapabilityDetection.GoToARMain);

        //popup modal
        requestCamPermissionGrantedLeaWin?.TurnOn();
    }

    private void OnDisable()
    {
        Debug.Log("CameraPermissionDeniedState.OnDisable");
        requestCamPermissionGrantedLeaWin?.OnOff.RemoveListener(arCapabilityDetection.GoToARMain);
    }
}
