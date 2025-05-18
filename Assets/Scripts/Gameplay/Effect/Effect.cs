using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Effect
{
    public class Effect : MonoBehaviour
    {
        public float lifeTime;
        private float liveTime;

        private void Start()
        {
            liveTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - liveTime > lifeTime) Destroy(this.gameObject);
        }

        public void OnDestroy()
        {
            if (gameObject != null) Destroy(gameObject);
        }

    }
}


