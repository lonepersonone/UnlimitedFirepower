using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewSpaceShip", menuName = "Game/Map/SpaceShipData")]
    public class SpaceShipDataSO : ScriptableObject, IGameData
    {
        public string ID;
        public GameObject SpaceShipPrefab;

        string IGameData.ID => ID;
    }
}



