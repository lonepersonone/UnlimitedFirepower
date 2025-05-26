using MyGame.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // ���󳡾�ת������������
    public abstract class SceneTransitionHandlerBase : ISceneTransitionHandler
    {
        public abstract string SceneName { get; }

        public virtual void SetupEvents()
        {
            // ע��ͨ���¼�
            RegisterPhaseEvents();
        }

        public virtual void CleanupEvents()
        {
            // ��������ע����¼�
            SceneLoadProcessController.Instance.ClearAllEvents();

            Debug.Log("Clear All Events");
        }

        // ע��׶��¼�
        protected virtual void RegisterPhaseEvents()
        {
            // ע����ʾ����UI�¼�
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.ShowingProgressUI, OnShowingProgressUI);

            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.ShowingProgressUI, OnLoadingData);

            // ע���ʼ���׶��¼�
            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.Initializing, OnInitializing);

            // ע�����ؽ���UI�¼�
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.HidingProgressUI, OnHidingProgressUI);

            // ע����������¼�
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.ScalingCamera, OnScalingCamera);

            // ע��UI�����¼�
            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.SpawningUI, OnSpawningUI);

            // ע������¼�
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.Ready, OnReady);
        }

        // �׶��¼�������
        protected  void OnShowingProgressUI()
        {
            Debug.Log("���ɽ�������ʾ");
            SceneTransitionManager.Instance.EnableInitScreen();
        }

        protected virtual IEnumerator OnLoadingData()
        {
            Debug.Log("��ͣ��������");
            yield return new WaitForSecondsRealtime(2f);
        }

        protected virtual IEnumerator OnInitializing()
        {
            yield return null;
        }

        protected virtual void OnHidingProgressUI()
        {
            Debug.Log("���ɽ���������");
            SceneTransitionManager.Instance.DisableInitScreen();
        }

        protected virtual void OnScalingCamera()
        {
            // Ĭ�ϵ���������߼�

        }

        protected virtual IEnumerator OnSpawningUI()
        {
            yield return null;
        }

        protected virtual void OnReady()
        {
            Debug.Log($"{SceneName} ������׼������");
        }
    }
}


