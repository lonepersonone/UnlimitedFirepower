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
        private PlanetDataSO[] planetSOs;
        private List<PlanetController> birthPlanets = new List<PlanetController>(); 
        private PlanetController currentPlanet;
        private GalaxyDataSO galaxyData;

        public string EN => enName;
        public string CN => cnName;

        public PlanetController CurrentPlanet => currentPlanet;
        public Vector3 CurrentPlanetPosition => currentPlanet.GetPosition();
        public Dictionary<string, PlanetController> PlanetDict => planetDict;
        public List<PlanetController> BirthPlanets => birthPlanets;

        public GalaxyAttribute(GalaxyDataSO so)
        {
            this.galaxyData = so;
            planetSOs = so.PlanetDataSOs;

            range = so.Range;
            id = so.ID;

            PowerName powerName = GalaxyNameGenerator.Instance.GetPowerName();
            enName = powerName.englishName;
            cnName = powerName.chineseName;
        }

        public GalaxyAttribute CreateGalaxy()
        {
            CreateBasicPlanets();
            CreateBirthPlanets();
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
            GameObject instance = Object.Instantiate(galaxyData.TransferPrefab);

            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, null);
            planet.GalaxyID = galaxyData.ID;
            planet.OnExplored();
            planet.SetType(HexCellType.Channel);

            planetDict[planet.LocationID] = planet;

            // 显示特效
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("Center"), instance.transform, Vector3.zero, Quaternion.EulerAngles(90, 0, 0));
        }

        private PlanetController CreateCell(int q, int r)
        {
            PlanetDataSO so = GetRandomPlanet();

            GameObject instance = Object.Instantiate(so.PlanetPrefab);
            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, so.Level);
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
            GameObject instance = Object.Instantiate(galaxyData.TransferPrefab);

            PlanetController planet = instance.GetComponent<PlanetController>();
            planet.Initialize(q, r, null);
            planet.GalaxyID = galaxyData.ID;
            planet.OnExplored();
            planet.SetType(HexCellType.Channel);

            // 显示特效
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("Channel"), instance.transform, Vector3.zero, Quaternion.EulerAngles(90, 0 ,0));

            birthPlanets.Add(planet);
        }

        public void DestoryMap()
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

        private PlanetDataSO GetRandomPlanet()
        {
            float value = Random.Range(0, 1);
            float cumulativeProbability = 0;
            for (int i = 0; i < planetSOs.Length; i++)
            {
                cumulativeProbability += planetSOs[i].Ratio;
                if (value <= cumulativeProbability) return planetSOs[i];
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


