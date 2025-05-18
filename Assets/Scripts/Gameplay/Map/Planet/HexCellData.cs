using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    [System.Serializable]
    public class HexCellData
    {
        public int Q;
        public int R;
        public int S;
        public string PowerCN;
        public string PowerEN;
        public string PlanetCN;
        public string PlanetEN;
        public string GalaxyCN;
        public string GalaxyEN;
        public string Key;
        public Vector3 LocalPosition;
        public Color Color;
        public HexCellState State;
        public HexCellType Type;
        public DifficultyType Difficulty;
        public int Wealth; //²Æ¸»Öµ
        public int Scale = 0;
    }
}


