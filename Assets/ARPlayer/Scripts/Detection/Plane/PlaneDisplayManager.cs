using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDisplayManager : MonoBehaviour
{
    [SerializeField]private ARPlaneManager _arPlaneManager;
    public GameObject verticalPlanePrefab;
    public GameObject horizontalPlanePrefab;
    
    #region Scan
    public void StopPlaneScan()
    {
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        _arPlaneManager.planePrefab = null;
        
        Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }
    
    public void SetHorizontal()
    {
        _arPlaneManager.planePrefab = horizontalPlanePrefab;
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
        ShowOnlyPlane(PlaneAlignment.HorizontalUp);
        Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }

    public void SetVertical()
    {
        _arPlaneManager.planePrefab = verticalPlanePrefab;
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
        ShowOnlyPlane(PlaneAlignment.Vertical);
        Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }
    #endregion
    
    #region Display
    public void ShowAllPlane(bool show)
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(show);
        }
    }

    public void ShowVerticalPlaneOnly()
    {
        ShowOnlyPlane(PlaneAlignment.Vertical);
    }

    public void ShowHorizontakPlaneOnly()
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalDown || plane.alignment == PlaneAlignment.HorizontalUp )
            {
                plane.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(false);
            }
        }
    }
    
    public void ShowOnlyPlane(PlaneAlignment alignment)
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            if (plane.alignment == alignment)
            {
                plane.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(false);
            }
        }
    }
    #endregion
}