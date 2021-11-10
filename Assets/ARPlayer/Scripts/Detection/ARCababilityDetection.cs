using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class ARCababilityDetection : MonoBehaviour
{
    [SerializeField] private ARSession arSession;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        switch (ARSession.state)
        {
            case ARSessionState.None:
            case ARSessionState.CheckingAvailability:
                Debug.Log("CheckAvailability");
                yield return ARSession.CheckAvailability();
                break;
            default:
                Debug.LogWarning("Unexpected States");
                break;
        }
        
        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
            Debug.Log("Unsupported");
        }
        else
        {
            // Start the AR session
            arSession.enabled = true;
            Debug.Log("End");
        }
    }
}