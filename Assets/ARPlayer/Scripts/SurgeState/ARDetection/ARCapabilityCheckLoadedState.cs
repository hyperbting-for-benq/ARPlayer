using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace ARPlayer.Scripts.SurgeState.ARDetection
{
    public class ARCapabilityCheckLoadedState : State
    {
        [SerializeField] private Text txt;
    
        [SerializeField] private Sequence txtTweenerSeq;
        private void OnEnable()
        {
            var targetTxt = txt.text;

            txtTweenerSeq = DOTween.Sequence();
            txtTweenerSeq.Append(txt.DOText("", 0f, false, ScrambleMode.None));
            txtTweenerSeq.Append(txt.DOText(targetTxt, 0.5f, true, ScrambleMode.None));
            
            txtTweenerSeq.SetLoops(-1, LoopType.Yoyo);
            txtTweenerSeq.Play();
        }

        private void OnDisable()
        {
            txtTweenerSeq?.Kill();
        }
    }
}
