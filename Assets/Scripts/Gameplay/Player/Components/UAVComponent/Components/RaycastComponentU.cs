using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class RaycastComponentU : MonoBehaviour, IUAVComponent
    {
        private UnmannedAerialVehicle uav;
        private Collider2D[] hits = new Collider2D[10];

        public void Initialize(UnmannedAerialVehicle uav)
        {
            this.uav = uav;
        }

        public void UpdateComponent()
        {

        }

        public bool hasEnemy()
        {
            int count = CircleCastNonAlloc(hits);
            return count > 0;
        }

        public Vector3 GetClosestEnemy()
        {
            int count = CircleCastNonAlloc(hits);
            List<Vector3> candidatePoints = new List<Vector3>();
            for (int i = 0; i < count; i++) candidatePoints.Add(hits[i].transform.position);
            return TransformUtil.FindClosestPoint(transform.position, candidatePoints);
        }

        private int CircleCastNonAlloc(Collider2D[] hits)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, uav.CharacterData.WeaponData.Range, hits, LayerMask.GetMask("Enemy"));
        }

    }
}


