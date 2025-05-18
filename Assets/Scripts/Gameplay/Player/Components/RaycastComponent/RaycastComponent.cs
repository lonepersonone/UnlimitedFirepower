using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public class RaycastComponent : MonoBehaviour, IAirplaneComponent
    {
        private Collider2D[] hits = new Collider2D[30];

        public void Initialize(PlayerController player)
        {

        }

        public void UpdateComponent()
        {
            //CircleRaycast();
        }

        private void CircleRaycast()
        {
            int count = CircleCastNonAlloc(hits);
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = hits[i];
            }
        }

        private int CircleCastNonAlloc(Collider2D[] hits)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, 5f, hits, LayerMask.GetMask("EnemyPrefab"));
        }
    }
}


