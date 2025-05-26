using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Guide
{

    // 引导面板状态枚举
    public enum GuidePanelState
    {
        Show = 0,       // 显示
        DoNotShow = 1,  // 不再显示
        Completed = 2   // 已完成（可选状态）
    }

    public class NewPlayerGuideManager : MonoBehaviour
    {
        // 单例模式
        public static NewPlayerGuideManager Instance { get; private set; }

        // 存储所有引导面板的状态
        private Dictionary<string, GuidePanelState> guidePanelStates = new Dictionary<string, GuidePanelState>();

        // 存储键前缀，用于PlayerPrefs
        private const string PLAYER_PREFS_KEY_PREFIX = "NewPlayerGuide_";

        // 已知面板ID列表（在Inspector中配置）
        [SerializeField]
        private List<string> knownPanelIds = new List<string> {
            "CombatGuide",    // 战斗引导界面
            "MapGuide",       // 地图引导界面
        // 添加更多已知面板ID...
        };

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            //ResetAllGuides();

            // 初始化已知面板的状态
            InitializeKnownPanels();

            // 加载已保存的引导状态
            LoadGuideStates();
        }

        // 初始化已知面板的默认状态
        private void InitializeKnownPanels()
        {
            // 确保每个已知面板都有默认状态
            foreach (string panelId in knownPanelIds)
            {
                if (!guidePanelStates.ContainsKey(panelId))
                {
                    guidePanelStates[panelId] = GuidePanelState.Show;
                }
            }
        }

        // 加载所有引导面板的状态
        private void LoadGuideStates()
        {
            guidePanelStates.Clear();

            // 获取所有以指定前缀开头的PlayerPrefs键
            string[] allKeys = PlayerPrefs.GetString("NewPlayerGuide_AllKeys", "").Split(',');

            foreach (string key in allKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                string fullKey = PLAYER_PREFS_KEY_PREFIX + key;
                if (PlayerPrefs.HasKey(fullKey))
                {
                    int stateValue = PlayerPrefs.GetInt(fullKey);
                    guidePanelStates[key] = (GuidePanelState)stateValue;
                }
            }

            // 确保所有已知面板都有状态（首次加载时可能缺失）
            InitializeKnownPanels();

            // 保存状态，确保所有已知面板的状态被持久化
            SaveGuideStates();
        }

        // 保存所有引导面板的状态
        private void SaveGuideStates()
        {
            // 构建所有键的列表
            string allKeys = string.Join(",", guidePanelStates.Keys);
            PlayerPrefs.SetString("NewPlayerGuide_AllKeys", allKeys);

            // 保存每个面板的状态
            foreach (var pair in guidePanelStates)
            {
                string fullKey = PLAYER_PREFS_KEY_PREFIX + pair.Key;
                PlayerPrefs.SetInt(fullKey, (int)pair.Value);
            }

            PlayerPrefs.Save();
        }

        // 设置引导面板的状态
        public void SetGuidePanelState(string panelId, GuidePanelState state)
        {
            guidePanelStates[panelId] = state;
            SaveGuideStates();
        }

        // 获取引导面板的状态
        public GuidePanelState GetGuidePanelState(string panelId)
        {
            if (guidePanelStates.TryGetValue(panelId, out GuidePanelState state))
            {
                return state;
            }

            // 如果不存在，返回默认状态（显示）
            return GuidePanelState.Show;
        }

        // 检查是否应该显示引导面板
        public bool ShouldShowPanel(string panelId)
        {
            return GetGuidePanelState(panelId) == GuidePanelState.Show;
        }

        // 重置所有引导面板状态（用于测试或特殊需求）
        public void ResetAllGuides()
        {
            guidePanelStates.Clear();

            // 删除所有相关的PlayerPrefs键
            string[] allKeys = PlayerPrefs.GetString("NewPlayerGuide_AllKeys", "").Split(',');
            foreach (string key in allKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                PlayerPrefs.DeleteKey(PLAYER_PREFS_KEY_PREFIX + key);
            }

            PlayerPrefs.DeleteKey("NewPlayerGuide_AllKeys");
            PlayerPrefs.Save();

            Debug.Log("已清除所有新手引导注册表信息");
        }

        // 清除所有注册表信息（更彻底的清除）
        public void ClearAllRegistryData()
        {
            // 获取所有PlayerPrefs键
            var allKeys = new List<string>();
            var type = typeof(PlayerPrefs);
            var method = type.GetMethod("GetAllKeys", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                allKeys.AddRange(method.Invoke(null, null) as IEnumerable<string>);
            }

            // 删除所有以指定前缀开头的键
            foreach (string key in allKeys)
            {
                if (key.StartsWith(PLAYER_PREFS_KEY_PREFIX))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }

            // 确保所有状态重置
            guidePanelStates.Clear();
            PlayerPrefs.Save();

            Debug.Log("已彻底清除所有新手引导注册表信息");
        }
    }


}

