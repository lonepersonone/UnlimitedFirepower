using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewEffectConfig", menuName = "Effects/Effect Config")]
    public class EffectConfig : ScriptableObject
    {
        [Header("基本信息")]
        public string effectName;
        public GameObject effectPrefab;

        [Header("播放设置")]
        public float defaultDuration = 3f; // 默认播放时长
        public bool autoDestroy = true;  // 是否自动销毁
        public bool attachToTarget = false; // 是否附着到目标

        [Header("声音设置")]
        public AudioClip soundEffect;
        public float soundVolume = 1f;
    }

}

