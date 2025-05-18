using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewGalaxy", menuName = "Game/Map/GalaxyData")]
    public class GalaxyDataSO : ScriptableObject, IGameData
    {
        public string ID;

        public int Range; //星系大小

        public GameObject TransferPrefab;
        public PlanetDataSO[] PlanetDataSOs; //持有星球

        string IGameData.ID => ID;
    }

}

