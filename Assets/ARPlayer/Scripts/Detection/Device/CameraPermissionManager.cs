using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPermissionManager : MonoBehaviour
{
        #region Camera Permission
        public bool HaveCameraPermission
        {
            get
            {
                #if UNITY_ANDROID
                return UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera);
                #else
                return Application.HasUserAuthorization(UserAuthorization.WebCam);
                #endif
            }
        }
        
        public IEnumerator RequestCameraPermission(Action success, Action fail)
        {
            yield return new WaitForSeconds(1f);
            
            if (!HaveCameraPermission)
            {
                yield return new WaitForSeconds(2f);
                
                
                #if UNITY_ANDROID
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
                #else
                yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
                #endif
            }

            if (HaveCameraPermission)
            {
                Debug.Log("Camera Permission Authorized!");
                ListWebCams();
                
                success?.Invoke();
            }
            else
            {
                //TODO:
                fail?.Invoke();
            }
        }
        
        private void ListWebCams()
        {
            foreach (var device in WebCamTexture.devices)
            {
                Debug.Log("Name: " + device.name);
            }
        }
        #endregion
}
