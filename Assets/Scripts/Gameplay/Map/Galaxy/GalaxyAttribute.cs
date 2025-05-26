using MyGame.Data.SO;
using MyGame.Gameplay.Effect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class GalaxyAttribute
    {
        private string id;
        private int range;
        private string enName;
        private string cnName;

        private List<PlanetController> planets = new List<PlanetController>();
        private Dictionary<string, PlanetController> planetDict = new Dictionary<string, PlanetController>();
        private List<PlanetData> planetDatas;
        private List<PlanetController> birthPlanets = new List<PlanetController>(); 
        private PlanetController currentPlanet;
        private GalaxyData galaxyData;

        public string EN => enName;
        public string CN => cnName;

        public PlanetController CurrentPlanet => currentPlanet;
        public Vector3 CurrentPlanetPosition => currentPlanet.GetPosition();
        public Dictionary<string, PlanetController> PlanetDict => planetDict;
        public List<PlanetController> BirthPlanets => birthPlanets;

        public GalaxyAttribute(GalaxyData data)
        {
            this.galaxyData = data;

            planetDatas = data.Planets;

            range = data.Range;
            id = data.ID;

            PowerName powerName = GalaxyNameGenerator.Instance.GetPowerName();
            enName = powerName.englishName;
            cnName = powerName.chineseName;
        }

        public GalaxyAttribute Generate()
        {
            CreateBasicPlanets();
            CreateBirthPlanets();
            SetPlanetsWealth();
            return this;
        }

        public void SetCurrentPlanet(PlanetController planetController)
        {
            currentPlanet = planetController;
        }

        public void CreateBasicPlanets()
        {
            int hexagoRange = range;
            for (int q = -hexagoRange; q <= hexagoRange; q++)
            {
                int r1 = Mathf.Max(-hexagoRange, -q - hexagoRange);
                int r2 = Mathf.Min(hexagoRange, -q + hexagoRange);
                for (int r = r1; r <= r2; r++)
                {
                    if (q == 0 && r == 0) CreateCenter(q, r);
                    else CreateCell(q, r);
                }
            }   
        }

        private void CreateCenter(int q, int r)
        {
            GameObject instance = Object.Instantiate(galaxyData.TransferPlanets[0].PlanetPrefab);

            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, null);
            planet.GalaxyID = galaxyData.ID;
            planet.OnExplored();
            planet.SetType(HexCellType.Channel);

            planetDict[planet.LocationID] = planet;
        }

        private void SetPlanetsWealth()
        {
            List<PlanetController> planetsClone = new List<PlanetController>();
            planetsClone.AddRange(planets);

            // 设置特殊星球财富值
            for(int i = 0; i < galaxyData.SpecialPlanetsCount; i++)
            {
                int seed = Random.Range(0, planetsClone.Count);
                int factorSeed = Random.Range(0, galaxyData.SpecialFactor.Count);
                PlanetController planet = planetsClone[seed];

                // 二次生成
                int[] id = planet.GetIDByInt();
                planets.Remove(planet);
                planetDict.Remove(planet.LocationID);
                Object.Destroy(planet.gameObject);

                int specialSeed = Random.Range(0, galaxyData.SpecialPlanets.Count);
                PlanetData specialData = galaxyData.SpecialPlanets[specialSeed];

                GameObject instance = Object.Instantiate(specialData.PlanetPrefab);
                PlanetController specialPlanet = instance.GetComponent<PlanetController>();
                specialPlanet.Initialize(id[0], id[1], specialData.Level);
                specialPlanet.GalaxyID = galaxyData.ID;
                specialPlanet.SetWealth((int)(galaxyData.WealthValue * galaxyData.SpecialFactor[factorSeed]));
                specialPlanet.SetType(HexCellType.Life);
                specialPlanet.OnExplored();

                planets.Add(specialPlanet);
                planetDict[specialPlanet.LocationID] = specialPlanet;

                planetsClone.Remove(planet);
            }

            // 设置普通星球财富值
            foreach(var planet in planetsClone)
            {
                float factor = Random.Range( 1 - galaxyData.BaseWealthFactor, 1 + galaxyData.BaseWealthFactor);
                planet.SetWealth((int)(galaxyData.WealthValue * factor));
            }
        } 

        private PlanetController CreateCell(int q, int r)
        {
            PlanetData planetData = GetRandomPlanet();

            GameObject instance = Object.Instantiate(planetData.PlanetPrefab);
            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, planetData.Level);
            planet.GalaxyID = galaxyData.ID;
            planet.SetType(HexCellType.Life);

            planets.Add(planet);
            planetDict[planet.LocationID] = planet;

            return planet;
        }

        private void CreateBirthPlanets()
        {
            int outerRange = range + 1;

            CreateTransferCell(outerRange, -outerRange);
            CreateTransferCell(-outerRange, outerRange);
            CreateTransferCell(outerRange, 0);
            CreateTransferCell(-outerRange, 0);
            CreateTransferCell(0, outerRange);
            CreateTransferCell(0, -outerRange);
        }

        private void CreateTransferCell(int q, int r)
        {            
            GameObject instance = Object.Instantiate(galaxyData.TransferPlanets[1].PlanetPrefab);

            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, null);
            planet.GalaxyID = galaxyData.ID;
            planet.OnExplored();
            planet.SetType(HexCellType.Channel);
            birthPlanets.Add(planet);
        }

        public void Destory()
        {
            for (int i = 0; i < planets.Count; i++)
            {
                Object.Destroy(planets[i].gameObject);
            }

            for (int i = 0; i < birthPlanets.Count; i++)
            {
                Object.Destroy(birthPlanets[i].gameObject);
            }
        }

        public void AdjustDynamicWealth(int factor)
        {
/*            foreach (var planet in planets)
            {
                planet.data.Wealth *= factor;
                planet.WealthText.text = planet.data.Wealth.ToString();
            }*/
        }

        public Vector3 GetPlanetPosition(string key)
        {
            if (planetDict.ContainsKey(key))
            {
                return planetDict[key].GetPosition();
            }
            return Vector3.zero;
        }

        private PlanetData GetRandomPlanet()
        {
            float value = Random.Range(0, 1);
            float cumulativeProbability = 0;
            for (int i = 0; i < planetDatas.Count; i++)
            {
                cumulativeProbability += planetDatas[i].Ratio;
                if (value <= cumulativeProbability) return planetDatas[i];
            }
            return null;
        }

        public PlanetController GetBirthPlanet()
        {
            currentPlanet = BirthPlanets[Random.Range(0, BirthPlanets.Count)];
            return currentPlanet;
        }
    }
}


