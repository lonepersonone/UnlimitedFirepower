using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [Serializable]
    public class WeaponData
    {
        public int MultiShot = 0; //�ӵ�����
        public float FireRate; //��������
        public float Attenuation; //�ᴩ����
        public float Damage; //�˺�
        public float Range; //��Χ
        public float Speed; //�ٶ�
        public float Size; //��С
        public float CriticalProbability; //��������
        public float CriticalRatio; //��������

        // ��¡����
        public WeaponData Clone()
        {
            return new WeaponData
            {
                MultiShot = this.MultiShot,
                FireRate = this.FireRate,
                Attenuation = this.Attenuation,
                Damage = this.Damage,
                Range = this.Range,
                Speed = this.Speed,
                Size = this.Size,
                CriticalProbability = this.CriticalProbability,
                CriticalRatio = this.CriticalRatio,
            };
        }
    }
}


