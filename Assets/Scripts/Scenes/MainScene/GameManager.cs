using Michsky.UI.Reach;
using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.UI.Settlement;
using MyGame.UI.Transition;
using System;
using UnityEngine;

namespace MyGame.Scene.Main
{
    /// <summary>
    /// 管理全局游戏对象生成流程
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameSystemBase[] systems;

        private async void Awake()
        {
            Instance = this;

            foreach (var system in systems)
            {
                await system.InitializeAsync(UpdateProgress);
            }           
        }

        private void Start()
        {
            GameEventManager.RegisterListener(GameEventType.SpawnUI, OnSpawnUI);
            GameEventManager.RegisterListener(GameEventType.ScaleCamera, OnScaleCamera);
            GameEventManager.RegisterListener(GameEventType.GameStarted, PlaySound);
        }

        private void OnDestroy()
        {
            GameEventManager.ClearListener(GameEventType.SpawnUI);
            GameEventManager.ClearListener(GameEventType.ScaleCamera);
            GameEventManager.ClearListener(GameEventType.GameStarted);
        }

        private void Update()
        {

        }

        public void UpdateProgress(float progress)
        {
            //Debug.Log(progress);
        }

        public void EnableGameSettlement()
        {
            //SettlementManager.Instance.EnableSettlement();
        }

        public void ReturnToLobby()
        {
            if (SceneController.Instance != null) SceneController.Instance.ReturnToLobby();
        }

        private void OnSpawnUI()
        {
            CanvasManager.Instance.ShowHudCanvas();
        }

        private void OnScaleCamera()
        {
            MainCameraManager.Instance.ScaleCamera();
        }

        public void PlaySound()
        {
            AudioHelper.PlayOneShot(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.Scene, AudioAction.Profound));
        }

    }
}


