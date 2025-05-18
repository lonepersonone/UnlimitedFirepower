using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Weapon
{
    public interface IExplosionDamageable
    {
        public void TakeDamage(DamageType type, float damage);
    }

}


