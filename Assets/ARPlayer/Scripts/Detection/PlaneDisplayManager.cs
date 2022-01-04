using ARPlayer.Scripts.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Detection
{
    public class PlaneDisplayManager : MonoBehaviour
    {
        private ARPlaneManager arPlaneManager
        {
            get => CoreManager.SharedARManager.MyARPlaneManager;
        }
        
        // private void Update()
        // {
        //     if (_arPlaneManager != null)
        //         CoreManager.SharedARState.CurrentDetectionMode = _arPlaneManager.currentDetectionMode;
        // }

        #region Scan
        public void StopPlaneScan()
        { 
            arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
            arPlaneManager.planePrefab = null;
        }
    
        public void SetHorizontalScanningAndInteraction()
        {
            arPlaneManager.planePrefab = CoreManager.SharedARState.HorizontalObjectPrefab;//horizontalPlanePrefab;
            arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
            ShowHorizontalPlaneOnly();
        }

        public void SetVerticalScanningAndInteraction()
        {
            arPlaneManager.planePrefab = CoreManager.SharedARState.VerticalObjectPrefab;
            arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
            ShowVerticalPlaneOnly();
        }
        #endregion
    
        #region Plane Display
        public void ShowAllPlane(bool show)
        {
            EnableAllPlaneInteraction(show);
            CoreManager.SharedARState.currentDisplayMode = PlaneDisplayMode.All;
        }

        public void ShowVerticalPlaneOnly()
        {
            EnableVerticalPlaneInteraction();
            CoreManager.SharedARState.currentDisplayMode = PlaneDisplayMode.Vertical;
        }

        public void ShowHorizontalPlaneOnly()
        {
            EnableHorizontalPlaneInteraction();
            CoreManager.SharedARState.currentDisplayMode = PlaneDisplayMode.Horizontal;
        }
        #endregion

        #region Place Interaction
        public void EnableAllPlaneInteraction(bool enabled)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(enabled);
            }
        }
        
        public void EnableVerticalPlaneInteraction()
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(plane.alignment == PlaneAlignment.Vertical);
            }
        }
        
        public void EnableHorizontalPlaneInteraction()
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                var toShow = plane.alignment == PlaneAlignment.HorizontalDown || 
                             plane.alignment == PlaneAlignment.HorizontalUp;
                plane.gameObject.SetActive(toShow);
            }
        }
        #endregion
    }
}