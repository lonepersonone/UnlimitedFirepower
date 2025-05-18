using MyGame.Gameplay.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Utilities
{
    public static class HexgonUtil
    {
        public static Vector3 HexCoordianteToPixel(int q, int r, float outerRadius)
        {
            float x = (float)(Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r) * outerRadius;
            float y = -(float)((1.5 * r) * outerRadius);
            return new Vector3(x, y, 0);
        }

        public static string WorldToLocationID(Vector3 position)
        {
            float q = position.x / (HexMetrics.innerRadius * 2f);
            float s = -q;
            float offset = -position.y / (HexMetrics.outerRadius * 3f);
            q -= offset;
            s -= offset;
            (int Q, int R, int S) = CubeRound(q, -q - s, s);
            return $"{Q}/{R}/{S}";
        }

        public static int[] WorldToLocation(Vector3 position)
        {
            float q = position.x / (HexMetrics.innerRadius * 2f);
            float s = -q;
            float offset = -position.y / (HexMetrics.outerRadius * 3f);
            q -= offset;
            s -= offset;
            (int Q, int R, int S) = CubeRound(q, -q - s, s);
            return new int[3]{Q, R, S};
        }

        public static (int, int, int) CubeRound(float q, float r, float s)
        {
            int Q = Mathf.RoundToInt(q);
            int R = Mathf.RoundToInt(r);
            int S = Mathf.RoundToInt(s);

            float dQ = Mathf.Abs(q - Q);
            float dR = Mathf.Abs(r - R);
            float dS = Mathf.Abs(s - S);

            if (dQ > dR && dQ > dS) { Q = -R - S; }
            else if (dR > dS) { R = -Q - S; }
            else { S = -Q - R; }

            return (Q, R, S);
        }

        public static List<PlanetController> GetHexesInSpiral(Dictionary<string, PlanetController> dict, int[] center, int maxRadius)
        {
            List<PlanetController> result = new List<PlanetController>();
            for (int radius = 1; radius <= maxRadius; radius++)
            {
                int Q = center[0];
                int R = center[1] - radius;
                int S = center[2] + radius;
                string key = $"{Q}/{R}/{S}";

                for (int i = 0; i < 6; i++) // 六个方向
                {
                    for (int j = 0; j < radius; j++) // 该方向上的步数
                    {
                        if (dict.ContainsKey(key)) result.Add(dict[key]);
                        Q += (int)directions[i].x;
                        R += (int)directions[i].y;
                        S += (int)directions[i].z;
                        key = $"{Q}/{R}/{S}";
                    }
                }
            }
            return result;
        }

        private static Vector3[] directions = {
        new Vector3(1, 0, -1), new Vector3(0, 1, -1),new Vector3(-1, 1, 0),
        new Vector3(-1, 0, 1), new Vector3(0, -1, 1),new Vector3(1, -1, 0)
    };

    }
}


