using MyGame.Data.SO;
using MyGame.Framework.Utilities;
using MyGame.Gameplay.Map;
using MyGame.Scene.Main;
using MyGame.UI.Transition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class MapAttribute
    {
        private Dictionary<string, GalaxyAttribute> galaxyDict = new Dictionary<string, GalaxyAttribute>();

        private List<GalaxyAttribute> galaxyLevels = new List<GalaxyAttribute>();
        private int currentIndex = -1;

        private SpaceShipController spaceShipController;

        public event Action<GalaxyAttribute> OnChanged;

        public GalaxyAttribute CurrentGalxy => currentIndex <galaxyLevels.Count ? galaxyLevels[currentIndex] : null;

        public SpaceShipController SpaceShipController => spaceShipController;

        public MapAttribute(ScriptableManager scriptable)
        {
            InitialGalaxyDict(scriptable.GetGalaxies());
            InitialGalaxyLevels();

            CreateSpaceShip(scriptable.GetSpaceShipById("Basic"));

            EnableNextGalay();
        }

        private void InitialGalaxyDict(GalaxyDataSO[] galaxies)
        {
            foreach (var value in galaxies)
            {
                galaxyDict[value.ID] = new GalaxyAttribute(value);
            }
        }

        private void InitialGalaxyLevels()
        {
            for (int i = 0; i < galaxyDict.Count; i++)
            {
                galaxyLevels.Add(galaxyDict[i.ToString()]);
            }
        }

        public GalaxyAttribute EnableNextGalay()
        {
            GalaxyAttribute galaxy = CreateGalaxy();
            if(galaxy != null)
            {
                PlanetController planet = galaxy.GetBirthPlanet();
                galaxy.SetCurrentPlanet(planet);
                ResetSpaceShip(planet);
                OnChanged?.Invoke(galaxy);
            }
            return galaxy;
        }

        private GalaxyAttribute CreateGalaxy()
        {
            currentIndex++;
            if (currentIndex >= 1) galaxyLevels[currentIndex - 1].DestoryMap();
            if (currentIndex < galaxyLevels.Count)
            {
                GalaxyAttribute galaxy = galaxyLevels[currentIndex].CreateGalaxy();
                return galaxy;
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


