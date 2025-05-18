using MyGame.Scene.BattleRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class MinimapComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;
        private MinimapCharacterController characterController;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateComponent()
        {

        }

        public void CreateMinimapIcon()
        {
            GameObject instance = Object.Instantiate(MinimapManager.Instance.EnemyPrefab, MinimapManager.Instance.CharacterParentObj.transform);
            characterController = instance.GetComponent<MinimapCharacterController>();
            characterController.Initialize(transform);
        }

        public void DestroyMinimapIcon()
        {
            Destroy(characterController.gameObject);
        }

    }
}


