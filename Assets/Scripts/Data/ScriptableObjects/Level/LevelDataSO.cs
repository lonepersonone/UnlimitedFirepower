using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/LevelData")]
    public class LevelDataSO : ScriptableObject, IGameData
    {
        public string ID;
        public int BaseDifficulty = 1; //»ù´¡ÄÑ¶È
        public List<WaveData> Waves = new List<WaveData>(); //ÀË³±

        string IGameData.ID => ID;
    }

}

