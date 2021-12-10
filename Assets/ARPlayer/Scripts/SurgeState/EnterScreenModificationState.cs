using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class EnterScreenModificationState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log("EnterScreenModificationState.OnEnable");
            
            // TryGet Vertical Anchor OR GOTOPreviousState

            //TODO: Allow/Show ScreenObjectMoveAround UI
            //CoreManager.SharedARManager.EnterScreenModificationState();
        }

        private void OnDisable()
        {
            Debug.Log("EnterScreenModificationState.OnDisable");
            //CoreManager.SharedARManager.LeaveScreenModificationState();
        }
    }
}