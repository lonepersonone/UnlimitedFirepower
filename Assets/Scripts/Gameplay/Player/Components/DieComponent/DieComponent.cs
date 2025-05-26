using Cinemachine;
using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Gameplay.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class DieComponent : MonoBehaviour, IAirplaneComponent
    {
        [SerializeField] private GameObject SpriteObj;
        [SerializeField] private float timeStopDuration = 3f; // 时停持续时间
        [SerializeField] private float originalTimescale = 1f;
        
        public void Initialize(PlayerController player)
        {
            
        }

        public void UpdateComponent()
        {
            
        }

        public void Die()
        {
            AudioHelper.PlayOneShot(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.System, AudioAction.GameOver));

            DisablePlayerControl();

            ShakeCamera();

            StartCoroutine(DeathSequence());
        }

        private void ShakeCamera()
        {
            CinemachineCollisionImpulseSource collisionImpulseSource = GetComponent<CinemachineCollisionImpulseSource>();
            collisionImpulseSource.m_ImpulseDefinition.m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;
            collisionImpulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
            collisionImpulseSource.m_ImpulseDefinition.m_ImpulseDuration = timeStopDuration;
            collisionImpulseSource.m_UseImpactDirection = true;

            collisionImpulseSource.GenerateImpulse(new Vector3(1, 1, 1));
        }

        private IEnumerator DeathSequence()
        {
            // 1. 保存原始时间缩放并设置为0（时停）
            originalTimescale = Time.timeScale;
            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(1f);

            GameEventManager.TriggerEvent(GameEventType.GameOver);

            // 5. 使用unscaledDeltaTime计时，确保在时停下计时器正常工作
            float elapsedTime = 0f;
            while (elapsedTime < timeStopDuration)
            {
                // 2. 播放死亡动画（使用unscaledTime确保动画在时停下仍能播放）
                if (EffectManager.Instance != null)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        float x = Random.Range(-60, 60);
                        float y = Random.Range(-60, 60);
                        Vector3 newPos = new Vector3(transform.position.x + x, transform.position.y + y, 0);

                        EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("PlayerDie"), newPos);
                        Debug.Log("播放死亡动画");
                    }
                }

                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            // 6. 恢复时间缩放
            Time.timeScale = originalTimescale;

            // 8. 返回主菜单
            SceneController.Instance.ReturnToLobby();

        }

        // 禁用玩家控制
        private void DisablePlayerControl()
        {
            if (SpriteObj != null) SpriteObj.SetActive(false);

            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("PlayerDie"), transform.position);

            // 禁用玩家移动脚本（假设存在PlayerMovement组件）
            MoveComponent movement = GetComponent<MoveComponent>();
            if (movement != null)
            {
                movement.enabled = false;
            }

            FireUnitComponent fireUnit = GetComponent<FireUnitComponent>();
            if (fireUnit != null)
            {
                fireUnit.enabled = false;
            }

            // 可以添加其他需要禁用的组件
        }

    }

}



