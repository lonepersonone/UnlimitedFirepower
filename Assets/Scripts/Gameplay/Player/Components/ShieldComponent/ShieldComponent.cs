using Michsky.UI.Reach;
using MyGame.Framework.Event;
using MyGame.Gameplay.Effect;
using MyGame.Gameplay.Weapon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class ShieldComponent : MonoBehaviour, IAirplaneComponent, IBulletDamageable, IExplosionDamageable
    {
        private CharacterAttribute data;

        [SerializeField] private GameObject ShieldObj;

        private ProgressBar shieldBar;
        private ProgressBar progressBar; //ÐîÁ¦Ìõ

        private float currentShield;
        private float currentProgress;
        private float lastShield;
        private float lastDamageTime;

        public float CurrentShield => currentShield;

        public void Initialize(PlayerController player)
        {
            this.data = player.CharacterData;
            shieldBar = PlayerManager.Instance.ShieldBar;

            shieldBar.SetRange(0, data.Shield);
            shieldBar.SetValue(data.Shield);
            currentShield = data.Shield;
            lastShield = data.Shield;

            progressBar = PlayerManager.Instance.ShieldProgressBar;
            progressBar.SetRange(0, 1);
            ResetProgress();

            GameEventManager.RegisterListener(GameEventType.PlayerReset, ResetShield);
        }

        public void UpdateComponent()
        {
            if (currentShield < data.Shield)
                if (Time.time - lastDamageTime > data.CoolTime)
                    RecoverProgress();

            if (lastShield != data.Shield) ResetShield();
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.PlayerReset, ResetShield);
        }

        private void ResetProgress()
        {
            currentProgress = 0;
            progressBar.SetValue(currentProgress);
        }

        private void RecoverProgress()
        {
            currentProgress += data.ShieldRecover;
            progressBar.SetValue(currentProgress);
            if (currentProgress >= 1) ResetShield();
        }

        private void ResetShield()
        {
            ResetProgress();
            currentShield = data.Shield;
            shieldBar?.SetValue(currentShield);
            ShieldObj.SetActive(true);
        }

        private void BrokenShield()
        {
            ShieldObj.SetActive(false);
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("BrokenShield"), transform.position);
        }

        public void TakeDamage(DamageType type, float damage)
        {
            lastDamageTime = Time.time;
            ResetProgress();
            if (currentShield > 0)
            {
                if(currentShield <= damage) BrokenShield();
                currentShield -= damage;
                shieldBar?.SetValue(Mathf.Clamp(currentShield, 0, data.Shield));
            }
        }

    }
}


