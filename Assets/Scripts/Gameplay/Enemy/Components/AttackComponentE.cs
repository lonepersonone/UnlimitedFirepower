using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class AttackComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;

        private float lastAttackTime;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateComponent()
        {

        }

        public bool CanAttack()
        {
            return Time.time - lastAttackTime >= enemy.CharacterData.WeaponData.AttackInterval;
        }

        public void RecordAttackTime()
        {
            lastAttackTime = Time.time;
        }

    }
}


