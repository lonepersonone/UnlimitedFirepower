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
        Easy, //��
        Normal, //��ͨ
        Difficult, //����
        Nightmare //ج��
    }

    /// <summary>
    /// ��Ϸ��̬�Ѷȵ���
    /// 1.����׶�����
    /// 2.��̬����ÿ���׶�Hex�Ѷ�
    /// </summary>
    public class RoundManager : GameSystemBase
    {
        public static RoundManager Instance;

        public ProgressBar RoundProgressBar;
        public TextMeshProUGUI GalaxyName;

        private int basicStep = 16;
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

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            currentStep = basicStep;

            RoundProgressBar.SetRange(0, currentStep);
            RoundProgressBar.SetValue(currentStep);

            LocalizationManager.instance.switchLanguageAction += LocalizeGalaxyName;
            
            GameEventManager.RegisterListener(GameEventType.LevelStarted, GenerateNextGalaxy);

            GenerateNextGalaxy();

            await Task.Delay(100);

            IsReady = true;
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.LevelStarted, GenerateNextGalaxy);
        }

        public void ResetStep()
        {
            currentStep = basicStep;
            RoundProgressBar.SetValue(currentStep);

            if (LocalizationManager.instance != null) LocalizeGalaxyName(LocalizationManager.instance.currentLanguage);
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

        // ִ���л���Ϊ
        public void ChangeGalaxy()
        {
            SceneController.Instance.EnterNextGalaxy();
        }

        public bool CanChangeGalaxy(PlanetController planet)
        {
            return currentStep == 0 || planet.GetPlanetType() == HexCellType.Channel;
        }

        // ������ϵ����
        private void GenerateNextGalaxy()
        {
            GalaxyAttribute galaxyAttribute = MainDataManager.Instance.MapData.GenerateNextGalay();

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
                if(MainDataManager.Instance.MapData.CurrentGalxy != null)
                    SetGalaxyName(MainDataManager.Instance.MapData.CurrentGalxy.EN);
            }
            else if(value.Contains("cn-CN"))
            {
                if (MainDataManager.Instance.MapData.CurrentGalxy != null)
                    SetGalaxyName(MainDataManager.Instance.MapData.CurrentGalxy.CN);
            }
        }

    }

}


