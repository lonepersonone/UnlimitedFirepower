using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityScene = UnityEngine.SceneManagement.Scene;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;


namespace MyGame.Framework.Transition
{
    // 加载阶段枚举
    public enum SceneLoadPhase
    {
        ShowingProgressUI,      // 显示进度界面
        Initializing,           // 初始化 
        HidingProgressUI,       // 隐藏进度界面
        ScalingCamera,          // 缩放相机
        SpawningUI,             // 生成UI
        Finalizing,             // 完成
        Ready                   // 就绪
    }

    // 场景加载流程控制器
    public class SceneLoadProcessController : MonoBehaviour
    {
        public static SceneLoadProcessController Instance { get; private set; }

        [Header("配置")]
        public float defaultPhaseDuration = 1.0f;
        public float sceneActivationDelay = 0.5f;

        // 阶段事件字典,一个事件类型拥有三个阶段
        private Dictionary<SceneLoadPhase, List<Action>> phaseStartEvents = new Dictionary<SceneLoadPhase, List<Action>>();
        private Dictionary<SceneLoadPhase, List<Func<IEnumerator>>> phaseProcessEvents = new Dictionary<SceneLoadPhase, List<Func<IEnumerator>>>();
        private Dictionary<SceneLoadPhase, List<Action>> phaseCompleteEvents = new Dictionary<SceneLoadPhase, List<Action>>();

        private SceneLoadPhase currentPhase = SceneLoadPhase.Initializing;
        private bool isLoading = false;
        private string currentSceneName;
        private Coroutine activeCoroutine;

        private AsyncOperation sceneLoadOperation;
        private bool sceneIsReady = false;

        // 场景加载完成事件
        public Action<string> OnSceneFullyLoaded;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化事件字典
            InitializeEventDictionaries();

            // 注册场景加载完成回调
            UnitySceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            UnitySceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // 初始化事件字典
        private void InitializeEventDictionaries()
        {
            foreach (SceneLoadPhase phase in System.Enum.GetValues(typeof(SceneLoadPhase)))
            {
                phaseStartEvents[phase] = new List<Action>();
                phaseProcessEvents[phase] = new List<Func<IEnumerator>>();
                phaseCompleteEvents[phase] = new List<Action>();
            }
        }

        #region 事件注册与注销
        // 注册阶段开始事件
        public void RegisterPhaseStartEvent(SceneLoadPhase phase, Action callback)
        {
            if (!phaseStartEvents.ContainsKey(phase))
            {
                phaseStartEvents[phase] = new List<Action>();
            }

            phaseStartEvents[phase].Add(callback);
        }

        // 注册阶段处理事件（可异步）
        public void RegisterPhaseProcessEvent(SceneLoadPhase phase, Func<IEnumerator> coroutineCallback)
        {
            if (!phaseProcessEvents.ContainsKey(phase))
            {
                phaseProcessEvents[phase] = new List<Func<IEnumerator>>();
            }

            phaseProcessEvents[phase].Add(coroutineCallback);
        }

        // 注册阶段完成事件
        public void RegisterPhaseCompleteEvent(SceneLoadPhase phase, Action callback)
        {
            if (!phaseCompleteEvents.ContainsKey(phase))
            {
                phaseCompleteEvents[phase] = new List<Action>();
            }

            phaseCompleteEvents[phase].Add(callback);
        }

        // 注销阶段开始事件
        public void UnregisterPhaseStartEvent(SceneLoadPhase phase, Action callback)
        {
            if (phaseStartEvents.ContainsKey(phase))
            {
                phaseStartEvents[phase].Remove(callback);
            }
        }

        // 注销阶段处理事件
        public void UnregisterPhaseProcessEvent(SceneLoadPhase phase, Func<IEnumerator> coroutineCallback)
        {
            if (phaseProcessEvents.ContainsKey(phase))
            {
                phaseProcessEvents[phase].Remove(coroutineCallback);
            }
        }

        // 注销阶段完成事件
        public void UnregisterPhaseCompleteEvent(SceneLoadPhase phase, Action callback)
        {
            if (phaseCompleteEvents.ContainsKey(phase))
            {
                phaseCompleteEvents[phase].Remove(callback);
            }
        }

        // 清理特定阶段的所有事件
        public void ClearPhaseEvents(SceneLoadPhase phase)
        {
            if (phaseStartEvents.ContainsKey(phase))
            {
                phaseStartEvents[phase].Clear();
            }

            if (phaseProcessEvents.ContainsKey(phase))
            {
                phaseProcessEvents[phase].Clear();
            }

            if (phaseCompleteEvents.ContainsKey(phase))
            {
                phaseCompleteEvents[phase].Clear();
            }
        }

        // 清理所有事件
        public void ClearAllEvents()
        {
            foreach (SceneLoadPhase phase in System.Enum.GetValues(typeof(SceneLoadPhase)))
            {
                ClearPhaseEvents(phase);
            }
        }
        #endregion

        #region 流程控制
        // 开始场景加载流程
        public void StartLoadingProcess(string sceneName, bool isReady, Action onComplete = null)
        {
            if (isLoading)
            {
                Debug.LogWarning("已有加载流程正在进行中！");
                return;
            }

            currentSceneName = sceneName;
            isLoading = true;
            currentPhase = SceneLoadPhase.ShowingProgressUI;
            sceneIsReady = isReady;

            // 开始执行加载流程
            activeCoroutine = StartCoroutine(ExecuteLoadingProcess(onComplete));
        }


        // 执行加载流程
        private IEnumerator ExecuteLoadingProcess(Action onComplete)
        {
            // 触发场景加载开始事件
            GameEventManager.TriggerEvent(GameEventType.SceneLoadStarted, currentSceneName);

            // 显示进度UI阶段
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.ShowingProgressUI));

            // 初始化阶段（加载场景）
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Initializing));

            // 确保场景完全加载完成
            yield return new WaitUntil(() => sceneIsReady);
            yield return new WaitForSeconds(sceneActivationDelay); // 额外延迟，确保所有物体初始化完成

            // 隐藏进度UI阶段
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.HidingProgressUI));

            // 后续阶段...
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.ScalingCamera));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.SpawningUI));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Finalizing));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Ready));

            // 加载完成
            isLoading = false;
            onComplete?.Invoke();

            // 触发场景加载完成事件
            GameEventManager.TriggerEvent(GameEventType.SceneLoadCompleted, currentSceneName);
            OnSceneFullyLoaded?.Invoke(currentSceneName);
        }

        // 执行单个阶段
        private IEnumerator ExecutePhase(SceneLoadPhase phase)
        {
            currentPhase = phase;

            // 触发阶段开始事件
            TriggerPhaseStartEvents(phase);

            // 执行阶段处理事件
            yield return StartCoroutine(ExecutePhaseProcessEvents(phase));

            // 触发阶段完成事件
            TriggerPhaseCompleteEvents(phase);
        }


        // 触发阶段开始事件
        private void TriggerPhaseStartEvents(SceneLoadPhase phase)
        {
            if (phaseStartEvents.ContainsKey(phase))
            {
                foreach (var callback in phaseStartEvents[phase])
                {
                    callback?.Invoke();
                }
            }
        }

        // 执行阶段处理事件
        private IEnumerator ExecutePhaseProcessEvents(SceneLoadPhase phase)
        {
            if (phaseProcessEvents.ContainsKey(phase))
            {
                for(int i = 0; i < phaseProcessEvents[phase].Count; i++)
                {
                    if (phaseProcessEvents[phase][i] != null)
                    {
                        yield return StartCoroutine(phaseProcessEvents[phase][i]());
                    }
                }
            }
            else
            {
                // 如果没有注册处理事件，等待默认时间
                yield return new WaitForSeconds(defaultPhaseDuration);
            }
        }

        // 触发阶段完成事件
        private void TriggerPhaseCompleteEvents(SceneLoadPhase phase)
        {
            if (phaseCompleteEvents.ContainsKey(phase))
            {
                foreach (var callback in phaseCompleteEvents[phase])
                {
                    callback?.Invoke();
                }
            }
        }

        // 暂停加载流程
        public void PauseLoading()
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
        }

        // 继续加载流程
        public void ResumeLoading()
        {
            if (isLoading && activeCoroutine == null)
            {
                activeCoroutine = StartCoroutine(ExecuteLoadingProcess(null));
            }
        }

        // 获取当前加载阶段
        public SceneLoadPhase GetCurrentPhase()
        {
            return currentPhase;
        }

        // 检查是否正在加载
        public bool IsLoading()
        {
            return isLoading;
        }
        #endregion

        #region 跨场景检测
        // 场景加载完成回调
        private void OnSceneLoaded(UnityScene scene, LoadSceneMode mode)
        {
            if (scene.name == currentSceneName)
            {
                Debug.Log($"场景 {scene.name} 已加载，但可能尚未完全初始化");

                // 检查场景是否真正准备好
                StartCoroutine(CheckSceneReadiness(scene));
            }
        }

        // 检查场景是否真正准备好
        private IEnumerator CheckSceneReadiness(UnityScene scene)
        {
            // 等待一帧，确保场景激活
            yield return null;

            // 可以添加额外的检查，例如：
            // - 等待所有MonoBehaviour的Start()方法执行完毕
            // - 等待特定的初始化组件完成工作

            // 示例：等待场景中的所有异步操作完成
            yield return new WaitForSeconds(0.2f); // 给场景一些时间完成初始化

            // 检查场景中的关键系统是否已初始化
            bool criticalSystemsInitialized = CheckCriticalSystems(scene);

            if (criticalSystemsInitialized)
            {
                sceneIsReady = true;
                Debug.Log($"场景 {scene.name} 已完全准备好");
            }
            else
            {
                // 如果关键系统未初始化，继续等待
                yield return new WaitUntil(() => CheckCriticalSystems(scene));
                sceneIsReady = true;
                Debug.Log($"场景 {scene.name} 已完全准备好");
            }
        }

        // 检查场景中的关键系统是否已初始化
        private bool CheckCriticalSystems(UnityScene scene)
        {
            // 这里可以添加具体的检查逻辑
            // 例如：查找场景中的关键管理器组件是否已激活

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject root in rootObjects)
            {
                // 示例：检查是否存在特定的管理器
                if (root.GetComponent<GameSystemBase>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }

}


