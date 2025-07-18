using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Reach
{
    [CreateAssetMenu(fileName = "New Controller Preset", menuName = "Reach UI/Controller/New Controller Preset")]
    public class ControllerPreset : ScriptableObject
    {
        [Header("Settings")]
        public string controllerName = "Controller PowerCN";

        [Space(10)]
        public List<ControllerItem> items = new List<ControllerItem>();

        public enum ItemType { Icon, Text }

        [System.Serializable]
        public class ControllerItem
        {
            public string itemID;
            public ItemType itemType;
            public Sprite itemIcon;
            public string itemText;
        }
    }
}