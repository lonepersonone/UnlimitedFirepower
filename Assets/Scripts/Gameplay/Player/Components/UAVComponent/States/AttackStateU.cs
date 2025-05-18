using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class AttackStateU : IUAVState
    {
        private UnmannedAerialVehicle uav;
        private Vector3 target;


        public AttackStateU(UnmannedAerialVehicle aerialVehicle)
        {
            this.uav = aerialVehicle;
        }

        public void Enter()
        {
            target = uav.RaycastComponentU.GetClosestEnemy();
        }

        public void Exit()
        {

        }

        public void Update()
        {
            AutoFire();
        }

        public void AutoFire()
        {
            if (uav.AttackComponentU.CanFire())
            {
                uav.MoveComponentU.UpdateRotation(target);

                uav.AttackComponentU.CreateProjectile();
                uav.AttackComponentU.RecordFireTime();

                target = uav.RaycastComponentU.GetClosestEnemy();

                if (uav.MoveComponentU.CanFllowParent()) uav.SetState(new IdleStateU(uav));
                if (Vector3.Distance(uav.transform.position, target) < 0.1f) uav.SetState(new PatrolStateU(uav));
            }
        }

    }
}


