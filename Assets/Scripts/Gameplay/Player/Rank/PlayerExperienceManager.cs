using MyGame.Data.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    // �ȼ��仯�¼�����
    public class RankChangedEventArgs : EventArgs
    {
        public int previousRank;
        public int newRank;
        public int addedExperience;
        public int requiredExperience;

        public RankChangedEventArgs(int previousRank, int newRank, int addedExperience, int requiredExperience)
        {
            this.previousRank = previousRank;
            this.newRank = newRank;
            this.addedExperience = addedExperience;
            this.requiredExperience = requiredExperience;
        }
    }

    // ��Ҿ��������
    public class PlayerExperienceManager : MonoBehaviour
    {
        [SerializeField] private RankConfig rankConfig;
        [SerializeField] private int currentRank = 1;
        [SerializeField] private int currentExperience = 0;

        // �¼�����
        public event EventHandler<RankChangedEventArgs> OnRankUp;
        public event EventHandler<RankChangedEventArgs> OnRankChanged;
        public event EventHandler<int> OnExperienceChanged;

        // ����ģʽ
        private static PlayerExperienceManager instance;
        public static PlayerExperienceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<PlayerExperienceManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("PlayerExperienceManager");
                        instance = obj.AddComponent<PlayerExperienceManager>();
                    }
                }
                return instance;
            }
        }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // ��ʼ��
            if (rankConfig == null)
            {
                Debug.LogError("PlayerExperienceManager: δ���õȼ����ã�");
            }
        }

        // ��Ӿ���
        public void AddExperience(int amount)
        {
            if (amount <= 0) return;

            int originalRank = currentRank;
            int originalExperience = currentExperience;

            currentExperience += amount;

            // ����Ƿ�����
            CheckRankUp();

            // ��������仯�¼�
            OnExperienceChanged?.Invoke(this, currentExperience);

            // �����ȼ��仯�¼�������б仯��
            if (originalRank != currentRank)
            {
                OnRankChanged?.Invoke(this, new RankChangedEventArgs(originalRank, currentRank, amount, GetRequiredExperienceForNextRank()));
            }
        }

        // ���þ���ֵ
        public void SetExperience(int amount)
        {
            if (amount < 0) amount = 0;

            int originalRank = currentRank;
            int originalExperience = currentExperience;

            currentExperience = amount;

            // ���¼���ȼ�
            CalculateRank();

            // ��������仯�¼�
            OnExperienceChanged?.Invoke(this, currentExperience);

            // �����ȼ��仯�¼�������б仯��
            if (originalRank != currentRank)
            {
                OnRankChanged?.Invoke(this, new RankChangedEventArgs(originalRank, currentRank, currentExperience - originalExperience, GetRequiredExperienceForNextRank()));
            }
        }

        // ��鲢��������
        private void CheckRankUp()
        {
            int originalRank = currentRank;

            while (currentRank < rankConfig.MaxRank)
            {
                int requiredExp = rankConfig.GetRequiredExperience(currentRank);
                if (currentExperience >= requiredExp)
                {
                    currentExperience -= requiredExp;
                    currentRank++;

                    // ���������¼�
                    OnRankUp?.Invoke(this, new RankChangedEventArgs(currentRank - 1, currentRank, 0, GetRequiredExperienceForNextRank()));
                }
                else
                {
                    break; // ���鲻�㣬ֹͣ���
                }
            }

            // ��ֹ���
            if (currentRank >= rankConfig.MaxRank)
            {
                currentRank = rankConfig.MaxRank;
                currentExperience = 0;
            }
        }

        // ���ݵ�ǰ����ֵ����ȼ�
        private void CalculateRank()
        {
            currentRank = 1;

            while (currentRank < rankConfig.MaxRank)
            {
                int requiredExp = rankConfig.GetRequiredExperience(currentRank);
                if (currentExperience >= requiredExp)
                {
                    currentExperience -= requiredExp;
                    currentRank++;
                }
                else
                {
                    break; // ���鲻�㣬ֹͣ����
                }
            }

            // ��ֹ���
            if (currentRank >= rankConfig.MaxRank)
            {
                currentRank = rankConfig.MaxRank;
                currentExperience = 0;
            }
        }

        // ��ȡ��ǰ�ȼ�
        public int GetCurrentRank()
        {
            return currentRank;
        }

        // ��ȡ��ǰ����
        public int GetCurrentExperience()
        {
            return currentExperience;
        }

        // ��ȡ�������辭��
        public int GetRequiredExperienceForNextRank()
        {
            return rankConfig.GetRequiredExperience(currentRank);
        }

        // ��ȡ��ǰ�ȼ����Ȱٷֱ�
        public float GetRankProgress()
        {
            if (currentRank >= rankConfig.MaxRank) return 1f;

            int requiredExp = GetRequiredExperienceForNextRank();
            if (requiredExp <= 0) return 0f;

            return (float)currentExperience / requiredExp;
        }

        // ��ȡ��ǰ�ȼ�����
        public string GetCurrentRankName()
        {
            return rankConfig.GetRankName(currentRank);
        }

        // ע��ȼ��仯�ص�
        public void RegisterRankUpCallback(Action<object, RankChangedEventArgs> callback)
        {
            OnRankUp += new EventHandler<RankChangedEventArgs>(callback);
        }

        // ע���ȼ��仯�ص�
        public void UnregisterRankUpCallback(Action<object, RankChangedEventArgs> callback)
        {
            OnRankUp -= new EventHandler<RankChangedEventArgs>(callback);
        }
    }

}

