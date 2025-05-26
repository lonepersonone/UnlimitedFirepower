using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // ����AudioSource����GameObject����������
    public class AudioSourceListener : MonoBehaviour
    {
        private AudioSource targetSource;
        private AudioSystem audioSystem;

        public void Initialize(AudioSource source, AudioSystem system)
        {
            targetSource = source;
            audioSystem = system;
        }

        private void OnDestroy()
        {
            // ��GameObject������ʱ���Զ�ע��AudioSource
            if (audioSystem != null && targetSource != null)
            {
                audioSystem.UnregisterAudioSource(targetSource);
            }
        }
    }

}


