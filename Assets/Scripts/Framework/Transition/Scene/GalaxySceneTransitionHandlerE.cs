using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public class GalaxySceneTransitionHandlerE : SceneTransitionHandlerBase
    {
        public override string SceneName => "MainScene";

        protected override IEnumerator OnInitializing()
        {
            GameEventManager.TriggerEvent(GameEventType.LevelStarted);
            yield return new WaitForSeconds(2F);
        }

        protected override void OnReady()
        {
            GameEventManager.TriggerEvent(GameEventType.LevelCompleted);
        }

    }
}


