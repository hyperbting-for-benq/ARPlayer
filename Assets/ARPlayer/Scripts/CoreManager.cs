using ARPlayer.Scripts.Data;
using ARPlayer.Scripts.Detection;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class CoreManager : MonoBehaviour
    {
        public static SharedARState SharedARState;
        public static SharedARManager SharedARManager;

        public StateMachine myFSM;
        
        public GameObject horizontalPlanePrefab;
        public GameObject verticalPlanePrefab;

        public GameObject ARAnchorPrefab;

        public NotificationUser notificationUser;
        
        [Header("Object Prefab")]
        public GameObject horizontalObjectPrefab;
        public GameObject verticalObjectPrefab;
    
        private void OnEnable()
        {
            SharedARState = new SharedARState { coreManager = this };
            SharedARManager = new SharedARManager { 
                coreManager = this, 
                sharedState = SharedARState,
                arsessionOrigin = m_arsessionOrigin, 
                arsession = m_arsession };
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

        #region Debug ContextMenu
        public void ResetState()
        {
            myFSM.ChangeState(myFSM.defaultState); 
        }

        public void NextState()
        {
            myFSM.Next();
        }
        
        public void PreviousState()
        {
            myFSM.Previous();
        }
        
        public void GoToScanningScreenState()
        {
            myFSM.ChangeState(1);
        }

        [ContextMenu("-BeforeScan State")]
        public void EnterBeforeScanState()
        {
            SharedARManager.EnterBeforeScanState();
        }
        
        [ContextMenu("-ScanningScreenAnPlace State")]
        public void EnterScanningScreenState()
        {
            SharedARManager.EnterScanningScreenState();
        }
        
        [ContextMenu("-End ScanningScreenAnPlace State")]
        public void LeaveScanningScreenState()
        {
            SharedARManager.LeaveScanningScreenState();
        }
        
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
        #endregion
    }
}
