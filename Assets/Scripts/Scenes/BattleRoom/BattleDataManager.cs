using MyGame.Data.SO;
using MyGame.Gameplay.Level;
using MyGame.Gameplay.Player;
using MyGame.Gameplay.Prop;
using MyGame.Gameplay.Upgrade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MyGame.Scene.BattleRoom
{
    [DefaultExecutionOrder(-100)]
    public class BattleDataManager : MonoBehaviour
    {
        public static BattleDataManager Instance;

        private ScriptableManager scriptableManager;
        private LevelAttribute levelAttribute;
        private PlayerAttribute playerAttribute;
        private UpgradeAttribute upgradeAttribute;
        private WealthAttribute playerWealth; //Íæ¼Ò²Æ¸»

        public ScriptableManager ScriptableManager => scriptableManager;
        public LevelAttribute LevelAttribute => levelAttribute;
        public PlayerAttribute PlayerAttribute => playerAttribute;
        public UpgradeAttribute UpgradeAttribute => upgradeAttribute;
        public WealthAttribute PlayerWealth => playerWealth;

        private void Awake()
        {
            Instance = this;
        }

        public async Task InitializeAsync(Action<float> onProgress = null)
        {
            scriptableManager = BattleDataBridge.Instance.ScriptableManager;
            playerWealth = BattleDataBridge.Instance.WealthAttribute;
            upgradeAttribute = BattleDataBridge.Instance.UpgradeAttribute;
            playerAttribute = BattleDataBridge.Instance.PlayerAttribute;
            levelAttribute = BattleDataBridge.Instance.LevelAttribute;

            UpgradeEventRegister.RegistEvent(upgradeAttribute.Items);

            await Task.Delay(100);

            onProgress?.Invoke(1f);
        }

        private void OnDestroy()
        {
            UpgradeEventRegister.UnRegistEvent(upgradeAttribute.Items);
        }

    }
}


