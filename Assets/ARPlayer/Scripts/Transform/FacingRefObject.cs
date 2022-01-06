using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FacingRefObject : MonoBehaviour
{
    public bool ignorePitch = true;
    
    [Header("Debug Purpose")]
    [SerializeField][MyBox.ReadOnly] private Transform refTransform;
    [SerializeField][MyBox.ReadOnly] private Vector3 refPoint;

    #region Unity LifeCycle
    private void OnDisable()
    {
        _tweener?.Kill(true);
    }

    private void FixedUpdate()
    {
        UpdateRefTransform();
            
        LookAt(refPoint);
    }
    #endregion

    public void SetRefTransform(Transform refTra)
    {
        refTransform = refTra;
    }

    public void SetRefPoint(Vector3 refPt)
    {
        refTransform = null;
        refPoint = refPt;
    }
    
    private void UpdateRefTransform()
    {
        if (refTransform == null)
            return;

        refPoint = refTransform.position;
    }

    [SerializeField][MyBox.ReadOnly] private Tweener _tweener;
    private void LookAt(Vector3 refPos)
    {
        if (_tweener!=null)
            return;
        
        var ac = AxisConstraint.None;
        if (ignorePitch)
            ac = AxisConstraint.Y;
        
        _tweener = transform
            .DOLookAt(refPos, 1f, ac)
            .OnComplete( ()=>{_tweener = null;} );
    }
}