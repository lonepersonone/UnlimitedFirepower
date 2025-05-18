using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Scene.BattleRoom
{
    public class BattleCameraManager : MonoBehaviour
    {
        public static BattleCameraManager Instance;

        public GameObject PlayerCameraPrefab;
        private GameObject playerCameraObject;
        private CinemachineVirtualCamera playerCamera;

        private bool animateable = false;
        private int highestPriority = 10;
        private int basicPriority = 0;
        private float fov = 36f;
        private float minFov = 6f;
        private float maxFov = 50f;
        private float fovSpeed = 5000f;
        private float battleFov = 30f;
        private float mouseX = 0f;
        private float mouseY = 0f;
        private float moveSpeed = 1f;

        private Transform background;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            background = transform.Find("Background");

            CreatePlayerCamera();
        }

        private void CreatePlayerCamera()
        {
            playerCameraObject = Instantiate(PlayerCameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            playerCamera = playerCameraObject.GetComponent<CinemachineVirtualCamera>();
            playerCamera.m_Lens.OrthographicSize = fov;
            playerCamera.Priority = highestPriority;
            background.localScale = new Vector3(fov * 0.25f, fov * 0.25f, fov * 0.25f);
        }

        public void SetPlayerCamera(Transform target)
        {
            playerCamera.Follow = target;
        }

        /// <summary>
        /// 聚焦PlayerCameral，以执行过渡动画
        /// </summary>
        /// <param name="animateTime"></param>
        public IEnumerator PlayerCameraAnimation(float animateTime)
        {
            float time = 0f;
            playerCamera.m_Lens.OrthographicSize = 56f;
            SpriteRenderer sprite = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<SpriteRenderer>();
            Vector3 spriteStartScale = sprite.transform.localScale;
            while (time < animateTime)
            {
                float ratio = time / animateTime;
                playerCamera.m_Lens.OrthographicSize = Mathf.Lerp(56f, 16f, ratio);
                sprite.transform.localScale = Vector3.Lerp(spriteStartScale, Vector3.one, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
        }

    }
}


