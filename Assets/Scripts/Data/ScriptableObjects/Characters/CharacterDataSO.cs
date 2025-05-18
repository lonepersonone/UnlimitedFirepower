using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    public enum AttackType
    {
        Melee, //��ս
        LongRane, //Զ��
        Mixture //��Ϲ���
    }

    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/CharacterData")]
    public class CharacterDataSO : ScriptableObject, IGameData
    {
        public string ID;

        public CharacterData CharacterData;
        public CharacterDataSO[] Childs; //��������
        public WeaponDataSO[] Weapons; //������������

        public GameObject CharacterPrafab;
        public GameObject DiedPrefab; //����ʱ��Ч
        public GameObject SpawnPrefab; //����ʱ��Ч    
        public GameObject ExplosionPrefab; //��ըʱ��Ч

        string IGameData.ID => ID;
    }

}

