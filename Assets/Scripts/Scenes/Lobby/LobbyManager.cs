using Cinemachine;
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
        private Vector3 moveDirection = new Vector3(1, 1, 0);      
        private float basicsFov = 16f;
        private float currentfov = 24f;
        private bool scaleable = false;

        private void Update()
        {
            if(scaleable) Move();
        }

        public void Play()
        {
            SceneController.Instance.EnterMainScene();
        }

        public void EnterLobby()
        {
            StartCoroutine(ScaleCamera());
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
    }
}


