using Michsky.UI.Reach;
using MyGame.Framework.Event;
using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class HealthComponent : MonoBehaviour, IAirplaneComponent, IBulletDamageable, IExplosionDamageable
    {
        private CharacterAttribute data;

        private ProgressBar healthBar;
        private ShieldComponent shieldComponent;

        private float currentHealth;
        private float lastHealth;
        private float lastDamageTime;

        public float CurrentHealth => currentHealth;

        public void Initialize(PlayerController player)
        {
            data = player.CharacterData;
            healthBar = PlayerManager.Instance.HealthBar;

            healthBar.SetRange(0, data.Health);
            currentHealth = data.Health;
            healthBar.SetValue(Mathf.Clamp(currentHealth, 0, data.Health));

            lastHealth = data.Health;

            if (transform.GetComponent<ShieldComponent>() != null) shieldComponent = transform.GetComponent<ShieldComponent>();
        }

        public void UpdateComponent()
        {
            if (currentHealth < data.Health)
                if (Time.time - lastDamageTime > data.CoolTime)
                    RecoverHealth();

            if (lastHealth != data.Health) ResetHealth();
        }

        public void RecoverHealth()
        {
            float recoverValue = data.Health * data.HealthRecover;
            currentHealth += recoverValue;
            healthBar?.SetValue(Mathf.Clamp(currentHealth, 0, data.Health));
        }

        public void TakeDamage(DamageType type, float damage)
        {
            lastDamageTime = Time.time;

            if (shieldComponent.CurrentShield <= 0)
            {
                if (currentHealth <= damage) GameEventManager.TriggerEvent(GameEventType.PlayerRebirth);
                currentHealth -= damage;
                healthBar.SetValue(Mathf.Clamp(currentHealth, 0, data.Health));
            }
        }

        public void ResetHealth()
        {
            healthBar.SetRange(0, data.Health);           
            currentHealth = data.Health;
            healthBar?.SetValue(Mathf.Clamp(currentHealth, 0, data.Health));
            lastHealth = data.Health;
        }

    }
}


