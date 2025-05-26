using MyGame.Data.SO;
using MyGame.Framework.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class MapAttribute
    {
        private Dictionary<string, GalaxyAttribute> galaxyDict = new Dictionary<string, GalaxyAttribute>();

        private List<GalaxyAttribute> galaxie = new List<GalaxyAttribute>();
        private int currentIndex = -1;

        private SpaceShipController spaceShipController;

        public event Action<GalaxyAttribute> OnGalaxyChanged;

        public GalaxyAttribute CurrentGalxy => currentIndex >=0 && currentIndex < galaxie.Count ? galaxie[currentIndex] : null;

        public SpaceShipController SpaceShipController => spaceShipController;

        public MapAttribute(GameMapConfigSO config)
        {
            InitialGalaxyDict(config.Galaxies);

            CreateSpaceShip(config.SpaceShipDataSO);
        }

        private void InitialGalaxyDict(List<GalaxyData> galaxies)
        {
            foreach (var value in galaxies)
            {
                galaxyDict[value.ID] = new GalaxyAttribute(value);
                galaxie.Add(galaxyDict[value.ID]);
            }
        }

        public GalaxyAttribute GenerateNextGalay()
        {
            if(currentIndex >= 0 && currentIndex < galaxie.Count)
            {
                CurrentGalxy.Destory();
            }

            currentIndex++;

            if(currentIndex < galaxie.Count)
            {
                GalaxyAttribute nextGalaxy = galaxie[currentIndex];
                nextGalaxy.Generate();

                PlanetController planet = nextGalaxy.GetBirthPlanet();
                ResetSpaceShip(planet);

                OnGalaxyChanged?.Invoke(nextGalaxy);

                return nextGalaxy;
            }

            return null;
        }

        private void CreateSpaceShip(SpaceShipDataSO so)
        {
            GameObject instance = UnityEngine.Object.Instantiate(so.SpaceShipPrefab); 
            spaceShipController = instance.GetComponent<SpaceShipController>();
            instance.transform.rotation = TransformUtil.GetLookRotation(instance.transform, Vector3.zero);
            instance.SetActive(false);
        }

        public void ResetSpaceShip(PlanetController birthPlanet)
        {
            spaceShipController.transform.position = birthPlanet.GetPosition();
            spaceShipController.transform.rotation = TransformUtil.GetLookRotation(spaceShipController.transform, Vector3.zero);
        }

    }
}


