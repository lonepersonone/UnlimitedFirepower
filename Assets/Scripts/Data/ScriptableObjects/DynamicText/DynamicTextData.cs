using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [System.Serializable]
    public class DynamicTextData
    {
        [Header("Emerge Phase")]
        public Color EmergeColor = Color.white;
        public Vector3 EmergePosition = new Vector3(0, 3, 0);
        public Vector3 EmergeScale = new Vector3(1, 1, 1);
        public Vector3 EmergeRotation = new Vector3(0, 0, 0);
        public float EmergeTime = 0.5f;

        [Header("Hover Phase")]
        public Color HoverColor = Color.white;
        public Vector3 HoverPosition = new Vector3(0, 2, 0);
        public Vector3 HoverScale = new Vector3(1, 1, 1);
        public Vector3 HoverRotation = new Vector3(0, 0, 0);
        public float HoverTime = 0.5f;

        [Header("Fade Phase")]
        public Color FadeColor = Color.white;
        public Vector3 FadePosition = new Vector3(0, 3, 0);
        public Vector3 FadeScale = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 FadeRotation = new Vector3(0, 0, 0);
        public float FadeTime = 0.5f;
    }
}


