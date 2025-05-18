using MyGame.Gameplay.Upgrade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Game/UpgradeData")]
    public class UpgradeDataSO : ScriptableObject, IGameData
    {
        public string ID;
        public Sprite Icon; //ÏÔÊ¾Í¼Æ¬
        public UpgradeType UpgradeType;
        public UpgradeData UpgradeData;

        string IGameData.ID => ID;
    }
}


