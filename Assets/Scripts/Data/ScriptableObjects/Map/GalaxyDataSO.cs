using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewGalaxy", menuName = "Game/Map/GalaxyData")]
    public class GalaxyDataSO : ScriptableObject, IGameData
    {
        public string ID;

        public int Range; //��ϵ��С

        public GameObject TransferPrefab;
        public PlanetDataSO[] PlanetDataSOs; //��������

        string IGameData.ID => ID;
    }

}

