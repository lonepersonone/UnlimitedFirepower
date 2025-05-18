using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// 航空母舰
    /// 1.不断制造无人机
    /// </summary>
    public class UAVComponent : MonoBehaviour, IAirplaneComponent
    {
        private CharacterAttribute uavData;
        private CharacterAttribute playerData;

        private int lastUAVCount;

        private List<GameObject> uavs = new List<GameObject>();

        public void Initialize(PlayerController player)
        {
            playerData = player.CharacterData;
            uavData = player.CharacterData.CurrentChild;

            lastUAVCount = playerData.UAVCount;

            CreateFightUAV(lastUAVCount);
        }

        public void UpdateComponent()
        {
            if (lastUAVCount != playerData.UAVCount) UpdateUAVCount();
        }

        public void CreateFightUAV(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject instance = Instantiate(uavData.CharacterPrefab);
                instance.GetComponent<UnmannedAerialVehicle>().Initialize(uavData);
                instance.transform.position = transform.position;
                uavs.Add(instance);
            }
        }

        private void UpdateUAVCount()
        {
            CreateFightUAV(playerData.UAVCount - lastUAVCount);
            lastUAVCount = playerData.UAVCount;
        }

        public void RemoveUAV(GameObject uavObj)
        {
            uavs.Remove(uavObj);
            Destroy(uavObj);
        }
    }

}


