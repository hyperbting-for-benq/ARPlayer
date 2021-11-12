using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void LoadScene_ARCheck()
    {
        _ = SceneManager.LoadSceneAsync("ARCapability", LoadSceneMode.Additive);
    }
    
    public void LoadScene_ARMain()
    {
        _ = SceneManager.LoadSceneAsync("ARMain", LoadSceneMode.Single);
    }
}
