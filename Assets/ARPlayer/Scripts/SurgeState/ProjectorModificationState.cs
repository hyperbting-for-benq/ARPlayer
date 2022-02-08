using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ProjectorModificationState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log("ProjectorModificationState.OnEnable");
            CoreManager.SharedARManager.EnterModifyingProjectorState();
        }

        private void OnDisable()
        {
            Debug.Log("ProjectorModificationState.OnDisable");
            CoreManager.SharedARManager.LeaveModifyingProjectorState();
        }
    }
}