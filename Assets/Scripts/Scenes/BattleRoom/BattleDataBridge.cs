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
    /// �洢ս��������������
    /// </summary>
    public class BattleDataBridge : MonoBehaviour
    {
        public static BattleDataBridge Instance;

        private ScriptableManager scriptableManager;
        private LevelAttribute levelAttribute;
        private PlayerAttribute playerAttribute;
        private UpgradeAttribute upgradeAttribute;
        private WealthAttribute playerWealth; //��ҲƸ�

        public ScriptableManager ScriptableManager => scriptableManager;
        public LevelAttribute LevelAttribute => levelAttribute;
        public PlayerAttribute PlayerAttribute => playerAttribute;
        public UpgradeAttribute UpgradeAttribute => upgradeAttribute;
        public WealthAttribute WealthAttribute => playerWealth;

        private void Awake()
        {
            Instance = this;
        }

        // ��ʼ������
        public void InitializeData(ScriptableManager manager, LevelAttribute level, PlayerAttribute player, UpgradeAttribute upgrade, WealthAttribute wealth)
        {
            scriptableManager = manager;
            levelAttribute = level;
            playerAttribute = player;
            upgradeAttribute = upgrade;
            playerWealth = wealth;
        }

        // ���õ������ݶ��󣨿�ѡ��
        public void SetScriptableManager(ScriptableManager manager) => scriptableManager = manager;
        public void SetLevelAttribute(LevelAttribute level) => levelAttribute = level;
        public void SetPlayerAttribute(PlayerAttribute player) => playerAttribute = player;
        public void SetUpgradeAttribute(UpgradeAttribute upgrade) => upgradeAttribute = upgrade;
        public void SetPlayerWealth(WealthAttribute wealth) => playerWealth = wealth;

        // �������ݣ������л�ʱ��
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



