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
        private WealthAttribute wealthData;

        public MapUIManager(TextMeshProUGUI text, WealthAttribute wealth)
        {
            wealthText = text;
            wealthData = wealth;
        }

        public void UpdateWealthText()
        {
            float[] wealths = wealthData.Wealths;
            wealthText.text = $"{wealths[0]}+{wealths[1]}({String.Format("{0:P}", wealths[2])})";
        }

    }

}


