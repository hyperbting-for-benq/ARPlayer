using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPlayer.Scripts
{
    public partial class CoreManager
    {
        #region Debug ContextMenu
        [ContextMenu("-BeforeScan State")]
        public void EnterBeforeScanState()
        {
            ResetState(); //SharedARManager.EnterBeforeScanState();
        }

        // [ContextMenu("-ScanningScreenAnPlace State")]
        // public void EnterScanningScreenState()
        // {
        //     SharedARManager.EnterScanningScreenState();
        // }
        
        // [ContextMenu("-End ScanningScreenAnPlace State")]
        // public void LeaveScanningScreenState()
        // {
        //     SharedARManager.LeaveScanningScreenState();
        // }

        // [ContextMenu("-ScreenPointed State")]
        // public void EnterScreenFirstPlacedState()
        // {
        //     Debug.Log("CoreManager.EnterScreenFirstPlacedState");
        //     SharedARState.CoreState = CoreScannerState.ModifyingScreen;
        //     
        //     //Stop scanning; Disable AllPlaneInteraction
        //     planeDisplayManager.StopPlaneScan();
        //     planeDisplayManager.EnableAllPlaneInteraction(false);
        //     
        //     //Allow/Show ScreenObjectMoveAround UI
        //     
        // }

        // [ContextMenu("-ScreenRelocation State")]
        // public void EnterScreenPlaceModificationState()
        // {
        //     
        //     Debug.Log("CoreManager.EnterScreenPlaceModificationState");
        //     SharedARState.CoreState = CoreScannerState.ModifyingScreen;
        // }

        [ContextMenu("Debug Print Shared States")]
        private void DebugPrintARStates()
        {
            Debug.LogWarning($"{SharedARState.ToString()}");
        }
        #endregion

        #region Debug Force ChangeState
        public void DebugForceGoToScanningScreenState()
        {
            myFSM.ChangeState(1);
        }
        #endregion
    }
}
