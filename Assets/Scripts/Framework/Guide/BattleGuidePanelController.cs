using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Framework.Event;


namespace MyGame.Framework.Guide
{
    public class BattleGuidePanelController : GuidePanelController
    {
        private void Start()
        {
            GameEventManager.RegisterListener(GameEventType.BattleInitial, ShowGuidePanel);
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.BattleInitial, ShowGuidePanel);
        }

    }
}

