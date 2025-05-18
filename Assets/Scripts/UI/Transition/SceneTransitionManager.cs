using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.UI.Transition
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance;

        public ImageFading imageFading;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void EnableInitScreen()
        {
            imageFading.FadeIn();
        }

        public void DisableInitScreen()
        {
            imageFading.FadeOut();
        }

    }


}

