using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Visuals.Layer
{
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float deltaX, float deltaY);
        public ParallaxCameraDelegate onCameraTranslate;
        public Action<Vector3> loopAction;

        private Vector3 oldPosition;

        void Start()
        {
            oldPosition = transform.position;
        }


        private void LateUpdate()
        {
            if (transform.position != oldPosition)
            {
                if (onCameraTranslate != null)
                {
                    float deltaX = transform.position.x - oldPosition.x;
                    float deltaY = transform.position.y - oldPosition.y;
                    onCameraTranslate(deltaX, deltaY);
                }

                loopAction?.Invoke(transform.position); //处理无限背景循环事件

                oldPosition = transform.position;
            }


        }
    }
}


