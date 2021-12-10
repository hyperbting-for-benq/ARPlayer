using System;
using Pixelplacement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts.Data
{
    public class SharedARManager
    {
        public CoreManager coreManager;
        public SharedARState sharedState;
        public ARSessionOrigin arsessionOrigin; 
        public ARSession arsession;

        #region Constructor/ Destructor
        public SharedARManager()
        {
            OnARRaycastHit += DefaultOnARRaycastHit;
        }

        ~SharedARManager()
        {
            OnARRaycastHit -= DefaultOnARRaycastHit;
        }
        #endregion
        
        #region
        public void ResetArSession()
        {
            arsession.Reset();
        }
        
        public void TryRemoveProjector()
        {
            if (!sharedState.IsHorizontalObjectSet())
                return;

            GameObject.Destroy(sharedState.HorizontalObject);
        }
        
        public void TryRemoveScreen()
        {
            if (!sharedState.IsVerticalObjectSet())
                return;

            GameObject.Destroy(sharedState.VerticalObject);
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
        public Action<ARRaycastHit> OnARRaycastHit;
        void DefaultOnARRaycastHit(ARRaycastHit arrh)
        {
            Debug.Log($"OnARRaycastHit: {arrh.trackableId} {arrh.hitType} {arrh.pose}");
        }
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
                
            Debug.Log($"anchor:{anchor.ToString()} Attached to plane {plane.trackableId}");

            return true;
        }

        public bool RemoveAnchor(ARAnchor anchor)
        {
            return RemoveAnchor(anchor.sessionId);
        }
        
        public bool RemoveAnchor(Guid guid)
        {
            if (!sharedState.anchors.TryGetValue(guid, out var anchor))
                return false;
            
            Debug.Log($"RemoveAnchor {guid}");
            
            GameObject.Destroy(anchor.gameObject);
            
            sharedState.anchors.Remove(guid);

            return true;
        }

        public void CleanAnchors()
        {
            foreach (var kvp in sharedState.anchors)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            
            sharedState.anchors.Clear();
        }
        #endregion
        
        public void EnterScanningScreenState()
        {
            Debug.Log("SharedARManager.EnterScanningScreenState");
            
            //Allow Scanning Vertical;
            //Allow Vertical Place Interaction;
            coreManager.planeDisplayManager.SetVerticalScanningAndInteraction();
            
            OnARRaycastHit += ScanningScreen_OnARRaycastHit;
            sharedState.OnVerticalObjectPlaced += ScanningScreen_OnVerticalObjectPlaced;
        }
        
        public void LeaveScanningScreenState()
        {
            Debug.Log("SharedARManager.LeaveScanningScreenState");
            
            //Stop Scanning Vertical;
            //Stop Place Interaction;
            coreManager.planeDisplayManager.StopPlaneScan();
            coreManager.planeDisplayManager.EnableAllPlaneInteraction(false);
            
            OnARRaycastHit -= ScanningScreen_OnARRaycastHit;
            sharedState.OnVerticalObjectPlaced -= ScanningScreen_OnVerticalObjectPlaced;
        }

        private void ScanningScreen_OnARRaycastHit(ARRaycastHit arRHit)
        {
            if (sharedState.VerticalObject != null)
            {
                Debug.LogWarning($"SharedARManager.ScanningScreen_OnARRaycastHit VerticalObject Assigned");
                return;
            }
            
            //Assign Anchor AS VerticalObject
            if (TryAttachARAnchor(arRHit, out var aranchor))
            {
                sharedState.VerticalObject = aranchor;
            }
        }

        private void ScanningScreen_OnVerticalObjectPlaced()
        {
            coreManager.myFSM.Next();
        }
    }
}
