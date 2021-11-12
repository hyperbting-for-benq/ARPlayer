using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public static SharedARState sharedARState;
    
    public GameObject horizontalTopObjectPrefab;
    public GameObject horizontalBottomObjectPrefab;
    public GameObject verticalObjectPrefab;
    
    private void OnEnable()
    {
        sharedARState = new SharedARState();
    }

    private void OnDisable()
    {
        sharedARState = null;
    }
}
