using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Record
{
    [Serializable]
    public class RecordSessionData 
    {
        public string playerName;           // �������
        public DateTime date;               // ��Ϸ����
        public float playTime;              // ��Ϸʱ�����룩
        public float totalDamage;             // ���˺�
        public int enemiesKilled;           // ��ɱ��������
        public int levelsCompleted;         // ��ɹؿ���
        public bool gameCompleted;          // �Ƿ�ͨ��

        // ����÷֣�ʾ�����ۺ�ָ�꣩
        public int CalculateScore()
        {
            int baseScore = (int)(totalDamage + (enemiesKilled * 100) + (levelsCompleted * 500));
            int timeBonus = Mathf.RoundToInt(playTime / 10); // ��Ϸʱ��Խ��������Խ��
            return baseScore + timeBonus;
        }
    }
}



