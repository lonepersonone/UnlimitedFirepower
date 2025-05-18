using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public enum BehaviorType
    {
        Splite, // ╥жая       
    }

    public interface IBehavoir
    {
        BehaviorType BehaviorType { get; }
        void ExecuteBehavior(EnemyController enemy);
    }

}

