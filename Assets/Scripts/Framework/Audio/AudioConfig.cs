using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{
    using UnityEngine;
    using UnityEngine.Audio;

    // ��Ƶ���ݽṹ
    [System.Serializable]
    public class AudioData
    {
        public string id;                 // ��ƵID��Ψһ��ʶ��
        public AudioClip clip;            // ��Ƶ����
        public float volume = 1f;         // Ĭ������
        public float pitch = 1f;          // Ĭ������
        public bool loop = false;         // �Ƿ�ѭ��
        public AudioMixerGroup mixerGroup; // �����Mixer��
    }

    // ��Ƶ�����ļ�
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public AudioData[] audioClips;    // ������Ƶ����

        // ����ID������Ƶ����
        public AudioData GetAudioData(string id)
        {
            foreach (var data in audioClips)
            {
                if (data.id == id)
                    return data;
            }

            Debug.LogError($"δ�ҵ�IDΪ '{id}' ����Ƶ����");
            return null;
        }
    }
}

