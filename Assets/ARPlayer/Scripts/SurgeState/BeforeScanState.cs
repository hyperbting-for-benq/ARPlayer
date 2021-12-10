using System.Collections;
using System.Collections.Generic;
using ARPlayer.Scripts;
using UnityEngine;
using Pixelplacement;

public class BeforeScanState : State
{
    private void OnEnable()
    {
        Debug.Log("BeforeScanState.OnEnable");
        
        //Reset
        // Remove Screen
        CoreManager.SharedARManager.TryRemoveScreen();
        // Remove Projector
        CoreManager.SharedARManager.TryRemoveProjector();
        
        // Remove Planes
        CoreManager.SharedARManager.ResetArSession();
        
        //Remove ARAnchor
        CoreManager.SharedARManager.CleanAnchors();
        
        //TODO: Show UI to Next State
    }

    private void OnDisable()
    {
        Debug.Log("BeforeScanState.OnDisable");
    }
}
