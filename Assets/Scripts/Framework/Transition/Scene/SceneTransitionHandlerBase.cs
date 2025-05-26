using MyGame.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    // 抽象场景转换处理器基类
    public abstract class SceneTransitionHandlerBase : ISceneTransitionHandler
    {
        public abstract string SceneName { get; }

        public virtual void SetupEvents()
        {
            // 注册通用事件
            RegisterPhaseEvents();
        }

        public virtual void CleanupEvents()
        {
            // 清理所有注册的事件
            SceneLoadProcessController.Instance.ClearAllEvents();

            Debug.Log("Clear All Events");
        }

        // 注册阶段事件
        protected virtual void RegisterPhaseEvents()
        {
            // 注册显示进度UI事件
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.ShowingProgressUI, OnShowingProgressUI);

            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.ShowingProgressUI, OnLoadingData);

            // 注册初始化阶段事件
            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.Initializing, OnInitializing);

            // 注册隐藏进度UI事件
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.HidingProgressUI, OnHidingProgressUI);

            // 注册相机缩放事件
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.ScalingCamera, OnScalingCamera);

            // 注册UI生成事件
            SceneLoadProcessController.Instance.RegisterPhaseProcessEvent(SceneLoadPhase.SpawningUI, OnSpawningUI);

            // 注册完成事件
            SceneLoadProcessController.Instance.RegisterPhaseStartEvent(SceneLoadPhase.Ready, OnReady);
        }

        // 阶段事件处理方法
        protected  void OnShowingProgressUI()
        {
            Debug.Log("过渡界面已显示");
            SceneTransitionManager.Instance.EnableInitScreen();
        }

        protected virtual IEnumerator OnLoadingData()
        {
            Debug.Log("暂停加载数据");
            yield return new WaitForSecondsRealtime(2f);
        }

        protected virtual IEnumerator OnInitializing()
        {
            yield return null;
        }

        protected virtual void OnHidingProgressUI()
        {
            Debug.Log("过渡界面已隐藏");
            SceneTransitionManager.Instance.DisableInitScreen();
        }

        protected virtual void OnScalingCamera()
        {
            // 默认的相机缩放逻辑

        }

        protected virtual IEnumerator OnSpawningUI()
        {
            yield return null;
        }

        protected virtual void OnReady()
        {
            Debug.Log($"{SceneName} 场景已准备就绪");
        }
    }
}


