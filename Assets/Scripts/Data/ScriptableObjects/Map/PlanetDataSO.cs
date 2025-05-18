using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewPlanet", menuName = "Game/Map/PlanetData")]
    public class PlanetDataSO : ScriptableObject, IGameData
    {
        public string ID;
        public float Ratio;

        public LevelDataSO Level;

        public GameObject PlanetPrefab;

        string IGameData.ID => ID;
    }

}


