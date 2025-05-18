using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map 
{
    public static class HexMetrics
    {
        public static float outerRadius = 2;
        public static float innerRadius = outerRadius * 0.866025404f;

        public static Vector3[] corners = new Vector3[] {
            new Vector3(0f, outerRadius, 0f),
            new Vector3(innerRadius, 0.5f * outerRadius, 0f),
            new Vector3(innerRadius, -0.5f * outerRadius, 0f),
            new Vector3(0f, -outerRadius, 0f),
            new Vector3(-innerRadius, -0.5f * outerRadius, 0f),
            new Vector3(-innerRadius, 0.5f * outerRadius, 0f),
            new Vector3(0f, outerRadius, 0f)
    };
    }
}



