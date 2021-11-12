using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class SharedARState
{
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
    public GameObject horizontalTopObject;
    public GameObject horizontalBottomObject;
    
    //Screen here
    public GameObject verticalObject;
    
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
}

public enum PlaneDisplayMode
{
    None,
    All,
    Vertical,
    Horizontal
}