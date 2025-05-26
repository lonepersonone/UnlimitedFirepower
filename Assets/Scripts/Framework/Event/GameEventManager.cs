using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Event
{
    // 游戏事件类型枚举
    public enum GameEventType
    {
        None,

        // 场景行为
        SceneLoadStarted,
        SceneLoadCompleted,

        // 游戏开始
        GameStarted,
        GamePaused,
        GameResumed,
        GameOver,

        // 切换星系
        LevelStarted,
        LevelCompleted,
        LevelFailed,
        LevelChange,

        // 进入关卡战斗
        BattleInitial,
        BattleStarted,
        BattleEnded,
        BattleWined,
        BattleFailed,

        // 玩家行为
        PlayerDied,
        PlayerRespawned,
        PlayerReset,
        ItemCollected,
        PowerupActivated,

        // 敌人行为
        EnemyKilled,
     
        // 场景行为
        ScaleCamera,
        SpawnUI,

        // 隐藏游戏对象
        SceneObjectInitial,
        SceneObjectShow,
        SceneObjectHide,
    }

    // 无参数的事件回调
    public delegate void GameEvent();

    // 带参数的事件回调
    public delegate void GameEvent<T>(T data);

    // 带两个参数的事件回调
    public delegate void GameEvent<T, U>(T data1, U data2);

    public class GameEventManager : MonoBehaviour
    {
        public static GameEventManager Instance { get; private set; }

        // 无参数事件字典
        private Dictionary<GameEventType, GameEvent> eventDictionary = new Dictionary<GameEventType, GameEvent>();

        // 带一个参数的事件字典
        private Dictionary<GameEventType, System.Delegate> eventDictionary1Param = new Dictionary<GameEventType, System.Delegate>();

        // 带两个参数的事件字典
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

        #region 无参数事件注册与注销
        public static void RegisterListener(GameEventType eventType, GameEvent listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                Debug.LogError("GameEventManager实例不存在！");
                return;
            }

            if (Instance.eventDictionary.ContainsKey(eventType))
            {
                Instance.eventDictionary[eventType] -= listener;
            }
        }
        #endregion

        #region 带一个参数的事件注册与注销
        public static void RegisterListener<T>(GameEventType eventType, GameEvent<T> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                Debug.LogError("GameEventManager实例不存在！");
                return;
            }

            if (Instance.eventDictionary1Param.ContainsKey(eventType))
            {
                Instance.eventDictionary1Param[eventType] = (GameEvent<T>)Instance.eventDictionary1Param[eventType] - listener;
            }
        }
        #endregion

        #region 带两个参数的事件注册与注销
        public static void RegisterListener<T, U>(GameEventType eventType, GameEvent<T, U> listener)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                Debug.LogError("GameEventManager实例不存在！");
                return;
            }

            if (Instance.eventDictionary2Param.ContainsKey(eventType))
            {
                Instance.eventDictionary2Param[eventType] = (GameEvent<T, U>)Instance.eventDictionary2Param[eventType] - listener;
            }
        }
        #endregion

        #region 事件触发
        public static void TriggerEvent(GameEventType eventType)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                    Debug.LogWarning($"事件 {eventType} 被触发，但没有监听器");
                }
            }
            else
            {
                Debug.LogWarning($"事件 {eventType} 未注册");
            }
        }

        public static void TriggerEvent<T>(GameEventType eventType, T data)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                        Debug.LogError($"触发事件 {eventType} 时出错: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"事件 {eventType} 被触发，但没有监听器");
                }
            }
            else
            {
                Debug.LogWarning($"事件 {eventType} 未注册");
            }
        }

        public static void TriggerEvent<T, U>(GameEventType eventType, T data1, U data2)
        {
            if (Instance == null)
            {
                Debug.LogError("GameEventManager实例不存在！");
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
                        Debug.LogError($"触发事件 {eventType} 时出错: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"事件 {eventType} 被触发，但没有监听器");
                }
            }
            else
            {
                Debug.LogWarning($"事件 {eventType} 未注册");
            }
        }
        #endregion

        // 清理所有事件监听
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


