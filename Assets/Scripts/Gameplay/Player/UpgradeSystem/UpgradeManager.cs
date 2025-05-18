using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Upgrade
{
    // ����ѡ������ö��
    public enum UpgradeType
    {
        Offense, // ����
        Defense, // ����
        Utility, // ����
        Special // ����
    }

    public class UpgradeManager 
    {
        private List<UpgradeItem> allItems = new List<UpgradeItem>(); // ����ѡ��
        private List<UpgradeItem> availableItems = new List<UpgradeItem>(); // ����ѡ���

        private Dictionary<UpgradeItem, float> itemProbabilities = new Dictionary<UpgradeItem, float>(); // ѡ�����
        private Dictionary<UpgradeType, float> typeWeights = new Dictionary<UpgradeType, float>(); // ����Ȩ��
        private Dictionary<UpgradeItem, int> selectionCounts = new Dictionary<UpgradeItem, int>(); // ѡ�����

        [Header("��������Ȩ��")]
        [SerializeField] private float offenseWeight = 40f; // ����Ȩ��
        [SerializeField] private float defenseWeight = 30f; // ����Ȩ��
        [SerializeField] private float utilityWeight = 20f; // ����Ȩ��
        [SerializeField] private float specialWeight = 10f; // ����Ȩ��

        [Header("���ʵ�������")]
        [SerializeField] private float selectionBiasFactor = 0.2f; // ѡ��������������
        [SerializeField] private float maxBiasFactor = 5f; // ��������������

        public UpgradeManager(List<UpgradeItem> items)
        {
            allItems.AddRange(items);
            availableItems.AddRange(allItems);
            Initialize();
        }

        // ��ʼ��ϵͳ
        private void Initialize()
        {
            // ��ʼ������Ȩ��
            typeWeights[UpgradeType.Offense] = offenseWeight;
            typeWeights[UpgradeType.Defense] = defenseWeight;
            typeWeights[UpgradeType.Utility] = utilityWeight;
            typeWeights[UpgradeType.Special] = specialWeight;

            // ��ʼ��ѡ�����
            RecalculateAllItemProbabilities();
        }

        // ���¼�������ѡ��ĸ���
        private void RecalculateAllItemProbabilities()
        {
            itemProbabilities.Clear();

            // �����ͷ���ѡ��
            var itemsByType = new Dictionary<UpgradeType, List<UpgradeItem>>();
            foreach (var type in Enum.GetValues(typeof(UpgradeType)))
            {
                itemsByType[(UpgradeType)type] = new List<UpgradeItem>();
            }

            foreach (var item in availableItems)
            {
                itemsByType[item.Type].Add(item);
            }

            // ����ÿ�����͵ĸ��ʷ���
            float totalWeight = 0f;
            foreach (var pair in typeWeights)
            {
                totalWeight += pair.Value;
            }

            // Ϊÿ��ѡ������������
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

            // ��һ�������ܺ�Ϊ1
            NormalizeProbabilities();
        }

        // ��һ�������ܺ�Ϊ1
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

        // ��ȡ�����ͬ������ѡ��
        public List<UpgradeItem> DrawItems(int count = 3)
        {
            if (count > availableItems.Count)
            {
                Debug.LogError($"�޷���ȡ {count} ��ѡ���Ϊ����ѡ���ֻ�� {availableItems.Count} ��");
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

        // �Ӹ���ѡ���б��г�ȡ����ѡ��
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

            // �Է���һ���������һ��ѡ��
            return items[items.Count - 1];
        }

        // ��̬����ĳ���͵�Ȩ��
        public void AdjustTypeWeight(UpgradeType type, float newWeight)
        {
            typeWeights[type] = Mathf.Max(0f, newWeight);
            RecalculateAllItemProbabilities();
        }

        // ��ȡ��ǰ����Ȩ��
        public float GetTypeWeight(UpgradeType type)
        {
            return typeWeights[type];
        }

        // ��ȡѡ��ĵ�ǰ����
        public float GetItemProbability(UpgradeItem item)
        {
            return itemProbabilities.GetValueOrDefault(item, 0f);
        }

        // ��������������ѡ��
        public void AddItemToPool(UpgradeItem item)
        {
            allItems.Add(item);
            availableItems.Add(item);
            RecalculateAllItemProbabilities();
        }

        // �ӳ������Ƴ�����ѡ��
        public void RemoveItemFromPool(UpgradeItem item)
        {
            allItems.Remove(item);
            availableItems.Remove(item);
            RecalculateAllItemProbabilities();
        }

        // ѡ������ѡ��
        public void SelectItem(UpgradeItem item)
        {
            item.AddSelectedCount();
            selectionCounts[item] = item.CurrentLevel;
            RecalculateAllItemProbabilities();
        }

        // ˢ��ѡ���
        public void RefreshItems()
        {
            // �����������߼�������ˢ�²�������������ĳЩ״̬
            RecalculateAllItemProbabilities();
        }
    }

}



