using MyGame.Data.SO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Upgrade
{
    public class UpgradeItem
    {
        private string id;
        private Sprite icon;
        private int cost;
        private float[] values;
        private UpgradeType type;

        private int index = 0;
        private float total;
        public Action<float> selectAction;

        public bool IsFullLevel => index == values.Length;
        public bool CanUpgrade => index < values.Length;
        public int CurrentLevel => index;
        public int Cost => cost;
        public string Id => id;
        public Sprite Icon => icon;
        public float Value => index < values.Length ? values[index] : 0;
        public float TotalValue => total;
        public UpgradeType Type => type;

        public UpgradeItem(UpgradeDataSO so)
        {
            id = so.ID;
            icon = so.Icon;
            cost = so.UpgradeData.Cost;
            values = so.UpgradeData.Values;
            type = so.UpgradeType;
        }

        public bool Upgrade()
        {
            if (CanUpgrade)
            {
                total += values[index];
                selectAction?.Invoke(total);
                return true;
            }
            return false;
        }

        public void ResetState() => index = 0;

        public void RegistEvent(Action<float> enevt)
        {
            selectAction += enevt;
        }

        public void UnRegistEvent()
        {
            selectAction = null;
        }

        public void AddSelectedCount()
        {
            index++;
        }

    }
}

