using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface IShipComponentE
    {
        void Initialize(EnemyController enemy);
        void UpdateComponent();

    }
}


