
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Visuals.Layer
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float parallaxFactor;

        private void Start()
        {

        }

        public void Move(float deltaX, float deltaY)
        {
            Vector3 newPos = transform.localPosition;

            newPos.x += deltaX * parallaxFactor;
            newPos.y += deltaY * parallaxFactor;

            //Debug.Log("deltaX:" + deltaX + " deltaY:" + deltaY + " newPos: " + newPos);
            transform.localPosition = newPos;
        }

        public void InfiniteLoop(Vector3 cameralPos)
        {

        }
    }
}


