using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // 监听AudioSource所在GameObject的生命周期
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
            // 当GameObject被销毁时，自动注销AudioSource
            if (audioSystem != null && targetSource != null)
            {
                audioSystem.UnregisterAudioSource(targetSource);
            }
        }
    }

}


