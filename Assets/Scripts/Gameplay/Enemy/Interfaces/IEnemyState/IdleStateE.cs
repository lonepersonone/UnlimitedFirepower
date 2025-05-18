using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class IdleStateE : IEnemyState
    {
        private EnemyController enemy;



        public IdleStateE(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Update()
        {

        }
    }
}


