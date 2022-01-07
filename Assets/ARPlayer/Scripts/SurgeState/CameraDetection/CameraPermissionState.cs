using System.Collections;
using System.Collections.Generic;
using ARPlayer.Scripts.Detection;
using Lean.Gui;
using MyBox;
using Pixelplacement;
using UnityEngine;

public class CameraPermissionState : State
{
    [SerializeField] private LeanWindow requestCamPermissionLeaWin;
    [SerializeField] private LeanWindow requestCamPermissionDeniedLeaWin;
    
    [Space]
    [Header("Debug Purpose")]
    [SerializeField][ReadOnly] private ARCapabilityDetection arCapabilityDetection;
    [SerializeField][ReadOnly] private CameraPermissionManager cameraPermissionManager;
    private void OnEnable()
    {
        Debug.Log("CameraPermissionState.OnEnable");
        
        if (cameraPermissionManager == null)
            cameraPermissionManager = GetComponent<CameraPermissionManager>();
        
        if (requestCamPermissionLeaWin == null)
        {
            return;
        }

        requestCamPermissionLeaWin.OnOn.AddListener(OnWindowOnAction);

        //popup modal
        requestCamPermissionLeaWin.TurnOn();
    }

    private void OnDisable()
    {
        Debug.Log("CameraPermissionState.OnDisable");
        if (requestCamPermissionLeaWin == null)
        {
            return;
        }
        
        requestCamPermissionLeaWin.OnOn.RemoveListener(OnWindowOnAction);
    }

    private void OnWindowOnAction()
    {
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
