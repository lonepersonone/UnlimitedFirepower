using Michsky.UI.Reach;
using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame.Scene.BattleRoom
{
    public class BattleGameInitializer : MonoBehaviour
    {
        public static BattleGameInitializer Instance;

        [SerializeField] private GameSystemBase[] systems;

        private async void Awake()
        {
            Instance = this;

            await BattleDataManager.Instance.InitializeAsync(UpdateProgress);

            foreach (var system in systems)
            {
                await system.InitializeAsync();
            }

        }

        private void Update()
        {
 
        }

        private void OnDestroy()
        {
        }

        private void UpdateProgress(float progress)
        {
            //Debug.Log(progress);
        }


    }
}


