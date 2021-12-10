using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class EnterScanningScreenState : Pixelplacement.State
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