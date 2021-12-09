using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class EnterScreenModificationState : State
{
    private void OnEnable()
    {
        Debug.Log("EnterScreenModificationState.OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log("EnterScreenModificationState.OnDisable");
    }
}
