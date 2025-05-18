using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Record
{
    [CreateAssetMenu(fileName = "RecordDataSettings", menuName = "Game/RecordSettings")]
    public class RecordDataSettings : ScriptableObject
    {
        [SerializeField] private int maxRankingEntries = 10;  // ���а�����¼��
        [SerializeField] private string saveFileName = "game_sessions.json"; // �����ļ���

        public int MaxRankingEntries => maxRankingEntries;
        public string SaveFileName => saveFileName;
    }
}


