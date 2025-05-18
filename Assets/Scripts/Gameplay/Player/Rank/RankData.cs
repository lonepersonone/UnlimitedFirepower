using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    // 玩家等级数据结构
    [System.Serializable]
    public class RankData
    {
        public string rankName; // 例如："新手", "大师"
        public int rank;
        public int requiredExperience;
        
        public RankData(int rank, int requiredExperience, string rankName = "")
        {
            this.rank = rank;
            this.requiredExperience = requiredExperience;
            this.rankName = rankName;
        }
    }
}

