using MyGame.Gameplay.Prop;
using MyGame.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class WealthComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateComponent()
        {

        }

        public void CreateWealth()
        {
            WealthSpawnData spawnData = MainDataManager.Instance.WealthData.WealthSpawnData;
            for (int i = 0; i < enemy.CharacterData.Wealth; i++)
            {
                GameObject instance = Object.Instantiate(spawnData.WealthPrefab);
                instance.transform.position = enemy.transform.position;
                instance.GetComponent<WealthController>().Initialize(spawnData.WealthData);
            }
        }

    }
}


