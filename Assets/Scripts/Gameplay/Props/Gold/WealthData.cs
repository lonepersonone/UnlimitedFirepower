using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Prop
{
    [System.Serializable]
    public class WealthData
    {
        public Vector3 MinEmergePosition = new Vector3(0, 1, 0);
        public Vector3 MaxEmergePosition = new Vector3(2, 5, 0);
        public float EmergeTime = 0.3f;

        public Vector3 MinFadePosition = new Vector3(0, -1, 0);
        public Vector3 MaxFadePosition = new Vector3(2, -5, 0);
        public float FadeTime = 0.3f;
    }
}


