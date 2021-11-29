using ARPlayer.Scripts.Data;
using ARPlayer.Scripts.Detection;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class CoreManager : MonoBehaviour
    {
        public static SharedARState SharedARState;
        
        public GameObject horizontalPlanePrefab;
        public GameObject verticalPlanePrefab;
        
        public GameObject horizontalObjectPrefab;
        public GameObject verticalObjectPrefab;
    
        private void OnEnable()
        {
            SharedARState = new SharedARState { coreManager = this };
        }

        private void OnDisable()
        {
            SharedARState = null;
        }
    
        #region Debug
        [ContextMenu("Debug Print Shared States")]
        private void DebugPrintARStates()
        {
            Debug.LogWarning($"{SharedARState.ToString()}");
        }
        #endregion

        [Header("Script Ref")]
        [SerializeField]private ARSession arsession;
        
        [ContextMenu("Reset CoreScannerState")]
        public void ResetCoreScannerState()
        {
            SharedARState.coreState = CoreScannerState.BeforeScan;
            
            //Remove Screen, Projector
            if(SharedARState.VerticalObject != null)
            {
                Destroy(SharedARState.VerticalObject);
            }
            
            if(SharedARState.HorizontalObject != null)
            {
                Destroy(SharedARState.HorizontalObject);
            }
            
            //Remove Planes
            arsession.Reset();
        }
        [SerializeField]private PlaneDisplayManager planeDisplayManager;
        [ContextMenu("ScanningScreenAnPlace State")]
        public void EnterScanningScreenState()
        {
            SharedARState.coreState = CoreScannerState.PlacingScreen;
            
            //Allow Scanning Vertical;
            //Allow Vertical Place Interaction;
            planeDisplayManager.SetVerticalScanningAndInteraction();
        }
        
        [ContextMenu("ScreenPointed State")]
        public void EnterScreenFirstPlacedState()
        {
            SharedARState.coreState = CoreScannerState.ModifyingScreen;
            
            //Stop scanning
            planeDisplayManager.StopPlaneScan();
            planeDisplayManager.EnableAllPlaneInteraction(false);
            
            //Allow/Show ScreenObject Move Around by Screen?
            
        }
        
        [ContextMenu("ScreenRelocation State")]
        public void EnterScreenPlaceModificationState()
        {
            SharedARState.coreState = CoreScannerState.ModifyingScreen;
        }
    }
}
