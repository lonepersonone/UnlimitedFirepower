using MyGame.Data.SO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace MyGame.UI
{
    public class DynamicTextController : MonoBehaviour
    {
        //文本显示对象
        public TextMeshProUGUI text;
        private RectTransform rect;

        private Color EmergeColor = Color.white;
        private Vector3 EmergePosition = new Vector3(0, 1, 0);
        private Vector3 EmergeScale = new Vector3(1, 1, 1);
        private Vector3 EmergeRotation = new Vector3(1, 1, 1);
        private float EmergeTime = 0.3f;
        private Color HoverColor = Color.white;
        private Vector3 HoverPosition = new Vector3(0, 1, 0);
        private Vector3 HoverScale = new Vector3(1, 1, 1);
        private Vector3 HoverRotation = new Vector3(1, 1, 1);
        private float HoverTime = 0.3f;
        private Color FadeColor = Color.white;
        private Vector3 FadePosition = new Vector3(0, 1, 0);
        private Vector3 FadeScale = new Vector3(1, 1, 1);
        private Vector3 FadeRotation = new Vector3(1, 1, 1);
        private float FadeTime = 0.3f;

        public void Initialize(DynamicTextData data, float value)
        {
            text.text = value.ToString();

            EmergeColor = data.EmergeColor;
            EmergePosition = data.EmergePosition;
            EmergeRotation = data.EmergeRotation;
            EmergeScale = data.EmergeScale;
            EmergeTime = data.EmergeTime;
            HoverColor = data.HoverColor;
            HoverPosition = data.HoverPosition;
            HoverScale = data.HoverScale;
            HoverRotation = data.HoverRotation;
            HoverTime = data.HoverTime;
            FadeColor = data.FadeColor;
            FadePosition = data.FadePosition;
            FadeScale = data.FadeScale;
            FadeRotation = data.FadeRotation;
            FadeTime = data.FadeTime;

            StartCoroutine(EmergeAnimation(EmergeTime));
        }

        private IEnumerator EmergeAnimation(float emergeTime)
        {
            float time = 0;
            Vector3 startPos = text.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + EmergePosition.x, startPos.y + EmergePosition.y, startPos.z + EmergePosition.z);
            Vector3 startScale = text.transform.localScale;
            Quaternion startRotation = text.transform.localRotation;
            Quaternion endRotation = Quaternion.Euler(EmergeRotation.x, EmergeRotation.y, EmergeRotation.z);
            while (time <= emergeTime)
            {
                float ratio = time / emergeTime;
                text.color = new Color(EmergeColor.r, EmergeColor.g, EmergeColor.b, Mathf.Lerp(0, 1, ratio));
                text.transform.localPosition = Vector3.Lerp(startPos, endPos, ratio);
                text.transform.localScale = Vector3.Lerp(startScale, EmergeScale, ratio);
                text.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(HoverAnimation(HoverTime));
        }

        private IEnumerator HoverAnimation(float hoverTime)
        {
            float time = 0;

            Vector3 startPos = text.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + HoverPosition.x, startPos.y + HoverPosition.y, startPos.z + HoverPosition.z);
            Vector3 startScale = text.transform.localScale;
            Quaternion startRotation = text.transform.localRotation;
            Quaternion endRotation = Quaternion.Euler(HoverRotation.x, HoverRotation.y, HoverRotation.z);
            while (time <= hoverTime)
            {
                float ratio = time / hoverTime;
                //text.color = new CurrentColor(HoverColor.r, HoverColor.g, HoverColor.b, Mathf.Lerp(0, 1, ratio));
                text.transform.localPosition = Vector3.Lerp(startPos, endPos, ratio);
                text.transform.localScale = Vector3.Lerp(startScale, HoverScale, ratio);
                text.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(FadeAnimation(FadeTime));
        }

        private IEnumerator FadeAnimation(float fadeTime)
        {
            float time = 0;
            Vector3 startPos = text.transform.localPosition;
            Vector3 endPos = new Vector3(startPos.x + FadePosition.x, startPos.y + FadePosition.y, startPos.z + FadePosition.z);
            Vector3 startScale = text.transform.localScale;
            Quaternion startRotation = text.transform.localRotation;
            Quaternion endRotation = Quaternion.Euler(FadeRotation.x, FadeRotation.y, FadeRotation.z);
            while (time <= fadeTime)
            {
                float ratio = time / fadeTime;
                text.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, Mathf.Lerp(1, 0, ratio));
                text.transform.localPosition = Vector3.Lerp(startPos, endPos, ratio);
                text.transform.localScale = Vector3.Lerp(startScale, FadeScale, ratio);
                text.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            Destroy(this.gameObject);
        }


    }
}



