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
        private PlaneDetectionMode m_CurrentDetectionMode = PlaneDetectionMode.None;
        public Action<PlaneDetectionMode> OnCurrentDetectionModeChanged = (pdm) => {
            Debug.Log($"SharedARState.DefaultOnCurrentDetectionModeChanged [{pdm}]");
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
        // public Dictionary<Guid, ARAnchor> anchors = new Dictionary<Guid, ARAnchor>();
        //
        // [ContextMenu("DebugPrint ARAnchor")]
        // private void DebugPrintArAnchors()
        // {
        //     Debug.LogWarning($"{JsonConvert.SerializeObject(anchors)}");
        // }
        
        //Projector in two type
        #region HorizontalObject as Projector
        public ARAnchorState HorizontalObject = new ARAnchorState("HorizontalObject");
        [ContextMenu("Debug HorizontalObject")]
        private void DebugPrintHorizontalObject()
        {
            Debug.LogWarning(HorizontalObject.ToString());
        }
        #endregion
        
        #region VerticalObject as Screen
        public ARAnchorState VerticalObject = new ARAnchorState("VerticalObject");
        [ContextMenu("Debug VerticalObject")]
        private void DebugPrintVerticalObject()
        {
            Debug.LogWarning(VerticalObject.ToString());
        }
        #endregion
        
        public void CleanAnchors()
        {
            HorizontalObject.Anchor = null; //HorizontalObject=null;
            VerticalObject.Anchor = null; //VerticalObject=null;
        }

        public void DrawlineBetweenVerticalHorizontalObject()
        {
            if(!HorizontalObject.IsSet() || !VerticalObject.IsSet())
            {
                return;
            }

            VerticalObject.SetupLine(HorizontalObject.Anchor.transform.position);
            HorizontalObject.SetupLine(VerticalObject.Anchor.transform.position);
        }

        public void SetHorizontalObjectFacingVerticalObject()
        {
             if (!HorizontalObject.IsSet())
             {
                 Debug.LogWarning("HorizontalObject_Facing_VerticalObject HorizontalObject_NotFound");
                 return;
             }
             
             if (!VerticalObject.IsSet())
             {
                 Debug.LogWarning("HorizontalObject_Facing_VerticalObject VerticalObject_NotFound");
                 return;
             }

             var haw = HorizontalObject.AnchorWorker; 
             if ( haw == null)
             {
                 Debug.LogWarning("HorizontalObject_Facing_VerticalObject HorizontalObject_ARAnchorWorker_NotFound");
                 return;
             }

             var vaw = VerticalObject.AnchorWorker; 
             if (vaw == null)
             {
                 Debug.LogWarning("HorizontalObject_Facing_VerticalObject VerticalObject_ARAnchorWorker_NotFound");
                 return;
             }
             
             haw.SetFacingRefTransform(vaw.root);
        }

        [JsonIgnore] public GameObject HorizontalObjectPrefab => coreManager.horizontalPlanePrefab;
        [JsonIgnore] public GameObject VerticalObjectPrefab => coreManager.verticalPlanePrefab;
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
    
    public class ARAnchorState
    {
        public string Name;
        public Action OnObjectPlaced;

        private ARAnchorWorker m_ARAnchorWorker;
        public ARAnchorWorker AnchorWorker
        {
            get => m_ARAnchorWorker;
        }

        private ARAnchor m_ARAnchor;
        public ARAnchor Anchor
        {
            get => m_ARAnchor;
            set
            {
                if (value == null && m_ARAnchor != null)
                    GameObject.Destroy(m_ARAnchor.gameObject);

                m_ARAnchor = value;
                if (value != null)
                {
                    m_ARAnchorWorker = value.GetComponent<ARAnchorWorker>();
                    OnObjectPlaced?.Invoke();
                }
            }
        }

        public override string ToString()
        {
            return string.Format("ARAnchorState:{0}, nullARAnchor?{1}, IsSet:{2}", Name, m_ARAnchor==null, IsSet());
        }
            
        public bool IsSet()
        {
            return m_ARAnchor!=null;
        }

        public ARAnchorState(string name)
        {
            Name = name;
        }

        public void SetupLine<T>(T data)
        {
            if (!IsSet())
            {
                Debug.Log($"ARAnchorState.SetupLine CannotFindObjectSet");
                return;
            }
            
            if (m_ARAnchorWorker == null)
            {
                Debug.Log($"ARAnchorState.SetupLine CannotFindARAnchorWorker");
                return;
            }
            
            switch (data)
            {
                case Vector3 v3:
                    m_ARAnchorWorker.SetupLine(v3);
                    break;
                case float fl:
                    m_ARAnchorWorker.SetupLine(fl);
                    break;
                default:
                    Debug.LogWarning($"ARAnchorState.SetupLine UnexpectedDataType Found: {data.GetType()}");
                    break;
            }
        }
    }
}