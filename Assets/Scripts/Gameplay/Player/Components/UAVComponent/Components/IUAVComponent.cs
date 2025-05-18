using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public interface IUAVComponent
    {
        void Initialize(UnmannedAerialVehicle uav);

        void UpdateComponent();

    }
}


