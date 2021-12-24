using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ScreenModificationState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log("ScreenModificationState.OnEnable");
            
            // TryGet Vertical Anchor OR GOTOPreviousState

            //TODO: Allow/Show ScreenObjectMoveAround UI
            CoreManager.SharedARManager.EnterModifyingScreenState();
        }

        private void OnDisable()
        {
            Debug.Log("ScreenModificationState.OnDisable");
            CoreManager.SharedARManager.LeaveModifyingScreenState();
        }
    }
}