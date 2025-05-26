using Michsky.UI.Reach;
using MyGame.Framework.Event;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MyGame.Framework.Guide
{

    public class GuidePanelController : MonoBehaviour
    {
        [SerializeField] protected string panelId;  // 在Inspector中设置面板ID
        [SerializeField] protected ButtonManager closeButton;
        [SerializeField] protected ButtonManager doNotRemindButton;
        [SerializeField] protected GameObject Panel;
        [SerializeField] protected List<GameObject> AnimateObjs;
        [SerializeField] protected float delay;

        protected bool isReady = false;

        private void Start()
        {
            GameEventManager.RegisterListener(GameEventType.BattleInitial, ShowGuidePanel);
        }

        private void OnDestroy()
        {
            GameEventManager.UnregisterListener(GameEventType.BattleInitial, ShowGuidePanel);
        }

        private void Update()
        {
            if (Panel.activeSelf && isReady) Time.timeScale = 0;
        }

        protected void ShowGuidePanel()
        {
            // 检查是否应该显示此面板
            bool shouldShow = NewPlayerGuideManager.Instance.ShouldShowPanel(panelId);

            // 注册按钮事件
            closeButton.onClick.AddListener(OnCloseButtonClick);
            doNotRemindButton.onClick.AddListener(OnDoNotRemindButtonClick);

            isReady = true;

            if (shouldShow) StartCoroutine(EmergeAnimation());
            else Panel.SetActive(false);
        }

        protected void OnCloseButtonClick()
        {
            Time.timeScale = 1;
            Panel.SetActive(false);
            // 可选：标记为已完成
            NewPlayerGuideManager.Instance.SetGuidePanelState(panelId, GuidePanelState.Show);
        }

        protected void OnDoNotRemindButtonClick()
        {
            Time.timeScale = 1;
            // 设置为不再显示状态
            NewPlayerGuideManager.Instance.SetGuidePanelState(panelId, GuidePanelState.DoNotShow);
            Panel.SetActive(false);
        }

        protected IEnumerator EmergeAnimation()
        {
            yield return new WaitForSecondsRealtime(2f);

            Panel.SetActive(true);

            for (int i = 0; i < AnimateObjs.Count; i++)
            {
                AnimateObjs[i].SetActive(true);

                yield return StartCoroutine(TextAnimation(AnimateObjs[i], delay));
            }
        }

        protected IEnumerator TextAnimation(GameObject obj, float animateTime)
        {
            float time = 0;

            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

            while (time < animateTime)
            {
                float ratio = time / animateTime;
                text.color = new Color(Color.white.r, Color.white.g, Color.white.b, Mathf.Lerp(0, 1, ratio));
                time += Time.fixedDeltaTime;
                yield return null;
            }
        }

    }
}


