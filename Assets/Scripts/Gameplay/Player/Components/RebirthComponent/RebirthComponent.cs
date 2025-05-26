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
        }

        public void UpdateComponent()
        {

        }

        public void Rebirth()
        {
            if (data.Health >= 100)
            {
                List<Vector3> birthPositions = TransformUtil.GetRingGridPositions(transform.localPosition, data.RebirthRange, data.RebirthRange, 30f);
                transform.localPosition = birthPositions[Random.Range(0, birthPositions.Count)];
                data.ReduceMaxHealth();
                GameEventManager.TriggerEvent(GameEventType.PlayerReset);
            }
            else
            {
                GetComponent<DieComponent>().Die();
            }
        }

    }

}


