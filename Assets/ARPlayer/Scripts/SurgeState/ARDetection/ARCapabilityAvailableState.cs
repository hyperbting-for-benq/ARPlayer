using ARPlayer.Scripts.Detection;

using Lean.Gui;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Events;

namespace ARPlayer.Scripts.SurgeState.ARDetection
{
    public class ARCapabilityAvailableState : State
    {
        [SerializeField] private LeanWindow leaWin;
        
        [Header("Debug purpose")]
        [SerializeField] [MyBox.ReadOnly] private ARCapabilityDetection arCapabilityDetection;

        UnityAction ToSpecificState
        {
            get
            {
                //TODO: Camera Permission Check
                return arCapabilityDetection.GoToCameraCheck;   
             
                //TODO: Or Skip Camera Check?
                //return arCapabilityDetection.GoToARMain;
            }
        }
        
        #region Unity LifeCycle
        private void OnEnable()
        {
            Debug.Log("ARCapabilityAvailableState.OnEnable");
            arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();
            
            leaWin?.OnOff.AddListener(ToSpecificState);
            
            //popup modal
            leaWin?.TurnOn();
        }

        private void OnDisable()
        {
            Debug.Log("ARCapabilityAvailableState.OnDisable");
            
            leaWin?.OnOff.RemoveListener(ToSpecificState);
        }
        #endregion
    }
}
