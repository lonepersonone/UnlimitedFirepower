using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Level
{
    public class LevelAttribute : MonoBehaviour
    {
        public string ID;
        public int BaseDifficulty = 1; //�����Ѷ�
        public List<WaveData> Waves; //�˳�

        public LevelAttribute(LevelDataSO so, int difficulty)
        {
            LevelDataSO clone = LevelWaveGenerator.Generate(so, difficulty);
            ID = clone.ID;
            BaseDifficulty = clone.BaseDifficulty;
            Waves = clone.Waves;
        }

    }
}


