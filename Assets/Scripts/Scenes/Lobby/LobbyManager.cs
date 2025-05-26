using Cinemachine;
using MyGame.Framework.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame.Scene.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        public CinemachineVirtualCamera CVM;
        public Transform Background;
        public GameObject[] Parallaxes;

        private Vector3 moveDirection = new Vector3(1, 1, 0);      
        private float basicsFov = 16f;
        private float currentfov = 24f;
        private bool scaleable = false;

        private float moveDuration = 240f;
        private float lastMoveTime = 0f;

        private void Start()
        {

        }

        private void Update()
        {
            if (scaleable) Move();

            if (Time.time - lastMoveTime > moveDuration) ResetState();
        }

        private void OnDestroy()
        {
            
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 编辑器模式下停止运行
            #else
                Application.Quit(); // 发布版本退出应用
            #endif
        }

        public void Play()
        {
            SceneController.Instance.EnterMainScene();
        }

        public void EnterHome()
        {
            StartCoroutine(ScaleCamera());

            AudioHelper.PlayLoop(gameObject, AudioIDManager.GetAudioID(Framework.Audio.AudioType.Scene, AudioAction.Passion));
        }

        private void Move()
        {
            transform.Translate(moveDirection * 0.5f * Time.deltaTime);
        }

        private IEnumerator ScaleCamera()
        {
            float duration = 3f;
            float elapsed = 0;
            while (elapsed < duration)
            {
                CVM.m_Lens.OrthographicSize = Mathf.Lerp(currentfov, basicsFov, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            scaleable = true;
        }

        private void ResetState()
        {
            lastMoveTime = Time.time;
            transform.position = Vector3.zero;

            foreach(var parallax in Parallaxes)
            {
                parallax.transform.position = Vector3.zero;
            }
        }


    }

}


