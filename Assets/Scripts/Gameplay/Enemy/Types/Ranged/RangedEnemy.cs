using MyGame.Framework.Audio;
using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class RangedEnemy : EnemyController, IRangedEnemy
    {

        public override void Attack()
        {
            Shoot();
        }

        public void Shoot()
        {
            if (attackComponentE.CanAttack())
            {
                foreach (var firePoint in FirePoints)
                {
                    Quaternion rotation = Quaternion.Euler(-(firePoint.rotation.eulerAngles.z + 90), 90, 0);
                    GameObject instance = Instantiate(characterData.WeaponData.ProjectilePrefab, firePoint.position, rotation);
                    Bullet bullet = instance.GetComponent<Bullet>();
                    bullet.Initialize(characterData.WeaponData, moveComponentE.CurrentSpeed, BulletType.Enemy);
                }

                AudioManager.Instance.PlayEnemyBasicFire();

                attackComponentE.RecordAttackTime();
            }
        }

    }
}

