using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class ARAnchorWorker : MonoBehaviour
    {
        public Transform root;
        
        public Transform verticalBase;
        public Transform horizontalTopBase;
        public Transform horizontalBottomBase;

        public void PlaceObject(ObjectOrientation objOrientation, Transform tarTransform)
        {
            switch (objOrientation)
            {
                case ObjectOrientation.Vertical:
                    tarTransform.parent = verticalBase;
                    break;
                case ObjectOrientation.HorizontalBottom:
                    tarTransform.parent = horizontalBottomBase;
                    break;
                case ObjectOrientation.HorizontalTop:
                    tarTransform.parent = horizontalTopBase;
                    break;
                default:
                case ObjectOrientation.None:
                    break;
            }
            
            tarTransform.localPosition = Vector3.zero;
            tarTransform.localRotation = Quaternion.identity;
            tarTransform.localScale = Vector3.one;
            
            SetFacingRefTransform();
        }

        public void SetFacingRefTransform(Transform tarTransform=null)
        {
            if (tarTransform == null)
                tarTransform = Camera.main.transform;
            
            var fro = horizontalBottomBase.GetComponent<FacingRefObject>();
            fro.refTransform = tarTransform;
            
            var fro2 = horizontalTopBase.GetComponent<FacingRefObject>();
            fro2.refTransform = tarTransform;
        }

        public enum ObjectOrientation
        {
            None,
            Vertical,
            HorizontalTop,
            HorizontalBottom
        }
    }
}
