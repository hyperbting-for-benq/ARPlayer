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
    [SerializeField] private LeanWindow requestCamPermissionGrantedLeaWin;

    [Space]
    [Header("Debug Purpose")]
    //[SerializeField][ReadOnly] private ARCapabilityDetection _arCapabilityDetection;
    [SerializeField][ReadOnly] private CameraPermissionManager _cameraPermissionManager;
    private void OnEnable()
    {
        Debug.Log("CameraPermissionState.OnEnable");
        // if (_arCapabilityDetection == null)
        //     _arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();
        
        if (_cameraPermissionManager == null)
            _cameraPermissionManager = GetComponent<CameraPermissionManager>();
        
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
        StartCoroutine(_cameraPermissionManager.RequestCameraPermission(
            () =>
            {
                requestCamPermissionLeaWin?.TurnOff();
                requestCamPermissionGrantedLeaWin?.TurnOn();
            },
            () =>
            {
                
            }
        ));
    }

    public void ShowPermissionGrantWindow()
    {
        if (requestCamPermissionGrantedLeaWin == null)
        {
            return;
        }
        
        requestCamPermissionGrantedLeaWin.TurnOn();
    }
}
