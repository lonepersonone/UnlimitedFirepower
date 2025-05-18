using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Unity.VisualScripting;
using MyGame.Framework.Manager;
using System.Threading.Tasks;

namespace MyGame.Gameplay.Map
{
    [System.Serializable]
    public class PlanetName
    {
        public string chineseName;
        public string englishName;
    }

    [System.Serializable]
    public class PlanetNameListWrapper
    {
        public List<PlanetName> planetNames;
    }

    [System.Serializable]
    public class GalaxyName
    {
        public string chineseName;
        public string englishName;
    }

    [System.Serializable]
    public class GalaxyNameListWrapper
    {
        public List<GalaxyName> galaxyNames;
    }

    [System.Serializable]
    public class PowerName
    {
        public string chineseName;
        public string englishName;
    }

    [System.Serializable]
    public class PowerNameListWrapper
    {
        public List<PowerName> powerNames;
    }

    public class GalaxyNameGenerator : GameSystemBase
    {
        public static GalaxyNameGenerator Instance;

        private List<PlanetName> PlanetNames = new List<PlanetName>();

        private List<GalaxyName> GalaxyNames = new List<GalaxyName>();
        private int galaxyIndex = 0;

        private List<PowerName> PowerNames = new List<PowerName>();
        private int powerIndex = 0;

        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            LoadPlanetNames();
            LoadGalaxyNames();
            LoadPowerNames();

            await Task.Delay(100);
        }

        public GalaxyName GetGalaxyName()
        {
            return GalaxyNames[galaxyIndex++];
        }

        public PowerName GetPowerName() { return PowerNames[powerIndex++]; }

        private void LoadPlanetNames()
        {
            TextAsset jsonText = Resources.Load<TextAsset>("Data/planet_names"); // 注意路径不带后缀
            if (jsonText != null)
            {
                string wrappedJson = "{\"planetNames\":" + jsonText.text + "}"; // 因为JSON是数组，需要包一层
                PlanetNames.AddRange(JsonUtility.FromJson<PlanetNameListWrapper>(wrappedJson).planetNames);
            }
            else
            {
                Debug.LogError("找不到 JSON 文件，请检查 Resources/mainData/planet_names.json");
            }
        }

        private void LoadGalaxyNames()
        {
            TextAsset jsonText = Resources.Load<TextAsset>("Data/galaxy_names");
            if (jsonText != null)
            {
                string wrappedJson = "{\"galaxyNames\":" + jsonText.text + "}";
                GalaxyNames.AddRange(JsonUtility.FromJson<GalaxyNameListWrapper>(wrappedJson).galaxyNames);
            }
            else
            {
                Debug.LogError("找不到 JSON 文件，请检查 Resources/Data/galaxy_names.json");
            }
        }

        private void LoadPowerNames()
        {
            TextAsset jsonText = Resources.Load<TextAsset>("Data/power_names");
            if (jsonText != null)
            {
                string wrappedJson = "{\"powerNames\":" + jsonText.text + "}";
                PowerNames.AddRange(JsonUtility.FromJson<PowerNameListWrapper>(wrappedJson).powerNames);
            }
            else
            {
                Debug.LogError("找不到 JSON 文件，请检查 Resources/Data/galaxy_names.json");
            }
        }


    }
}



