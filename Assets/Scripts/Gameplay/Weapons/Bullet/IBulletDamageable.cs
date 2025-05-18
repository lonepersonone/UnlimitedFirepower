using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Weapon
{
    public interface IBulletDamageable
    {
        public void TakeDamage(DamageType type, float damage);
    }
}


