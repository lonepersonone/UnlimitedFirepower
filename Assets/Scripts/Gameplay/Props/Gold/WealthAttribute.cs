using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Prop
{
    public class WealthSpawnData
    {
        public GameObject WealthPrefab;
        public WealthData WealthData;

        public WealthSpawnData(GameObject prefab, WealthData wealthData)
        {
            WealthPrefab = prefab;
            WealthData = wealthData;
        }
    }

    public class WealthAttribute
    {
        private int maxWealth = 99999999;
        private int currentWealth = 0;

        private int totalEarnings;
        private int expectEarnings;
        private int depositLimit1 = 1000;
        private int depositLimit2 = 10000;
        private int depositLimit3 = 20000;
        private int depositLimit4 = 50000;
        private int depositLimit5 = 100000;
        private float interestRate1 = 0.01f;
        private float interestRate2 = 0.05f;
        private float interestRate3 = 0.1f;
        private float interestRate4 = 0.15f;
        private float interestRate5 = 0.2f;

        private WealthSpawnData wealthSpawnData;

        public WealthSpawnData WealthSpawnData => wealthSpawnData;

        public int TotalEarnings => totalEarnings;
        public int ExpectEarnings => CalculateExpectEarnings();
        public int CurrentWealth => currentWealth;
        public float CurrentInterestRate => GetCurrentInterestRate();
        public int[] DepositLimits => new int[5] { depositLimit1, depositLimit2, depositLimit3, depositLimit4, depositLimit5 };
        public float[] InterestRates => new float[5] { interestRate1, interestRate2, interestRate3, interestRate4, interestRate5 };
        public float[] Wealths => new float[3] { (float)currentWealth, CalculateExpectEarnings(), GetCurrentInterestRate() };

        public WealthAttribute(ScriptableManager scriptable)
        {
            WealthDataSO so = scriptable.GetWealthById("Basic");
            wealthSpawnData = new WealthSpawnData(so.WealthPrefab, so.WealthData);

            currentWealth = 10000;
        }

        public float[] AddUltimateWealth(int value)
        {
            AddExpectEarnings();
            AddWealth(value);
            return new float[3] { (float)currentWealth, CalculateExpectEarnings(), GetCurrentInterestRate() };
        }


        public void AddWealth(int value)
        {
            if (currentWealth + value <= maxWealth) currentWealth += value;
            else currentWealth = maxWealth;
        }

        public bool ReduceWealth(int value)
        {
            if (currentWealth < value) return false;
            else currentWealth -= value;
            return true;
        }

        public void AddExpectEarnings()
        {
            int earnings = CalculateExpectEarnings();
            if (earnings > 0)
            {
                totalEarnings += earnings;
                AddWealth(earnings);
            }
        }

        private int CalculateExpectEarnings()
        {
            return Mathf.RoundToInt((currentWealth * GetCurrentInterestRate()));
        }

        private float GetCurrentInterestRate()
        {
            if (currentWealth >= depositLimit1 && currentWealth < depositLimit2) return interestRate1;
            else if (currentWealth >= depositLimit2 && currentWealth < depositLimit3) return interestRate2;
            else if (currentWealth >= depositLimit3 && currentWealth < depositLimit4) return interestRate3;
            else if (currentWealth >= depositLimit4 && currentWealth < depositLimit5) return interestRate4;
            else if (currentWealth >= depositLimit5) return interestRate5;
            return 0;
        }

    }
}


