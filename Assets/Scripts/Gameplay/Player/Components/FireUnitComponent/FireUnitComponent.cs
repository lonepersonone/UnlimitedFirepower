using MyGame.Gameplay.Effect;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// »ðÁ¦ÇãÐºµ¥Î»
    /// </summary>
    public class FireUnitComponent : MonoBehaviour, IAirplaneComponent
    {
        private CharacterAttribute characterAttribute;

        public List<Transform> UnitTransform;

        private List<GameObject> fireUnits = new List<GameObject>();
        private Queue<Vector3> positionQueue = new Queue<Vector3>();

        public void Initialize(PlayerController player)
        {
            characterAttribute = player.CharacterData;

            foreach (var trans in UnitTransform)
            {
                positionQueue.Enqueue(trans.position);
            }

            for(int i = 0; i < characterAttribute.FireUnitCount; i++)
            {
                AddFireUnit();
            }           
        }

        public void UpdateComponent()
        {

        }

        public void AddFireUnit()
        {
            if (positionQueue.Count > 0)
            {
                Vector3 pos = positionQueue.Dequeue();
                GameObject instance = Instantiate(characterAttribute.WeaponData.WeaponPrefab, transform);
                instance.transform.SetParent(transform);
                instance.transform.localPosition = pos;
                instance.GetComponent<FireUnit>().Initialize(characterAttribute.WeaponData);
                fireUnits.Add(instance);

                EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("SpawnFireUnit"), transform.position);
            }
        }

        public bool IsFull()
        {
            return  characterAttribute.FireUnitCount < UnitTransform.Count;
        }
    }
}


