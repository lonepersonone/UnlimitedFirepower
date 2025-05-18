using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public interface ISceneTransitionHandler
    {
        string SceneName { get; }
        void SetupEvents();
        void CleanupEvents();
    }
}


