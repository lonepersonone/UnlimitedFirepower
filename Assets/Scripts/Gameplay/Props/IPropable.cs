using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Prop
{
    public interface IPropable
    {
        public void OnPickedUp(Transform target);
    }
}


