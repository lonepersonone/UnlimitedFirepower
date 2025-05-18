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

        public void ExploreAroundGalaxy(string id)
        {
            List<PlanetController> planets = HexgonUtil.GetHexesInSpiral(
                galaxyAttribute.PlanetDict, galaxyAttribute.PlanetDict[id].GetIDByInt(), 1);
            planets.Add(galaxyAttribute.PlanetDict[id]);

            foreach (PlanetController planet in planets)
            {
                planet.OnExplored();
            }
        }

    }

}


