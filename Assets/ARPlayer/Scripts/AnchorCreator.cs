using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARPlayer.Scripts
{
    public class AnchorCreator : MonoBehaviour
    {
        // ARAnchor CreateAnchor(ARRaycastHit hit) // ARAnchor CreateAnchor(in ARRaycastHit hit)
        // {
        //     ARAnchor anchor = null;
        //
        //     // If we hit a plane, try to "attach" the anchor to the plane
        //     if (hit.trackable is ARPlane plane && CoreManager.SharedARManager.MyARPlaneManager)
        //     {
        //         Debug.Log("Creating anchor attachment.");
        //         var myARAnchorManager = CoreManager.SharedARManager.MyARAnchorManager;
        //         var oldPrefab = myARAnchorManager.anchorPrefab;
        //         myARAnchorManager.anchorPrefab = prefab;
        //         anchor = myARAnchorManager.AttachAnchor(plane, hit.pose);
        //         myARAnchorManager.anchorPrefab = oldPrefab;
        //         
        //         Debug.Log($"anchor:{anchor.ToString()} Attached to plane {plane.trackableId}");
        //         return anchor;
        //     }
        //
        //     // Otherwise, just create a regular anchor at the hit pose
        //     Debug.LogWarning("Creating regular anchor.");
        //
        //     // Note: the anchor can be anywhere in the scene hierarchy
        //     var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
        //
        //     // Make sure the new GameObject has an ARAnchor component
        //     if (!gameObject.TryGetComponent(out anchor))
        //     {
        //         anchor = gameObject.AddComponent<ARAnchor>();
        //     }
        //
        //     //SetAnchorText(anchor, $"Anchor (from {hit.hitType})");
        //     Debug.Log($"anchor:{anchor.ToString()} (from {hit.hitType})");
        //     return anchor;
        // }

        void Update()
        {
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (!CoreManager.SharedARManager.MyARRaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            {
                return;
            }
            
            // Raycast hits are sorted by distance, so the first one will be the closest hit.
            var hit = s_Hits[0];
            if (!hit.trackable.gameObject.activeSelf)
                return;

            // Create a new anchor
            CoreManager.SharedARManager.OnARRaycastHit(hit);
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    }
}
