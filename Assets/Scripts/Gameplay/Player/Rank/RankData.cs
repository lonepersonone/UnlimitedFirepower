using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    // ��ҵȼ����ݽṹ
    [System.Serializable]
    public class RankData
    {
        public string rankName; // ���磺"����", "��ʦ"
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

