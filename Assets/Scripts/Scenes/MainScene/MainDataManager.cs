using MyGame.Data.SO;
using MyGame.Framework.Manager;
using MyGame.Gameplay.Map;
using MyGame.Gameplay.Player;
using MyGame.Gameplay.Prop;
using MyGame.Gameplay.Upgrade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace MyGame.Scene.Main
{

    [DefaultExecutionOrder(-100)]
    public class MainDataManager : GameSystemBase
    {
        public static MainDataManager Instance;

       [SerializeField] private GameMapConfigSO GameMapConfig;

        // ScriptableObject 数据
        private ScriptableManager scriptableManager;

        // 运行实例
        private PlayerAttribute playerData;
        private UpgradeAttribute upgradeData;
        private WealthAttribute wealthData; //玩家财富
        private MapAttribute mapData;

        public ScriptableManager ScriptableManager => scriptableManager;
        public PlayerAttribute PlayerData => playerData;
        public UpgradeAttribute UpgradeData => upgradeData;
        public WealthAttribute WealthData => wealthData;
        public MapAttribute MapData => mapData;


        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            scriptableManager = new ScriptableManager();

            await scriptableManager.LoadScriptableData(onProgress);

            await CreateData();

            LogicInitializer.InitialLogic(this);

            await Task.Delay(100);

            IsReady = true;
        }

        private async Task CreateData()
        {
            playerData = new PlayerAttribute(scriptableManager);
            upgradeData = new UpgradeAttribute(scriptableManager);
            wealthData = new WealthAttribute(scriptableManager);
            mapData = new MapAttribute(GameMapConfig);

            await Task.Delay(100);
        }
    }

}

