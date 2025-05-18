using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class MoveComponentU : MonoBehaviour, IUAVComponent
    {
        private UnmannedAerialVehicle uav;
        private Vector3 defaultAnchorage; //默认停泊位置, 向量表示

        private UAVComponent parent;

        public Vector3 RealtimeAnchorage => parent != null ? new Vector3(
        defaultAnchorage.x + parent.transform.localPosition.x,
        defaultAnchorage.y + parent.transform.localPosition.y,
        defaultAnchorage.z + parent.transform.localPosition.z) : defaultAnchorage;

        public void Initialize(UnmannedAerialVehicle uav)
        {
            this.uav = uav;

            List<Vector3> uavArea = TransformUtil.GetRingGridPositions(Vector3.zero, 10, 10, 2);
            defaultAnchorage = uavArea[Random.Range(0, uavArea.Count)];
        }

        public void UpdateComponent()
        {
            if (parent == null) parent = PlayerController.Instance.GetComponent<UAVComponent>();
        }

        public void UpdatePosition(Vector3 target)
        {
            UpdateRotation(target);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, uav.CharacterData.MoveSpeed * Time.deltaTime);
        }

        public bool CanFllowParent()
        {
            if(parent == null) return false;
            return Vector3.Distance(transform.position, parent.transform.position) > 20f;
        }

        public void UpdateRotation(Vector3 target)
        {
            Vector3 direction = target - transform.localPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, uav.CharacterData.RotateSpeed);
        }

    }
}


