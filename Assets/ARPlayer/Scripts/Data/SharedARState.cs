using System;
using System.Collections;
using System.Collections.Generic;
using ARPlayer.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Data
{
    [System.Serializable]
    public class SharedARState
    {
        [JsonIgnore] public CoreManager coreManager;

        #region CoreScannerState
        public CoreScannerState coreState = CoreScannerState.Unknown;
        #endregion
            
        #region CurrentDetectionMode
        private PlaneDetectionMode _currentDetectionMode;
        public Action<PlaneDetectionMode> OnCurrentDetectionModeChanged = (pdm)=>{
            Debug.Log($"OnCurrentDetectionModeChanged {pdm}");
        };
        public PlaneDetectionMode CurrentDetectionMode
        {
            get => _currentDetectionMode;
            set
            {
                if (value == _currentDetectionMode)
                    return;

                _currentDetectionMode = value;
                OnCurrentDetectionModeChanged?.Invoke(_currentDetectionMode);
            }
        }
        #endregion
    
        public PlaneDisplayMode currentDisplayMode;

        //Projector in two type
        private GameObject horizontalObject;
        public Action OnHorizontalObjectPlaced;
        public GameObject HorizontalObject
        {
            get => horizontalObject;
            set
            {
                if (horizontalObject != null && value == horizontalObject)
                    return;

                horizontalObject = value;
                OnHorizontalObjectPlaced?.Invoke();
            }
        }

        //Screen here
        private GameObject verticalObject;
        public Action OnVerticalObjectPlaced;
        public GameObject VerticalObject
        {
            get => verticalObject;
            set
            {
                if (verticalObject != null && value == verticalObject)
                    return;

                verticalObject = value;
                OnVerticalObjectPlaced?.Invoke();
            }
        }
        [JsonIgnore] public GameObject HorizontalObjectPrefab => coreManager.horizontalPlanePrefab;
        [JsonIgnore] public GameObject VerticalObjectPrefab => coreManager.verticalPlanePrefab;
    
        #region Checker
        public bool IsDisplayingVerticalPlane()
        {
            switch (currentDisplayMode)
            {
                case PlaneDisplayMode.All:
                case PlaneDisplayMode.Vertical:
                    return true;    
            }
            return false;
        }
    
        public bool IsDisplayingHorizontalPlane()
        {
            switch (currentDisplayMode)
            {
                case PlaneDisplayMode.All:
                case PlaneDisplayMode.Horizontal:
                    return true;    
            }
        
            return false;
        }
        #endregion
    
        public override string ToString()
        {
            //return JsonUtility.ToJson(this);
            return JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                }
            );
        }
    }

    public enum PlaneDisplayMode
    {
        None,
        All,
        Vertical,
        Horizontal
    }

    public enum CoreScannerState
    {
        Unknown,
        BeforeScan,
        ScanningVertical,
        PlacingScreen,
        ModifyingScreen,
        PlacingProjector,
        ModifyingProjector
    }
}