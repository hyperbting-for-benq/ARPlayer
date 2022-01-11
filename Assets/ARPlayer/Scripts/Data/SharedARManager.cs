using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Data
{
    public class SharedARManager
    {
        public CoreManager coreManager;
        public SharedARState sharedState;
        public ARSessionOrigin arsessionOrigin; 
        public ARSession arsession;

        #region Constructor/ Destructor
        CancellationTokenSource periodicUpdateCTS;
        public SharedARManager()
        {
            periodicUpdateCTS = new CancellationTokenSource();
            _ = PeriodicUpdate(periodicUpdateCTS);
        }

        ~SharedARManager()
        {
            periodicUpdateCTS.Cancel();
        }
        
        private async Task PeriodicUpdate(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(100);
                sharedState.CurrentDetectionMode = MyARPlaneManager.currentDetectionMode;
            }
        }
        #endregion
        
        #region
        public void ResetArSession()
        {
            arsession.Reset();
        }
        #endregion
        
        #region script getter
        private ARAnchorManager m_ARAnchorManager;
        public ARAnchorManager MyARAnchorManager
        {
            get
            {
                if(m_ARAnchorManager == null)
                    m_ARAnchorManager = arsessionOrigin.GetComponent<ARAnchorManager>();

                return m_ARAnchorManager;
            }
        }
        
        private ARRaycastManager m_ARRaycastManager;
        public ARRaycastManager MyARRaycastManager
        {
            get
            {
                if (m_ARRaycastManager == null)
                    m_ARRaycastManager = arsessionOrigin.GetComponent<ARRaycastManager>();

                return m_ARRaycastManager;
            }
        }
        
        private ARPlaneManager m_ARPlaneManager;
        public ARPlaneManager MyARPlaneManager
        {
            get
            {
                if (m_ARPlaneManager == null)
                    m_ARPlaneManager = arsessionOrigin.GetComponent<ARPlaneManager>();
                
                return m_ARPlaneManager;
            }
        }
        #endregion
        
        #region ARRaycastHit
        public Action<ARRaycastHit> OnARRaycastHit = (arrh) =>
        {
            Debug.Log($"OnARRaycastHit: {arrh.trackableId} {arrh.hitType} {arrh.pose}");
        };
        #endregion
        
        #region ARAnchor CRUD
        bool TryAttachARAnchor(ARRaycastHit hit, out ARAnchor anchor)
        { 
            anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (!(hit.trackable is ARPlane plane) || !CoreManager.SharedARManager.MyARPlaneManager) 
                return false;
            
            Debug.Log("Creating anchor attachment.");
            var oldPrefab = MyARAnchorManager.anchorPrefab;
            MyARAnchorManager.anchorPrefab = coreManager.ARAnchorPrefab;
            anchor = MyARAnchorManager.AttachAnchor(plane, hit.pose);
            MyARAnchorManager.anchorPrefab = oldPrefab;

            if (anchor == null)
            {
                Debug.LogWarning("anchor null");
                return false;
            }

            Debug.Log($"anchor sessionId:{anchor.sessionId}, Attached to plane trackableId:{plane.trackableId}");
            return true;
        }

        public bool RemoveAnchor(ARAnchor anchor)
        {
            
            //return anchor != null && RemoveAnchor(anchor.sessionId);

            GameObject.Destroy(anchor.gameObject);
            return true;
        }
        #endregion
        
        #region States
        #region BeforeScanState
        public void EnterBeforeScanState()
        {
            Debug.Log("SharedARManager.EnterBeforeScanState");
            //Reset
        
            // Remove Planes
            ResetArSession();
        
            //Remove ARAnchor
            sharedState.CleanAnchors();
        
            //TODO: Show UI to Next State
            coreManager.notificationUser.ShowNotification("EnterBeforeScanState");
        }

        public void LeaveBeforeScanState()
        {
            Debug.Log("SharedARManager.LeaveBeforeScanState");
            //TODO: Close UI Above
        }
        #endregion BeforeScanState
        
        #region ScanningScreenState
        public void EnterScanningScreenState()
        {
            Debug.Log("SharedARManager.EnterScanningScreenState");
            sharedState.VerticalObject.Anchor = null; //ScanningScreen_Reset();
            
            //Allow Scanning Vertical; Allow Vertical Place Interaction;
            coreManager.planeDisplayManager.SetVerticalScanningAndInteraction();
            
            // Register Callbacks
            OnARRaycastHit += ScanningScreen_OnARRaycastHit;
            sharedState.VerticalObject.OnObjectPlaced += ScanningScreen_OnVerticalObjectPlaced;

            coreManager.notificationUser.ShowNotification("EnterScanningScreenState");
        }
        
        public void LeaveScanningScreenState()
        {
            Debug.Log("SharedARManager.LeaveScanningScreenState");
            
            //Stop Scanning Vertical
            coreManager.planeDisplayManager.StopPlaneScan();
            
            //Stop Place Interaction;
            coreManager.planeDisplayManager.EnableAllPlaneInteraction(false);
            
            // Unregister Callbacks
            OnARRaycastHit -= ScanningScreen_OnARRaycastHit;
            sharedState.VerticalObject.OnObjectPlaced -= ScanningScreen_OnVerticalObjectPlaced;
        }

        #region ScanningScreen Callbacks
        private void ScanningScreen_OnARRaycastHit(ARRaycastHit arRHit)
        {
            if (sharedState.VerticalObject.IsSet())
            {
                Debug.LogWarning($"SharedARManager.ScanningScreen_OnARRaycastHit VerticalObject Assigned");
                return;
            }
            
            //Assign Anchor AS VerticalObject
            if (!TryAttachARAnchor(arRHit, out var aranchor))
                return;
            
            // Attach proper object over aranchor
            var go = GameObject.Instantiate<GameObject>(coreManager.wallScreenPrefab);

            var araw = aranchor.GetComponent<ARAnchorWorker>();
            if (araw == null)
                return;
            
            araw.PlaceObject(ARAnchorWorker.ObjectOrientation.Vertical, go.transform);
            sharedState.VerticalObject.Anchor = aranchor;
        }

        private void ScanningScreen_OnVerticalObjectPlaced()
        {
            sharedState.VerticalObject.SetupLine(1.56f);
            
            coreManager.myFSM.Next();
        }
        #endregion ScanningScreen Callbacks
        #endregion
        
        #region ModifyingScreenState
        public void EnterModifyingScreenState()
        {
            Debug.Log("SharedARManager.EnterModifyingScreenState");
        }
        
        public void LeaveModifyingScreenState()
        {
            Debug.Log("SharedARManager.LeaveModifyingScreenState");
        }
        #endregion
        
        #region ScanningProjectorState
        public void EnterScanningProjectorState()
        {
            Debug.Log("SharedARManager.EnterScanningProjectorState");
            sharedState.HorizontalObject.Anchor = null; //ScanningProjector_Reset();
            
            // Register Callback
            OnARRaycastHit += ScanningProjector_OnARRaycastHit;
            sharedState.HorizontalObject.OnObjectPlaced += ScanningProjector_OnHorizontalObjectPlaced;

            coreManager.notificationUser.ShowNotification("EnterScanningProjectorState");
            
            //Allow Scanning Vertical; Allow Vertical Place Interaction;
            coreManager.planeDisplayManager.SetHorizontalScanningAndInteraction();
        }
        
        public void LeaveScanningProjectorState()
        {
            Debug.Log("SharedARManager.LeaveScanningProjectorState");
            
            //Stop Scanning Horizontal
            coreManager.planeDisplayManager.StopPlaneScan();
            
            //Stop Place Interaction;
            coreManager.planeDisplayManager.EnableAllPlaneInteraction(false);
            
            // Unregister Callbacks
            OnARRaycastHit -= ScanningProjector_OnARRaycastHit;
            sharedState.HorizontalObject.OnObjectPlaced -= ScanningProjector_OnHorizontalObjectPlaced;
        }
        
        private void ScanningProjector_OnARRaycastHit(ARRaycastHit arRHit)
        {
            if (sharedState.HorizontalObject.IsSet())
            {
                Debug.LogWarning($"SharedARManager.ScanningProjector_OnARRaycastHit HorizontalObject Assigned");
                return;
            }

            if (!TryAttachARAnchor(arRHit, out var aranchor))
                return;
            
            if (!(arRHit.trackable is ARPlane plane))
            {
                Debug.LogWarning($"ScanningProjector_OnARRaycastHit {arRHit.trackable} Not ARPlane");
                return;
            }
            
            var araw = aranchor.GetComponent<ARAnchorWorker>();
            if (araw == null)
            {
                Debug.LogWarning("ScanningProjector_OnARRaycastHit ARAnchorWorker_NotFound");
                return;
            }
            
            GameObject go;
            switch (plane.alignment)
            {
                case PlaneAlignment.HorizontalDown:
                    go = GameObject.Instantiate(coreManager.ceilingProjectorPrefab);
                    araw.PlaceObject(ARAnchorWorker.ObjectOrientation.HorizontalTop, go.transform);
                    break;
                case PlaneAlignment.HorizontalUp:
                    go = GameObject.Instantiate(coreManager.floorProjectorPrefab);
                    araw.PlaceObject(ARAnchorWorker.ObjectOrientation.HorizontalBottom, go.transform);
                    break;
                default:
                    Debug.LogWarning($"ScanningProjector_OnARRaycastHit Unexpected PlaneAlignment:{plane.alignment}");
                    return;//break;
            }
            
            sharedState.HorizontalObject.Anchor = aranchor;
        }
        
        private void ScanningProjector_OnHorizontalObjectPlaced()
        {
            // Draw line between Vertical and Horizontal Object
            sharedState.DrawlineBetweenVerticalHorizontalObject();
            
            coreManager.myFSM.Next();
        }
        #endregion
        
        #region objectsPlaced
        public void EnterObjectsPlacedState()
        {
            Debug.Log("SharedARManager.EnterObjectsPlacedState");
            
            //Stop Scanning Horizontal
            coreManager.planeDisplayManager.StopPlaneScan();
            
            //Stop Place Interaction;
            coreManager.planeDisplayManager.EnableAllPlaneInteraction(false);

            coreManager.notificationUser.ShowNotification("EnterObjectsPlacedState");
            
            // Let Horizontal z Facing Vertical
            sharedState.SetHorizontalObjectFacingVerticalObject();
        }
        
        public void LeaveObjectsPlacedState()
        {
            Debug.Log("SharedARManager.LeaveObjectsPlacedState");
        }
        #endregion objectsPlaced
        #endregion States
    }
}