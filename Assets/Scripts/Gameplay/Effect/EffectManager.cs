using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Effect
{
    using UnityEngine;
    using System.Collections.Generic;
    using MyGame.Data.SO;

    public class EffectManager : MonoBehaviour, IEffectPlayer
    {
        // ����ʵ��
        private static EffectManager instance;
        public static EffectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EffectManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("EffectManager");
                        instance = obj.AddComponent<EffectManager>();
                    }
                }
                return instance;
            }
        }

        [Header("����")]
        [SerializeField] private Transform effectContainer; // ��Ч����
        [SerializeField] private AudioSource audioSourcePrefab; // ��ЧԴԤ����

        // ���Ч�б�
        private List<GameObject> activeEffects = new List<GameObject>();
        private Dictionary<AudioSource, GameObject> soundEffectMap = new Dictionary<AudioSource, GameObject>();

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

            // ������Ч����
            if (effectContainer == null)
            {
                GameObject container = new GameObject("Effects");
                effectContainer = container.transform;
                effectContainer.SetParent(transform);
            }
        }

        //===== ��Ч���Žӿ� =====

        public GameObject PlayEffect(EffectConfig config, Vector3 position, Quaternion rotation = default)
        {
            if (config == null || config.effectPrefab == null)
            {
                Debug.LogWarning("EffectManager: ��Ч����Ч����");
                return null;
            }

            // ʵ������Ч
            GameObject effectInstance = Instantiate(config.effectPrefab, position, rotation, effectContainer);
            effectInstance.name = config.effectName;

            // ��¼���Ч
            activeEffects.Add(effectInstance);

            // ������Ч
            if (config.soundEffect != null)
            {
                PlaySoundEffect(config.soundEffect, position, config.soundVolume, effectInstance);
            }

            // �����Զ�����
            if (config.autoDestroy)
            {
                float destroyTime = config.defaultDuration > 0 ? config.defaultDuration : 5f;
                Destroy(effectInstance, destroyTime);
                Invoke(nameof(CleanupDestroyedEffects), destroyTime + 0.1f);
            }

            return effectInstance;
        }

        public GameObject PlayEffect(EffectConfig config, Transform parent, Vector3 localPosition = default, Quaternion localRotation = default)
        {
            if (config == null || config.effectPrefab == null || parent == null)
            {
                Debug.LogWarning("EffectManager: ��Ч����Ч���û򸸶���");
                return null;
            }

            // ������������
            Vector3 worldPosition = parent.TransformPoint(localPosition);
            Quaternion worldRotation = parent.rotation * localRotation;

            // ʵ������Ч
            GameObject effectInstance = Instantiate(config.effectPrefab, worldPosition, worldRotation,
                config.attachToTarget ? parent : effectContainer);
            effectInstance.name = config.effectName;

            // ��������ŵ�Ŀ�꣬����Ҫ�����Ŀ��λ�ã������λ��
            if (!config.attachToTarget)
            {
                effectInstance.transform.position = worldPosition;
                effectInstance.transform.rotation = worldRotation;
            }

            // ��¼���Ч
            activeEffects.Add(effectInstance);

            // ������Ч
            if (config.soundEffect != null)
            {
                PlaySoundEffect(config.soundEffect, worldPosition, config.soundVolume, effectInstance);
            }

            // �����Զ�����
            if (config.autoDestroy)
            {
                float destroyTime = config.defaultDuration > 0 ? config.defaultDuration : 5f;
                Destroy(effectInstance, destroyTime);
                Invoke(nameof(CleanupDestroyedEffects), destroyTime + 0.1f);
            }

            return effectInstance;
        }

        public void StopEffect(GameObject effectInstance)
        {
            if (effectInstance == null) return;

            // ֹͣ��������Ч
            foreach (var pair in soundEffectMap)
            {
                if (pair.Value == effectInstance && pair.Key != null)
                {
                    Destroy(pair.Key.gameObject);
                }
            }

            // �ӻ�б����Ƴ�
            activeEffects.Remove(effectInstance);

            // ������Ч
            Destroy(effectInstance);
        }

        public void StopAllEffects()
        {
            // ֹͣ������Ч
            foreach (var audioSource in soundEffectMap.Keys)
            {
                if (audioSource != null)
                {
                    Destroy(audioSource.gameObject);
                }
            }
            soundEffectMap.Clear();

            // ����������Ч
            foreach (var effect in activeEffects)
            {
                if (effect != null)
                {
                    Destroy(effect);
                }
            }
            activeEffects.Clear();
        }

        // ���������ٵ���Ч����
        private void CleanupDestroyedEffects()
        {
            activeEffects.RemoveAll(e => e == null);

            // ������Чӳ��
            List<AudioSource> keysToRemove = new List<AudioSource>();
            foreach (var pair in soundEffectMap)
            {
                if (pair.Key == null || pair.Value == null)
                {
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                soundEffectMap.Remove(key);
            }
        }

        // ������Ч
        private AudioSource PlaySoundEffect(AudioClip clip, Vector3 position, float volume, GameObject parentEffect)
        {
            if (clip == null || audioSourcePrefab == null) return null;

            // ������ʱAudioSource
            AudioSource audioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity, effectContainer);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            // ��¼��Ч����Ч�Ĺ���
            soundEffectMap[audioSource] = parentEffect;

            // �����Զ�����
            Destroy(audioSource.gameObject, clip.length + 0.1f);

            return audioSource;
        }
    }
}


