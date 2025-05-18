using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyGame.Framework.Manager 
{
    public class GameObjectManager
    {
        private static GameObject[] filteredObjects;

        public static void FindGameObjects()
        {
            filteredObjects = null;

            GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            // ɸѡ��Ϊָ��Tag����Ϸ����
            filteredObjects = gameObjects
                .Where(obj => obj != null && !obj.CompareTag("MainData") && !obj.CompareTag("MainScene"))
                .ToArray();
        }

        public static void OnDisableObjects()
        {
            foreach (var obj in filteredObjects)
            {
                obj.SetActive(false);
            }

        }

        public static void OnEnableObjects()
        {
            foreach (var obj in filteredObjects)
            {
                obj.SetActive(true);
            }
        }

    }
}



