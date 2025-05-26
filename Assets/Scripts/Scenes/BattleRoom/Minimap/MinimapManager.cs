using MyGame.Framework.Manager;
using MyGame.Gameplay.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Scene.BattleRoom
{
    public class MinimapManager : GameSystemBase
    {
        public static MinimapManager Instance;

        public GameObject CharacterParentObj;
        public GameObject PlayerObj;
        public GameObject EnemyPrefab;

        public float MapScale = 10;

        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            await Task.Delay(100);

            IsReady = true;
        }

        public void Initialize(Transform player)
        {
            PlayerObj.GetComponent<MinimapCharacterController>().mappingObject = player;
        }

        public MinimapCharacterController CreateEnemy(Transform target)
        {
            GameObject instance = Instantiate(EnemyPrefab, CharacterParentObj.transform);
            instance.GetComponent<MinimapCharacterController>().mappingObject = target;
            return instance.GetComponent<MinimapCharacterController>();
        }

    }
}



