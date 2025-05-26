using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Guide
{

    // �������״̬ö��
    public enum GuidePanelState
    {
        Show = 0,       // ��ʾ
        DoNotShow = 1,  // ������ʾ
        Completed = 2   // ����ɣ���ѡ״̬��
    }

    public class NewPlayerGuideManager : MonoBehaviour
    {
        // ����ģʽ
        public static NewPlayerGuideManager Instance { get; private set; }

        // �洢������������״̬
        private Dictionary<string, GuidePanelState> guidePanelStates = new Dictionary<string, GuidePanelState>();

        // �洢��ǰ׺������PlayerPrefs
        private const string PLAYER_PREFS_KEY_PREFIX = "NewPlayerGuide_";

        // ��֪���ID�б���Inspector�����ã�
        [SerializeField]
        private List<string> knownPanelIds = new List<string> {
            "CombatGuide",    // ս����������
            "MapGuide",       // ��ͼ��������
        // ��Ӹ�����֪���ID...
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

            // ��ʼ����֪����״̬
            InitializeKnownPanels();

            // �����ѱ��������״̬
            LoadGuideStates();
        }

        // ��ʼ����֪����Ĭ��״̬
        private void InitializeKnownPanels()
        {
            // ȷ��ÿ����֪��嶼��Ĭ��״̬
            foreach (string panelId in knownPanelIds)
            {
                if (!guidePanelStates.ContainsKey(panelId))
                {
                    guidePanelStates[panelId] = GuidePanelState.Show;
                }
            }
        }

        // ����������������״̬
        private void LoadGuideStates()
        {
            guidePanelStates.Clear();

            // ��ȡ������ָ��ǰ׺��ͷ��PlayerPrefs��
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

            // ȷ��������֪��嶼��״̬���״μ���ʱ����ȱʧ��
            InitializeKnownPanels();

            // ����״̬��ȷ��������֪����״̬���־û�
            SaveGuideStates();
        }

        // ����������������״̬
        private void SaveGuideStates()
        {
            // �������м����б�
            string allKeys = string.Join(",", guidePanelStates.Keys);
            PlayerPrefs.SetString("NewPlayerGuide_AllKeys", allKeys);

            // ����ÿ������״̬
            foreach (var pair in guidePanelStates)
            {
                string fullKey = PLAYER_PREFS_KEY_PREFIX + pair.Key;
                PlayerPrefs.SetInt(fullKey, (int)pair.Value);
            }

            PlayerPrefs.Save();
        }

        // ������������״̬
        public void SetGuidePanelState(string panelId, GuidePanelState state)
        {
            guidePanelStates[panelId] = state;
            SaveGuideStates();
        }

        // ��ȡ��������״̬
        public GuidePanelState GetGuidePanelState(string panelId)
        {
            if (guidePanelStates.TryGetValue(panelId, out GuidePanelState state))
            {
                return state;
            }

            // ��������ڣ�����Ĭ��״̬����ʾ��
            return GuidePanelState.Show;
        }

        // ����Ƿ�Ӧ����ʾ�������
        public bool ShouldShowPanel(string panelId)
        {
            return GetGuidePanelState(panelId) == GuidePanelState.Show;
        }

        // ���������������״̬�����ڲ��Ի���������
        public void ResetAllGuides()
        {
            guidePanelStates.Clear();

            // ɾ��������ص�PlayerPrefs��
            string[] allKeys = PlayerPrefs.GetString("NewPlayerGuide_AllKeys", "").Split(',');
            foreach (string key in allKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;
                PlayerPrefs.DeleteKey(PLAYER_PREFS_KEY_PREFIX + key);
            }

            PlayerPrefs.DeleteKey("NewPlayerGuide_AllKeys");
            PlayerPrefs.Save();

            Debug.Log("�����������������ע�����Ϣ");
        }

        // �������ע�����Ϣ�������׵������
        public void ClearAllRegistryData()
        {
            // ��ȡ����PlayerPrefs��
            var allKeys = new List<string>();
            var type = typeof(PlayerPrefs);
            var method = type.GetMethod("GetAllKeys", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                allKeys.AddRange(method.Invoke(null, null) as IEnumerable<string>);
            }

            // ɾ��������ָ��ǰ׺��ͷ�ļ�
            foreach (string key in allKeys)
            {
                if (key.StartsWith(PLAYER_PREFS_KEY_PREFIX))
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }

            // ȷ������״̬����
            guidePanelStates.Clear();
            PlayerPrefs.Save();

            Debug.Log("�ѳ������������������ע�����Ϣ");
        }
    }


}

