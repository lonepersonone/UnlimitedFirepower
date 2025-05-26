using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Visuals
{
    // 图形质量级别
    public enum GraphicsQuality
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Ultra
    }

    // 图形设置管理器
    public class GraphicsSettingsManager : MonoBehaviour
    {
        // 单例模式
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

        // 当前图形设置
        [System.Serializable]
        public class GraphicsSettings
        {
            public Resolution currentResolution;
            public FullScreenMode windowMode = FullScreenMode.ExclusiveFullScreen;
            public int targetFrameRate = 60;
            public bool vsyncEnabled = false;
            public int qualityLevel = 3; // 默认中等质量
            public bool antiAliasing = false;
            public bool bloomEffect = true;
            public int textureQuality = 2; // 0-3
            public int shadowQuality = 2; // 0-3
        }

        public GraphicsSettings settings = new GraphicsSettings();

        // 可用分辨率列表
        private Resolution[] availableResolutions;
        private List<string> resolutionOptions = new List<string>();
        private int currentResolutionIndex = 0;

        // 支持的分辨率列表
        private Resolution[] supportedResolutions =
        {
            new Resolution { width = 800, height = 600 },
            new Resolution { width = 1280, height = 720 },
            new Resolution { width = 1920, height = 1080 },
            new Resolution { width = 2560, height = 1440 },
            new Resolution { width = 3840, height = 2160 },
        };

        private List<int> frameRateOptions = new List<int> { 30, 60, 120, 144, 240, -1 }; // -1表示无限


        void Awake()
        {
            // 确保单例唯一性
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            // 首次打开，设置屏幕原生分辨率
            SetToNativeResolution();

            // 初始化分辨率列表
            InitializeResolutions();

            // 初始化垂直同步
            InitializeVsync();

            // 初始化帧率
            InitializeFrameRate();

            // 加载保存的设置或应用默认设置
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

        // 初始化可用分辨率列表
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

            Debug.Log($"找到 {resolutionOptions.Count} 个可用分辨率");
            Debug.Log($"当前索引 {currentResolutionIndex} 当前分辨率 {settings.currentResolution.width} {settings.currentResolution.height}");
        }

        // 设置为当前显示器的原生分辨率
        public void SetToNativeResolution()
        {
            Resolution nativeResolution = Screen.currentResolution;
            Screen.SetResolution(
                nativeResolution.width,
                nativeResolution.height,
                FullScreenMode.FullScreenWindow
            );

            Debug.Log($"已自动设置为显示器原生分辨率: {nativeResolution.width}x{nativeResolution.height}@{nativeResolution.refreshRate}Hz");

        }

        // 应用图形设置
        public void ApplySettings()
        {

            // 应用分辨率 以及 窗口模式
            Screen.SetResolution(settings.currentResolution.width, settings.currentResolution.height, settings.windowMode);
           
            // 应用帧率限制
            Application.targetFrameRate = settings.targetFrameRate;

            // 应用垂直同步
            QualitySettings.vSyncCount = settings.vsyncEnabled ? 1 : 0;

            // 应用质量级别
            QualitySettings.SetQualityLevel(settings.qualityLevel);

            // 应用抗锯齿
            QualitySettings.antiAliasing = settings.antiAliasing ? 8 : 0;

            // 应用后处理效果
            SetBloomEffect(settings.bloomEffect);

            // 应用纹理质量
            QualitySettings.masterTextureLimit = Mathf.Clamp(3 - settings.textureQuality, 0, 3);

            // 应用阴影质量
            QualitySettings.shadows = (ShadowQuality)Mathf.Clamp(settings.shadowQuality, 0, 3);

            Debug.Log("图形设置已应用");

            SaveSettings();
        }

        // 设置后处理效果（示例方法，需要实际的后处理体积组件）
        private void SetBloomEffect(bool enable)
        {
            // 在实际项目中，这里需要找到后处理体积组件并启用/禁用Bloom效果
            Debug.Log($"Bloom效果已设置为: {enable}");
        }

        // 获取可用分辨率列表
        public List<string> GetResolutionOptions()
        {
            return resolutionOptions;
        }

        // 获取可用帧率
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

        // 获取当前分辨率索引
        public int GetCurrentResolutionIndex()
        {
            return currentResolutionIndex;
        }

        // 设置分辨率
        public void SetResolution(int index)
        {
            if (index >= 0 && index < availableResolutions.Length)
            {
                settings.currentResolution = availableResolutions[index];
                currentResolutionIndex = index;
                ApplySettings();
            }
        }

        // 设置窗口模式
        public void SetWindowMode(int mode)
        {
            settings.windowMode = (FullScreenMode)mode;

            // 只有在全屏独占的模式下才能够设置垂直同步
            settings.vsyncEnabled = (FullScreenMode)mode == FullScreenMode.ExclusiveFullScreen;

            ApplySettings();
        }

        // 设置目标帧率
        public void SetTargetFrameRate(int index)
        {
            settings.targetFrameRate = frameRateOptions[index];
            Debug.Log($"设置目标帧率 {settings.targetFrameRate}");
            ApplySettings();
        }

        // 设置垂直同步
        public void SetVSync(bool enable)
        {
            Debug.Log($"垂直同步状态 {enable}");
            settings.vsyncEnabled = enable;
            ApplySettings();
        }

        // 设置图形质量级别
        public void SetQualityLevel(GraphicsQuality quality)
        {
            settings.qualityLevel = (int)quality;
            ApplySettings();
        }

        // 设置抗锯齿
        public void SetAntiAliasing(bool enable)
        {
            settings.antiAliasing = enable;
            ApplySettings();
        }

        // 设置纹理质量
        public void SetTextureQuality(int quality)
        {
            settings.textureQuality = Mathf.Clamp(quality, 0, 3);
            ApplySettings();
        }

        // 设置阴影质量
        public void SetShadowQuality(int quality)
        {
            settings.shadowQuality = Mathf.Clamp(quality, 0, 3);
            ApplySettings();
        }

        // 保存设置到PlayerPrefs
        public void SaveSettings()
        {
            PlayerPrefs.SetInt("TargetFrameRate", settings.targetFrameRate);
            PlayerPrefs.SetInt("VSync", settings.vsyncEnabled ? 1 : 0);
            PlayerPrefs.SetInt("QualityLevel", settings.qualityLevel);
            PlayerPrefs.SetInt("AntiAliasing", settings.antiAliasing ? 1 : 0);
            PlayerPrefs.SetInt("TextureQuality", settings.textureQuality);
            PlayerPrefs.SetInt("ShadowQuality", settings.shadowQuality);

            PlayerPrefs.Save();
            Debug.Log("图形设置已保存");
        }

        // 从PlayerPrefs加载设置
        public void LoadSettings()
        {
            // 加载其他设置
            settings.targetFrameRate = PlayerPrefs.GetInt("TargetFrameRate", 60);
            settings.vsyncEnabled = PlayerPrefs.GetInt("VSync", 0) == 1;
            settings.qualityLevel = PlayerPrefs.GetInt("QualityLevel", 3);
            settings.antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0) == 1;
            settings.textureQuality = PlayerPrefs.GetInt("TextureQuality", 2);
            settings.shadowQuality = PlayerPrefs.GetInt("ShadowQuality", 2);

            Debug.Log("图形设置已加载");
        }

        private int GetResolutionIndex(Resolution resolution)
        {
            for(int i = 0; i < resolutionOptions.Count; i++)
            {
                if (resolutionOptions[i].Contains(resolution.width.ToString())) return i;
            }
            return 0;
        }

        // 恢复默认设置
        public void ResetToDefaults()
        {
            settings = new GraphicsSettings();
            InitializeResolutions();
            ApplySettings();
            SaveSettings();
            Debug.Log("图形设置已重置为默认值");
        }
    }
}


