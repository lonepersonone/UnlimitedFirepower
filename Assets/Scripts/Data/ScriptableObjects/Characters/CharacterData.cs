using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [Serializable]
    public class CharacterData
    {
        public int Wealth;
        public float MoveSpeed;
        public float RotateSpeed;
        public float Deceleration;
        public float Health;
        public float HealthRecover;
        public float Shield;
        public float ShieldRecover;
        public float CoolTime;
        public float ThrusterRate;
        public float ThrusterDuration;
        public float ThrusterRecover;
        public float ThrusterReduce;
        public float ThrusterCoolTime;
        public float DamageReduction; //…À∫¶ºı√‚
        public float RebirthRange;
        public float RebirthRate;

        public CharacterData Clone()
        {
            return new CharacterData
            {
                Wealth = this.Wealth,
                MoveSpeed = this.MoveSpeed,
                RotateSpeed = this.RotateSpeed,
                Deceleration = this.Deceleration,
                Health = this.Health,
                HealthRecover = this.HealthRecover,
                Shield = this.Shield,
                ShieldRecover = this.ShieldRecover,
                CoolTime = this.CoolTime,
                ThrusterRate = this.ThrusterRate,
                ThrusterDuration = this.ThrusterDuration,
                ThrusterRecover = this.ThrusterRecover,
                ThrusterReduce = this.ThrusterReduce,
                ThrusterCoolTime = this.ThrusterCoolTime,
                DamageReduction = this.DamageReduction,
                RebirthRange = this.RebirthRange,
                RebirthRate = this.RebirthRate,
            };
        }
    }
}



