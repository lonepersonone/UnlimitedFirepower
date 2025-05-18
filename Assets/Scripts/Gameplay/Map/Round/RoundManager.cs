using Michsky.UI.Reach;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.Scene.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public enum DifficultyType
    {
        Easy, //简单
        Normal, //普通
        Difficult, //困难
        Nightmare //噩梦
    }

    /// <summary>
    /// 游戏动态难度调整
    /// 1.计算阶段数据
    /// 2.动态调整每个阶段Hex难度
    /// </summary>
    public class RoundManager : GameSystemBase
    {
        public static RoundManager Instance;

        public ProgressBar RoundProgressBar;
        public TextMeshProUGUI GalaxyName;

        private int basicStep = 5;
        private int currentStep = 0;

        public int CurrentStep => currentStep;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneController.Instance.EnterNextGalaxy();
            }
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener<PlanetController>(GameEventType.RoundChange, EnterNextRound);
            GameEventManager.UnregisterListener(GameEventType.LevelStarted, CreateNextGalaxy);
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            currentStep = basicStep;

            RoundProgressBar.SetRange(0, currentStep);
            RoundProgressBar.SetValue(currentStep);

            LocalizationManager.instance.switchLanguageAction += LocalizeGalaxyName;

            LocalizeGalaxyName(LocalizationManager.instance.currentLanguage);

            GameEventManager.RegisterListener<PlanetController>(GameEventType.RoundChange, EnterNextRound);
            GameEventManager.RegisterListener(GameEventType.LevelStarted, CreateNextGalaxy);

            await Task.Delay(100);
        }

        public void ResetStep()
        {
            currentStep = basicStep;
            RoundProgressBar.SetValue(currentStep);

            LocalizeGalaxyName(LocalizationManager.instance.currentLanguage);
        }

        public bool ReduceStep(int value)
        {
            Debug.Log(value);
            if (currentStep < value) return false;
            else
            {
                currentStep -= value;
                RoundProgressBar.SetValue(currentStep);
                return true;
            }
        }

        public void EnterNextRound(PlanetController planet)
        {
            if (currentStep == 0 || planet.GetPlanetType() == HexCellType.Channel)
            {
                SceneController.Instance.EnterNextGalaxy();
            }            
        }

        private void CreateNextGalaxy()
        {
            GalaxyAttribute galaxyAttribute = MainDataManager.Instance.MapData.EnableNextGalay();

            if (galaxyAttribute != null)
            {
                ResetStep();
            }
            else
            {
                GameManager.Instance.ReturnToLobby();
            }
        }

        private void SetGalaxyName(string value)
        {
            GalaxyName.text = value;
        }

        public void LocalizeGalaxyName(string value)
        {
            //Debug.Log(value);
            if(value.Contains("en-US"))
            {
                SetGalaxyName(MainDataManager.Instance.MapData.CurrentGalxy.EN);
            }
            else if(value.Contains("cn-CN"))
            {
                SetGalaxyName(MainDataManager.Instance.MapData.CurrentGalxy.CN);
            }
        }

    }

}


