using System.Collections;
using System.Collections.Generic;
using ARPlayer.Scripts.Detection;
using Lean.Gui;
using Pixelplacement;
using UnityEngine;

public class CameraPermissionState : State
{
    [SerializeField] private LeanWindow requestCamPermissionLeaWin;
    [SerializeField] private LeanWindow requestCamPermissionGrantedLeaWin;

    [SerializeField] private ARCapabilityDetection arcd;
    private void OnEnable()
    {
        Debug.Log("CameraPermissionState.OnEnable");
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
        StartCoroutine(arcd.RequestCameraPermission(
            () =>
            {
                requestCamPermissionLeaWin.TurnOff();
                requestCamPermissionGrantedLeaWin.TurnOn();
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
