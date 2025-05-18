using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Upgrade
{
    // 升级选项类型枚举
    public enum UpgradeType
    {
        Offense, // 攻击
        Defense, // 防御
        Utility, // 辅助
        Special // 特殊
    }

    public class UpgradeManager 
    {
        private List<UpgradeItem> allItems = new List<UpgradeItem>(); // 所有选项
        private List<UpgradeItem> availableItems = new List<UpgradeItem>(); // 可用选项池

        private Dictionary<UpgradeItem, float> itemProbabilities = new Dictionary<UpgradeItem, float>(); // 选项概率
        private Dictionary<UpgradeType, float> typeWeights = new Dictionary<UpgradeType, float>(); // 类型权重
        private Dictionary<UpgradeItem, int> selectionCounts = new Dictionary<UpgradeItem, int>(); // 选择计数

        [Header("基础概率权重")]
        [SerializeField] private float offenseWeight = 40f; // 攻击权重
        [SerializeField] private float defenseWeight = 30f; // 防御权重
        [SerializeField] private float utilityWeight = 20f; // 辅助权重
        [SerializeField] private float specialWeight = 10f; // 特殊权重

        [Header("概率调整参数")]
        [SerializeField] private float selectionBiasFactor = 0.2f; // 选择后概率增加因子
        [SerializeField] private float maxBiasFactor = 5f; // 最大概率增加上限

        public UpgradeManager(List<UpgradeItem> items)
        {
            allItems.AddRange(items);
            availableItems.AddRange(allItems);
            Initialize();
        }

        // 初始化系统
        private void Initialize()
        {
            // 初始化类型权重
            typeWeights[UpgradeType.Offense] = offenseWeight;
            typeWeights[UpgradeType.Defense] = defenseWeight;
            typeWeights[UpgradeType.Utility] = utilityWeight;
            typeWeights[UpgradeType.Special] = specialWeight;

            // 初始化选项概率
            RecalculateAllItemProbabilities();
        }

        // 重新计算所有选项的概率
        private void RecalculateAllItemProbabilities()
        {
            itemProbabilities.Clear();

            // 按类型分组选项
            var itemsByType = new Dictionary<UpgradeType, List<UpgradeItem>>();
            foreach (var type in Enum.GetValues(typeof(UpgradeType)))
            {
                itemsByType[(UpgradeType)type] = new List<UpgradeItem>();
            }

            foreach (var item in availableItems)
            {
                itemsByType[item.Type].Add(item);
            }

            // 计算每个类型的概率分配
            float totalWeight = 0f;
            foreach (var pair in typeWeights)
            {
                totalWeight += pair.Value;
            }

            // 为每个选项分配基础概率
            foreach (var pair in itemsByType)
            {
                var type = pair.Key;
                var items = pair.Value;

                if (items.Count == 0) continue;

                float typeProbability = typeWeights[type] / totalWeight;
                float baseItemProbability = typeProbability / items.Count;

                foreach (var item in items)
                {
                    float bias = selectionBiasFactor * selectionCounts.GetValueOrDefault(item, 0);
                    float finalProbability = Mathf.Min(baseItemProbability * (1 + bias), baseItemProbability * maxBiasFactor);
                    itemProbabilities[item] = finalProbability;
                }
            }

            // 归一化概率总和为1
            NormalizeProbabilities();
        }

        // 归一化概率总和为1
        private void NormalizeProbabilities()
        {
            float sum = 0f;
            foreach (var pair in itemProbabilities)
            {
                sum += pair.Value;
            }

            if (sum <= 0) return;
            
            foreach(var item in availableItems)
            {
                if(itemProbabilities.ContainsKey(item))
                    itemProbabilities[item] /= sum;
            }

        }

        // 抽取多个不同的升级选项
        public List<UpgradeItem> DrawItems(int count = 3)
        {
            if (count > availableItems.Count)
            {
                Debug.LogError($"无法抽取 {count} 个选项，因为可用选项池只有 {availableItems.Count} 个");
                return null;
            }

            var result = new List<UpgradeItem>();
            var localAvailableItems = new List<UpgradeItem>(availableItems);

            for (int i = 0; i < count; i++)
            {
                if (localAvailableItems.Count == 0) break;

                var selected = DrawSingleItem(localAvailableItems);
                result.Add(selected);
                localAvailableItems.Remove(selected);
            }

            return result;
        }

        // 从给定选项列表中抽取单个选项
        private UpgradeItem DrawSingleItem(List<UpgradeItem> items)
        {
            float randomValue = UnityEngine.Random.value;
            float cumulativeProbability = 0f;

            foreach (var item in items)
            {
                cumulativeProbability += itemProbabilities[item];
                if (randomValue <= cumulativeProbability)
                {
                    return item;
                }
            }

            // 以防万一，返回最后一个选项
            return items[items.Count - 1];
        }

        // 动态调整某类型的权重
        public void AdjustTypeWeight(UpgradeType type, float newWeight)
        {
            typeWeights[type] = Mathf.Max(0f, newWeight);
            RecalculateAllItemProbabilities();
        }

        // 获取当前类型权重
        public float GetTypeWeight(UpgradeType type)
        {
            return typeWeights[type];
        }

        // 获取选项的当前概率
        public float GetItemProbability(UpgradeItem item)
        {
            return itemProbabilities.GetValueOrDefault(item, 0f);
        }

        // 向池子中添加升级选项
        public void AddItemToPool(UpgradeItem item)
        {
            allItems.Add(item);
            availableItems.Add(item);
            RecalculateAllItemProbabilities();
        }

        // 从池子中移除升级选项
        public void RemoveItemFromPool(UpgradeItem item)
        {
            allItems.Remove(item);
            availableItems.Remove(item);
            RecalculateAllItemProbabilities();
        }

        // 选择升级选项
        public void SelectItem(UpgradeItem item)
        {
            item.AddSelectedCount();
            selectionCounts[item] = item.CurrentLevel;
            RecalculateAllItemProbabilities();
        }

        // 刷新选项池
        public void RefreshItems()
        {
            // 这里可以添加逻辑来处理刷新操作，例如重置某些状态
            RecalculateAllItemProbabilities();
        }
    }

}



