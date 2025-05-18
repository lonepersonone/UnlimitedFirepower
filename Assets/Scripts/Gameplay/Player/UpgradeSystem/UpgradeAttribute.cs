using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Upgrade
{
    public class UpgradeAttribute
    {
        private UpgradeItem defaultItem;
        private List<UpgradeItem> upgradeItems = new List<UpgradeItem>();
        private List<UpgradeItem> selectedItems = new List<UpgradeItem>();
        private Dictionary<string, UpgradeItem> itemDict = new Dictionary<string, UpgradeItem>();

        public List<UpgradeItem> Items => upgradeItems;
        public List<UpgradeItem> UpgradeItems => upgradeItems;
        public List<UpgradeItem> SelectedItems => selectedItems;

        public UpgradeAttribute(ScriptableManager scriptable)
        {
            UpgradeDataSO[] sos = scriptable.GetUpgrades();

            foreach (var so in sos)
            {
                if (so.ID == "Default")
                {
                    defaultItem = new UpgradeItem(so);
                    itemDict[so.ID] = defaultItem;
                }
                else
                {
                    UpgradeItem item = new UpgradeItem(so);
                    upgradeItems.Add(item);
                    itemDict[so.ID] = item;
                }
            }

            // UpgradeEventRegister.RegistEvent(upgradeItems);
        }

      

        public bool AddSelectedItem(UpgradeItem item)
        {
            if (selectedItems.Contains(item)) return false;
            selectedItems.Add(item);
            return true;
        }

    }


}

