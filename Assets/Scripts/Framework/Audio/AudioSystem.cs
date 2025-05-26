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
        // ����ʵ��
        public static AudioSystem Instance { get; private set; }

        [SerializeField] private AudioConfig audioConfig;
        [SerializeField] private AudioMixer mainMixer;


        // ���������Ƴ���
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SFXVolume";
        private const string UI_VOLUME = "UIVolume";

        // �������ã���Χ0-1��
        private Dictionary<string, float> volumeSettings = new Dictionary<string, float>
        {
            { MASTER_VOLUME, 1f },
            { MUSIC_VOLUME, 0.7f },
            { SFX_VOLUME, 0.8f },
            { UI_VOLUME, 0.9f }
        };

        // ��ע���AudioSource����������
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

            // �������ú��������
            LoadAudioConfig();
            LoadPlayerSettings();
        }


        // �Զ�ע�Ტ������Ƶ
        public AudioSource PlayAudio(GameObject owner, string audioId, bool loop = false)
        {
            // ��ȡ�����AudioSource���
            AudioSource source = owner.GetComponent<AudioSource>();
            if (source == null)
                source = owner.AddComponent<AudioSource>();

            // ���δע�ᣬ���Զ�ע��
            if (!registeredSources.ContainsKey(source))
            {
                RegisterAudioSource(source, audioId);
            }


            // ���ò��Ų���
            AudioData data = audioConfig.GetAudioData(audioId);
            if (data != null)
            {
                source.loop = loop;
                source.Play();
            }

            return source;
        }

        // ע��AudioSource
        private void RegisterAudioSource(AudioSource source, string audioId)
        {
            // ����AudioSource
            AudioData data = audioConfig.GetAudioData(audioId);
            if (data != null)
            {
                source.clip = data.clip;
                source.volume = data.volume;
                source.pitch = data.pitch;
                source.outputAudioMixerGroup = data.mixerGroup;
            }

            // ����������ڼ�����
            AudioSourceListener listener = source.gameObject.AddComponent<AudioSourceListener>();
            listener.Initialize(source, this);

            // ��¼ע����Ϣ
            registeredSources.Add(source, audioId);
        }

        // ע��AudioSource
        public void UnregisterAudioSource(AudioSource source)
        {
            if (source == null || !registeredSources.ContainsKey(source)) return;

            registeredSources.Remove(source);
        }

        #region ��ȡ��¼

        private void LoadAudioConfig()
        {
            if (audioConfig == null)
            {
                Debug.LogError("δָ����Ƶ�����ļ���");
                return;
            }
        }

        private void LoadPlayerSettings()
        {
            // ��PlayerPrefs���������������
            volumeSettings[MASTER_VOLUME] = PlayerPrefs.GetFloat(MASTER_VOLUME, 1);
            volumeSettings[MUSIC_VOLUME] = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1);
            volumeSettings[SFX_VOLUME] = PlayerPrefs.GetFloat(SFX_VOLUME, 1);
            volumeSettings[UI_VOLUME] = PlayerPrefs.GetFloat(UI_VOLUME, 1);

            // Ӧ�����õ�Mixer
            ApplyVolumeSettings();
        }

        private void ApplyVolumeSettings()
        {
            foreach (var pair in volumeSettings)
            {
                // ��0-1������ֵת��ΪdB��Mixerʹ�õĵ�λ��
                float dB = ConvertVolumeToDB(pair.Value);
                mainMixer.SetFloat(pair.Key, dB);
            }
        }

        // ��0-1������ֵת��ΪdB
        private float ConvertVolumeToDB(float volume)
        {
            // ������������е���ֵ
            if (volume <= 0)
                return -80f; // �ӽ�������dBֵ

            // ����ת����dB = 20 * log10(volume)
            return 20f * Mathf.Log10(volume);
        }


        #endregion

        #region ��������

        // ��������
        // ����������0-1��Χ��
        public void SetVolume(string volumeGroup, float volume)
        {
            // ����������Χ
            volume = Mathf.Clamp01(volume);

            if (volumeSettings.ContainsKey(volumeGroup))
            {
                volumeSettings[volumeGroup] = volume;
                PlayerPrefs.SetFloat(volumeGroup, volume);

                // Ӧ��������
                float dB = ConvertVolumeToDB(volume);
                mainMixer.SetFloat(volumeGroup, dB);

                Debug.Log($"����{volumeGroup}����Ϊ: {volume} ({dB} dB)");
            }
            else
            {
                Debug.LogError($"δ�ҵ�������: {volumeGroup}");
            }
        }

        // ��ȡ������0-1��Χ��
        public float GetVolume(string volumeGroup)
        {
            if (volumeSettings.ContainsKey(volumeGroup))
            {
                return volumeSettings[volumeGroup];
            }

            Debug.LogError($"δ�ҵ�������: {volumeGroup}");
            return 0f;
        }

        #endregion
    }
}


