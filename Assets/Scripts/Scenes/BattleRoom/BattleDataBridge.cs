using MyGame.Data.SO;
using MyGame.Gameplay.Level;
using MyGame.Gameplay.Player;
using MyGame.Gameplay.Prop;
using MyGame.Gameplay.Upgrade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Scene.BattleRoom
{
    /// <summary>
    /// 存储战斗房间所需数据
    /// </summary>
    public class BattleDataBridge : MonoBehaviour
    {
        public static BattleDataBridge Instance;

        private ScriptableManager scriptableManager;
        private LevelAttribute levelAttribute;
        private PlayerAttribute playerAttribute;
        private UpgradeAttribute upgradeAttribute;
        private WealthAttribute playerWealth; //玩家财富

        public ScriptableManager ScriptableManager => scriptableManager;
        public LevelAttribute LevelAttribute => levelAttribute;
        public PlayerAttribute PlayerAttribute => playerAttribute;
        public UpgradeAttribute UpgradeAttribute => upgradeAttribute;
        public WealthAttribute WealthAttribute => playerWealth;

        private void Awake()
        {
            Instance = this;
        }

        // 初始化数据
        public void InitializeData(ScriptableManager manager, LevelAttribute level, PlayerAttribute player, UpgradeAttribute upgrade, WealthAttribute wealth)
        {
            scriptableManager = manager;
            levelAttribute = level;
            playerAttribute = player;
            upgradeAttribute = upgrade;
            playerWealth = wealth;
        }

        // 设置单个数据对象（可选）
        public void SetScriptableManager(ScriptableManager manager) => scriptableManager = manager;
        public void SetLevelAttribute(LevelAttribute level) => levelAttribute = level;
        public void SetPlayerAttribute(PlayerAttribute player) => playerAttribute = player;
        public void SetUpgradeAttribute(UpgradeAttribute upgrade) => upgradeAttribute = upgrade;
        public void SetPlayerWealth(WealthAttribute wealth) => playerWealth = wealth;

        // 清理数据（场景切换时）
        public void ClearData()
        {
            scriptableManager = null;
            levelAttribute = null;
            playerAttribute = null;
            upgradeAttribute = null;
            playerWealth = null;
        }

    }

}



