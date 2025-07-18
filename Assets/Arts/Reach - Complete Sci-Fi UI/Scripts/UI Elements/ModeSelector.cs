using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.Reach
{
    [DisallowMultipleComponent]
    public class ModeSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        // Content
        public int currentModeIndex = 0;
        public List<Item> items = new List<Item>();
        public string headerTitle = "Selected Mode";
        public string headerTitleKey = "";

        // Resources
        [SerializeField] private UIPopup modeSelectPopup;
        [SerializeField] private ImageFading transitionPanel;
        [SerializeField] private Transform itemParent;
        [SerializeField] private ButtonManager itemPreset;
        [SerializeField] private CanvasGroup normalCG;
        [SerializeField] private CanvasGroup highlightCG;
        [SerializeField] private CanvasGroup disabledCG;
        [SerializeField] private TextMeshProUGUI disabledHeaderObj;
        [SerializeField] private TextMeshProUGUI normalHeaderObj;
        [SerializeField] private TextMeshProUGUI highlightHeaderObj;
        [SerializeField] private TextMeshProUGUI disabledTextObj;
        [SerializeField] private TextMeshProUGUI normalTextObj;
        [SerializeField] private TextMeshProUGUI highlightTextObj;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image disabledIconObj;
        [SerializeField] private Image normalIconObj;
        [SerializeField] private Image highlightIconObj;

        // Settings
        public bool isInteractable = true;
        public bool useLocalization = true;
        public bool useUINavigation = false;
        public Navigation.Mode navigationMode = Navigation.Mode.Automatic;
        public GameObject selectOnUp;
        public GameObject selectOnDown;
        public GameObject selectOnLeft;
        public GameObject selectOnRight;
        public bool wrapAround = false;
        public bool useSounds = true;
        [Range(1, 15)] public float fadingMultiplier = 8;

        // Events
        public UnityEvent onClick = new UnityEvent();
        public UnityEvent onHover = new UnityEvent();
        public UnityEvent onLeave = new UnityEvent();
        public UnityEvent onSelect = new UnityEvent();
        public UnityEvent onDeselect = new UnityEvent();

        // Helpers
        bool isInitialized = false;
        Button targetButton;
        LocalizedObject localizedObject;
        string tempTitleOutput;

        [System.Serializable]
        public class Item
        {
            public string title = "Mode TitleCN";
            public string titleKey = "";
            public Sprite icon;
            public Sprite background;
            public UnityEvent onModeSelection = new UnityEvent();
        }

        void OnEnable()
        {
            if (!isInitialized)
                Initialize();

            if (useLocalization && !string.IsNullOrEmpty(headerTitleKey))
            {
                LocalizedObject tempLoc = disabledHeaderObj.GetComponent<LocalizedObject>();
                if (tempLoc != null) { tempLoc.localizationKey = headerTitleKey; tempLoc.UpdateItem(); }

                tempLoc = normalHeaderObj.GetComponent<LocalizedObject>();
                if (tempLoc != null) { tempLoc.localizationKey = headerTitleKey; tempLoc.UpdateItem(); }

                tempLoc = highlightHeaderObj.GetComponent<LocalizedObject>();
                if (tempLoc != null) { tempLoc.localizationKey = headerTitleKey; tempLoc.UpdateItem(); }
            }

            else 
            {
                disabledHeaderObj.text = headerTitle;
                normalHeaderObj.text = headerTitle;
                highlightHeaderObj.text = headerTitle;
            }

            normalCG.alpha = 1;
            highlightCG.alpha = 0;
            disabledCG.alpha = 0;

            if (useLocalization == true && !string.IsNullOrEmpty(items[currentModeIndex].titleKey)) 
            { 
                tempTitleOutput = localizedObject.GetKeyOutput(items[currentModeIndex].titleKey);
                SelectMode(currentModeIndex, false);
            }
        }

        public void Initialize()
        {
            if (items.Count == 0)
                return;

            #region Core Init
            if (ControllerManager.instance != null) { ControllerManager.instance.modeSelectors.Add(this); }
            if (UIManagerAudio.instance == null) { useSounds = false; }
            if (useUINavigation) { AddUINavigation(); }
            if (normalCG == null) { normalCG = new GameObject().AddComponent<CanvasGroup>(); normalCG.gameObject.AddComponent<RectTransform>(); normalCG.transform.SetParent(transform); normalCG.gameObject.name = "Normal"; }
            if (highlightCG == null) { highlightCG = new GameObject().AddComponent<CanvasGroup>(); highlightCG.gameObject.AddComponent<RectTransform>(); highlightCG.transform.SetParent(transform); highlightCG.gameObject.name = "Highlight"; }
            if (disabledCG == null) { disabledCG = new GameObject().AddComponent<CanvasGroup>(); disabledCG.gameObject.AddComponent<RectTransform>(); disabledCG.transform.SetParent(transform); disabledCG.gameObject.name = "Disabled"; }
            if (gameObject.GetComponent<Image>() == null)
            {
                Image raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }
            if (useLocalization)
            {
                localizedObject = gameObject.GetComponent<LocalizedObject>();
                if (localizedObject == null || !localizedObject.CheckLocalizationStatus()) { useLocalization = false; }
            }
            #endregion

            #region Mode Init
            foreach (Transform child in itemParent) { Destroy(child.gameObject); }
            for (int i = 0; i < items.Count; ++i)
            {
                int tempIndex = i;

                GameObject itemGO = Instantiate(itemPreset.gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                itemGO.transform.SetParent(itemParent, false);
                itemGO.gameObject.name = items[i].title;

                ButtonManager itemButton = itemGO.GetComponent<ButtonManager>();
                itemButton.buttonIcon = items[i].icon;

                if (!useLocalization || string.IsNullOrEmpty(items[tempIndex].titleKey)) { itemButton.buttonText = items[i].title; }
                else
                {
                    LocalizedObject tempLoc = itemButton.GetComponent<LocalizedObject>();
                    if (tempLoc != null)
                    {
                        tempLoc.tableIndex = localizedObject.tableIndex;
                        tempLoc.localizationKey = items[i].titleKey;
                        tempLoc.onLanguageChanged.AddListener(delegate
                        {
                            itemButton.buttonText = tempLoc.GetKeyOutput(tempLoc.localizationKey);
                            itemButton.UpdateUI();
                        });
                        tempLoc.InitializeItem();
                        tempLoc.UpdateItem();
                    }
                }

                itemButton.onClick.AddListener(delegate 
                {
                    if (!string.IsNullOrEmpty(items[tempIndex].titleKey) && useLocalization) { tempTitleOutput = localizedObject.GetKeyOutput(items[tempIndex].titleKey); }
                    SelectMode(tempIndex, false);
                });

                itemButton.UpdateUI();

                if (tempIndex == currentModeIndex && !string.IsNullOrEmpty(items[tempIndex].titleKey) && useLocalization)
                {
                    tempTitleOutput = localizedObject.GetKeyOutput(items[tempIndex].titleKey);
                }
            }

            onClick.AddListener(delegate { modeSelectPopup.Animate(); });
            items[currentModeIndex].onModeSelection.Invoke();
            modeSelectPopup.gameObject.SetActive(false);
            ApplyModeData(currentModeIndex);
            #endregion

            isInitialized = true;
        }

        public void SelectMode(int index)
        {
            if (currentModeIndex == index) { return; }
            if (useLocalization && !string.IsNullOrEmpty(items[index].titleKey)) { tempTitleOutput = localizedObject.GetKeyOutput(items[index].titleKey); }

            SelectMode(index, false);
        }

        public void SelectMode(int index, bool applyData)
        {
            if (transitionPanel == null) { ApplyModeData(index); }
            else
            {
                transitionPanel.onFadeInEnd.RemoveAllListeners();
                transitionPanel.onFadeInEnd.AddListener(delegate { ApplyModeData(index); });
                transitionPanel.FadeIn();
            }

            currentModeIndex = index;
            items[index].onModeSelection.Invoke();
            modeSelectPopup.PlayOut();

            // Apply mod mainData if enabled
            if (applyData) { ApplyModeData(index); }
        }

        void ApplyModeData(int index)
        {
            backgroundImage.sprite = items[index].background;
            disabledIconObj.sprite = items[index].icon;
            normalIconObj.sprite = items[index].icon;
            highlightIconObj.sprite = items[index].icon;

            if (string.IsNullOrEmpty(tempTitleOutput))
            {
                disabledTextObj.text = items[index].title;
                normalTextObj.text = items[index].title;
                highlightTextObj.text = items[index].title;
            }

            else
            {
                disabledTextObj.text = tempTitleOutput;
                normalTextObj.text = tempTitleOutput;
                highlightTextObj.text = tempTitleOutput;
            }
        }

        public void UpdateState()
        {
            if (!Application.isPlaying || !gameObject.activeInHierarchy)
                return;

            if (!modeSelectPopup.isOn) { modeSelectPopup.PlayOut(); }
            else { modeSelectPopup.PlayIn(); }

            if (!isInteractable) { StartCoroutine("SetDisabled"); }
            else { StartCoroutine("SetNormal"); }
        }

        public void Interactable(bool value)
        {
            isInteractable = value;

            if (value == false) { modeSelectPopup.PlayOut(); }
            if (!gameObject.activeInHierarchy) { return; }
            if (!isInteractable) { StartCoroutine("SetDisabled"); }
            else if (isInteractable && disabledCG.alpha == 1) { StartCoroutine("SetNormal"); }
        }

        public void AddUINavigation()
        {
            if (targetButton == null)
            {
                targetButton = gameObject.AddComponent<Button>();
                targetButton.transition = Selectable.Transition.None;
            }

            Navigation customNav = new Navigation();
            customNav.mode = navigationMode;

            if (navigationMode == Navigation.Mode.Vertical || navigationMode == Navigation.Mode.Horizontal) { customNav.wrapAround = wrapAround; }
            else if (navigationMode == Navigation.Mode.Explicit) { StartCoroutine("InitUINavigation", customNav); return; }

            targetButton.navigation = customNav;
        }

        public void DisableUINavigation()
        {
            if (targetButton != null)
            {
                Navigation customNav = new Navigation();
                Navigation.Mode navMode = Navigation.Mode.None;
                customNav.mode = navMode;
                targetButton.navigation = customNav;
            }
        }

        public void InvokeOnClick() { onClick.Invoke(); }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isInteractable || eventData.button != PointerEventData.InputButton.Left) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }

            // Invoke click actions
            onClick.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound); }

            StartCoroutine("SetHighlight");
            onHover.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isInteractable)
                return;

            StartCoroutine("SetNormal");
            onLeave.Invoke();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound); }

            StartCoroutine("SetHighlight");
            onSelect.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!isInteractable)
                return;

            StartCoroutine("SetNormal");
            onDeselect.Invoke();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (!isInteractable) { return; }
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }
            if (EventSystem.current.currentSelectedGameObject != gameObject) { StartCoroutine("SetNormal"); }

            onClick.Invoke();
        }

        IEnumerator SetNormal()
        {
            StopCoroutine("SetHighlight");
            StopCoroutine("SetDisabled");

            while (normalCG.alpha < 0.99f)
            {
                normalCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                highlightCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                disabledCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            normalCG.alpha = 1;
            highlightCG.alpha = 0;
            disabledCG.alpha = 0;
        }

        IEnumerator SetHighlight()
        {
            StopCoroutine("SetNormal");
            StopCoroutine("SetDisabled");

            while (highlightCG.alpha < 0.99f)
            {
                normalCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                highlightCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                disabledCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            normalCG.alpha = 0;
            highlightCG.alpha = 1;
            disabledCG.alpha = 0;
        }

        IEnumerator SetDisabled()
        {
            StopCoroutine("SetNormal");
            StopCoroutine("SetHighlight");

            while (disabledCG.alpha < 0.99f)
            {
                normalCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                highlightCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                disabledCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            normalCG.alpha = 0;
            highlightCG.alpha = 0;
            disabledCG.alpha = 1;
        }

        IEnumerator InitUINavigation(Navigation nav)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            if (selectOnUp != null) { nav.selectOnUp = selectOnUp.GetComponent<Selectable>(); }
            if (selectOnDown != null) { nav.selectOnDown = selectOnDown.GetComponent<Selectable>(); }
            if (selectOnLeft != null) { nav.selectOnLeft = selectOnLeft.GetComponent<Selectable>(); }
            if (selectOnRight != null) { nav.selectOnRight = selectOnRight.GetComponent<Selectable>(); }
            
            targetButton.navigation = nav;
        }
    }
}