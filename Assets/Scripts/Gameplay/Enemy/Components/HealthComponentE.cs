using MyGame.Framework.Record;
using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class HealthComponentE : MonoBehaviour, IShipComponentE, IBulletDamageable, IExplosionDamageable
    {
        private EnemyController enemy;
        private float currentHealth;


        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
            currentHealth = enemy.CharacterData.Health;
        }

        public void UpdateComponent()
        {

        }

        public void TakeDamage(DamageType type, float damage)
        {
            //¼ÇÂ¼ÉËº¦
            RecordDataManager.Instance.UpdateDamage(damage);

            if (type == DamageType.Basics) enemy.DynamicTextComponentE.CreateBasicsDynamicText(damage);
            else if (type == DamageType.Critical) enemy.DynamicTextComponentE.CreateCriticalDynamicText(damage);

            if (currentHealth < damage) enemy.Die();
            currentHealth -= damage;            
        }

    }
}


