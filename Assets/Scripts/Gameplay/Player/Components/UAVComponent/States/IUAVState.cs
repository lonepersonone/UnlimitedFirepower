using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// ÎÞÈË»ú×´Ì¬
    /// </summary>
    public interface IUAVState
    {
        public void Enter();
        public void Exit();
        public void Update();
    }
}


