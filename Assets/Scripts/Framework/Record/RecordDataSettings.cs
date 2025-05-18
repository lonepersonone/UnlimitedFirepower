using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Record
{
    [CreateAssetMenu(fileName = "RecordDataSettings", menuName = "Game/RecordSettings")]
    public class RecordDataSettings : ScriptableObject
    {
        [SerializeField] private int maxRankingEntries = 10;  // 排行榜最大记录数
        [SerializeField] private string saveFileName = "game_sessions.json"; // 保存文件名

        public int MaxRankingEntries => maxRankingEntries;
        public string SaveFileName => saveFileName;
    }
}


