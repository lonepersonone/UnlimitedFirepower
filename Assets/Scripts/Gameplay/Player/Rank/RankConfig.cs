using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    // ��ҵȼ�����
    [CreateAssetMenu(fileName = "RankConfig", menuName = "Player/Rank Config")]
    public class RankConfig : ScriptableObject
    {
        [SerializeField] private List<RankData> rankDataList = new List<RankData>();
        [SerializeField] private int maxRank = 100;

        // ��ȡ�������辭��
        public int GetRequiredExperience(int rank)
        {
            if (rank >= maxRank) return int.MaxValue; // �Ѵ���߼�

            foreach (var data in rankDataList)
            {
                if (data.rank == rank + 1)
                {
                    return data.requiredExperience;
                }
            }

            // Ĭ��ÿ������100����
            return 100 * (rank + 1);
        }

        // ��ȡ��ǰ�ȼ�����
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

