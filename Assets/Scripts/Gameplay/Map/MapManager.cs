using Michsky.UI.Reach;
using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.Framework.Utilities;
using MyGame.Gameplay.Effect;
using MyGame.Scene.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class MapManager : GameSystemBase
    {
        public static MapManager Instance;

        public TextMeshProUGUI WealthText;
        public FeedNotification SuccessNotification;
        public QuestItem FailedNotification;

        private MapNavigationController navigationController;
        private MapUIManager uiManager;
        private GalaxyExplorationManager explorationManager;
        private MainDataManager mainData;
        private RoundManager roundManager;

        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            mainData = MainDataManager.Instance;
            roundManager = RoundManager.Instance;

            // 初始化各管理器
            navigationController = new MapNavigationController(
                mainData.MapData.SpaceShipController,
                mainData.MapData.CurrentGalxy);

            uiManager = new MapUIManager(
                WealthText, SuccessNotification, FailedNotification,
                mainData.WealthData);

            explorationManager = new GalaxyExplorationManager(
                mainData.MapData.CurrentGalxy);

            // 注册到达事件
            navigationController.OnArrive += HandleArrive;

            // 更新UI
            uiManager.UpdateWealthText();

            // 设置飞船和相机
            MainCameraManager.Instance.SetObserverCamera(
                mainData.MapData.SpaceShipController.transform);

            // 注册更换星系事件
            mainData.MapData.OnChanged += explorationManager.ResetGalaxy;
            mainData.MapData.OnChanged += navigationController.ResetGalaxy;

            GameEventManager.RegisterListener(GameEventType.PlanetConquered, ConquereAimGalaxy);

            await Task.Delay(100);
        }

        private void Update()
        {
            if(mainData != null && Camera.main != null)
            {
                HandleMouseInput();
            }
           
        }

        private void HandleMouseInput()
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            string locationID = HexgonUtil.WorldToLocationID(world);
         
            if (Input.GetMouseButtonDown(0) &&
                mainData.MapData.CurrentGalxy.PlanetDict.ContainsKey(locationID) &&
                !GameState.Pauseable)
            {
                int distance = CalculateDistance(mainData.MapData.CurrentGalxy.CurrentPlanetPosition, mainData.MapData.CurrentGalxy.GetPlanetPosition(locationID));
                if(distance == 1)
                {
                    if (roundManager.ReduceStep(1))
                    {
                        navigationController.MoveToGalaxy(locationID);
                    }                    
                }              
            }
        }

        private void HandleArrive(Vector3 pos)
        {
            string locationID = HexgonUtil.WorldToLocationID(pos);

            mainData.WealthData.AddExpectEarnings();
            uiManager.UpdateWealthText();
            explorationManager.ExploreAroundGalaxy(locationID);

            PlanetController planet = mainData.MapData.CurrentGalxy.CurrentPlanet;
            if(planet.GetPlanetType() == HexCellType.Life)
            {
                StartCoroutine(EnterBatter());
            }
            else if(planet.GetPlanetType() == HexCellType.Channel)
            {
                GameEventManager.TriggerEvent<PlanetController>(GameEventType.RoundChange, planet);
            }           
        }

        private IEnumerator EnterBatter()
        {
            yield return new WaitForSeconds(1f);
            SceneController.Instance.EnterBattleScene();
        }

        public void ConquereAimGalaxy()
        {
            StartCoroutine(Conquered());
        }

        private IEnumerator Conquered()
        {
            yield return new WaitForSeconds(1f);
            if (SuccessNotification != null) SuccessNotification.SetLocalizationKey("SuccessConquered");

            PlanetController planet = mainData.MapData.CurrentGalxy.CurrentPlanet;
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("ConquerPlanet"), planet.transform.position); 
            if (planet != null) planet.OnConquered();

            yield return new WaitForSeconds(1f);
            GameEventManager.TriggerEvent<PlanetController>(GameEventType.RoundChange, planet);
        }

        private int CalculateDistance(Vector3 start, Vector3 end)
        {
            int[] a = HexgonUtil.WorldToLocation(start); 
            int[] b = HexgonUtil.WorldToLocation(end);

            return (Mathf.Abs(a[0] - b[0]) + Mathf.Abs(a[1] - b[1]) + Mathf.Abs(a[2] - b[2])) / 2;
        }

        public void ArriveNextGalaxy()
        {
            GalaxyAttribute galaxyAttribute = mainData.MapData.EnableNextGalay();
            if(galaxyAttribute == null)
            {
                GameManager.Instance.EnableGameSettlement();
            }
            else
            {
                roundManager.ResetStep();
            }
            
        }

    }
}








