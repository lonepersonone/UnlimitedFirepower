using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public interface IEnemy
    {
        void Initialize(CharacterAttribute attribute); //��ʼ��

        void Attack(); //����

        void Die(); //����

        void AddBehavior(IBehavoir behavoir); //���������Ϊ

        void RemoveBehavior(BehaviorType type); //�Ƴ�������Ϊ
    }
}

