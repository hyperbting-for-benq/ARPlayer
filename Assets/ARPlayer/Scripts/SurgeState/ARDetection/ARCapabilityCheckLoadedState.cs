using ARPlayer.Scripts.Detection;
using DG.Tweening;
using Lean.Gui;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace ARPlayer.Scripts.SurgeState.ARDetection
{
    public class ARCapabilityCheckLoadedState : State
    {
        [SerializeField] private Text txt;
        [SerializeField] private LeanWindow arCapCheckLeaWin;
        [SerializeField] private LeanWindow arCapDeniedLeaWin;
        [Space]
        [SerializeField] private LeanButton arCapDeniedLeaBtn;
        
        [Header("Debug purpose")]
        [SerializeField] [MyBox.ReadOnly] private ARCapabilityDetection arCapabilityDetection;
        private Sequence _txtTweenerSeq;
        
        private void OnEnable()
        {
            arCapabilityDetection = GetComponentInParent<ARCapabilityDetection>();
            arCapDeniedLeaBtn.OnClick.AddListener(CheckARCapbility);

            CheckARCapbility();
        }

        private void OnDisable()
        {
            _txtTweenerSeq?.Kill();
            
            arCapCheckLeaWin.TurnOff();
            arCapDeniedLeaWin.TurnOff();

            arCapDeniedLeaBtn.interactable = false;
            arCapDeniedLeaBtn.OnClick.RemoveListener(CheckARCapbility);
        }

        private void CheckARCapbility()
        {
            arCapDeniedLeaWin.TurnOff();
            arCapCheckLeaWin.TurnOn();
            
            BuildScrambleModeText();
            
            StartCoroutine(arCapabilityDetection.DetectARCapability(
                    () => {
                        if (arCapabilityDetection.CheckPlaneDetectionCapability())
                        {
                            arCapabilityDetection.GoToARCapbilityEnabled();
                        }
                        else
                        {
                            Debug.LogError("Cannot DetectPlane!");
                            
                            arCapDeniedLeaBtn.interactable = true;
                            
                            arCapCheckLeaWin.TurnOff();
                            arCapDeniedLeaWin.TurnOn();
                        }
                    },
                    () => {
                        arCapDeniedLeaBtn.interactable = true;
                        
                        arCapCheckLeaWin.TurnOff();
                        arCapDeniedLeaWin.TurnOn();
                    },
                    () => {
                        Debug.LogError("Unexpected State Detected!");
                    }
                ) 
            );
        }

        private void BuildScrambleModeText()
        { 
            var targetTxt = txt.text;
            
            _txtTweenerSeq?.Kill(true);
            
            _txtTweenerSeq = DOTween.Sequence();
            _txtTweenerSeq
                .Append(txt.DOText("", 0f, false, ScrambleMode.None))
                .Append(txt.DOText(targetTxt, 0.35f, true, ScrambleMode.None))
                .AppendInterval(0.15f)
                .SetLoops(-1, LoopType.Yoyo)
                .OnComplete(() => { _txtTweenerSeq = null;});
            
            _txtTweenerSeq.Play();
        }
    }
}
