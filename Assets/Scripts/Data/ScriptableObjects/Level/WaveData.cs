using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{

    [System.Serializable]
    public class WaveData
    {
        public float DelayAfterPreviousWave = 3f; //���ʱ��
        public List<EnemySpawnData> Enemies; //�����������
    }
}

