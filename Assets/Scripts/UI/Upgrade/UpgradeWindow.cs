using Michsky.UI.Reach;
using MyGame.Framework.Utilities;
using MyGame.Gameplay.Upgrade;
using MyGame.Scene.BattleRoom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame.UI
{
    public class UpgradeWindow : MonoBehaviour
    {
        public TextMeshProUGUI wealth;
        public RectTransform selected;
        public TextMeshProUGUI selectedDesc;
        public LocalizedObject loDesc;

        private RectTransform currentRect;
        private RectTransform lastRect;

        private List<UpgradeItem> selectedUpgrades;
        private Dictionary<RectTransform, UpgradeItem> selectedDict = new Dictionary<RectTransform, UpgradeItem>();

        private UpgradeAttribute upgradeAttribute;

        private void Start()
        {
            ResetState();
        }

        private void Update()
        {
            UpdateCursorItem();
            UpdateUI();
        }

        private void OnEnable()
        {

        }

        private void ResetState()
        {
            wealth.text = BattleDataManager.Instance.PlayerWealth.CurrentWealth.ToString();
            upgradeAttribute = BattleDataManager.Instance.UpgradeAttribute;
            selectedUpgrades = upgradeAttribute.SelectedItems;
            SetSelectedIcons();
        }

        private void UpdateUI()
        {
            wealth.text = BattleDataManager.Instance.PlayerWealth.CurrentWealth.ToString();
            upgradeAttribute = BattleDataManager.Instance.UpgradeAttribute;
            selectedUpgrades = upgradeAttribute.SelectedItems;
            SetSelectedIcons();
        }

        private void UpdateDesc(UpgradeItem item)
        {
            selectedDesc.text = $"{loDesc.GetKeyOutput($"{item.Id}Title")} +{item.TotalValue}";
        }

        private void SetSelectedIcons()
        {
            float startWidth = 60;
            float startHeight = -80;
            float space = 55;
            int raw = 24;
            for (int i = 0; i < selectedUpgrades.Count; i++)
            {
                if (!selectedDict.ContainsValue(selectedUpgrades[i]))
                {
                    GameObject instance = new GameObject($"SelectedIcon{i}");
                    instance.transform.SetParent(transform.Find("Selected"));
                    UpgradeItem item = selectedUpgrades[i];
                    instance.AddComponent<CanvasRenderer>();
                    Image image = instance.AddComponent<Image>();
                    image.sprite = item.Icon;
                    image.rectTransform.sizeDelta = new Vector2(45, 45);
                    image.rectTransform.anchorMin = new Vector2(0, 1);
                    image.rectTransform.anchorMax = new Vector2(0, 1);

                    selectedDict[image.rectTransform] = item;

                    if (i % 2 == 0) instance.transform.localPosition = new Vector3(startWidth, startHeight - space * (i % raw), 0);
                    else instance.transform.localPosition = new Vector3(startWidth + space, startHeight - space * (i % raw), 0);
                    if ((i + 1) % raw == 0 && i != 0)
                    {
                        startWidth += space * 2;
                        startHeight = -50f;
                    }
                }

            }
        }

        private void UpdateCursorItem()
        {
            if (EventSystemUtil.GetMosueOverUI(transform.parent.gameObject) != null)
            {
                currentRect = EventSystemUtil.GetMosueOverUI(transform.parent.gameObject).GetComponent<RectTransform>();

                if (lastRect != currentRect && lastRect != null)
                {
                    if (lastRect.name.Contains("Item"))
                    {
                        StartCoroutine(ShrinkAnimation(lastRect, 1f));

                    }

                }
                if (currentRect.name.Contains("SelectedIcon"))
                {
                    Debug.Log("Slected");
                    if (!selectedDesc.gameObject.activeSelf)
                        selectedDesc.gameObject.SetActive(true);
                    selectedDesc.transform.localPosition = new Vector3(currentRect.localPosition.x + 200, currentRect.localPosition.y, 0);
                    UpdateDesc(selectedDict[currentRect]);
                }
                else
                {
                    if (selectedDesc.gameObject.activeSelf)
                        selectedDesc.gameObject.SetActive(false);
                }

                lastRect = currentRect;
                Debug.Log(currentRect.name);
            }
        }

        private IEnumerator ShrinkAnimation(Transform rect, float animateTime)
        {
            float time = 0;
            Vector3 startScale = rect.localScale;
            Vector3 endScale = Vector3.one;
            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                rect.localScale = Vector3.Lerp(startScale, endScale, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
        }

        private IEnumerator TopFloatAnimation(Transform rect, float animateTime)
        {
            float time = 0;
            Vector3 startPos = rect.localPosition;
            Vector3 topPos = new Vector3(rect.localPosition.x, rect.localPosition.y + 30, 0);
            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                rect.localPosition = Vector3.Lerp(startPos, topPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            time = 0;
            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                rect.localPosition = Vector3.Lerp(topPos, startPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(BottomFloatAnimation(rect, animateTime));
        }

        private IEnumerator BottomFloatAnimation(Transform rect, float animateTime)
        {
            float time = 0;
            Vector3 startPos = rect.localPosition;
            Vector3 bottomPos = new Vector3(rect.localPosition.x, rect.localPosition.y - 30, 0);
            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                rect.localPosition = Vector3.Lerp(startPos, bottomPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            time = 0;
            while (time <= animateTime)
            {
                float ratio = time / animateTime;
                rect.localPosition = Vector3.Lerp(bottomPos, startPos, ratio);
                time += Time.fixedDeltaTime;
                yield return null;
            }
            StartCoroutine(TopFloatAnimation(rect, animateTime));
        }

    }
}


