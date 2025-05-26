using MyGame.Framework.Event;
using MyGame.Framework.Notification;
using MyGame.Gameplay.Effect;
using MyGame.Scene.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame.Gameplay.Map
{
    public class SpaceShipController : MonoBehaviour
    {
        public SpriteRenderer ship;
        public Transform airplane;
        public Transform detector;
        public Action<Vector3> onArrive;

        private Vector3 detectionRange = new Vector3(1f, 1f, 1f);


        private void Awake()
        {
            GameEventManager.RegisterListener(GameEventType.LevelCompleted, SpawnAnimation);
        }

        void Start()
        {
            StartCoroutine(ExpandAnimation(detector, 1f));
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.LevelCompleted, SpawnAnimation);
        }

        private void SpawnAnimation()
        {
            gameObject.SetActive(true);
            EffectManager.Instance.PlayEffect(EffectLibraryManager.GetEffect("SpawnSpaceShip"), transform.position);
            MainCameraManager.Instance.SetObserverCamera(transform);
            MainCameraManager.Instance.SelectCamera(CameralType.Observer);

            NotificationManager.Instance.ShowFeedNotification("LevelStarted");
        }

        private void Arrival()
        {
            onArrive?.Invoke(transform.position);
        }

        public void SetDetectionRange(int range)
        {
            detectionRange = new Vector3(range * 1.8f, range * 1.8f, range * 1.8f);
        }

        public void Move(Vector3 target)
        {
            StartCoroutine(Rotate(target));
        }

        private IEnumerator MoveCoroutine(Vector3 target, float moveSpeed)
        {
            float time = 0;
            Vector3 startPositon = transform.localPosition;
            while (time < moveSpeed)
            {
                float ratio = time / moveSpeed;
                transform.localPosition = Vector3.Lerp(startPositon, target, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }

            Arrival();
        }

        public IEnumerator Rotate(Vector3 target)
        {
            Vector3 direction = target - airplane.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, airplane.forward);
            yield return StartCoroutine(RotateCoroutine(rotation, 1f));
            StartCoroutine(MoveCoroutine(target, 1f));
        }

        private IEnumerator RotateCoroutine(Quaternion rotation, float rotateSpeed)
        {
            float time = 0;
            Quaternion startRotation = airplane.rotation;
            while (time < rotateSpeed)
            {
                float ratio = time / rotateSpeed;
                transform.rotation = Quaternion.Slerp(startRotation, rotation, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
        }

        private IEnumerator ExpandAnimation(Transform trans, float animateTime)
        {
            float time = 0;
            float rotationSpeed = 10;
            SpriteRenderer sprite = trans.GetComponent<SpriteRenderer>();

            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                sprite.transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
                sprite.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), detectionRange, ratio);
                sprite.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Lerp(0, 1, ratio));
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(ShrinkAnimation(trans, animateTime * 5));
        }

        private IEnumerator ShrinkAnimation(Transform trans, float animateTime)
        {
            float time = 0;
            float rotationSpeed = 10;
            SpriteRenderer sprite = trans.GetComponent<SpriteRenderer>();

            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                sprite.transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
                sprite.transform.localScale = Vector3.Lerp(detectionRange, new Vector3(0.5f, 0.5f, 0.5f), ratio);
                sprite.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Lerp(1, 0, ratio));
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(ExpandAnimation(trans, animateTime / 5));
        }


    }
}


