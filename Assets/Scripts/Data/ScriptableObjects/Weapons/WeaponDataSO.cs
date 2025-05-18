using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/WeaponData")]
    public class WeaponDataSO : ScriptableObject, IGameData
    {
        public string ID; // ΨһID�����ڲ���

        public WeaponData WeaponData;

        public GameObject WeaponPrefab; //����ģ��
        public GameObject ProjectilePrefab; //�ӵ�ģ��

        string IGameData.ID => ID;
    }

}

