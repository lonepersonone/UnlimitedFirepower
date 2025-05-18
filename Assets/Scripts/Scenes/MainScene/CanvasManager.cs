using Michsky.UI.Reach;
using MyGame.Framework.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Scene.Main
{
    public class CanvasManager : GameSystemBase
    {
        public static CanvasManager Instance;

        public GameObject HudCanvasObj;

        private void Awake()
        {
            Instance = this;
        }

        public override async Task InitializeAsync(Action<float> onProgress = null)
        {
            DisablePause();

            await Task.Delay(100);
        }

        public void EnablePause() => GameState.Pauseable = true;

        public void DisablePause() => GameState.Pauseable = false;

        public void ShowHudCanvas()
        {
            HudCanvasObj.SetActive(true);
        }

    }

}


