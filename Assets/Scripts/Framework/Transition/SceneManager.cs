using MyGame.Framework.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{

    // Ӧ��������ע�������¼�
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

            // ע�᳡����������������
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

        // ���س�������
        public void LoadScene(string sceneName, bool isReady, Action onComplete = null)
        {
            // ��������������
            ISceneTransitionHandler handler = SceneTransitionFactory.CreateHandler(sceneName);

            if (handler != null)
            {
                // ���ó��������¼�
                handler.SetupEvents();

                // ��ʼ��������
                SceneLoadProcessController.Instance.StartLoadingProcess(sceneName, isReady, () => {
                    // �����¼�
                    handler.CleanupEvents();

                    // ������ɻص�
                    onComplete?.Invoke();
                });
            }
            else
            {
                Debug.LogError($"�޷����س��� {sceneName}��δ�ҵ���Ӧ�Ĵ�����");
            }
        }
    }

}

