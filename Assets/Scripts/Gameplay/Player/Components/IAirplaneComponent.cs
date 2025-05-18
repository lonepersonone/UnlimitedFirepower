using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public interface IAirplaneComponent
    {
        void Initialize(PlayerController player);
        void UpdateComponent();

    }
}



