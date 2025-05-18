using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface ISpecialEnemy : IEnemy
    {
        void Explode(); //自爆
        void Split(); //分裂
        void SpeedUp(); //加速
    }
}


