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
    // ���ؽ׶�ö��
    public enum SceneLoadPhase
    {
        ShowingProgressUI,      // ��ʾ���Ƚ���
        Initializing,           // ��ʼ�� 
        HidingProgressUI,       // ���ؽ��Ƚ���
        ScalingCamera,          // �������
        SpawningUI,             // ����UI
        Finalizing,             // ���
        Ready                   // ����
    }

    // �����������̿�����
    public class SceneLoadProcessController : MonoBehaviour
    {
        public static SceneLoadProcessController Instance { get; private set; }

        [Header("����")]
        public float defaultPhaseDuration = 1.0f;
        public float sceneActivationDelay = 0.5f;

        // �׶��¼��ֵ�,һ���¼�����ӵ�������׶�
        private Dictionary<SceneLoadPhase, List<Action>> phaseStartEvents = new Dictionary<SceneLoadPhase, List<Action>>();
        private Dictionary<SceneLoadPhase, List<Func<IEnumerator>>> phaseProcessEvents = new Dictionary<SceneLoadPhase, List<Func<IEnumerator>>>();
        private Dictionary<SceneLoadPhase, List<Action>> phaseCompleteEvents = new Dictionary<SceneLoadPhase, List<Action>>();

        private SceneLoadPhase currentPhase = SceneLoadPhase.Initializing;
        private bool isLoading = false;
        private string currentSceneName;
        private Coroutine activeCoroutine;

        private AsyncOperation sceneLoadOperation;
        private bool sceneIsReady = false;

        // ������������¼�
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

            // ��ʼ���¼��ֵ�
            InitializeEventDictionaries();

            // ע�᳡��������ɻص�
            UnitySceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            UnitySceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // ��ʼ���¼��ֵ�
        private void InitializeEventDictionaries()
        {
            foreach (SceneLoadPhase phase in System.Enum.GetValues(typeof(SceneLoadPhase)))
            {
                phaseStartEvents[phase] = new List<Action>();
                phaseProcessEvents[phase] = new List<Func<IEnumerator>>();
                phaseCompleteEvents[phase] = new List<Action>();
            }
        }

        #region �¼�ע����ע��
        // ע��׶ο�ʼ�¼�
        public void RegisterPhaseStartEvent(SceneLoadPhase phase, Action callback)
        {
            if (!phaseStartEvents.ContainsKey(phase))
            {
                phaseStartEvents[phase] = new List<Action>();
            }

            phaseStartEvents[phase].Add(callback);
        }

        // ע��׶δ����¼������첽��
        public void RegisterPhaseProcessEvent(SceneLoadPhase phase, Func<IEnumerator> coroutineCallback)
        {
            if (!phaseProcessEvents.ContainsKey(phase))
            {
                phaseProcessEvents[phase] = new List<Func<IEnumerator>>();
            }

            phaseProcessEvents[phase].Add(coroutineCallback);
        }

        // ע��׶�����¼�
        public void RegisterPhaseCompleteEvent(SceneLoadPhase phase, Action callback)
        {
            if (!phaseCompleteEvents.ContainsKey(phase))
            {
                phaseCompleteEvents[phase] = new List<Action>();
            }

            phaseCompleteEvents[phase].Add(callback);
        }

        // ע���׶ο�ʼ�¼�
        public void UnregisterPhaseStartEvent(SceneLoadPhase phase, Action callback)
        {
            if (phaseStartEvents.ContainsKey(phase))
            {
                phaseStartEvents[phase].Remove(callback);
            }
        }

        // ע���׶δ����¼�
        public void UnregisterPhaseProcessEvent(SceneLoadPhase phase, Func<IEnumerator> coroutineCallback)
        {
            if (phaseProcessEvents.ContainsKey(phase))
            {
                phaseProcessEvents[phase].Remove(coroutineCallback);
            }
        }

        // ע���׶�����¼�
        public void UnregisterPhaseCompleteEvent(SceneLoadPhase phase, Action callback)
        {
            if (phaseCompleteEvents.ContainsKey(phase))
            {
                phaseCompleteEvents[phase].Remove(callback);
            }
        }

        // �����ض��׶ε������¼�
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

        // ���������¼�
        public void ClearAllEvents()
        {
            foreach (SceneLoadPhase phase in System.Enum.GetValues(typeof(SceneLoadPhase)))
            {
                ClearPhaseEvents(phase);
            }
        }
        #endregion

        #region ���̿���
        // ��ʼ������������
        public void StartLoadingProcess(string sceneName, bool isReady, Action onComplete = null)
        {
            if (isLoading)
            {
                Debug.LogWarning("���м����������ڽ����У�");
                return;
            }

            currentSceneName = sceneName;
            isLoading = true;
            currentPhase = SceneLoadPhase.ShowingProgressUI;
            sceneIsReady = isReady;

            // ��ʼִ�м�������
            activeCoroutine = StartCoroutine(ExecuteLoadingProcess(onComplete));
        }


        // ִ�м�������
        private IEnumerator ExecuteLoadingProcess(Action onComplete)
        {
            // �����������ؿ�ʼ�¼�
            GameEventManager.TriggerEvent(GameEventType.SceneLoadStarted, currentSceneName);

            // ��ʾ����UI�׶�
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.ShowingProgressUI));

            // ��ʼ���׶Σ����س�����
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Initializing));

            // ȷ��������ȫ�������
            yield return new WaitUntil(() => sceneIsReady);
            yield return new WaitForSeconds(sceneActivationDelay); // �����ӳ٣�ȷ�����������ʼ�����

            // ���ؽ���UI�׶�
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.HidingProgressUI));

            // �����׶�...
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.ScalingCamera));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.SpawningUI));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Finalizing));
            yield return StartCoroutine(ExecutePhase(SceneLoadPhase.Ready));

            // �������
            isLoading = false;
            onComplete?.Invoke();

            // ����������������¼�
            GameEventManager.TriggerEvent(GameEventType.SceneLoadCompleted, currentSceneName);
            OnSceneFullyLoaded?.Invoke(currentSceneName);
        }

        // ִ�е����׶�
        private IEnumerator ExecutePhase(SceneLoadPhase phase)
        {
            currentPhase = phase;

            // �����׶ο�ʼ�¼�
            TriggerPhaseStartEvents(phase);

            // ִ�н׶δ����¼�
            yield return StartCoroutine(ExecutePhaseProcessEvents(phase));

            // �����׶�����¼�
            TriggerPhaseCompleteEvents(phase);
        }


        // �����׶ο�ʼ�¼�
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

        // ִ�н׶δ����¼�
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
                // ���û��ע�ᴦ���¼����ȴ�Ĭ��ʱ��
                yield return new WaitForSeconds(defaultPhaseDuration);
            }
        }

        // �����׶�����¼�
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

        // ��ͣ��������
        public void PauseLoading()
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
        }

        // ������������
        public void ResumeLoading()
        {
            if (isLoading && activeCoroutine == null)
            {
                activeCoroutine = StartCoroutine(ExecuteLoadingProcess(null));
            }
        }

        // ��ȡ��ǰ���ؽ׶�
        public SceneLoadPhase GetCurrentPhase()
        {
            return currentPhase;
        }

        // ����Ƿ����ڼ���
        public bool IsLoading()
        {
            return isLoading;
        }
        #endregion

        #region �糡�����
        // ����������ɻص�
        private void OnSceneLoaded(UnityScene scene, LoadSceneMode mode)
        {
            if (scene.name == currentSceneName)
            {
                Debug.Log($"���� {scene.name} �Ѽ��أ���������δ��ȫ��ʼ��");

                // ��鳡���Ƿ�����׼����
                StartCoroutine(CheckSceneReadiness(scene));
            }
        }

        // ��鳡���Ƿ�����׼����
        private IEnumerator CheckSceneReadiness(UnityScene scene)
        {
            // �ȴ�һ֡��ȷ����������
            yield return null;

            // ������Ӷ���ļ�飬���磺
            // - �ȴ�����MonoBehaviour��Start()����ִ�����
            // - �ȴ��ض��ĳ�ʼ�������ɹ���

            // ʾ�����ȴ������е������첽�������
            yield return new WaitForSeconds(0.2f); // ������һЩʱ����ɳ�ʼ��

            // ��鳡���еĹؼ�ϵͳ�Ƿ��ѳ�ʼ��
            bool criticalSystemsInitialized = CheckCriticalSystems(scene);

            if (criticalSystemsInitialized)
            {
                sceneIsReady = true;
                Debug.Log($"���� {scene.name} ����ȫ׼����");
            }
            else
            {
                // ����ؼ�ϵͳδ��ʼ���������ȴ�
                yield return new WaitUntil(() => CheckCriticalSystems(scene));
                sceneIsReady = true;
                Debug.Log($"���� {scene.name} ����ȫ׼����");
            }
        }

        // ��鳡���еĹؼ�ϵͳ�Ƿ��ѳ�ʼ��
        private bool CheckCriticalSystems(UnityScene scene)
        {
            // ���������Ӿ���ļ���߼�
            // ���磺���ҳ����еĹؼ�����������Ƿ��Ѽ���

            GameObject[] rootObjects = scene.GetRootGameObjects();
            foreach (GameObject root in rootObjects)
            {
                // ʾ��������Ƿ�����ض��Ĺ�����
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


