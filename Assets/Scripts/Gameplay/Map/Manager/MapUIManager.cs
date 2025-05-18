using Michsky.UI.Reach;
using MyGame.Gameplay.Prop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class MapUIManager
    {
        private TextMeshProUGUI wealthText;
        private FeedNotification successNotification;
        private QuestItem failedNotification;
        private WealthAttribute wealthData;

        public MapUIManager(TextMeshProUGUI text, FeedNotification success, QuestItem failed, WealthAttribute wealth)
        {
            wealthText = text;
            successNotification = success;
            failedNotification = failed;
            wealthData = wealth;
        }

        public void UpdateWealthText()
        {
            float[] wealths = wealthData.Wealths;
            wealthText.text = $"{wealths[0]}+{wealths[1]}({String.Format("{0:P}", wealths[2])})";
        }

        public void ShowSuccessMessage(string key)
        {
            if (successNotification != null)
                successNotification.SetLocalizationKey(key);
        }

        public void ShowFailedMessage(string key)
        {
            if (failedNotification != null)
                failedNotification.SetLocalizationKey(key);
        }
    }

}


