using MyGame.Gameplay.Prop;
using MyGame.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Upgrade
{
    public class UpgradeController : MonoBehaviour
    {
        public static UpgradeController Instance;

        private UpgradeAttribute upgradeAttribute;
        private WealthAttribute wealthAttribute;
        private UpgradeManager upgradeManager;

        private void Awake()
        {
            Instance = this;
        }

        public void Initalize(MainDataManager data)
        {
            upgradeAttribute = data.UpgradeData;
            wealthAttribute = data.WealthData;

            upgradeManager = new UpgradeManager(upgradeAttribute.UpgradeItems);
        }

        public bool CanBuy(UpgradeItem item)
        {
            if (wealthAttribute.ReduceWealth(item.Cost) && item.Upgrade())
            {
                Debug.Log($"{item.Id} Parchased");
                upgradeAttribute.AddSelectedItem(item);
                upgradeManager.SelectItem(item);
                return true;
            }
            return false;
        }

        public bool Refresh() 
        {
            if (wealthAttribute.ReduceWealth(200))
            {
                upgradeManager.RefreshItems();
                return true;
            }
            return false;
        }

        public List<UpgradeItem> GetUpgradeItem()
        {
            return upgradeManager.DrawItems();
        }

    }
}



