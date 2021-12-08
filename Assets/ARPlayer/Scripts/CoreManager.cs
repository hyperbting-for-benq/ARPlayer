using ARPlayer.Scripts.Data;
using ARPlayer.Scripts.Detection;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class CoreManager : MonoBehaviour
    {
        public static SharedARState SharedARState;
        public static SharedARManager SharedARManager;
        
        public GameObject horizontalPlanePrefab;
        public GameObject verticalPlanePrefab;

        public GameObject ARAnchorPrefab;
        
        public GameObject horizontalObjectPrefab;
        public GameObject verticalObjectPrefab;
    
        private void OnEnable()
        {
            SharedARState = new SharedARState { coreManager = this };
            SharedARManager = new SharedARManager { coreManager = this, arsessionOrigin = m_arsessionOrigin, arsession = m_arsession };

            EnterScanningScreenState();
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
        [SerializeField]private ARSessionOrigin m_arsessionOrigin;
        [SerializeField]private ARSession m_arsession;
        
        [ContextMenu("-Reset CoreScannerState")]
        public void ResetCoreScannerState()
        {
            SharedARState.CoreState = CoreScannerState.BeforeScan;
            
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
            m_arsession.Reset();
        }
        
        private PlaneDisplayManager m_planeDisplayManager;
        public PlaneDisplayManager planeDisplayManager
        {
            get
            {
                if (m_planeDisplayManager == null)
                    m_planeDisplayManager = GetComponent<PlaneDisplayManager>();
                
                return m_planeDisplayManager;
            }
        }

        [ContextMenu("-ScanningScreenAnPlace State")]
        public void EnterScanningScreenState()
        {
            Debug.Log("CoreManager.EnterScanningScreenState");
            SharedARState.CoreState = CoreScannerState.PlacingScreen;
            
            //Allow Scanning Vertical;
            //Allow Vertical Place Interaction;
            planeDisplayManager.SetVerticalScanningAndInteraction();
        }
        
        [ContextMenu("-ScreenPointed State")]
        public void EnterScreenFirstPlacedState()
        {
            Debug.Log("CoreManager.EnterScreenFirstPlacedState");
            SharedARState.CoreState = CoreScannerState.ModifyingScreen;
            
            //Stop scanning; Disable AllPlaneInteraction
            planeDisplayManager.StopPlaneScan();
            planeDisplayManager.EnableAllPlaneInteraction(false);
            
            //Allow/Show ScreenObjectMoveAround UI
            
        }
        
        [ContextMenu("-ScreenRelocation State")]
        public void EnterScreenPlaceModificationState()
        {
            
            Debug.Log("CoreManager.EnterScreenPlaceModificationState");
            SharedARState.CoreState = CoreScannerState.ModifyingScreen;
        }
    }
}
