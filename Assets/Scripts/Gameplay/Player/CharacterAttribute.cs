using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{

    public class CharacterAttribute
    {
        public AttackType AttackType;

        public string ID;

        public GameObject CharacterPrefab => so.CharacterPrafab;
        public GameObject DiedPrefab => so.DiedPrefab;
        public GameObject SpawnPrefab => so.SpawnPrefab;
        public GameObject ExplosionPrefab => so.ExplosionPrefab;

        private CharacterDataSO so;

        private float moveSpeedOffset;
        private float rotateSpeedOffset;
        private float decelerationOffset;
        private float healthOffset;
        private float healthRecoverOffset;
        private float shieldOffset;
        private float shieldRecoverOffset;
        private float coolTimeOffset;
        private float thrusterRateOffset;
        private float thrusterDurationOffset;
        private float thrusterRecoverOffset;
        private float thrusterReduceOffset;
        private float thrusterCoolTimeOffset;
        private float damageReductionOffset;
        private float rebirthRangeOffset;
        private float rebirthRateOffset;


        public int FireUnitCount = 1;

        public int UAVCount = 3;

        public float Wealth => so.CharacterData.Wealth;
        public float MoveSpeed => so.CharacterData.MoveSpeed + moveSpeedOffset;
        public float RotateSpeed => so.CharacterData.RotateSpeed + rotateSpeedOffset;
        public float Deceleration => so.CharacterData.Deceleration + decelerationOffset;
        public float Health => so.CharacterData.Health + healthOffset;
        public float HealthRecover => so.CharacterData.HealthRecover + healthRecoverOffset;
        public float Shield => so.CharacterData.Shield + shieldOffset;
        public float ShieldRecover => so.CharacterData.ShieldRecover + shieldRecoverOffset;
        public float CoolTime => so.CharacterData.CoolTime + coolTimeOffset;
        public float ThrusterRate => so.CharacterData.ThrusterRate + thrusterRateOffset;
        public float ThrusterDuration => so.CharacterData.ThrusterDuration + thrusterDurationOffset;
        public float ThrusterRecover => so.CharacterData.ThrusterRecover + thrusterRecoverOffset;
        public float ThrusterReduce => so.CharacterData.ThrusterReduce + thrusterReduceOffset;
        public float ThrusterCoolTime => so.CharacterData.ThrusterCoolTime + thrusterCoolTimeOffset;
        public float DamageReduction => so.CharacterData.DamageReduction + damageReductionOffset; //�˺�����
        public float RebirthRange => so.CharacterData.RebirthRange + rebirthRangeOffset;
        public float RebirthRate => so.CharacterData.RebirthRate + rebirthRateOffset;

        // ����
        private WeaponAttribute[] weapons = new WeaponAttribute[10];
        private int weaponIndex = 0; //��������

        public WeaponAttribute WeaponData => weapons[weaponIndex];

        // ���࣬�������˻����ٻ����
        private CharacterAttribute[] childs = new CharacterAttribute[10];
        private int childIndex = 0;

        public CharacterAttribute CurrentChild => childs[childIndex];

        public CharacterAttribute(CharacterDataSO so)
        {
            ID = so.ID;

            this.so = so;

            if (so.Weapons.Length > 0)
                for (int i = 0; i < so.Weapons.Length; i++) weapons[i] = new WeaponAttribute(so.Weapons[i]);

            if (so.Childs.Length > 0)
                for (int i = 0; i < so.Childs.Length; i++) childs[i] = new CharacterAttribute(so.Childs[i]);                  
        }

        public void ChangeCharacterData(CharacterDataSO so)
        {
            ID = so.ID;
            this.so = so;

            weaponIndex = 0;            
            if (so.Weapons.Length > 0)
                for (int i = 0; i < so.Weapons.Length; i++) weapons[i] = new WeaponAttribute(so.Weapons[i]);

            childIndex = 0;
            if (so.Childs.Length > 0)
                for (int i = 0; i < so.Childs.Length; i++) childs[i] = new CharacterAttribute(so.Childs[i]);
        }

        // ��̬����
        public void SetMoveSpeedOffset(float value) => moveSpeedOffset = value;
        public void SetDecelerationOffset(float value) => decelerationOffset = value;
        public void SetHealthOffset(float value) => healthOffset = value;
        public void SetHealthRecoverOffset(float value) => healthRecoverOffset = value;
        public void SetShieldOffset(float value) => shieldOffset = value;
        public void SetShieldRecoverOffset(float value) => shieldRecoverOffset = value;
        public void SetCoolTimeOffset(float value) => coolTimeOffset = value;
        public void SetThrusterRateOffset(float value) => thrusterRateOffset = value;
        public void SetThrusterDurationOffset(float value) => thrusterDurationOffset = value;
        public void SetThrusterRecoverOffset(float value) => thrusterRecoverOffset = value;
        public void SetThrusterReduceOffset(float value) => thrusterReduceOffset = value;
        public void SetThrusterCoolTimeOffset(float value) => thrusterCoolTimeOffset = value;
        public void SetDamageReductionOffset(float value) => damageReductionOffset = value;
        public void SetRebirthRangeOffset(float value) => rebirthRangeOffset = value;
        public void SetRebirthRateOffset(float value) => rebirthRateOffset = value;
        public void ReduceMaxHealth() => so.CharacterData.Health *= RebirthRate;

        // ��������
        public void SwitchWeapon(int index) => this.weaponIndex = index;

        public void AddUAVConut(float value) => this.UAVCount += (int)value;
    }

}


