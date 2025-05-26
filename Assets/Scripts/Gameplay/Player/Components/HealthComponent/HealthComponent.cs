using Cinemachine;
using Michsky.UI.Reach;
using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Gameplay.Effect;
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

            GameEventManager.RegisterListener(GameEventType.PlayerReset, ResetHealth);
        }

        public void UpdateComponent()
        {
            if (currentHealth < data.Health)
                if (Time.time - lastDamageTime > data.CoolTime)
                    RecoverHealth();

            if (lastHealth != data.Health) ResetHealth();
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.PlayerReset, ResetHealth);
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

            damage *= (1 - data.DamageReduction);

            if (shieldComponent.CurrentShield <= 0)
            {                 
                currentHealth -= damage;
                healthBar.SetValue(Mathf.Clamp(currentHealth, 0, data.Health));

                if (currentHealth <= 0)
                {
                    GetComponent<RebirthComponent>().Rebirth();
                    return;
                }
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


