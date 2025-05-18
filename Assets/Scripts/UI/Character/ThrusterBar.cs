using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
    public class ThrusterBar : MonoBehaviour
    {
        private Image image;

        public float currentValue;
        public float minValue = 0;
        public float maxValue = 2;
        public float minValueLimit = 0;
        public float maxValueLimit = 10;

        private void Start()
        {
            image = transform.Find("Image").GetComponent<Image>();
        }

        public void SetAmount(float amount)
        {
            currentValue = amount;
            UpdateUI();
        }

        public void SetRange(float min, float max)
        {
            minValue = min;
            if (maxValue > 0) maxValue = max;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (image != null) { image.fillAmount = currentValue / maxValue; }
        }
    }
}


