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
                    // ����������������Ŀ
                    enemy.BaseCount = Mathf.CeilToInt(enemy.BaseCount * scaleFactor);
                }

                // �Ѷȸ��򲨴θ�����
                wave.DelayAfterPreviousWave = Mathf.Max(1f, wave.DelayAfterPreviousWave - (difficulty - baseLevel.BaseDifficulty) * 0.5f);
            }

            baseLevel.BaseDifficulty = difficulty;

            return clonedLevel;
        }

        private static LevelDataSO CloneLevelData(LevelDataSO original)
        {
            // ����������� ScriptableObject.Instantiate ��¡
            return Instantiate(original);
        }
    }
}



