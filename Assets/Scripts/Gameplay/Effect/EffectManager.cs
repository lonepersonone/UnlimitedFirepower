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
        // 单例实现
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

        [Header("配置")]
        [SerializeField] private Transform effectContainer; // 特效容器
        [SerializeField] private AudioSource audioSourcePrefab; // 音效源预制体

        // 活动特效列表
        private List<GameObject> activeEffects = new List<GameObject>();
        private Dictionary<AudioSource, GameObject> soundEffectMap = new Dictionary<AudioSource, GameObject>();

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

            // 创建特效容器
            if (effectContainer == null)
            {
                GameObject container = new GameObject("Effects");
                effectContainer = container.transform;
                effectContainer.SetParent(transform);
            }
        }

        //===== 特效播放接口 =====

        public GameObject PlayEffect(EffectConfig config, Vector3 position, Quaternion rotation = default)
        {
            if (config == null || config.effectPrefab == null)
            {
                Debug.LogWarning("EffectManager: 无效的特效配置");
                return null;
            }

            // 实例化特效
            GameObject effectInstance = Instantiate(config.effectPrefab, position, rotation, effectContainer);
            effectInstance.name = config.effectName;

            // 记录活动特效
            activeEffects.Add(effectInstance);

            // 播放音效
            if (config.soundEffect != null)
            {
                PlaySoundEffect(config.soundEffect, position, config.soundVolume, effectInstance);
            }

            // 设置自动销毁
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
                Debug.LogWarning("EffectManager: 无效的特效配置或父对象");
                return null;
            }

            // 计算世界坐标
            Vector3 worldPosition = parent.TransformPoint(localPosition);
            Quaternion worldRotation = parent.rotation * localRotation;

            // 实例化特效
            GameObject effectInstance = Instantiate(config.effectPrefab, worldPosition, worldRotation,
                config.attachToTarget ? parent : effectContainer);
            effectInstance.name = config.effectName;

            // 如果不附着到目标，但需要相对于目标位置，则调整位置
            if (!config.attachToTarget)
            {
                effectInstance.transform.position = worldPosition;
                effectInstance.transform.rotation = worldRotation;
            }

            // 记录活动特效
            activeEffects.Add(effectInstance);

            // 播放音效
            if (config.soundEffect != null)
            {
                PlaySoundEffect(config.soundEffect, worldPosition, config.soundVolume, effectInstance);
            }

            // 设置自动销毁
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

            // 停止关联的音效
            foreach (var pair in soundEffectMap)
            {
                if (pair.Value == effectInstance && pair.Key != null)
                {
                    Destroy(pair.Key.gameObject);
                }
            }

            // 从活动列表中移除
            activeEffects.Remove(effectInstance);

            // 销毁特效
            Destroy(effectInstance);
        }

        public void StopAllEffects()
        {
            // 停止所有音效
            foreach (var audioSource in soundEffectMap.Keys)
            {
                if (audioSource != null)
                {
                    Destroy(audioSource.gameObject);
                }
            }
            soundEffectMap.Clear();

            // 销毁所有特效
            foreach (var effect in activeEffects)
            {
                if (effect != null)
                {
                    Destroy(effect);
                }
            }
            activeEffects.Clear();
        }

        // 清理已销毁的特效引用
        private void CleanupDestroyedEffects()
        {
            activeEffects.RemoveAll(e => e == null);

            // 清理音效映射
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

        // 播放音效
        private AudioSource PlaySoundEffect(AudioClip clip, Vector3 position, float volume, GameObject parentEffect)
        {
            if (clip == null || audioSourcePrefab == null) return null;

            // 创建临时AudioSource
            AudioSource audioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity, effectContainer);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            // 记录音效与特效的关联
            soundEffectMap[audioSource] = parentEffect;

            // 设置自动销毁
            Destroy(audioSource.gameObject, clip.length + 0.1f);

            return audioSource;
        }
    }
}


