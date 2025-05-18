using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class PatrolStateU : IUAVState
    {
        private UnmannedAerialVehicle uav;
        private Vector3 target; //巡逻目标
        private float radius; //活动半径

        public PatrolStateU(UnmannedAerialVehicle uav)
        {
            this.uav = uav;
        }

        public void Enter()
        {
            radius = 5f;
            SetPatrolPosition();
        }

        public void Exit()
        {

        }

        public void Update()
        {
            Raycast();
            UpdatePosition();
        }

        private void SetPatrolPosition()
        {
            float x = Random.Range(-radius, radius);
            float y = Random.Range(-radius, radius);
            float z = Random.Range(-radius, radius);
            target = new Vector3(uav.MoveComponentU.RealtimeAnchorage.x + x, uav.MoveComponentU.RealtimeAnchorage.y + y, uav.MoveComponentU.RealtimeAnchorage.z + z);
        }

        private void Raycast()
        {
            if (uav.RaycastComponentU.hasEnemy()) uav.SetState(new AttackStateU(uav));
        }

        private void UpdatePosition()
        {
            uav.MoveComponentU.UpdatePosition(target);
            if (Vector3.Distance(uav.transform.localPosition, target) <= 0.1f)
            {
                if (uav.MoveComponentU.CanFllowParent()) uav.SetState(new IdleStateU(uav));
                else SetPatrolPosition();
            }
        }

    }
}


