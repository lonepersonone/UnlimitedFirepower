using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{

    [System.Serializable]
    public class WaveData
    {
        public float DelayAfterPreviousWave = 3f; //间隔时间
        public List<EnemySpawnData> Enemies; //本波敌人组成
    }
}

