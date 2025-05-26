using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Notification
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        // Notification组件
        [SerializeField] private FeedNotification feedNotification;

        public bool isActive = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ShowFeedNotification(string key)
        {
            if(isActive)
                feedNotification.SetLocalizationKey(key);
        }


        public void SetActive(bool value)
        {
            isActive = value;

            Debug.Log($"设置提醒状态 {value}");
        }

    }

}



