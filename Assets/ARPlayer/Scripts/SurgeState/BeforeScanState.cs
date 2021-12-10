using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class BeforeScanState : Pixelplacement.State
    {
        private void OnEnable()
        {
            CoreManager.SharedARManager.EnterBeforeScanState();
        }

        private void OnDisable()
        {
            CoreManager.SharedARManager.LeaveBeforeScanState();
        }
    }
}