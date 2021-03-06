using ARPlayer.Scripts.Data;
using ARPlayer.Scripts.Detection;
using Newtonsoft.Json.Serialization;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public partial class CoreManager : MonoBehaviour
    {
        public static SharedARState SharedARState;
        public static SharedARManager SharedARManager;

        public StateMachine myFSM;

        public NotificationUser notificationUser;
        
        [Header("Object Prefab")]
        public GameObject horizontalPlanePrefab;
        public GameObject verticalPlanePrefab;
        [Space]
        public GameObject ARAnchorPrefab;
        [Space]
        public GameObject ceilingProjectorPrefab;
        public GameObject floorProjectorPrefab;
        public GameObject wallScreenPrefab;
    
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
            SharedARManager = null;
        }

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
        
        #region FSM Usage
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
        #endregion

        #region Debug States

        [Header("Debug GridStay")]
        [SerializeField] private bool debugGridStay = false;
        public bool DebugGridStay
        {
            get => debugGridStay;
            set
            {
                debugGridStay = value;
            }
        }

        [ContextMenu("Debug Placed Objects")]
        private void DebugPrintPlacedObjects()
        {
            Debug.LogWarningFormat("DebugPrintPlacedObjects: HorizontalObjectSet:{0}, VerticalObjectSet:{1}",
                CoreManager.SharedARState.HorizontalObject.IsSet(),
                CoreManager.SharedARState.VerticalObject.IsSet()
            );
            
            // Debug.LogWarningFormat("DebugPrintPlacedObjects: HorizontalObject:{0}, VerticalObject:{1}",
            //     CoreManager.SharedARState.HorizontalObject,
            //     CoreManager.SharedARState.VerticalObject
            // );
        }
        #endregion
    }
}
