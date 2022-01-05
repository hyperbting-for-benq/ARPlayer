using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class FacingRefObject : MonoBehaviour
{
    public Transform refTransform;
    public bool ignorePitch = true;
    private void Update()
    {
        LookAt(refTransform);
    }

    [SerializeField][MyBox.ReadOnly] private Tweener _tweener;
    void LookAt(Transform refTransform)
    {
        if (refTransform == null)
            return;
        
        if (_tweener!=null && _tweener.IsPlaying())
            return;

        var ac = AxisConstraint.None;
        if (ignorePitch)
            ac = AxisConstraint.Y;
        
        _tweener = transform.DOLookAt(refTransform.position, 1f, ac);
    }
}
