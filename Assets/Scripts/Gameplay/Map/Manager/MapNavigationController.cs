using MyGame.Framework.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    // 定义接口抽象
    public interface ISpaceShipController
    {
        event Action<Vector3> onArrive;
        void Move(Vector3 target);
        void SetGalaxy(PlanetController galaxy);
        PlanetController CurrentGalaxy { get; }
    }

    public interface IGalaxyAttribute
    {
        Dictionary<string, PlanetController> PlanetDict { get; }
    }


    public class MapNavigationController
    {
        private SpaceShipController spaceShipController;
        private GalaxyAttribute galaxyAttribute;

        public event Action<Vector3> OnArrive;

        public MapNavigationController(SpaceShipController ship, GalaxyAttribute galaxy)
        {
            spaceShipController = ship;
            galaxyAttribute = galaxy;
            spaceShipController.onArrive += HandleArrive;

        }

        public void ResetGalaxy(GalaxyAttribute galaxy)
        {
            galaxyAttribute = galaxy;
        }

        public void MoveToGalaxy(string locationID)
        {
            if (galaxyAttribute.PlanetDict.ContainsKey(locationID))
            {
                Vector3 targetPos = galaxyAttribute.PlanetDict[locationID].GetPosition();
                spaceShipController.Move(targetPos);
            }
        }

        private void HandleArrive(Vector3 pos)
        {      
            string locationID = HexgonUtil.WorldToLocationID(pos);

            // 更新Planet状态
            galaxyAttribute.CurrentPlanet?.OnArrived(false);
            galaxyAttribute.SetCurrentPlanet(galaxyAttribute.PlanetDict[locationID]);
            galaxyAttribute.CurrentPlanet?.OnArrived(true);

            OnArrive?.Invoke(pos);
        }

    }

}

