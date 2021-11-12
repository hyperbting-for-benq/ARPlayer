using System;
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

    private void Update()
    {
        if (_arPlaneManager != null)
            CoreManager.sharedARState.CurrentDetectionMode = _arPlaneManager.currentDetectionMode;
    }

    #region Scan
    public void StopPlaneScan()
    {
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        _arPlaneManager.planePrefab = null;
        
        //Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }
    
    public void SetHorizontal()
    {
        _arPlaneManager.planePrefab = horizontalPlanePrefab;
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
        ShowHorizontalPlaneOnly();
        
        //Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }

    public void SetVertical()
    {
        _arPlaneManager.planePrefab = verticalPlanePrefab;
        _arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
        ShowVerticalPlaneOnly();
        
        //Debug.Log(_arPlaneManager.currentDetectionMode.ToString());
    }
    #endregion
    
    #region Plane Display
    public void ShowAllPlane(bool show)
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(show);
        }
        CoreManager.sharedARState.currentDisplayMode = PlaneDisplayMode.All;
    }

    public void ShowVerticalPlaneOnly()
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(plane.alignment == PlaneAlignment.Vertical);
        }
        CoreManager.sharedARState.currentDisplayMode = PlaneDisplayMode.Vertical;
    }

    public void ShowHorizontalPlaneOnly()
    {
        foreach (var plane in _arPlaneManager.trackables)
        {
            var toShow = plane.alignment == PlaneAlignment.HorizontalDown || 
                         plane.alignment == PlaneAlignment.HorizontalUp;
            plane.gameObject.SetActive(toShow);
        }
        
        CoreManager.sharedARState.currentDisplayMode = PlaneDisplayMode.Horizontal;
    }
    #endregion
}