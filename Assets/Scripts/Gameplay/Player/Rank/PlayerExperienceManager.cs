using MyGame.Data.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    // 等级变化事件参数
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

    // 玩家经验管理器
    public class PlayerExperienceManager : MonoBehaviour
    {
        [SerializeField] private RankConfig rankConfig;
        [SerializeField] private int currentRank = 1;
        [SerializeField] private int currentExperience = 0;

        // 事件声明
        public event EventHandler<RankChangedEventArgs> OnRankUp;
        public event EventHandler<RankChangedEventArgs> OnRankChanged;
        public event EventHandler<int> OnExperienceChanged;

        // 单例模式
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

            // 初始化
            if (rankConfig == null)
            {
                Debug.LogError("PlayerExperienceManager: 未设置等级配置！");
            }
        }

        // 添加经验
        public void AddExperience(int amount)
        {
            if (amount <= 0) return;

            int originalRank = currentRank;
            int originalExperience = currentExperience;

            currentExperience += amount;

            // 检查是否升级
            CheckRankUp();

            // 触发经验变化事件
            OnExperienceChanged?.Invoke(this, currentExperience);

            // 触发等级变化事件（如果有变化）
            if (originalRank != currentRank)
            {
                OnRankChanged?.Invoke(this, new RankChangedEventArgs(originalRank, currentRank, amount, GetRequiredExperienceForNextRank()));
            }
        }

        // 设置经验值
        public void SetExperience(int amount)
        {
            if (amount < 0) amount = 0;

            int originalRank = currentRank;
            int originalExperience = currentExperience;

            currentExperience = amount;

            // 重新计算等级
            CalculateRank();

            // 触发经验变化事件
            OnExperienceChanged?.Invoke(this, currentExperience);

            // 触发等级变化事件（如果有变化）
            if (originalRank != currentRank)
            {
                OnRankChanged?.Invoke(this, new RankChangedEventArgs(originalRank, currentRank, currentExperience - originalExperience, GetRequiredExperienceForNextRank()));
            }
        }

        // 检查并处理升级
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

                    // 触发升级事件
                    OnRankUp?.Invoke(this, new RankChangedEventArgs(currentRank - 1, currentRank, 0, GetRequiredExperienceForNextRank()));
                }
                else
                {
                    break; // 经验不足，停止检查
                }
            }

            // 防止溢出
            if (currentRank >= rankConfig.MaxRank)
            {
                currentRank = rankConfig.MaxRank;
                currentExperience = 0;
            }
        }

        // 根据当前经验值计算等级
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
                    break; // 经验不足，停止计算
                }
            }

            // 防止溢出
            if (currentRank >= rankConfig.MaxRank)
            {
                currentRank = rankConfig.MaxRank;
                currentExperience = 0;
            }
        }

        // 获取当前等级
        public int GetCurrentRank()
        {
            return currentRank;
        }

        // 获取当前经验
        public int GetCurrentExperience()
        {
            return currentExperience;
        }

        // 获取升级所需经验
        public int GetRequiredExperienceForNextRank()
        {
            return rankConfig.GetRequiredExperience(currentRank);
        }

        // 获取当前等级进度百分比
        public float GetRankProgress()
        {
            if (currentRank >= rankConfig.MaxRank) return 1f;

            int requiredExp = GetRequiredExperienceForNextRank();
            if (requiredExp <= 0) return 0f;

            return (float)currentExperience / requiredExp;
        }

        // 获取当前等级名称
        public string GetCurrentRankName()
        {
            return rankConfig.GetRankName(currentRank);
        }

        // 注册等级变化回调
        public void RegisterRankUpCallback(Action<object, RankChangedEventArgs> callback)
        {
            OnRankUp += new EventHandler<RankChangedEventArgs>(callback);
        }

        // 注销等级变化回调
        public void UnregisterRankUpCallback(Action<object, RankChangedEventArgs> callback)
        {
            OnRankUp -= new EventHandler<RankChangedEventArgs>(callback);
        }
    }

}

