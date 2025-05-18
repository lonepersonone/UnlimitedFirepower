using MyGame.Gameplay.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame.Gameplay.Enemy
{
    public class EffectComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateComponent()
        {

        }

        public void CreateSpawnEffect(Vector3 postion)
        {
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("SpawnEnemy"), postion);
            //Object.Instantiate(enemy.CharacterData.SpawnPrefab).transform.position = postion;
        }

        public void CreateDieEffect(Vector3 position)
        {
            Object.Instantiate(enemy.CharacterData.DiedPrefab).transform.position = position;
        }

    }
}


