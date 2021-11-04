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
                yield return ARSession.CheckAvailability();
                break;
            case ARSessionState.Unsupported:
                //
                break;
            default:
                arSession.enabled = true;
                break;
        }
    }
}
