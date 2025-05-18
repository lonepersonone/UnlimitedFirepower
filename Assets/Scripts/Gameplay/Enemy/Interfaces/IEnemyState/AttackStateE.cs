using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Enemy
{
    public class AttackStateE : IEnemyState
    {
        private EnemyController enemy;

        public AttackStateE(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            enemy.MoveComponentE.SetCurrentSpeed(0);
        }

        public void Exit()
        {
            enemy.MoveComponentE.SetCurrentSpeed(enemy.CharacterData.MoveSpeed);
        }

        public void Update()
        {
            if (enemy.AttackComponentE.CanAttack())
            {
                enemy.Attack();
                enemy.AttackComponentE.RecordAttackTime();
            }
        }

    }
}


