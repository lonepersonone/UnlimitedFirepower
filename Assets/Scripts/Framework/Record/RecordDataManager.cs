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

            // ��ʼ������·��
            saveFilePath = Path.Combine(Application.persistentDataPath, settings.SaveFileName);

            // ������������
            LoadAllSessions();

            // ��ʼ����Ϸ�Ự
            StartNewSession();
        }

        // ��ʼ�µ���Ϸ�Ự
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

        // ���µ�ǰ�Ự����
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


        // ���浱ǰ�Ự����
        public void SaveCurrentSession()
        {
            if (currentSession == null) return;

            // ����ʱ���
            currentSession.date = DateTime.Now;

            // ��ӵ��Ự�б�
            allSessions.Add(currentSession);

            // ������������������
            SortSessionsByScore();
            TrimSessions();

            // ���浽�ļ�
            SaveAllSessions();
        }

        // ��ȡ���а�����
        public List<RecordSessionData> GetRankingData()
        {
            return allSessions;
        }

        // ����������Ự
        private void SortSessionsByScore()
        {
            allSessions.Sort((a, b) => b.CalculateScore().CompareTo(a.CalculateScore()));
        }

        // ���ƻỰ����
        private void TrimSessions()
        {
            if (allSessions.Count > settings.MaxRankingEntries)
            {
                allSessions.RemoveRange(settings.MaxRankingEntries, allSessions.Count - settings.MaxRankingEntries);
            }
        }

        // �������лỰ���ݵ��ļ�
        private void SaveAllSessions()
        {
            try
            {
                string jsonData = JsonUtility.ToJson(new GameSessionDataList { sessions = allSessions }, true);
                File.WriteAllText(saveFilePath, jsonData);
                Debug.Log($"��Ϸ�����ѱ��浽: {saveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"������Ϸ����ʧ��: {e.Message}");
            }
        }

        // ���ļ��������лỰ����
        private void LoadAllSessions()
        {
            try
            {
                if (File.Exists(saveFilePath))
                {
                    string jsonData = File.ReadAllText(saveFilePath);
                    GameSessionDataList dataList = JsonUtility.FromJson<GameSessionDataList>(jsonData);
                    allSessions = dataList.sessions ?? new List<RecordSessionData>();
                    Debug.Log($"�Ѽ��� {allSessions.Count} ����Ϸ��¼");
                }
                else
                {
                    Debug.Log("δ�ҵ���Ϸ�����ļ������������ļ�");
                    allSessions = new List<RecordSessionData>();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"������Ϸ����ʧ��: {e.Message}");
                allSessions = new List<RecordSessionData>();
            }
        }

        // ����JSON���л��ĸ�����
        [Serializable]
        private class GameSessionDataList
        {
            public List<RecordSessionData> sessions;
        }
    }
}


