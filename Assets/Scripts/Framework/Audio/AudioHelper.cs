using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    public static class AudioHelper
    {
        // 播放一次性音效（自动销毁）
        public static void PlayOneShot(GameObject owner, string audioId, Vector3? position = null)
        {
            // 如果指定了位置，创建临时GameObject
            if (position.HasValue)
            {
                GameObject tempObj = new GameObject("TempAudio_" + audioId);
                tempObj.transform.position = position.Value;
                //tempObj.transform.SetParent(owner.transform);

                // 播放音效
                AudioSystem.Instance.PlayAudio(tempObj, audioId);

                // 自动销毁
                AudioData data = AudioSystem.Instance.AudioConfig.GetAudioData(audioId);
                if (data != null)
                {
                    Object.Destroy(tempObj, data.clip.length + 0.1f);
                }
            }
            else
            {
                // 直接在目标对象上播放
                AudioSystem.Instance.PlayAudio(owner, audioId);
            }
        }

        // 播放循环音效 - BGM
        public static AudioSource PlayLoop(GameObject owner, string audioId)
        {
            return AudioSystem.Instance.PlayAudio(owner, audioId, true);
        }

        // 停止播放
        public static void StopAudio(GameObject owner)
        {
            AudioSource source = owner.GetComponent<AudioSource>();
            if (source != null)
            {
                source.Stop();
            }
        }
    }

}

