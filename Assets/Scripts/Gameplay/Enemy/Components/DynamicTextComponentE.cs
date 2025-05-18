using MyGame.Data.SO;
using MyGame.Scene.BattleRoom;
using MyGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class DynamicTextComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;
        private DynamicTextSO basics;
        private DynamicTextSO critical;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;

            basics = BattleDataManager.Instance.ScriptableManager.GetDynamicTextById("Default");
            critical = BattleDataManager.Instance.ScriptableManager.GetDynamicTextById("Critical");
        }

        public void UpdateComponent()
        {

        }

        public void CreateBasicsDynamicText(float value)
        {
            GameObject instance = Instantiate(basics.DynamicTextPrefab);
            instance.transform.position = transform.position;
            DynamicTextController text = instance.GetComponent<DynamicTextController>();
            text.Initialize(basics.DynamicTextData, value);
        }

        public void CreateCriticalDynamicText(float value)
        {
            GameObject instance = Instantiate(critical.DynamicTextPrefab);
            instance.transform.position = transform.position;
            DynamicTextController text = instance.GetComponent<DynamicTextController>();
            text.Initialize(critical.DynamicTextData, value);
        }

    }
}


