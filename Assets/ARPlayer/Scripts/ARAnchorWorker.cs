using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class ARAnchorWorker : MonoBehaviour
    {
        public Transform VerticalBase;
        public Transform HorizontalTopBase;
        public Transform HorizontalBottomBase;

        public void PlaceObject(ObjectOrientation objOrientation, Transform tarTransform)
        {
            switch (objOrientation)
            {
                case ObjectOrientation.Vertical:
                    tarTransform.parent = VerticalBase;
                    break;
                case ObjectOrientation.HorizontalBottom:
                    tarTransform.parent = HorizontalBottomBase;
                    break;
                case ObjectOrientation.HorizontalTop:
                    tarTransform.parent = HorizontalTopBase;
                    break;
                default:
                case ObjectOrientation.None:
                    break;
            }
            
            tarTransform.localPosition = Vector3.zero;
            tarTransform.localRotation = Quaternion.identity;
            tarTransform.localScale = Vector3.one;
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
