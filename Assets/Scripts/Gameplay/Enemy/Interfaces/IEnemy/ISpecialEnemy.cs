using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface ISpecialEnemy : IEnemy
    {
        void Explode(); //�Ա�
        void Split(); //����
        void SpeedUp(); //����
    }
}


