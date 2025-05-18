using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Data.SO
{
    public class ScriptableObjectData
    {
        public static ScriptableObjectData Instance;
        private static Dictionary<string, WeaponDataSO> weaponDict;
        private static Dictionary<string, CharacterDataSO> characterDict;

        public static void LoadSO()
        {
            LoadWeapons();
            LoadCharacters();
        }

        private static void LoadWeapons()
        {
            WeaponDataSO[] weapons = Resources.LoadAll<WeaponDataSO>("mainData/SO/Weapon");
            weaponDict = new Dictionary<string, WeaponDataSO>();

            foreach (WeaponDataSO weapon in weapons)
            {
                weaponDict[weapon.ID] = weapon;
            }
        }

        public static WeaponDataSO GetWeaponById(string id)
        {
            return weaponDict.ContainsKey(id) ? weaponDict[id] : null;
        }

        private static void LoadCharacters()
        {
            CharacterDataSO[] characters = Resources.LoadAll<CharacterDataSO>("mainData/SO/Character");
            characterDict = new Dictionary<string, CharacterDataSO>();
            foreach (CharacterDataSO character in characters)
            {
                characterDict[character.ID] = character;
            }
        }

        public static CharacterDataSO GetCharacterById(string id)
        {
            return characterDict.ContainsKey(id) ? characterDict[id] : null;
        }

    }
}


