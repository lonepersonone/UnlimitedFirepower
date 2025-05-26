using MyGame.Framework.Audio;
using MyGame.Framework.Record;
using MyGame.Framework.Utilities;
using MyGame.Gameplay.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    public enum FireType
    {
        AutoLock,
        Random
    }

    public class FireUnit : MonoBehaviour
    {
        public Transform[] FirePoints;

        private WeaponAttribute weaponData;

        private MoveComponent moveComponent;
        private Collider2D[] hits = new Collider2D[10];
        private Quaternion targetRotation;
        private float lastFireTime = 0f;
        private float defaultAngle;
        private bool fireable = false;

        private Vector3 autoLockPos;

        private Queue<float> angleQueue = new Queue<float>();

        private void Update()
        {
            AutoFire();

            CircleRaycast();
        }


        public void Initialize(WeaponAttribute data)
        {
            weaponData = data;

            angleQueue.Enqueue(60);
            angleQueue.Enqueue(-60);

            Vector3 direction = transform.localPosition.normalized;
            defaultAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            moveComponent = PlayerController.Instance.GetComponent<MoveComponent>();
        }

        private void StartRandomRotation()
        {
            if (!fireable)
            {
                float angle = angleQueue.Peek();
                targetRotation = Quaternion.AngleAxis(defaultAngle + angle, transform.forward);
                angleQueue.Dequeue();
                angleQueue.Enqueue(angle);
                fireable = true;
            }

            if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
            {
                fireable = false;
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, 30f * Time.deltaTime);
            }
        }

        private void AutoFire()
        {
            if (CanFire() && !moveComponent.Thrusterable())
            {
                foreach (var firePoint in FirePoints)
                {
                    Quaternion rotation = Quaternion.Euler(-(firePoint.rotation.eulerAngles.z + 90), 90, 0);
                    GameObject instance = Instantiate(weaponData.ProjectilePrefab, firePoint.position, rotation);
                    Bullet bullet = instance.GetComponent<Bullet>();
                    bullet.Initialize(weaponData, moveComponent.CurrentSpeed, BulletType.Player);
                }

                PlaySound();

                RecordFireTime();
            }
        }

        public void UpdateRotation(Vector3 targetPosition, float speed)
        {
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
            transform.rotation = rotation;
        }

        public bool CanFire()
        {
            return Time.time - lastFireTime >= weaponData.AttackInterval;
        }

        public void RecordFireTime()
        {
            lastFireTime = Time.time;
        }

        private void CircleRaycast()
        {
            int count = CircleCastNonAlloc(hits);
            if (count > 0)
            {
                List<Vector3> pos = new List<Vector3>();
                for (int i = 0; i < count; i++)
                {
                    pos.Add(hits[i].transform.position);
                }
                autoLockPos = TransformUtil.FindClosestPoint(transform.position, pos);
                UpdateRotation(autoLockPos, 100);
            }
            else
            {
                StartRandomRotation();
            }
        }

        private int CircleCastNonAlloc(Collider2D[] hits)
        {
            return Physics2D.OverlapCircleNonAlloc(transform.position, weaponData.Range, hits, LayerMask.GetMask("Enemy"));
        }

        private void PlaySound()
        {
            AudioHelper.PlayOneShot(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.Player, AudioAction.Shoot));
        }
    }
}


