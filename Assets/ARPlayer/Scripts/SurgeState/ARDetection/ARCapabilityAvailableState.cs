using Lean.Gui;
using Pixelplacement;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState.ARDetection
{
    public class ARCapabilityAvailableState : State
    {
        [SerializeField] private LeanWindow leaWin;
        private void OnEnable()
        {
            Debug.Log("ARCapabilityAvailableState.OnEnable");
        
            //popup modal
            leaWin?.TurnOn();
        }

        private void OnDisable()
        {
            Debug.Log("ARCapabilityAvailableState.OnDisable");
        }
    }
}
