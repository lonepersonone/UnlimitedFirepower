using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.Reach;

namespace MyGame.Framework.Guide
{
    public class MapGuidePanelController : GuidePanelController
    {
        private void Start()
        {
            GameEventManager.RegisterListener(GameEventType.GameStarted, ShowGuidePanel);
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.GameStarted, ShowGuidePanel);
        }
    }

}

