using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyGame.Gameplay.Map
{
    public class PlanetView : MonoBehaviour
    {
        // 自身对象引用
        public GameObject Background;
        public GameObject DefaultFrame;
        public GameObject PlayerFrame;
        public GameObject ViewFrame;
        public GameObject ConqueredIcon;
        public GameObject Core;
        public TextMeshProUGUI WealthText;

        // 颜色设置
        public Color ExploredColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.2f);
        public Color ConqueredColor = new Color(Color.green.r, Color.green.g, Color.green.b, 0.2f);
        public Color HideColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0.6f);
        public Color DestoryColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0.3f);
        public Color FightingColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.4f);
        public Color NotAnalysedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.3f);

        // 视图状态方法
        public void SetHidden()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = HideColor;
            if (WealthText != null) WealthText.gameObject.SetActive(false);
            if (Core != null) Core.SetActive(false);
            if (ConqueredIcon != null) ConqueredIcon.SetActive(false);
            if (PlayerFrame != null) PlayerFrame.SetActive(false);
            if (ViewFrame != null) ViewFrame.SetActive(false);
        }

        public void SetExplored()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = ExploredColor;
            if (WealthText != null) WealthText.gameObject.SetActive(true);
            if (Core != null) Core.SetActive(true);
            if (ConqueredIcon != null) ConqueredIcon.SetActive(false);
        }

        public void SetConquered()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = ConqueredColor;
            if (ConqueredIcon != null) ConqueredIcon.SetActive(true);
            if (WealthText != null) WealthText.gameObject.SetActive(false);
            if (Core != null) Core.SetActive(false);
        }

        public void SetDestroyed()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = DestoryColor;
            if (ViewFrame != null) ViewFrame.SetActive(false);
            if (PlayerFrame != null) PlayerFrame.SetActive(false);
            if (DefaultFrame != null) DefaultFrame.SetActive(false);
            if (ConqueredIcon != null) ConqueredIcon.SetActive(false);
        }

        public void SetFighting()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = FightingColor;
        }

        public void SetNotAnalysed()
        {
            if (Background != null) Background.GetComponent<SpriteRenderer>().color = NotAnalysedColor;
        }

        // 其他视图操作方法
        public void SetWealthText(string text)
        {
            if (WealthText != null) WealthText.text = text;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetPlayerFrameActive(bool active)
        {
            if (PlayerFrame != null) PlayerFrame.SetActive(active);
        }

        public void SetViewFrameActive(bool active)
        {
            if (ViewFrame != null) ViewFrame.SetActive(active);
        }

        public bool IsPlayerFrameActive()
        {
            return PlayerFrame == null ? PlayerFrame.activeSelf : false;
        }

    }
}


