using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Notification
{
    public class NotificationSettingController : MonoBehaviour
    {
        [SerializeField] private SwitchManager switchManager;

        private void Start()
        {
            switchManager.isOn = NotificationManager.Instance.isActive;
            switchManager.UpdateUI();
            switchManager.onValueChanged.AddListener(NotificationManager.Instance.SetActive);
        }


    }
}


