using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class RaycastComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;
        protected Collider2D[] colliderHits = new Collider2D[30];


        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void UpdateComponent()
        {
            CircleRaycast();
        }

        private void CircleRaycast()
        {
            if (CircleCastNonAlloc(colliderHits) > 0 && !(enemy.CurrentState is AttackStateE)) enemy.SetState(new AttackStateE(enemy));
            if (CircleCastNonAlloc(colliderHits) == 0 && !(enemy.CurrentState is IdleStateE)) enemy.SetState(new IdleStateE(enemy));
        }

        private int CircleCastNonAlloc(Collider2D[] hits)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, enemy.CharacterData.WeaponData.Range, hits, LayerMask.GetMask("Player", "UAV"));
        }
    }
}


