using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class MeleeEnemy : EnemyController
    {
        private RaycastHit2D[] rayHits = new RaycastHit2D[5];

        public override void Attack()
        {
            Raycast();
        }

        protected void Raycast()
        {
            int count = RaycastNonAlloc();
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D hit = rayHits[i];
                IBulletDamageable[] bulletDamageables = hit.collider.GetComponents<IBulletDamageable>();
                foreach (var bulletDamageable in bulletDamageables)
                {
                    bulletDamageable.TakeDamage(DamageType.Basics, characterData.WeaponData.Damage);
                }
            }
        }

        protected int RaycastNonAlloc()
        {
            return Physics2D.RaycastNonAlloc(transform.position, transform.forward, rayHits, 2f, LayerMask.GetMask("Player", "UAV"));
        }
    }
}


