using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// ���˻�״̬
    /// </summary>
    public interface IUAVState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
}


