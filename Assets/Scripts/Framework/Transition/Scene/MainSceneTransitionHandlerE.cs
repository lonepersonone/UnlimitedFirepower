using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // ���ɻ��Ѵ�����������
    public class MainSceneTransitionHandlerE : SceneTransitionHandlerBase
    {
        public override string SceneName => "BattleScene";

        protected override IEnumerator OnInitializing()
        {
            yield return null;

            DynamicSceneManager.Instance.UnloadScene(SceneName, (string sceneName, string instanceId) =>
            {
                Debug.Log($"ս������ {sceneName} (ID: {instanceId}) ж����ɣ�����������");

                GameEventManager.TriggerEvent(GameEventType.SceneObjectShow);

                // ���������������¼�
                //GameEventManager.Instance.TriggerEvent(GameEventType.ReturnedToMainScene);
            });
        }

        protected override void OnReady()
        {

            GameEventManager.TriggerEvent(GameEventType.BattleWined);
        }
    }

}

