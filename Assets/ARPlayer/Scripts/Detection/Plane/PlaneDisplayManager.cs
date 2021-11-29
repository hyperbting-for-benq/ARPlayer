using ARPlayer.Scripts.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Detection
{
    public class PlaneDisplayManager : MonoBehaviour
    {
        [SerializeField]private ARPlaneManager _arPlaneManager;

        private void Update()
        {
            if (_arPlaneManager != null)
                CoreManager.SharedARState.CurrentDetectionMode = _arPlaneManager.currentDetectionMode;
        }

        #region Scan
        public void StopPlaneScan()
        {
            _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
            _arPlaneManager.planePrefab = null;
        }
    
        public void SetHorizontalScanningAndInteraction()
        {
            _arPlaneManager.planePrefab = CoreManager.SharedARState.HorizontalObjectPrefab;//horizontalPlanePrefab;
            _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
            ShowHorizontalPlaneOnly();
        }

        public void SetVerticalScanningAndInteraction()
        {
            _arPlaneManager.planePrefab = CoreManager.SharedARState.VerticalObjectPrefab;
            _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
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
            foreach (var plane in _arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(enabled);
            }
        }
        
        public void EnableVerticalPlaneInteraction()
        {
            foreach (var plane in _arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(plane.alignment == PlaneAlignment.Vertical);
            }
        }
        
        public void EnableHorizontalPlaneInteraction()
        {
            foreach (var plane in _arPlaneManager.trackables)
            {
                var toShow = plane.alignment == PlaneAlignment.HorizontalDown || 
                             plane.alignment == PlaneAlignment.HorizontalUp;
                plane.gameObject.SetActive(toShow);
            }
        }
        #endregion
    }
}