using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // 过渡回已创建的主场景
    public class MainSceneTransitionHandlerE : SceneTransitionHandlerBase
    {
        public override string SceneName => "BattleScene";

        protected override IEnumerator OnInitializing()
        {
            yield return null;

            DynamicSceneManager.Instance.UnloadScene(SceneName, (string sceneName, string instanceId) =>
            {
                Debug.Log($"战斗场景 {sceneName} (ID: {instanceId}) 卸载完成，返回主场景");

                GameEventManager.TriggerEvent(GameEventType.SceneObjectShow);

                // 触发返回主场景事件
                //GameEventManager.Instance.TriggerEvent(GameEventType.ReturnedToMainScene);
            });
        }

        protected override void OnReady()
        {

            GameEventManager.TriggerEvent(GameEventType.BattleWined);
        }
    }

}

