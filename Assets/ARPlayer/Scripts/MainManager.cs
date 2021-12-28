using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Pixelplacement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [Space] 
    [SerializeField] private GameObject CheckerLoaded;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    #region SceneManagement
    public void LoadScene_ARCheck()
    {
        _ = AsyncLoadScene_ARCheck(
            () => { stateMachine.Next(CheckerLoaded);},
            () => { Debug.LogError("Fail to Load Scene ARCapabilityCheck");});
    }
    
    public void LoadScene_ARMain()
    {
        _ = SceneManager.LoadSceneAsync("200ARMain", LoadSceneMode.Single);
    }
    
    public async Task<bool> AsyncLoadScene_ARCheck(Action success, Action fail)
    {
        var asyncLoad = SceneManager.LoadSceneAsync("010ARCapability", LoadSceneMode.Additive);
        await Task.Delay(100);
        
        var cts = new CancellationTokenSource(30000);
        while (!cts.IsCancellationRequested)
        {
            await Task.Delay(100, cts.Token);
            
            // Wait until the asynchronous scene fully loads
            if (asyncLoad.isDone)
                cts.Cancel();
        }

        if (asyncLoad.isDone)
        {
            success?.Invoke();
            return true;
        }
        
        fail?.Invoke();
        return false;
    }
    #endregion
}
