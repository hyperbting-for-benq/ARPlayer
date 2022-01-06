using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ARPlayer.Scripts
{
    public class ARAnchorWorker : MonoBehaviour
    {
        public LineDrawer lineDrawer;
        [Space]
        public Transform root;
        [Space]
        public Transform verticalPlane;
        [Space]
        public Transform verticalBase;
        public Transform horizontalTopBase;
        public Transform horizontalBottomBase;

        [ContextMenu("Print Normal")]
        private void DebugPrintNormal()
        {
            Debug.LogWarning($"forward:{transform.forward}");
            
            // build a plane over current transform
            var pl = BuildPlane(transform);
            Debug.LogWarning($"pl.normal:{pl.normal}");
            
            var dis = pl.GetDistanceToPoint(transform.position);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual( dis, 0f);
        }

        public void SetupLine(Vector3 refPoint)
        {
            var plVer = BuildPlane(verticalPlane.transform);
            
            #if UNITY_EDITOR
            var normalPt = plVer.normal + verticalPlane.transform.position;
            var go0 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go0.transform.localScale = Vector3.one*0.1f;
            go0.transform.position = verticalPlane.transform.position;
            var go1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go1.transform.localScale = Vector3.one*0.1f;
            go1.transform.position = normalPt;
            var newNor = (go1.transform.position - go0.transform.position).normalized;
            Debug.LogWarning($"{newNor} vs {plVer.normal}");
            #endif
            
            Debug.LogWarning($" side:{plVer.GetSide(refPoint)}; vs 0 ide:{plVer.GetSide(Vector3.zero)}");
            
            var dis = plVer.GetDistanceToPoint(refPoint);
            lineDrawer.Setup(dis);//lineDrawer.Setup(Math.Abs(dis));
        }

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
            fro.SetRefTransform(tarTransform);
            
            var fro2 = horizontalTopBase.GetComponent<FacingRefObject>();
            fro2.SetRefTransform(tarTransform);
        }

        public Plane BuildPlane(Transform tra)
        {
            return new Plane(-tra.forward, tra.position);
        }

        public Vector3 CalculateShortestPtToReference(Vector3 refPt)
        {
            var plane = BuildPlane(verticalPlane);
            Debug.LogWarning($"distance:{plane.distance} normal:{plane.normal} closePt:{plane.ClosestPointOnPlane(refPt)}");
            
            return plane.ClosestPointOnPlane(refPt);
        }

        [ContextMenu("Debug CalculateShortestPtToReference")]
        private void CalculateShortestPtToReference()
        {
            Debug.LogWarning($"pos:{verticalPlane.position}, for:{verticalPlane.forward}");
            
            var v3_1 = CalculateShortestPtToReference(Vector3.one);
            Debug.LogWarning($"Vector3.one, {v3_1}, {Vector3.Distance(Vector3.one,v3_1)}");
            var pt1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pt1.transform.SetParent(transform);
            pt1.transform.localScale= Vector3.one * 0.1f;
            pt1.transform.position = v3_1;
            
            var v3_2 = CalculateShortestPtToReference(Vector3.up);
            Debug.LogWarning($"Vector3.up, {v3_2} {Vector3.Distance(Vector3.up,v3_2)}");
            var pt2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pt2.transform.SetParent(transform);
            pt2.transform.localScale= Vector3.one * 0.1f;
            pt2.transform.position = v3_2;
            
            var v3_3 = CalculateShortestPtToReference(Vector3.right);
            Debug.LogWarning($"Vector3.right, {v3_3} {Vector3.Distance(Vector3.right,v3_3)}");
            var pt3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pt3.transform.SetParent(transform);
            pt3.transform.localScale= Vector3.one * 0.1f;
            pt3.transform.position = v3_3;

            // var plgo = GameObject.CreatePrimitive(PrimitiveType.Plane);
            // plgo.transform.SetParent(transform);
            // plgo.transform.position = v3_3;
            // plgo.transform.rotation = Quaternion.Euler(Vector3.Cross(v3_2-v3_1, v3_3-v3_1));
            
            var pl = new Plane(v3_1, v3_2, v3_3);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, pl.GetDistanceToPoint(verticalPlane.position));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, pl.GetDistanceToPoint(v3_1));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, pl.GetDistanceToPoint(v3_2));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(0f, pl.GetDistanceToPoint(v3_3));
            Debug.LogWarning($"pl.normal:{pl.normal}, :{pl.GetDistanceToPoint(verticalPlane.position)}, pos:{verticalPlane.position}, for:{verticalPlane.forward}");
            
            var d3_1_2 = pl.ClosestPointOnPlane(Vector3.one);
            Debug.LogWarning($"Vector3.one, {d3_1_2}");
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
