using MyGame.Framework.Utilities;
using MyGame.Gameplay.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class SplitBehaviour : IBehavoir
    {
        private BehaviorType type = BehaviorType.Splite;

        public BehaviorType BehaviorType { get => type; }

        public void ExecuteBehavior(EnemyController enemy)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instance = Object.Instantiate(enemy.gameObject);
                instance.transform.localScale = new Vector3(instance.transform.localScale.x / 2, instance.transform.localScale.y / 2, instance.transform.localScale.z / 2);
                instance.transform.position = TransformUtil.GetRandomPosition(instance.transform.position, 2f);

                IEnemy unit = instance.GetComponent<IEnemy>();
                unit.Initialize(enemy.CharacterData);
                unit.RemoveBehavior(BehaviorType.Splite);

                LevelManager.Instance.AddEnemy(instance);
            }
        }

    }
}


