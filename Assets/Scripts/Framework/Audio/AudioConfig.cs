using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{
    using UnityEngine;
    using UnityEngine.Audio;

    // 音频数据结构
    [System.Serializable]
    public class AudioData
    {
        public string id;                 // 音频ID（唯一标识）
        public AudioClip clip;            // 音频剪辑
        public float volume = 1f;         // 默认音量
        public float pitch = 1f;          // 默认音调
        public bool loop = false;         // 是否循环
        public AudioMixerGroup mixerGroup; // 输出的Mixer组
    }

    // 音频配置文件
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public AudioData[] audioClips;    // 所有音频数据

        // 根据ID查找音频数据
        public AudioData GetAudioData(string id)
        {
            foreach (var data in audioClips)
            {
                if (data.id == id)
                    return data;
            }

            Debug.LogError($"未找到ID为 '{id}' 的音频数据");
            return null;
        }
    }
}

