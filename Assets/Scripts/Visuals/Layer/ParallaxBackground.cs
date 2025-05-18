using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Visuals.Layer
{
    public class ParallaxBackground : MonoBehaviour
    {
        public ParallaxCamera parallaxCamera;
        List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

        void Start()
        {
            SetLayers();
            parallaxCamera.onCameraTranslate += Move;
            parallaxCamera.loopAction += Loop;
        }

        private void Update()
        {

        }

        void SetLayers()
        {
            parallaxLayers.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

                if (layer != null)
                {
                    layer.name = "Layer-" + i;
                    parallaxLayers.Add(layer);
                }
            }
        }

        public void Move(float deltaX, float deltaY)
        {
            foreach (ParallaxLayer layer in parallaxLayers)
            {
                layer.Move(deltaX, deltaY);
            }
        }

        public void Loop(Vector3 cameralPos)
        {
            foreach (ParallaxLayer layer in parallaxLayers)
            {
                layer.InfiniteLoop(cameralPos);
            }
        }
    }
}


