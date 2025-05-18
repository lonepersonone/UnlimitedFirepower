using Cinemachine;
using MyGame.Gameplay.Effect;
using MyGame.Gameplay.Level;
using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class CannoFodderEnemy : EnemyController
    {
        public override void Initialize(CharacterAttribute characterAttribute)
        {
            base.Initialize(characterAttribute);
            AddBehavior(new SplitBehaviour());
        }


        public override void Attack()
        {
            Explode();
        }

        private void Explode()
        {
            ExplosionEffect explosion = Object.Instantiate(characterData.ExplosionPrefab).GetComponent<ExplosionEffect>();
            explosion.SetDamage(characterData.WeaponData.Damage);
            explosion.transform.position = transform.position;
            minimapComponentE.DestroyMinimapIcon();
            LevelManager.Instance.ReduceEnemy(this.gameObject);
        }

    }
}


