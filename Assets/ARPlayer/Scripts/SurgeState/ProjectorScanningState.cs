using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ProjectorScanningState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log("ProjectorScanningState.OnEnable");
            CoreManager.SharedARManager.EnterScanningProjectorState();
        }

        private void OnDisable()
        {
            Debug.Log("ProjectorScanningState.OnDisable");
            CoreManager.SharedARManager.LeaveScanningProjectorState();
        }
    }
}
