using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Level
{
    public class LevelAttribute : MonoBehaviour
    {
        public string ID;
        public int BaseDifficulty = 1; //»ù´¡ÄÑ¶È
        public List<WaveData> Waves; //ÀË³±

        public LevelAttribute(LevelData data, int difficulty)
        {
            LevelData clone = LevelWaveGenerator.Generate(data, difficulty);
            ID = clone.ID;
            BaseDifficulty = clone.Difficulty;
            Waves = clone.Waves;
        }

    }
}


