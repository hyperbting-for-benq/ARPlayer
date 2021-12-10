using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts.Data
{
    [System.Serializable]
    public class SharedARState
    {
        #region Constructor/ Destructor
        public SharedARState()
        {
        }
        
        ~SharedARState()
        {
        }
        #endregion
        
        [JsonIgnore] public CoreManager coreManager;

        #region CurrentDetectionMode
        private PlaneDetectionMode m_CurrentDetectionMode;
        public Action<PlaneDetectionMode> OnCurrentDetectionModeChanged = (pdm)=>{
            Debug.Log($"SharedARState.DefaultOOnCurrentDetectionModeChanged [{pdm}]");
        };
        public PlaneDetectionMode CurrentDetectionMode
        {
            get => m_CurrentDetectionMode;
            set
            {
                if (value == m_CurrentDetectionMode)
                    return;

                m_CurrentDetectionMode = value;
                OnCurrentDetectionModeChanged?.Invoke(m_CurrentDetectionMode);
            }
        }
        #endregion
    
        public PlaneDisplayMode currentDisplayMode;

        #region InWorld ARAnchor
        //Projector in two type
        private ARAnchor m_HorizontalObject;
        public ARAnchor HorizontalObject
        {
            get => m_HorizontalObject;
            set
            {
                if (m_HorizontalObject != null && value == m_HorizontalObject)
                    return;

                m_HorizontalObject = value;
                OnHorizontalObjectPlaced?.Invoke();
            }
        }
        [ContextMenu("Debug HorizontalObject")]
        private void DebugHorizontalObject()
        {
            Debug.Log($"HorizontalObject?:{m_HorizontalObject==null}; {m_HorizontalObject?.ToString()}");
        }
        
        public bool IsHorizontalObjectSet()
        {
            return !m_HorizontalObject;
        }
        
        public Action OnHorizontalObjectPlaced;

        //Screen here
        private ARAnchor m_VerticalObject;
        public ARAnchor VerticalObject
        {
            get => m_VerticalObject;
            set
            {
                if (m_VerticalObject != null && value == m_VerticalObject)
                    return;

                m_VerticalObject = value;
                OnVerticalObjectPlaced?.Invoke();
            }
        }

        [ContextMenu("Debug VerticalARAnchor")]
        private void DebugVerticalARAnchor()
        {
            Debug.Log($"VerticalObject?:{m_VerticalObject==null}; {m_VerticalObject?.ToString()}");
        }

        public bool IsVerticalObjectSet()
        {
            return !m_VerticalObject;
        }
        
        public Action OnVerticalObjectPlaced;
        
        [JsonIgnore] public GameObject HorizontalObjectPrefab => coreManager.horizontalPlanePrefab;
        [JsonIgnore] public GameObject VerticalObjectPrefab => coreManager.verticalPlanePrefab;

        public Dictionary<Guid, ARAnchor> anchors = new Dictionary<Guid, ARAnchor>();
        public void CleanAnchors()
        {
            foreach (var kvp in anchors)
            {
                GameObject.Destroy(kvp.Value.gameObject);
            }
            
            anchors.Clear();
        }
        #endregion
        
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

    // public enum CoreScannerState
    // {
    //     Unknown,
    //     BeforeScan,
    //     ScanningVertical,
    //     PlacingScreen,
    //     ModifyingScreen,
    //     PlacingProjector,
    //     ModifyingProjector
    // }
}