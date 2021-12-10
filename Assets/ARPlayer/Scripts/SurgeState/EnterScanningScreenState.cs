using System;
using ARPlayer.Scripts;
using UnityEngine;
using Pixelplacement;

public class EnterScanningScreenState : State
{
    private void OnEnable()
    {
        CoreManager.SharedARManager.EnterScanningScreenState();
    }

    private void OnDisable()
    {
        CoreManager.SharedARManager.LeaveScanningScreenState();
    }
}
