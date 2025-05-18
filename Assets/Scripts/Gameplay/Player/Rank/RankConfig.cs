using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    // 玩家等级配置
    [CreateAssetMenu(fileName = "RankConfig", menuName = "Player/Rank Config")]
    public class RankConfig : ScriptableObject
    {
        [SerializeField] private List<RankData> rankDataList = new List<RankData>();
        [SerializeField] private int maxRank = 100;

        // 获取升级所需经验
        public int GetRequiredExperience(int rank)
        {
            if (rank >= maxRank) return int.MaxValue; // 已达最高级

            foreach (var data in rankDataList)
            {
                if (data.rank == rank + 1)
                {
                    return data.requiredExperience;
                }
            }

            // 默认每级增加100经验
            return 100 * (rank + 1);
        }

        // 获取当前等级名称
        public string GetRankName(int rank)
        {
            foreach (var data in rankDataList)
            {
                if (data.rank == rank)
                {
                    return data.rankName;
                }
            }

            return "Rank " + rank;
        }

        public int MaxRank => maxRank;
    }

}

