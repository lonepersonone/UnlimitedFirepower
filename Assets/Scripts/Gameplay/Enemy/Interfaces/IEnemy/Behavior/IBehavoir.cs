using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public enum BehaviorType
    {
        Splite, // ����       
    }

    public interface IBehavoir
    {
        BehaviorType BehaviorType { get; }
        void ExecuteBehavior(EnemyController enemy);
    }

}

