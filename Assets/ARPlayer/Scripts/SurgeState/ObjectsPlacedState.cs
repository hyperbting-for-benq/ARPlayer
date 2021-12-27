using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPlayer.Scripts.SurgeState
{
    public class ObjectsPlacedState : Pixelplacement.State
    {
        private void OnEnable()
        {
            Debug.Log($"{typeof(ObjectsPlacedState)}.OnEnable");
            CoreManager.SharedARManager.EnterObjectsPlacedState();
        }

        private void OnDisable()
        {
            Debug.Log($"{typeof(ObjectsPlacedState)}.OnDisable");
            CoreManager.SharedARManager.LeaveObjectsPlacedState();
        }
    }
}
