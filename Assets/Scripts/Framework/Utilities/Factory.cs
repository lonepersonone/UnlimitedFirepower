using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Utilities
{
    public abstract class Factory
    {
        public abstract GameObject Create(Vector3 position, Quaternion quaternion);
        public abstract GameObject Create(Vector3 position, Quaternion quaternion, Transform parent);
    }

}

