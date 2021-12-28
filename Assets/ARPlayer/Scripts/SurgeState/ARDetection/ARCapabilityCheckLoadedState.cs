using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace ARPlayer.Scripts.SurgeState.ARDetection
{
    public class ARCapabilityCheckLoadedState : State
    {
        [SerializeField] private Text txt;
    
        //[SerializeField] private Twee
        private void OnEnable()
        {
            var targetTxt = txt.text;
            txt.text = "";
        
            txt.DOText(targetTxt, 1f, true, ScrambleMode.None);
        }

        private void OnDisable()
        {
        
        }
    }
}
