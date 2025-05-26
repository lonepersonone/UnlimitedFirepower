using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    public static class AudioHelper
    {
        // ����һ������Ч���Զ����٣�
        public static void PlayOneShot(GameObject owner, string audioId, Vector3? position = null)
        {
            // ���ָ����λ�ã�������ʱGameObject
            if (position.HasValue)
            {
                GameObject tempObj = new GameObject("TempAudio_" + audioId);
                tempObj.transform.position = position.Value;
                //tempObj.transform.SetParent(owner.transform);

                // ������Ч
                AudioSystem.Instance.PlayAudio(tempObj, audioId);

                // �Զ�����
                AudioData data = AudioSystem.Instance.AudioConfig.GetAudioData(audioId);
                if (data != null)
                {
                    Object.Destroy(tempObj, data.clip.length + 0.1f);
                }
            }
            else
            {
                // ֱ����Ŀ������ϲ���
                AudioSystem.Instance.PlayAudio(owner, audioId);
            }
        }

        // ����ѭ����Ч - BGM
        public static AudioSource PlayLoop(GameObject owner, string audioId)
        {
            return AudioSystem.Instance.PlayAudio(owner, audioId, true);
        }

        // ֹͣ����
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

