using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyGame.Framework.Utilities
{
    /// <summary>
    /// 根据字符长度动态调节字符大小
    /// </summary>
    public class DynamicTextResizer : MonoBehaviour
    {
        public float paddingX = 1f; // 左右内边距
        public float paddingY = 1f; // 上下内边距

        public float minWidth = 10f;
        public float maxWidth = 600f;

        public float minHeight = 30f;
        public float maxHeight = 1000f;

        private TextMeshProUGUI tmp;
        private RectTransform rect;

        void Awake()
        {
            tmp = transform.GetComponent<TextMeshProUGUI>();
            rect = transform.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 设置文本并根据内容动态调整大小
        /// </summary>
        /// <param name="text"></param>
        public void SetTextAndResize(string text)
        {
            tmp.text = text;

            tmp.ForceMeshUpdate();

            Vector2 preferredSize = tmp.GetPreferredValues(text, maxWidth, maxHeight);

            float newWidth = Mathf.Clamp(preferredSize.x + paddingX, minWidth, maxWidth);
            float newHeight = Mathf.Clamp(preferredSize.y + paddingY, minHeight, maxHeight);

            rect.sizeDelta = new Vector2(newWidth, newHeight);
        }
    }


}
