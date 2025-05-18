using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Record
{
    [Serializable]
    public class RecordSessionData 
    {
        public string playerName;           // 玩家名称
        public DateTime date;               // 游戏日期
        public float playTime;              // 游戏时长（秒）
        public float totalDamage;             // 总伤害
        public int enemiesKilled;           // 击杀敌人数量
        public int levelsCompleted;         // 完成关卡数
        public bool gameCompleted;          // 是否通关

        // 计算得分（示例：综合指标）
        public int CalculateScore()
        {
            int baseScore = (int)(totalDamage + (enemiesKilled * 100) + (levelsCompleted * 500));
            int timeBonus = Mathf.RoundToInt(playTime / 10); // 游戏时间越长，奖励越低
            return baseScore + timeBonus;
        }
    }
}



