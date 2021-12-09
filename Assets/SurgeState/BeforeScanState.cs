using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class BeforeScanState : State
{
    private void OnEnable()
    {
        Debug.Log("BeforeScanState.OnEnable");
    }

    private void OnDisable()
    {
        Debug.Log("BeforeScanState.OnDisable");
    }
}
