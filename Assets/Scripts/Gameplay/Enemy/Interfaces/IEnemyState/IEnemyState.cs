using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface IEnemyState
    {
        void Enter();
        void Exit();
        void Update();
    }
}

