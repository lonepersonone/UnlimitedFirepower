using MyGame.Framework.Event;
using MyGame.Framework.Utilities;
using MyGame.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class RebirthComponent : MonoBehaviour, IAirplaneComponent
    {
        private CharacterAttribute data;

        public void Initialize(PlayerController player)
        {
            data = player.CharacterData;

            GameEventManager.RegisterListener(GameEventType.PlayerRebirth, Rebirth);
        }

        public void UpdateComponent()
        {

        }

        /// <summary>
        /// ∏¥ªÓ÷ÿ…˙
        /// </summary>
        public void Rebirth()
        {
            if (data.Health >= 100)
            {
                List<Vector3> birthPositions = TransformUtil.GetRingGridPositions(transform.localPosition, data.RebirthRange, data.RebirthRange, 30f);
                transform.localPosition = birthPositions[Random.Range(0, birthPositions.Count)];
                data.ReduceMaxHealth();
                GameEventManager.TriggerEvent(GameEventType.PlayerReset);
                StartCoroutine(RebirthAnimation(3f));
            }
            else
            {
                GameManager.Instance.ReturnToLobby();
            }
        }

        private IEnumerator RebirthAnimation(float animateTime)
        {
            SpriteRenderer[] sprites = transform.GetComponents<SpriteRenderer>();

            for (int i = 0; i < 3; i++)
            {
                float time = 0;
                while (time <= animateTime)
                {
                    float ratio = time / animateTime;
                    foreach (var sprite in sprites)
                        sprite.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Lerp(1, 0, ratio));
                    time += Time.fixedDeltaTime;
                    yield return null;
                }

                time = 0;
                while (time <= animateTime)
                {
                    float ratio = time / animateTime;
                    foreach (var sprite in sprites)
                        sprite.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Lerp(0, 1, ratio));
                    time += Time.fixedDeltaTime;
                    yield return null;
                }
            }
        }
    }

}


