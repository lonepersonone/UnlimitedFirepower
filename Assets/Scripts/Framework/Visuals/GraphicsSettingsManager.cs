using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Visuals
{
    // ͼ����������
    public enum GraphicsQuality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Ultra
    }

    // ͼ�����ù�����
    public class GraphicsSettingsManager : MonoBehaviour
    {
        // ����ģʽ
        private static GraphicsSettingsManager instance;
        public static GraphicsSettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GraphicsSettingsManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("GraphicsSettingsManager");
                        instance = obj.AddComponent<GraphicsSettingsManager>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }

        // ��ǰͼ������
        [System.Serializable]
        public class GraphicsSettings
        {
            public Resolution currentResolution;
            public FullScreenMode windowMode = FullScreenMode.ExclusiveFullScreen;
            public int targetFrameRate = 60;
            public bool vsyncEnabled = false;
            public int qualityLevel = 3; // Ĭ���е�����
            public bool antiAliasing = false;
            public bool bloomEffect = true;
            public int textureQuality = 2; // 0-3
            public int shadowQuality = 2; // 0-3
        }

        public GraphicsSettings settings = new GraphicsSettings();

        // ���÷ֱ����б�
        private Resolution[] availableResolutions;
        private List<string> resolutionOptions = new List<string>();
        private int currentResolutionIndex = 0;

        // ֧�ֵķֱ����б�
        private Resolution[] supportedResolutions =
        {
            new Resolution { width = 800, height = 600 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 2560, height = 1440 },
            new Resolution { width = 3840, height = 2160 },
        };

        private List<int> frameRateOptions = new List<int> { 30, 60, 120, 144, 240, -1 }; // -1��ʾ����


        void Awake()
        {
            // ȷ������Ψһ��
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // �״δ򿪣�������Ļԭ���ֱ���
            SetToNativeResolution();

            // ��ʼ���ֱ����б�
            InitializeResolutions();

            // ��ʼ����ֱͬ��
            InitializeVsync();

            // ��ʼ��֡��
            InitializeFrameRate();

            // ���ر�������û�Ӧ��Ĭ������
            LoadSettings();
            ApplySettings();
        }

        private void InitializeVsync()
        {
            settings.vsyncEnabled = true;
        }

        private void InitializeFrameRate()
        {
            SetTargetFrameRate(1);
        }

        // ��ʼ�����÷ֱ����б�
        private void InitializeResolutions()
        {
            availableResolutions = (Resolution[])supportedResolutions.Clone();
            resolutionOptions.Clear();

            for(int i = 0; i < availableResolutions.Length; i++)
            {
                string option = $"{availableResolutions[i].width} x {availableResolutions[i].height}";
                resolutionOptions.Add(option);

                if(availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                    settings.currentResolution = availableResolutions[i];
                }
            }

            Debug.Log($"�ҵ� {resolutionOptions.Count} �����÷ֱ���");
            Debug.Log($"��ǰ���� {currentResolutionIndex} ��ǰ�ֱ��� {settings.currentResolution.width} {settings.currentResolution.height}");
        }

        // ����Ϊ��ǰ��ʾ����ԭ���ֱ���
        public void SetToNativeResolution()
        {
            Resolution nativeResolution = Screen.currentResolution;
            Screen.SetResolution(
                nativeResolution.width,
                nativeResolution.height,
                FullScreenMode.FullScreenWindow
            );

            Debug.Log($"���Զ�����Ϊ��ʾ��ԭ���ֱ���: {nativeResolution.width}x{nativeResolution.height}@{nativeResolution.refreshRate}Hz");

        }

        // Ӧ��ͼ������
        public void ApplySettings()
        {

            // Ӧ�÷ֱ��� �Լ� ����ģʽ
            Screen.SetResolution(settings.currentResolution.width, settings.currentResolution.height, settings.windowMode);
           
            // Ӧ��֡������
            Application.targetFrameRate = settings.targetFrameRate;

            // Ӧ�ô�ֱͬ��
            QualitySettings.vSyncCount = settings.vsyncEnabled ? 1 : 0;

            // Ӧ����������
            QualitySettings.SetQualityLevel(settings.qualityLevel);

            // Ӧ�ÿ����
            QualitySettings.antiAliasing = settings.antiAliasing ? 8 : 0;

            // Ӧ�ú���Ч��
            SetBloomEffect(settings.bloomEffect);

            // Ӧ����������
            QualitySettings.masterTextureLimit = Mathf.Clamp(3 - settings.textureQuality, 0, 3);

            // Ӧ����Ӱ����
            QualitySettings.shadows = (ShadowQuality)Mathf.Clamp(settings.shadowQuality, 0, 3);

            Debug.Log("ͼ��������Ӧ��");

            SaveSettings();
        }

        // ���ú���Ч����ʾ����������Ҫʵ�ʵĺ�����������
        private void SetBloomEffect(bool enable)
        {
            // ��ʵ����Ŀ�У�������Ҫ�ҵ�����������������/����BloomЧ��
            Debug.Log($"BloomЧ��������Ϊ: {enable}");
        }

        // ��ȡ���÷ֱ����б�
        public List<string> GetResolutionOptions()
        {
            return resolutionOptions;
        }

        // ��ȡ����֡��
        public List<int> GetFrameRateOptions()
        {
            return frameRateOptions;
        }

        public int GetWindowMode()
        {
            return (int)settings.windowMode;
        }

        public int GetCurrentFrameRate()
        {
            for(int i = 0; i < frameRateOptions.Count; i++)
            {
                if (settings.targetFrameRate == frameRateOptions[i]) return i;
            }
            return 0;
        }

        // ��ȡ��ǰ�ֱ�������
        public int GetCurrentResolutionIndex()
        {
            return currentResolutionIndex;
        }

        // ���÷ֱ���
        public void SetResolution(int index)
        {
            if (index >= 0 && index < availableResolutions.Length)
            {
                settings.currentResolution = availableResolutions[index];
                currentResolutionIndex = index;
                ApplySettings();
            }
        }

        // ���ô���ģʽ
        public void SetWindowMode(int mode)
        {
            settings.windowMode = (FullScreenMode)mode;

            // ֻ����ȫ����ռ��ģʽ�²��ܹ����ô�ֱͬ��
            settings.vsyncEnabled = (FullScreenMode)mode == FullScreenMode.ExclusiveFullScreen;

            ApplySettings();
        }

        // ����Ŀ��֡��
        public void SetTargetFrameRate(int index)
        {
            settings.targetFrameRate = frameRateOptions[index];
            Debug.Log($"����Ŀ��֡�� {settings.targetFrameRate}");
            ApplySettings();
        }

        // ���ô�ֱͬ��
        public void SetVSync(bool enable)
        {
            Debug.Log($"��ֱͬ��״̬ {enable}");
            settings.vsyncEnabled = enable;
            ApplySettings();
        }

        // ����ͼ����������
        public void SetQualityLevel(GraphicsQuality quality)
        {
            settings.qualityLevel = (int)quality;
            ApplySettings();
        }

        // ���ÿ����
        public void SetAntiAliasing(bool enable)
        {
            settings.antiAliasing = enable;
            ApplySettings();
        }

        // ������������
        public void SetTextureQuality(int quality)
        {
            settings.textureQuality = Mathf.Clamp(quality, 0, 3);
            ApplySettings();
        }

        // ������Ӱ����
        public void SetShadowQuality(int quality)
        {
            settings.shadowQuality = Mathf.Clamp(quality, 0, 3);
            ApplySettings();
        }

        // �������õ�PlayerPrefs
        public void SaveSettings()
        {
            PlayerPrefs.SetInt("TargetFrameRate", settings.targetFrameRate);
            PlayerPrefs.SetInt("VSync", settings.vsyncEnabled ? 1 : 0);
            PlayerPrefs.SetInt("QualityLevel", settings.qualityLevel);
            PlayerPrefs.SetInt("AntiAliasing", settings.antiAliasing ? 1 : 0);
            PlayerPrefs.SetInt("TextureQuality", settings.textureQuality);
            PlayerPrefs.SetInt("ShadowQuality", settings.shadowQuality);

            PlayerPrefs.Save();
            Debug.Log("ͼ�������ѱ���");
        }

        // ��PlayerPrefs��������
        public void LoadSettings()
        {
            // ������������
            settings.targetFrameRate = PlayerPrefs.GetInt("TargetFrameRate", 60);
            settings.vsyncEnabled = PlayerPrefs.GetInt("VSync", 0) == 1;
            settings.qualityLevel = PlayerPrefs.GetInt("QualityLevel", 3);
            settings.antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0) == 1;
            settings.textureQuality = PlayerPrefs.GetInt("TextureQuality", 2);
            settings.shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2);

            Debug.Log("ͼ�������Ѽ���");
        }

        private int GetResolutionIndex(Resolution resolution)
        {
            for(int i = 0; i < resolutionOptions.Count; i++)
            {
                if (resolutionOptions[i].Contains(resolution.width.ToString())) return i;
            }
            return 0;
        }

        // �ָ�Ĭ������
        public void ResetToDefaults()
        {
            settings = new GraphicsSettings();
            InitializeResolutions();
            ApplySettings();
            SaveSettings();
            Debug.Log("ͼ������������ΪĬ��ֵ");
        }
    }
}


