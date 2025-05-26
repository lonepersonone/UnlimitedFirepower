using Cinemachine;
using MyGame.Framework.Manager;
using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame.Scene.Main
{
    public enum CameralType
    {
        Map, // 查看地图
        Observer, // 始终跟随地图玩家图标
    }

    public class MainCameraManager : GameSystemBase
    {
        public static MainCameraManager Instance;

        public GameObject MapCameraPrefab;
        private GameObject map;
        public GameObject ObserverCameraPrefab;
        private GameObject observer;

        private CinemachineVirtualCamera mapCamera;
        private CinemachineVirtualCamera observerCamera;
        private CinemachineVirtualCamera currentCamera;
        private CinemachineVirtualCamera lastCamera;

        public Action<float> ZoomAction;
        private Transform background;

        private bool animateable = false;
        private int highestPriority = 10;
        private int basicPriority = 0;
        private float basicsFov = 16f;
        private float currentfov = 32f;
        private float lastFov = 0;
        private float minFov = 6f;
        private float maxFov = 50f;
        private float fovSpeed = 5000f;
        private float battleFov = 30f;
        private float mouseX = 0f;
        private float mouseY = 0f;
        private float moveSpeed = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            background = transform.Find("Background");
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            CreateMapCamera();
            CreateObserverCamera();
            SelectCamera(CameralType.Map);
            Debug.Log("[MainCameraManager] 初始化完成");

            await Task.Delay(100);

            IsReady = true;
        }

        // Update is called once per frame
        void Update()
        {

            if (currentCamera != null && !animateable && Input.GetAxis("Mouse ScrollWheel") != 0)
                ZoomCamera(currentCamera);

            if (Input.GetMouseButton(2))
            {
                lastCamera = currentCamera;
                if (Input.GetMouseButtonDown(2))
                {
                    mapCamera.transform.position = lastCamera.transform.position;
                    SelectCamera(CameralType.Map);
                }
                MoveCamera(currentCamera);
            }
        }

        private void CreateMapCamera()
        {
            map = Instantiate(MapCameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            mapCamera = map.GetComponent<CinemachineVirtualCamera>();
            mapCamera.Priority = basicPriority;
            currentCamera = mapCamera;
        }

        private void CreateObserverCamera()
        {
            observer = Instantiate(ObserverCameraPrefab, new Vector3(0, 0, -10), Quaternion.identity);
            observerCamera = observer.GetComponent<CinemachineVirtualCamera>();
            observerCamera.Priority = highestPriority;
        }

        public void SetObserverCamera(Transform target)
        {
            observerCamera.Follow = target;
            observerCamera.transform.position = new Vector3(target.position.x, target.position.y, -10);
            observerCamera.m_Lens.OrthographicSize = basicsFov;
        }

        public void SelectCamera(CameralType type)
        {
            switch (type)
            {
                case CameralType.Map:
                    mapCamera.Priority = highestPriority;
                    observerCamera.Priority = basicPriority;
                    currentCamera = mapCamera;
                    break;

                case CameralType.Observer:
                    mapCamera.Priority = basicPriority;
                    observerCamera.Priority = highestPriority;
                    currentCamera = observerCamera;
                    break;
            }
        }

        public void ZoomCamera(CinemachineVirtualCamera camera)
        {
            //Debug.Log("Mouse ScrollWheel:" + Input.GetAxis("Mouse ScrollWheel"));
            currentfov -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * fovSpeed;
            currentfov = Mathf.Clamp(currentfov, minFov, maxFov);
            background.localScale = new Vector3(currentfov * 0.125f, currentfov * 0.125f, currentfov * 0.125f);
            camera.m_Lens.OrthographicSize = currentfov;
            ZoomAction?.Invoke(currentfov);
        }

        public void MoveCamera(CinemachineVirtualCamera camera)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            Vector3 moveDir = mouseX * -camera.transform.right + mouseY * -camera.transform.up;

            moveDir.z = 0;
            camera.transform.position += moveDir * 0.5f * moveSpeed;
        }
        public void SetCameraState(bool state)
        {
            if (state)
            {
                map.SetActive(false);
                observer.SetActive(false);
            }
            else
            {
                map.SetActive(true);
                observer.SetActive(true);
            }
        }

        public void ScaleCamera()
        {
            StartCoroutine(ScaleCameraForMainScene());
        }

        private IEnumerator ScaleCameraForMainScene()
        {
            float duration = 2f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                currentCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentfov, basicsFov, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            lastFov = currentfov;
        }

    }

}


