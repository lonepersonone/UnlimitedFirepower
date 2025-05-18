using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Framework.Manager
{
    public abstract class GameSystemBase : MonoBehaviour
    {
        public abstract Task InitializeAsync(Action<float> onProgress = null);
    }
}


