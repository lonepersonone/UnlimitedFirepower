using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Level
{
    public class LevelWaveGenerator : MonoBehaviour
    {
        public static LevelDataSO Generate(LevelDataSO baseLevel, int difficulty)
        {
            var clonedLevel = CloneLevelData(baseLevel);
            float scaleFactor = 1f + (difficulty - baseLevel.BaseDifficulty) * 0.25f;

            foreach (var wave in clonedLevel.Waves)
            {
                foreach (var enemy in wave.Enemies)
                {
                    // 按比例提升敌人数目
                    enemy.BaseCount = Mathf.CeilToInt(enemy.BaseCount * scaleFactor);
                }

                // 难度高则波次更紧凑
                wave.DelayAfterPreviousWave = Mathf.Max(1f, wave.DelayAfterPreviousWave - (difficulty - baseLevel.BaseDifficulty) * 0.5f);
            }

            baseLevel.BaseDifficulty = difficulty;

            return clonedLevel;
        }

        private static LevelDataSO CloneLevelData(LevelDataSO original)
        {
            // 可用深拷贝或者 ScriptableObject.Instantiate 克隆
            return Instantiate(original);
        }
    }
}



