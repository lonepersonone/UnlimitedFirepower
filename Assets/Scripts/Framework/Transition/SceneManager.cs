using MyGame.Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{

    // 应用启动后注册所有事件
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 注册场景处理器工厂方法
            RegisterSceneHandlers();
        }

        private void RegisterSceneHandlers()
        {
            SceneTransitionFactory.RegisterHandlerFactory("MainScene", () => new MainSceneTransitionHandler());
            SceneTransitionFactory.RegisterHandlerFactory("MainSceneE", () => new MainSceneTransitionHandlerE());
            SceneTransitionFactory.RegisterHandlerFactory("BattleScene", () => new BattleSceneTransitionHandler());
            SceneTransitionFactory.RegisterHandlerFactory("GalaxySceneE", () => new GalaxySceneTransitionHandlerE());

            //SceneTransitionFactory.RegisterHandlerFactory("LobbyScene", () => new LobbySceneTransitionHandler());
            //SceneTransitionFactory.RegisterHandlerFactory("LevelScene", () => new LevelSceneTransitionHandler());
            //`SceneTransitionFactory.RegisterHandlerFactory("MapScene", () => new MapSceneTransitionHandler());
        }

        // 加载场景方法
        public void LoadScene(string sceneName, bool isReady, Action onComplete = null)
        {
            // 创建场景处理器
            ISceneTransitionHandler handler = SceneTransitionFactory.CreateHandler(sceneName);

            if (handler != null)
            {
                // 设置场景加载事件
                handler.SetupEvents();

                // 开始加载流程
                SceneLoadProcessController.Instance.StartLoadingProcess(sceneName, isReady, () => {
                    // 清理事件
                    handler.CleanupEvents();

                    // 调用完成回调
                    onComplete?.Invoke();
                });
            }
            else
            {
                Debug.LogError($"无法加载场景 {sceneName}：未找到对应的处理器");
            }
        }
    }

}

