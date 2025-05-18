using MyGame.Gameplay.Prop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewWealth", menuName = "Game/Props/Wealth")]
    public class WealthDataSO : ScriptableObject, IGameData
    {
        public string ID;
        public GameObject WealthPrefab;
        public WealthData WealthData;

        string IGameData.ID => ID;
    }

}

