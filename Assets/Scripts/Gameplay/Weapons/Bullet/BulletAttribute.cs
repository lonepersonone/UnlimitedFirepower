using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Weapon
{
    /// <summary>
    /// 子弹属性
    /// </summary>
    public struct BulletAttribute
    {
        [Header("Common")]
        public float Speed;
        public float Damage;
        public float Range;
        public float Size;

        [Header("Snipe")]
        public float AttenuationRatio; //衰减比率

        [Header("Grenade")]
        public float DamageIncreace;
        public float SizeIncreace;

        public BulletAttribute(float Speed, float Damage, float Range, float Size)
        {
            this.Speed = Speed;
            this.Damage = Damage;
            this.Range = Range;
            this.Size = Size;

            this.DamageIncreace = 0;
            this.SizeIncreace = 0;


            this.AttenuationRatio = 0;
        }

        public BulletAttribute(float Speed, float Damage, float Range, float Size, float value1, float value2)
        {
            this.Speed = Speed;
            this.Damage = Damage;
            this.Range = Range;
            this.Size = Size;

            this.DamageIncreace = value1;
            this.SizeIncreace = value2;

            this.AttenuationRatio = 0;
        }

        public BulletAttribute(float Speed, float Damage, float Range, float Size, float value1)
        {
            this.Speed = Speed;
            this.Damage = Damage;
            this.Range = Range;
            this.Size = Size;

            this.DamageIncreace = value1;
            this.SizeIncreace = value1;

            this.AttenuationRatio = value1;
        }
    }
}



