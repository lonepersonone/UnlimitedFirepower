using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Level
{
    public class LevelWaveGenerator : MonoBehaviour
    {
        public static LevelData Generate(LevelData baseLevel, int difficulty)
        {
            var clonedLevel = CloneLevelData(baseLevel);
            float scaleFactor = 1f + (difficulty - baseLevel.Difficulty) * 0.25f;

            foreach (var wave in clonedLevel.Waves)
            {
                foreach (var enemy in wave.Enemies)
                {
                    // 按比例提升敌人数目
                    enemy.Count = Mathf.CeilToInt(enemy.Count * scaleFactor);
                }

                // 难度高则波次更紧凑
                wave.Delay = Mathf.Max(1f, wave.Delay - (difficulty - baseLevel.Difficulty) * 0.5f);
            }

            baseLevel.Difficulty = difficulty;

            return clonedLevel;
        }

        private static LevelData CloneLevelData(LevelData original)
        {
            // 可用深拷贝或者 ScriptableObject.Instantiate 克隆
            return original.Clone();
        }
    }
}



