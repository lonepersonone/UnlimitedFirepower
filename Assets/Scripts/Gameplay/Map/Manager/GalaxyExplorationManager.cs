using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class GalaxyExplorationManager
    {
        private GalaxyAttribute galaxyAttribute;

        public GalaxyExplorationManager(GalaxyAttribute galaxy)
        {
            galaxyAttribute = galaxy;
        }

        public void ResetGalaxy(GalaxyAttribute galaxy)
        {
            galaxyAttribute = galaxy;
        }

        public void ExploreAroundGalaxy(PlanetController planet)
        {
            List<PlanetController> planets = HexgonUtil.GetHexesInSpiral(
                galaxyAttribute.PlanetDict, planet.GetIDByInt(), 3);
            planets.Add(planet);

            foreach (PlanetController item in planets)
            {
                item.OnExplored();
            }
        }

    }

}


