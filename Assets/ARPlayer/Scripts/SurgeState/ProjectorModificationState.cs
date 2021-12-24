using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ProjectorModificationState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log("ProjectorModificationState.OnEnable");
            //CoreManager.SharedARManager.EnterModifyingScreenState();
        }

        private void OnDisable()
        {
            Debug.Log("ProjectorModificationState.OnDisable");
            //CoreManager.SharedARManager.LeaveModifyingScreenState();
        }
    }
}