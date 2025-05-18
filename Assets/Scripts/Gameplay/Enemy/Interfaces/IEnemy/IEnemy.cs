using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface IEnemy
    {
        void Initialize(CharacterAttribute attribute); //初始化

        void Attack(); //攻击

        void Die(); //死亡

        void AddBehavior(IBehavoir behavoir); //添加特殊行为

        void RemoveBehavior(BehaviorType type); //移除特殊行为
    }
}

