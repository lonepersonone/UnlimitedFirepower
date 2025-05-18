using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyGame.Framework.Record
{
    public class RecordDataManager: MonoBehaviour
    {
        private static RecordDataManager instance;
        public static RecordDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RecordDataManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("RecordDataManager");
                        instance = obj.AddComponent<RecordDataManager>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }

        [SerializeField] private RecordDataSettings settings;
        private List<RecordSessionData> allSessions = new List<RecordSessionData>();
        private RecordSessionData currentSession;
        private string saveFilePath;

        public RecordSessionData CurrentSession => currentSession;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            // 初始化保存路径
            saveFilePath = Path.Combine(Application.persistentDataPath, settings.SaveFileName);

            // 加载已有数据
            LoadAllSessions();

            // 开始新游戏会话
            StartNewSession();
        }

        // 开始新的游戏会话
        public void StartNewSession(string playerName = "Player")
        {
            currentSession = new RecordSessionData
            {
                playerName = playerName,
                date = DateTime.Now,
                playTime = 0f,
                totalDamage = 0,
                enemiesKilled = 0,
                levelsCompleted = 0,
                gameCompleted = false
            };
        }

        // 更新当前会话数据
        public void UpdateCurrentSession(float playTime = 0, float damage = 0, int enemiesKilled = 0, int levelsCompleted = 0, bool gameCompleted = false)
        {
            if (currentSession == null) return;

            if (playTime > 0) currentSession.playTime = playTime;
            if (damage > 0) currentSession.totalDamage += damage;
            if (enemiesKilled > 0) currentSession.enemiesKilled += enemiesKilled;
            if (levelsCompleted > 0) currentSession.levelsCompleted += levelsCompleted;
            if (gameCompleted) currentSession.gameCompleted = true;
        }

        public void UpdatePlayerTime(float playTime = 0)
        {
            if (currentSession == null) return;
            if (playTime > 0) currentSession.playTime = playTime;
        }

        public void UpdateDamage(float damage)
        {
            if (currentSession == null) return;
            if (damage > 0) currentSession.totalDamage += damage;
        }

        public void UpdateEnemiesKilled(int enemiesKilled)
        {
            if (currentSession == null) return;
            if (enemiesKilled > 0) currentSession.enemiesKilled += enemiesKilled;
        }

        public void UpdateLevelCompleted(int levelsCompleted)
        {
            if (currentSession == null) return;
            if (levelsCompleted > 0) currentSession.levelsCompleted += levelsCompleted;
        }

        public void UpdateGameCompleted(bool gameCompleted = false)
        {
            if (currentSession == null) return;
            if (gameCompleted) currentSession.gameCompleted = true;
        }


        // 保存当前会话数据
        public void SaveCurrentSession()
        {
            if (currentSession == null) return;

            // 更新时间戳
            currentSession.date = DateTime.Now;

            // 添加到会话列表
            allSessions.Add(currentSession);

            // 按分数排序并限制数量
            SortSessionsByScore();
            TrimSessions();

            // 保存到文件
            SaveAllSessions();
        }

        // 获取排行榜数据
        public List<RecordSessionData> GetRankingData()
        {
            return allSessions;
        }

        // 按分数排序会话
        private void SortSessionsByScore()
        {
            allSessions.Sort((a, b) => b.CalculateScore().CompareTo(a.CalculateScore()));
        }

        // 限制会话数量
        private void TrimSessions()
        {
            if (allSessions.Count > settings.MaxRankingEntries)
            {
                allSessions.RemoveRange(settings.MaxRankingEntries, allSessions.Count - settings.MaxRankingEntries);
            }
        }

        // 保存所有会话数据到文件
        private void SaveAllSessions()
        {
            try
            {
                string jsonData = JsonUtility.ToJson(new GameSessionDataList { sessions = allSessions }, true);
                File.WriteAllText(saveFilePath, jsonData);
                Debug.Log($"游戏数据已保存到: {saveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"保存游戏数据失败: {e.Message}");
            }
        }

        // 从文件加载所有会话数据
        private void LoadAllSessions()
        {
            try
            {
                if (File.Exists(saveFilePath))
                {
                    string jsonData = File.ReadAllText(saveFilePath);
                    GameSessionDataList dataList = JsonUtility.FromJson<GameSessionDataList>(jsonData);
                    allSessions = dataList.sessions ?? new List<RecordSessionData>();
                    Debug.Log($"已加载 {allSessions.Count} 条游戏记录");
                }
                else
                {
                    Debug.Log("未找到游戏数据文件，将创建新文件");
                    allSessions = new List<RecordSessionData>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"加载游戏数据失败: {e.Message}");
                allSessions = new List<RecordSessionData>();
            }
        }

        // 用于JSON序列化的辅助类
        [Serializable]
        private class GameSessionDataList
        {
            public List<RecordSessionData> sessions;
        }
    }
}


