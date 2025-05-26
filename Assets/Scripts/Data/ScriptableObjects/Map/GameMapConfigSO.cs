using MyGame.Gameplay.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{

    [CreateAssetMenu(fileName = "GameMapConfig", menuName = "Game/Map/GameMapConfig")]
    public class GameMapConfigSO : ScriptableObject, IGameData
    {
        public string ID => "GameMapConfig";

        // 全局配置
        public float defaultWealthFactor = 0.2f;
        public float defaultSpecialFactor = 1.5f;

        // 飞行器
        public SpaceShipDataSO SpaceShipDataSO;

        // 多星系数据数组
        [SerializeField] private List<GalaxyData> galaxies = new List<GalaxyData>();

        // 访问器
        public List<GalaxyData> Galaxies => galaxies;

    }
        // 星系数据结构
        [System.Serializable]
    public class GalaxyData
    {
        public string ID;
        public string Name;
        public int Range; // 星系大小
        public float WealthValue;
        public float BaseWealthFactor;
        public int SpecialPlanetsCount;
        public List<float> SpecialFactor;

        // 0为中心， 1为边界
        public List<PlanetData> TransferPlanets = new List<PlanetData>();
        public List<PlanetData> SpecialPlanets = new List<PlanetData>();
        public List<PlanetData> Planets = new List<PlanetData>();

        // 生成唯一行星ID
        public string GeneratePlanetID(int q, int r)
        {
            return $"Galaxy_{ID}_Planet_{q}_{r}";
        }
    }

    [System.Serializable]
    public class PlanetData
    {
        public string ID;
        public float Ratio;
        public LevelData Level;
        public GameObject PlanetPrefab;
        public HexCellType Type;
        public bool IsBirthPlanet;
    }

    // 关卡数据结构
    [System.Serializable]
    public class LevelData
    {
        public string ID;
        public int Difficulty = 1;
        public List<WaveData> Waves = new List<WaveData>();

        public LevelData Clone()
        {
            return new LevelData
            {
                ID = this.ID,
                Difficulty = this.Difficulty,
                Waves = this.Waves,
            };
        }
    }

    // 波次数据结构（保持不变）
    [System.Serializable]
    public class WaveData
    {
        public float Delay;
        public List<EnemySpawnData> Enemies = new List<EnemySpawnData>();
    }

    // 敌人生成数据结构（示例）
    [System.Serializable]
    public class EnemySpawnData
    {
        public CharacterDataSO Enemy;
        public int Count;       
    }

}


