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

        // ȫ������
        public float defaultWealthFactor = 0.2f;
        public float defaultSpecialFactor = 1.5f;

        // ������
        public SpaceShipDataSO SpaceShipDataSO;

        // ����ϵ��������
        [SerializeField] private List<GalaxyData> galaxies = new List<GalaxyData>();

        // ������
        public List<GalaxyData> Galaxies => galaxies;

    }
        // ��ϵ���ݽṹ
        [System.Serializable]
    public class GalaxyData
    {
        public string ID;
        public string Name;
        public int Range; // ��ϵ��С
        public float WealthValue;
        public float BaseWealthFactor;
        public int SpecialPlanetsCount;
        public List<float> SpecialFactor;

        // 0Ϊ���ģ� 1Ϊ�߽�
        public List<PlanetData> TransferPlanets = new List<PlanetData>();
        public List<PlanetData> SpecialPlanets = new List<PlanetData>();
        public List<PlanetData> Planets = new List<PlanetData>();

        // ����Ψһ����ID
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

    // �ؿ����ݽṹ
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

    // �������ݽṹ�����ֲ��䣩
    [System.Serializable]
    public class WaveData
    {
        public float Delay;
        public List<EnemySpawnData> Enemies = new List<EnemySpawnData>();
    }

    // �����������ݽṹ��ʾ����
    [System.Serializable]
    public class EnemySpawnData
    {
        public CharacterDataSO Enemy;
        public int Count;       
    }

}


