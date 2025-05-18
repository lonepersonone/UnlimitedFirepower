using MyGame.Framework.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Data.SO
{
    public class ScriptableManager
    {
        // 数据副本
        private Dictionary<string, CharacterDataSO> characterDict;
        private Dictionary<string, DynamicTextSO> dynamicTextDict;
        private Dictionary<string, UpgradeDataSO> upgradeDict;
        private Dictionary<string, GalaxyDataSO> galaxyDict;
        private Dictionary<string, SpaceShipDataSO> spaceShipDict;
        private Dictionary<string, WealthDataSO> wealthDict;

        public async Task LoadScriptableData(Action<float> onProgress = null)
        {
            float totalSteps = 3;
            float step = 0;

            // 1. 加载所有角色资源
            var characterArray = await ResourcesUtil.LoadAllAsync<CharacterDataSO>("Data/SO/Player", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });

            characterDict = characterArray.ToDictionary(c => c.ID, c => c);
            step++;

            // 3. 加载动态文本资源
            var dynamicTextArray = await ResourcesUtil.LoadAllAsync<DynamicTextSO>("Data/SO/DynamicTexts", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });
            dynamicTextDict = dynamicTextArray.ToDictionary(c => c.ID, c => c);
            step++;

            // 4. 加载升级选项资源
            var upgradeArray = await ResourcesUtil.LoadAllAsync<UpgradeDataSO>("Data/SO/Upgrades", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });
            upgradeDict = upgradeArray.ToDictionary(c => c.ID, c => c);
            step++;

            // 5. 加载地图资源
            var galaxyArray = await ResourcesUtil.LoadAllAsync<GalaxyDataSO>("Data/SO/Maps/Galaxies", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });
            galaxyDict = galaxyArray.ToDictionary(c => c.ID, c => c);
            step++;

            // 6. 加载地图飞行器资源
            var spaceShipArray = await ResourcesUtil.LoadAllAsync<SpaceShipDataSO>("Data/SO/Maps", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });
            spaceShipDict = spaceShipArray.ToDictionary(c => c.ID, c => c);
            step++;

            // 7. 加载Wealth资源
            var wealthArray = await ResourcesUtil.LoadAllAsync<WealthDataSO>("Data/SO/Props", p =>
            {
                onProgress?.Invoke((step + p) / totalSteps);
            });
            wealthDict = wealthArray.ToDictionary(c => c.ID, c => c);
            step++;

            onProgress?.Invoke(1f);
        }

        public CharacterDataSO GetCharacterById(string id) { return characterDict.ContainsKey(id) ? characterDict[id] : null; }

        public DynamicTextSO GetDynamicTextById(string id) { return dynamicTextDict.ContainsKey(id) ? dynamicTextDict[id] : null; }

        public UpgradeDataSO GetUpgradeById(string id) { return upgradeDict.ContainsKey(id) ? upgradeDict[id] : null; }

        public GalaxyDataSO GetGalaxyById(string id) { return galaxyDict.ContainsKey(id) ? galaxyDict[id] : null; }

        public SpaceShipDataSO GetSpaceShipById(string id) { return spaceShipDict.ContainsKey(id) ? spaceShipDict[id] : null; }

        public WealthDataSO GetWealthById(string id) { return wealthDict.ContainsKey(id) ? wealthDict[id] : null; }

        public UpgradeDataSO[] GetUpgrades() { return upgradeDict.Values.ToArray(); }

        public GalaxyDataSO[] GetGalaxies() { return galaxyDict.Values.ToArray(); }
    }
}


