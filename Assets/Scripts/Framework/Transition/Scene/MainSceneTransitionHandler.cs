using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // ���ɵ��´�����������
    public class MainSceneTransitionHandler : SceneTransitionHandlerBase
    {
        public override string SceneName => "MainScene";

        protected override IEnumerator OnInitializing()
        {
            // ʹ��DynamicSceneManager����������
            DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"��ʼ����������: {name}"),
                (name, id) => Debug.Log($"�������������: {name}")
            );

            // �ȴ�����������ɣ�������Ҫ����DynamicSceneManager��ʵ�ֵ�����
            yield return new WaitUntil(() => UnityEngine.SceneManagement.SceneManager.GetSceneByName(SceneName).isLoaded);
        }

        protected override void OnScalingCamera()
        {
            Debug.Log("����������Camera...");

            GameEventManager.TriggerEvent(GameEventType.ScaleCamera);
        }

        protected override IEnumerator OnSpawningUI()
        {
            Debug.Log("����������UI...");

            // �������ض���UI�����߼�
            yield return new WaitForSeconds(0.8f);

            // ����UI��������¼�
            GameEventManager.TriggerEvent(GameEventType.SpawnUI);

            yield return new WaitForSeconds(2F);
        }

        protected override void OnReady()
        {
            GameEventManager.TriggerEvent(GameEventType.LevelCompleted);
        }

    }
}


