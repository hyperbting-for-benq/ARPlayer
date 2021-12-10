using Pixelplacement;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class EnterScreenModificationState : State
    {
        private void OnEnable()
        {
            Debug.Log("EnterScreenModificationState.OnEnable");
            
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
