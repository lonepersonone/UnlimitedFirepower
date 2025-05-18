using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/WeaponData")]
    public class WeaponDataSO : ScriptableObject, IGameData
    {
        public string ID; // 唯一ID，用于查找

        public WeaponData WeaponData;

        public GameObject WeaponPrefab; //武器模型
        public GameObject ProjectilePrefab; //子弹模型

        string IGameData.ID => ID;
    }

}

