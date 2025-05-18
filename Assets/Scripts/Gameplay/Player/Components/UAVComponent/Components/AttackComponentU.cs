using MyGame.Framework.Audio;
using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    public class AttackComponentU : MonoBehaviour, IUAVComponent
    {
        private UnmannedAerialVehicle uav;
        private float lastFireTime;

        public void Initialize(UnmannedAerialVehicle uav)
        {
            this.uav = uav;
        }

        public void UpdateComponent()
        {

        }

        public void CreateProjectile()
        {
            Quaternion rotation = Quaternion.Euler(-(transform.rotation.eulerAngles.z + 90), 90, 0);
            GameObject instance = Instantiate(uav.CharacterData.WeaponData.ProjectilePrefab, transform.position, rotation);
            Bullet bullet = instance.GetComponent<Bullet>();
            bullet.Initialize(uav.CharacterData.WeaponData, 0f, BulletType.Player);
        }

        public bool CanFire() { return Time.time - lastFireTime >= uav.CharacterData.WeaponData.FireRate; }

        public void RecordFireTime() { lastFireTime = Time.time; }

    }
}


