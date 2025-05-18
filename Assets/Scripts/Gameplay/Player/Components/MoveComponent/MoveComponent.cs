using Michsky.UI.Reach;
using MyGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// 控制飞机移动组件
    /// </summary>
    public class MoveComponent : MonoBehaviour, IAirplaneComponent
    {
        private CharacterAttribute data;

        private Transform center;
        public Action<Vector3> MoveAction;

        [Header("MoveAttribute")]
        private float currentSpeed;
        private float minDistance = 1f;

        [Header("ThrusterAttribute")]
        private ProgressBar thrusterBar;
        private float currentDuration = 2;
        private float lastExhaustTime;
        private bool thrusterLock = false;


        public float CurrentSpeed => currentSpeed;

        public void Initialize(PlayerController player)
        {
            data = player.CharacterData;
            center = player.transform;
            currentSpeed = data.MoveSpeed;

            thrusterBar = PlayerManager.Instance.ThrusterBar;
            currentDuration = data.ThrusterDuration;

            thrusterBar.SetRange(0, data.ThrusterDuration);
            thrusterBar.SetValue(Mathf.Clamp(currentDuration, 0, data.ThrusterDuration));
        }

        public void UpdateComponent()
        {
            Move();
            MoveAction?.Invoke(center.localPosition);

            if (currentDuration < data.ThrusterDuration)
                if (Time.time - lastExhaustTime > data.ThrusterCoolTime)
                    RecoverThruster();
        }


        public void Move()
        {
            float dealtX = Input.GetAxis("Horizontal");
            float dealtY = Input.GetAxis("Vertical");

            if (dealtX != 0 || dealtY != 0)
            {
                Vector3 moveDir = new Vector3(dealtX, dealtY, 0);
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;
                Quaternion rotation = Quaternion.AngleAxis(angle, transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, data.RotateSpeed * Time.deltaTime);

                if (Input.GetKey(KeyCode.Space))
                {
                    if (currentDuration > 0 && !thrusterLock)
                    {
                        thrusterLock = true;
                        currentSpeed *= data.ThrusterRate;
                    }

                    currentDuration -= data.ThrusterReduce;
                    thrusterBar?.SetValue(Mathf.Clamp(currentDuration, 0, data.ThrusterDuration));

                    if (currentDuration <= 0 && thrusterLock)
                    {
                        thrusterLock = false;
                        lastExhaustTime = Time.time;
                        currentSpeed /= data.ThrusterRate;
                    }
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    thrusterLock = false;
                    lastExhaustTime = Time.time;
                    currentSpeed = data.MoveSpeed;
                }

                transform.Translate(moveDir * currentSpeed * Time.deltaTime, Space.World);
            }
        }

        private void RecoverThruster()
        {
            if (!thrusterLock)
            {
                currentDuration += data.ThrusterRecover;
                thrusterBar?.SetValue(Mathf.Clamp(currentDuration, 0, data.ThrusterDuration));
            }
        }

        public bool Thrusterable()
        {
            return thrusterLock;
        }
    }
}


