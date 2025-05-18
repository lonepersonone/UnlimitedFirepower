using Michsky.UI.Reach;
using MyGame.Framework.Manager;
using MyGame.Gameplay.Prop;
using MyGame.Scene.BattleRoom;
using MyGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace MyGame.Gameplay.Upgrade
{
    /// <summary>
    /// 控制UpgradeUI显示信息
    /// </summary>
    public class CanvasManagerU : GameSystemBase
    {
        public ShopButtonManager ShopButtonManager1;
        public ShopButtonManager ShopButtonManager2;
        public ShopButtonManager ShopButtonManager3;

        public TextMeshProUGUI shopValue1;
        public TextMeshProUGUI shopValue2;
        public TextMeshProUGUI shopValue3;

        public UpgradeWindow UpgradeWindow;

        public ButtonManager RefreshButton;
        public ButtonManager CloseButton;

        private List<UpgradeItem> upgrades;


        private WealthAttribute wealth;

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            upgrades = UpgradeController.Instance.GetUpgradeItem();
            wealth = BattleDataManager.Instance.PlayerWealth;

            SetUpgradeOptions();

            RefreshButton.onClick.AddListener(Refresh);
            CloseButton.onClick.AddListener(Close);

            await Task.Delay(100);
        }

        void Update()
        {
            ShopButtonManager1.UpdateLanguage();
            ShopButtonManager2.UpdateLanguage();
            ShopButtonManager3.UpdateLanguage();
        }

        private void SetUpgradeOptions()
        {
            ShopButtonManager1.SetValue(upgrades[0], UpgradeController.Instance.CanBuy);
            ShopButtonManager2.SetValue(upgrades[1], UpgradeController.Instance.CanBuy);
            ShopButtonManager3.SetValue(upgrades[2], UpgradeController.Instance.CanBuy);

            if (upgrades[0].Value < 1) shopValue1.text = $"+ {upgrades[0].Value:P1}";
            else shopValue1.text = $"+ {upgrades[0].Value}";

            if (upgrades[1].Value < 1) shopValue2.text = $"+ {upgrades[1].Value:P1}";
            else shopValue2.text = $"+ {upgrades[1].Value}";

            if (upgrades[2].Value < 1) shopValue3.text = $"+ {upgrades[2].Value:P1}";
            else shopValue3.text = $"+ {upgrades[2].Value}";
        }

        private void Refresh()
        {
            if (UpgradeController.Instance.Refresh())
            {
                upgrades = UpgradeController.Instance.GetUpgradeItem();

                ShopButtonManager1.UpdateValue(upgrades[0]);
                ShopButtonManager2.UpdateValue(upgrades[1]);
                ShopButtonManager3.UpdateValue(upgrades[2]);

                if (upgrades[0].Value < 1) shopValue1.text = $"+ {upgrades[0].Value:P1}";
                else shopValue1.text = $"+ {upgrades[0].Value}";

                if (upgrades[1].Value < 1) shopValue2.text = $"+ {upgrades[1].Value:P1}";
                else shopValue2.text = $"+ {upgrades[1].Value}";

                if (upgrades[2].Value < 1) shopValue3.text = $"+ {upgrades[2].Value:P1}";
                else shopValue3.text = $"+ {upgrades[2].Value}";
            }
        }


        private void Close()
        {
            Time.timeScale = 1;
            this.gameObject.SetActive(false);
        }


    }

}

