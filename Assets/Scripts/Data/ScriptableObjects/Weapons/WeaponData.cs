using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [Serializable]
    public class WeaponData
    {
        public int MultiShot = 0; //子弹数量
        public float FireRate; //开火速率
        public float Attenuation; //贯穿属性
        public float Damage; //伤害
        public float Range; //范围
        public float Speed; //速度
        public float Size; //大小
        public float CriticalProbability; //暴击概率
        public float CriticalRatio; //暴击倍率

        // 克隆副本
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


