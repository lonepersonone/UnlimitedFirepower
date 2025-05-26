using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Event
{
    // ��Ϸ�¼�����ö��
    public enum GameEventType
    {
        None,

        // ������Ϊ
        SceneLoadStarted,
        SceneLoadCompleted,

        // ��Ϸ��ʼ
        GameStarted,
        GamePaused,
        GameResumed,
        GameOver,

        // �л���ϵ
        LevelStarted,
        LevelCompleted,
        LevelFailed,
        LevelChange,

        // ����ؿ�ս��
        BattleInitial,
        BattleStarted,
        BattleEnded,
        BattleWined,
        BattleFailed,

        // �����Ϊ
        PlayerDied,
        PlayerRespawned,
        PlayerReset,
        ItemCollected,
        PowerupActivated,

        // ������Ϊ
        EnemyKilled,
     
        // ������Ϊ
        ScaleCamera,
        SpawnUI,

        // ������Ϸ����
        SceneObjectInitial,
        SceneObjectShow,
        SceneObjectHide,
    }

    // �޲������¼��ص�
    public delegate void GameEvent();

    // ���������¼��ص�
    public delegate void GameEvent<T>(T data);

    // �������������¼��ص�
    public delegate void GameEvent<T, U>(T data1, U data2);

    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        // �޲����¼��ֵ�
        private Dictionary<GameEventType, GameEvent> eventDictionary = new Dictionary<GameEventType, GameEvent>();

        // ��һ���������¼��ֵ�
        private Dictionary<GameEventType, System.Delegate> eventDictionary1Param = new Dictionary<GameEventType, System.Delegate>();

        // �������������¼��ֵ�
        private Dictionary<GameEventType, System.Delegate> eventDictionary2Param = new Dictionary<GameEventType, System.Delegate>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #region �޲����¼�ע����ע��
        public static void RegisterListener(GameEventType eventType, GameEvent listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (!Instance.eventDictionary.ContainsKey(eventType))
            {
                Instance.eventDictionary[eventType] = null;
            }

            Instance.eventDictionary[eventType] += listener;
        }


        public static void UnregisterListener(GameEventType eventType, GameEvent listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (Instance.eventDictionary.ContainsKey(eventType))
            {
                Instance.eventDictionary[eventType] -= listener;
            }
        }
        #endregion

        #region ��һ���������¼�ע����ע��
        public static void RegisterListener<T>(GameEventType eventType, GameEvent<T> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (!Instance.eventDictionary1Param.ContainsKey(eventType))
            {
                Instance.eventDictionary1Param[eventType] = null;
            }

            Instance.eventDictionary1Param[eventType] = (GameEvent<T>)Instance.eventDictionary1Param[eventType] + listener;
        }

        public static void UnregisterListener<T>(GameEventType eventType, GameEvent<T> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (Instance.eventDictionary1Param.ContainsKey(eventType))
            {
                Instance.eventDictionary1Param[eventType] = (GameEvent<T>)Instance.eventDictionary1Param[eventType] - listener;
            }
        }
        #endregion

        #region �������������¼�ע����ע��
        public static void RegisterListener<T, U>(GameEventType eventType, GameEvent<T, U> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (!Instance.eventDictionary2Param.ContainsKey(eventType))
            {
                Instance.eventDictionary2Param[eventType] = null;
            }

            Instance.eventDictionary2Param[eventType] = (GameEvent<T, U>)Instance.eventDictionary2Param[eventType] + listener;
        }

        public static void UnregisterListener<T, U>(GameEventType eventType, GameEvent<T, U> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (Instance.eventDictionary2Param.ContainsKey(eventType))
            {
                Instance.eventDictionary2Param[eventType] = (GameEvent<T, U>)Instance.eventDictionary2Param[eventType] - listener;
            }
        }
        #endregion

        #region �¼�����
        public static void TriggerEvent(GameEventType eventType)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            GameEvent thisEvent;
            if (Instance.eventDictionary.TryGetValue(eventType, out thisEvent))
            {
                if (thisEvent != null)
                {
                    thisEvent();
                }
                else
                {
                    Debug.LogWarning($"�¼� {eventType} ����������û�м�����");
                }
            }
            else
            {
                Debug.LogWarning($"�¼� {eventType} δע��");
            }
        }

        public static void TriggerEvent<T>(GameEventType eventType, T data)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (Instance.eventDictionary1Param.TryGetValue(eventType, out System.Delegate thisEvent))
            {
                if (thisEvent != null)
                {
                    try
                    {
                        ((GameEvent<T>)thisEvent)(data);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"�����¼� {eventType} ʱ����: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"�¼� {eventType} ����������û�м�����");
                }
            }
            else
            {
                Debug.LogWarning($"�¼� {eventType} δע��");
            }
        }

        public static void TriggerEvent<T, U>(GameEventType eventType, T data1, U data2)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManagerʵ�������ڣ�");
                return;
            }

            if (Instance.eventDictionary2Param.TryGetValue(eventType, out System.Delegate thisEvent))
            {
                if (thisEvent != null)
                {
                    try
                    {
                        ((GameEvent<T, U>)thisEvent)(data1, data2);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"�����¼� {eventType} ʱ����: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"�¼� {eventType} ����������û�м�����");
                }
            }
            else
            {
                Debug.LogWarning($"�¼� {eventType} δע��");
            }
        }
        #endregion

        // ���������¼�����
        public static void ClearAllListeners()
        {
            if (Instance == null) return;

            Instance.eventDictionary.Clear();
            Instance.eventDictionary1Param.Clear();
            Instance.eventDictionary2Param.Clear();
        }

        public static void ClearListener(GameEventType type)
        {
            Instance.eventDictionary[type] = null;
            Instance.eventDictionary1Param[type] = null;
            Instance.eventDictionary2Param[type] = null;
        }
    }

}


