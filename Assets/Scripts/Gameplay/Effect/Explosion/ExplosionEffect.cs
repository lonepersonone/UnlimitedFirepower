using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Effect
{
    public class ExplosionEffect : MonoBehaviour
    {
        private float damage = 0;

        public void SetDamage(float value)
        {
            damage = value;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.CompareTag("Player") || collision.CompareTag("UAV"))
                {
                    OnHitTarget(collision);
                }
            }
        }

        public void OnHitTarget(Collider2D collider)
        {
            IExplosionDamageable[] explosionDamageables = collider.GetComponents<IExplosionDamageable>();
            foreach (var explosionDamageable in explosionDamageables)
            {
                explosionDamageable.TakeDamage(DamageType.Basics, damage);
            }
        }

        private void OnDestroy()
        {
            Destroy(this.gameObject);
        }
    }


}
