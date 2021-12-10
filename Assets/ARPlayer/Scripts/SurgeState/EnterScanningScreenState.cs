using System;
using UnityEngine;
using Pixelplacement;

public class EnterScanningScreenState : State
{
    private void OnEnable()
    {
        Debug.Log("EnterScanningScreenState.OnEnable");
        
        ////Allow Scanning Vertical;
        ////Allow Vertical Place Interaction;
        //planeDisplayManager.SetVerticalScanningAndInteraction();
        
        //SharedARManager.OnARRaycastHit += 
        //SharedARState.OnVerticalObjectPlaced +=
    }

    private void OnDisable()
    {
        Debug.Log("EnterScanningScreenState.OnDisable");
        
        //SharedARManager.OnARRaycastHit -= 
        //SharedARState.OnVerticalObjectPlaced -=
    }
}
