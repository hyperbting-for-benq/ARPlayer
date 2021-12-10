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
        
        // Remove Screen, Projector
        CoreManager.SharedARManager.TryRemoveScreen();
        CoreManager.SharedARManager.TryRemoveProjector();
        
        // Remove Planes
        CoreManager.SharedARManager.ResetArSession();
    }

    private void OnDisable()
    {
        Debug.Log("BeforeScanState.OnDisable");
    }
}
