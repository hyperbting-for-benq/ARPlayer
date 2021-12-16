using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUser : MonoBehaviour, INotificationUser
{
    [SerializeField] private Text notificationTxt;

    [SerializeField] private  LeanPulse lPulse;
    public void ShowNotification(string content)
    {
        if(notificationTxt!=null)
            notificationTxt.text = content;
        
        lPulse.Pulse();
    }
}

public interface INotificationUser
{
    void ShowNotification(string content);
}