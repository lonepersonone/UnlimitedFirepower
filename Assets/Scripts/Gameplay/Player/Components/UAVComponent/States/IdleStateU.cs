using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class IdleStateU : IUAVState
    {
        private UnmannedAerialVehicle uav;

        public IdleStateU(UnmannedAerialVehicle aerialVehicle)
        {
            this.uav = aerialVehicle;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Update()
        {
            if (uav.transform.localPosition != uav.MoveComponentU.RealtimeAnchorage)
            {
                uav.MoveComponentU.UpdatePosition(uav.MoveComponentU.RealtimeAnchorage);

                if (Vector3.Distance(uav.transform.localPosition, uav.MoveComponentU.RealtimeAnchorage) <= 0.1f)
                {
                    uav.SetState(new PatrolStateU(uav));
                    //uav.RecordMoveAnchorageTime();
                }
            }
        }


    }
}


