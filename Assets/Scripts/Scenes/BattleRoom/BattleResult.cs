using MyGame.Gameplay.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Scene.BattleRoom
{
    public class BattleResult
    {
        public bool IsWin;
        public PlanetController Galaxy;

        public BattleResult(PlanetController galaxy)
        {
            this.Galaxy = galaxy;
            this.IsWin = false;
        }

        public void SetResult(bool state)
        {
            IsWin = state;
        }
    }

}


