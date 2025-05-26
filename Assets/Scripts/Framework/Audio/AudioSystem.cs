using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.Audio;

    public class AudioSystem : MonoBehaviour
    {
        // 单例实现
        public static AudioSystem Instance { get; private set; }

        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private AudioMixer mainMixer;


        // 音量组名称常量
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SFXVolume";
        private const string UI_VOLUME = "UIVolume";

        // 音量设置（范围0-1）
        private Dictionary<string, float> volumeSettings = new Dictionary<string, float>
        {
            { MASTER_VOLUME, 1f },
            { MUSIC_VOLUME, 0.7f },
            { SFX_VOLUME, 0.8f },
            { UI_VOLUME, 0.9f }
        };

        // 已注册的AudioSource及其监听组件
        private Dictionary<AudioSource, string> registeredSources = new Dictionary<AudioSource, string>();

        public AudioConfig AudioConfig => audioConfig;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 加载配置和玩家设置
            LoadAudioConfig();
            LoadPlayerSettings();
        }


        // 自动注册并播放音频
        public AudioSource PlayAudio(GameObject owner, string audioId, bool loop = false)
        {
            // 获取或添加AudioSource组件
            AudioSource source = owner.GetComponent<AudioSource>();
            if (source == null)
                source = owner.AddComponent<AudioSource>();

            // 如果未注册，则自动注册
            if (!registeredSources.ContainsKey(source))
            {
                RegisterAudioSource(source, audioId);
            }


            // 配置播放参数
            AudioData data = audioConfig.GetAudioData(audioId);
            if (data != null)
            {
                source.loop = loop;
                source.Play();
            }

            return source;
        }

        // 注册AudioSource
        private void RegisterAudioSource(AudioSource source, string audioId)
        {
            // 配置AudioSource
            AudioData data = audioConfig.GetAudioData(audioId);
            if (data != null)
            {
                source.clip = data.clip;
                source.volume = data.volume;
                source.pitch = data.pitch;
                source.outputAudioMixerGroup = data.mixerGroup;
            }

            // 添加生命周期监听器
            AudioSourceListener listener = source.gameObject.AddComponent<AudioSourceListener>();
            listener.Initialize(source, this);

            // 记录注册信息
            registeredSources.Add(source, audioId);
        }

        // 注销AudioSource
        public void UnregisterAudioSource(AudioSource source)
        {
            if (source == null || !registeredSources.ContainsKey(source)) return;

            registeredSources.Remove(source);
        }

        #region 读取记录

        private void LoadAudioConfig()
        {
            if (audioConfig == null)
            {
                Debug.LogError("未指定音频配置文件！");
                return;
            }
        }

        private void LoadPlayerSettings()
        {
            // 从PlayerPrefs加载玩家音量设置
            volumeSettings[MASTER_VOLUME] = PlayerPrefs.GetFloat(MASTER_VOLUME, 1);
            volumeSettings[MUSIC_VOLUME] = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1);
            volumeSettings[SFX_VOLUME] = PlayerPrefs.GetFloat(SFX_VOLUME, 1);
            volumeSettings[UI_VOLUME] = PlayerPrefs.GetFloat(UI_VOLUME, 1);

            // 应用设置到Mixer
            ApplyVolumeSettings();
        }

        private void ApplyVolumeSettings()
        {
            foreach (var pair in volumeSettings)
            {
                // 将0-1的音量值转换为dB（Mixer使用的单位）
                float dB = ConvertVolumeToDB(pair.Value);
                mainMixer.SetFloat(pair.Key, dB);
            }
        }

        // 将0-1的音量值转换为dB
        private float ConvertVolumeToDB(float volume)
        {
            // 避免对数计算中的零值
            if (volume <= 0)
                return -80f; // 接近静音的dB值

            // 对数转换：dB = 20 * log10(volume)
            return 20f * Mathf.Log10(volume);
        }


        #endregion

        #region 设置音量

        // 设置音量
        // 设置音量（0-1范围）
        public void SetVolume(string volumeGroup, float volume)
        {
            // 限制音量范围
            volume = Mathf.Clamp01(volume);

            if (volumeSettings.ContainsKey(volumeGroup))
            {
                volumeSettings[volumeGroup] = volume;
                PlayerPrefs.SetFloat(volumeGroup, volume);

                // 应用新音量
                float dB = ConvertVolumeToDB(volume);
                mainMixer.SetFloat(volumeGroup, dB);

                Debug.Log($"设置{volumeGroup}音量为: {volume} ({dB} dB)");
            }
            else
            {
                Debug.LogError($"未找到音量组: {volumeGroup}");
            }
        }

        // 获取音量（0-1范围）
        public float GetVolume(string volumeGroup)
        {
            if (volumeSettings.ContainsKey(volumeGroup))
            {
                return volumeSettings[volumeGroup];
            }

            Debug.LogError($"未找到音量组: {volumeGroup}");
            return 0f;
        }

        #endregion
    }
}


