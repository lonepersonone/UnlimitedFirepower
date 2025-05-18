using MyGame.Framework.Record;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyGame.UI.Settlement
{
    public class SettlementCanvas : MonoBehaviour
    {
        public TextMeshProUGUI PlayerTimeText;
        public TextMeshProUGUI DamageText;
        public TextMeshProUGUI EnemiesKilledText;
        public TextMeshProUGUI LevelCompletedText;

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnEnable()
        {
            SetValue();
        }

        private void SetValue()
        {
            PlayerTimeText.text = RecordDataManager.Instance.CurrentSession.playTime.ToString();
            DamageText.text = RecordDataManager.Instance.CurrentSession.totalDamage.ToString();
            EnemiesKilledText.text = RecordDataManager.Instance.CurrentSession.enemiesKilled.ToString();
            LevelCompletedText.text = RecordDataManager.Instance.CurrentSession.levelsCompleted.ToString();
        }

    }
}

