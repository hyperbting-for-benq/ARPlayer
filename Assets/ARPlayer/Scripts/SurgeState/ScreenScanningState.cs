using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ScreenScanningState : Pixelplacement.State
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
}