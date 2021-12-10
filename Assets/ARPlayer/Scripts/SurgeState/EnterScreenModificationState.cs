using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class EnterScreenModificationState : State
{
    private void OnEnable()
    {
        Debug.Log("EnterScreenModificationState.OnEnable");
        
        //     //Stop scanning; Disable AllPlaneInteraction
        //     planeDisplayManager.StopPlaneScan();
        //     planeDisplayManager.EnableAllPlaneInteraction(false);
        //     
        //     //Allow/Show ScreenObjectMoveAround UI
    }

    private void OnDisable()
    {
        Debug.Log("EnterScreenModificationState.OnDisable");
    }
}
