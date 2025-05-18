using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class HealthComponentU : MonoBehaviour, IUAVComponent, IBulletDamageable, IExplosionDamageable
    {
        private UnmannedAerialVehicle uav;
        private float currentHealth;

        private UAVComponent parent;

        public void Initialize(UnmannedAerialVehicle uav)
        {
            this.uav = uav;
            currentHealth = uav.CharacterData.Health;
        }

        public void UpdateComponent()
        {
            if (parent == null) parent = PlayerController.Instance.GetComponent<UAVComponent>();
        }

        public void TakeDamage(DamageType type, float damage)
        {
            if (currentHealth - damage <= 0 && parent != null) parent.RemoveUAV(this.gameObject);
            currentHealth -= damage;
        }

    }
}


