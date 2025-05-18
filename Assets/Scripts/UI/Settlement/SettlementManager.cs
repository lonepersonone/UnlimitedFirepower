using MyGame.Framework.Record;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace MyGame.UI.Settlement
{
    public class SettlementManager : MonoBehaviour
    {
        public static SettlementManager Instance;

        public GameObject SettlementPanel;

        private void Awake()
        {
            Instance = this;           
        }

        public void EnableSettlement()
        {
            SettlementPanel.SetActive(true); 
        }

    }
}


