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
                    // ����������������Ŀ
                    enemy.Count = Mathf.CeilToInt(enemy.Count * scaleFactor);
                }

                // �Ѷȸ��򲨴θ�����
                wave.Delay = Mathf.Max(1f, wave.Delay - (difficulty - baseLevel.Difficulty) * 0.5f);
            }

            baseLevel.Difficulty = difficulty;

            return clonedLevel;
        }

        private static LevelData CloneLevelData(LevelData original)
        {
            // ����������� ScriptableObject.Instantiate ��¡
            return original.Clone();
        }
    }
}



