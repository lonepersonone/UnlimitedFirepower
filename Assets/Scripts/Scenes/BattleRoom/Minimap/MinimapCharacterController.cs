using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Scene.BattleRoom
{
    public class MinimapCharacterController : MonoBehaviour
    {
        public Transform mappingObject;

        // Update is called once per frame
        void Update()
        {
            if (mappingObject != null) transform.rotation = Quaternion.Slerp(transform.rotation, mappingObject.rotation, 30 * Time.deltaTime);

            if (PlayerController.Instance != null) UpdatePosition();
        }

        public void Initialize(Transform parent)
        {
            this.mappingObject = parent;
        }

        private void UpdatePosition()
        {
            if (mappingObject != null)
            {
                Vector3 direction = mappingObject.position - PlayerController.Instance.transform.position;
                Vector3 localPos = new Vector3(direction.x * 2, direction.y * 2, 0);
                transform.localPosition = localPos;
            }
        }


    }
}


