using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // 过渡到新创建的主场景
    public class MainSceneTransitionHandler : SceneTransitionHandlerBase
    {
        public override string SceneName => "MainScene";

        protected override IEnumerator OnInitializing()
        {
            // 使用DynamicSceneManager加载主场景
            DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"开始加载主场景: {name}"),
                (name, id) => Debug.Log($"主场景加载完成: {name}")
            );

            // 等待场景加载完成（这里需要根据DynamicSceneManager的实现调整）
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName(SceneName).isLoaded);
        }

        protected override void OnScalingCamera()
        {
            Debug.Log("缩放主场景Camera...");

            GameEventManager.TriggerEvent(GameEventType.ScaleCamera);
        }

        protected override IEnumerator OnSpawningUI()
        {
            Debug.Log("生成主场景UI...");

            // 主场景特定的UI生成逻辑
            yield return new WaitForSeconds(0.8f);

            // 触发UI生成完成事件
            GameEventManager.TriggerEvent(GameEventType.SpawnUI);

            yield return new WaitForSeconds(2F);
        }

        protected override void OnReady()
        {
            GameEventManager.TriggerEvent(GameEventType.LevelCompleted);
        }

    }
}


