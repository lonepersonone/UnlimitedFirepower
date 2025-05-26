using MyGame.Framework.Notification;
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

        private MapNavigationController navigationController;
        private MapUIManager uiManager;
        private GalaxyExplorationManager explorationManager;
        private MainDataManager mainData;
        private RoundManager roundManager;

        public bool handleLock = false;

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

            uiManager = new MapUIManager(WealthText, mainData.WealthData);

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
            mainData.MapData.OnGalaxyChanged += explorationManager.ResetGalaxy;
            mainData.MapData.OnGalaxyChanged += navigationController.ResetGalaxy;

            GameEventManager.RegisterListener(GameEventType.BattleWined, ConquereAimGalaxy);
            GameEventManager.RegisterListener(GameEventType.LevelCompleted, ReleaseHandleLock);
            GameEventManager.RegisterListener(GameEventType.LevelCompleted, InitialExplore);

            await Task.Delay(100);

            IsReady = true;
        }

        private void Update()
        {
            if(mainData != null && Camera.main != null)
            {
                HandleMouseInput();
            }
           
            if(uiManager != null)
            {
                if(WealthText != null)
                {
                    uiManager.UpdateWealthText();
                }
            }
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.BattleWined, ConquereAimGalaxy);
            GameEventManager.UnregisterListener(GameEventType.LevelCompleted, ReleaseHandleLock);
            GameEventManager.UnregisterListener(GameEventType.LevelCompleted, InitialExplore);
        }

        private void HandleMouseInput()
        {
            Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            string locationID = HexgonUtil.WorldToLocationID(world);
         
            if (Input.GetMouseButtonDown(0) && !handleLock)
            {
                if (mainData.MapData.CurrentGalxy!= null && mainData.MapData.CurrentGalxy.PlanetDict.ContainsKey(locationID))
                {
                    if(mainData.MapData.CurrentGalxy.PlanetDict[locationID].GetState() == HexCellState.Explored
                        || mainData.MapData.CurrentGalxy.PlanetDict[locationID].GetState() == HexCellState.Conquered)

                        if(!GameState.Pauseable)
                    
                        
                        {
                            int distance = CalculateDistance(mainData.MapData.CurrentGalxy.CurrentPlanetPosition, mainData.MapData.CurrentGalxy.GetPlanetPosition(locationID));
                            if (distance == 1)
                            {
                                if (roundManager.ReduceStep(1))
                                {
                                    navigationController.MoveToGalaxy(locationID);
                                    handleLock = true;
                                }
                        }
                    }
                }        
            }
        }

        private void HandleArrive(Vector3 pos)
        {
            PlanetController planet = mainData.MapData.CurrentGalxy.CurrentPlanet;

            mainData.WealthData.AddExpectEarnings();

            explorationManager.ExploreAroundGalaxy(planet);

            if (planet.GetPlanetType() == HexCellType.Channel)
            {
                roundManager.ChangeGalaxy();
                return;
            }
           
            if (planet.GetState() == HexCellState.Conquered)
            {
                if (roundManager.CanChangeGalaxy(planet))
                {
                    roundManager.ChangeGalaxy();
                    return;
                }
            }

            if (planet.GetPlanetType() == HexCellType.Life)
            {
                if(planet.GetState() == HexCellState.Explored)
                {
                    StartCoroutine(EnterBatter());
                }              
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

            handleLock = false;

            NotificationManager.Instance.ShowFeedNotification("BattleWined");

            AudioHelper.PlayOneShot(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.System, AudioAction.Conquered));

            PlanetController planet = mainData.MapData.CurrentGalxy.CurrentPlanet;
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("ConquerPlanet"), planet.transform.position); 
            if (planet != null) planet.OnConquered();

            mainData.WealthData.AddWealth(planet.GetWealth());

            yield return new WaitForSeconds(1f);

            if (roundManager.CanChangeGalaxy(planet))
            {
                roundManager.ChangeGalaxy();
            }
        }

        private int CalculateDistance(Vector3 start, Vector3 end)
        {
            int[] a = HexgonUtil.WorldToLocation(start); 
            int[] b = HexgonUtil.WorldToLocation(end);

            return (Mathf.Abs(a[0] - b[0]) + Mathf.Abs(a[1] - b[1]) + Mathf.Abs(a[2] - b[2])) / 2;
        }

        private void ReleaseHandleLock()
        {
            handleLock = false;
        }

        private void InitialExplore()
        {
            PlanetController planet = mainData.MapData.CurrentGalxy.CurrentPlanet;
            explorationManager.ExploreAroundGalaxy(planet);
        }

    }
}








