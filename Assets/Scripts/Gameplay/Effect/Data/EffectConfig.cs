using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewEffectConfig", menuName = "Effects/Effect Config")]
    public class EffectConfig : ScriptableObject
    {
        [Header("������Ϣ")]
        public string effectName;
        public GameObject effectPrefab;

        [Header("��������")]
        public float defaultDuration = 3f; // Ĭ�ϲ���ʱ��
        public bool autoDestroy = true;  // �Ƿ��Զ�����
        public bool attachToTarget = false; // �Ƿ��ŵ�Ŀ��

        [Header("��������")]
        public AudioClip soundEffect;
        public float soundVolume = 1f;
    }

}

