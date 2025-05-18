using MyGame.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Enemy
{
    public class MoveComponentE : MonoBehaviour, IShipComponentE
    {
        private EnemyController enemy;
        private Transform target;
        private float currentSpeed;

        public float CurrentSpeed => currentSpeed;

        public void Initialize(EnemyController enemy)
        {
            this.enemy = enemy;
            currentSpeed = enemy.CharacterData.MoveSpeed;
            if (PlayerController.Instance != null) target = PlayerController.Instance.transform;
        }

        public void UpdateComponent()
        {
            if (target == null) target = PlayerController.Instance.transform;
            if (target != null) MoveToPlayer(target.transform.position);
        }

        public void MoveToPlayer(Vector3 target)
        {
            UpdateRotation(target, 2f);
            transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
        }

        public void UpdateRotation(Vector3 targetPosition, float speed)
        {
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
        }

        public void SetCurrentSpeed(float value)
        {
            this.currentSpeed = value;
        }

    }
}


