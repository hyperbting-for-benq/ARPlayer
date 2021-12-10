using Pixelplacement;

namespace ARPlayer.Scripts.SurgeState
{
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
}
