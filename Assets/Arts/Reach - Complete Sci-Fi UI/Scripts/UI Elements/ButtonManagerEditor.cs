#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Reach
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ButtonManager))]
    public class ButtonManagerEditor : Editor
    {
        private ButtonManager buttonTarget;
        private GUISkin customSkin;

        private void OnEnable()
        {
            buttonTarget = (ButtonManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = ReachUIEditorHandler.GetDarkEditor(customSkin); }
            else { customSkin = ReachUIEditorHandler.GetLightEditor(customSkin); }
        }

        public override void OnInspectorGUI()
        {
            ReachUIEditorHandler.DrawComponentHeader(customSkin, "TopHeader_Button");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            buttonTarget.latestTabIndex = ReachUIEditorHandler.DrawTabs(buttonTarget.latestTabIndex, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab_Content")))
                buttonTarget.latestTabIndex = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab_Resources")))
                buttonTarget.latestTabIndex = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab_Settings")))
                buttonTarget.latestTabIndex = 2;

            GUILayout.EndHorizontal();

            var normalCG = serializedObject.FindProperty("normalCG");
            var highlightCG = serializedObject.FindProperty("highlightCG");
            var disabledCG = serializedObject.FindProperty("disabledCG");
            var normalTextObj = serializedObject.FindProperty("normalTextObj");
            var highlightTextObj = serializedObject.FindProperty("highlightTextObj");
            var disabledTextObj = serializedObject.FindProperty("disabledTextObj");
            var normalImageObj = serializedObject.FindProperty("normalImageObj");
            var highlightImageObj = serializedObject.FindProperty("highlightImageObj");
            var disabledImageObj = serializedObject.FindProperty("disabledImageObj");

            var buttonIcon = serializedObject.FindProperty("buttonIcon");
            var buttonText = serializedObject.FindProperty("buttonText");
            var iconScale = serializedObject.FindProperty("iconScale");
            var textSize = serializedObject.FindProperty("textSize");

            var autoFitContent = serializedObject.FindProperty("autoFitContent");
            var padding = serializedObject.FindProperty("padding");
            var spacing = serializedObject.FindProperty("spacing");
            var disabledLayout = serializedObject.FindProperty("disabledLayout");
            var normalLayout = serializedObject.FindProperty("normalLayout");
            var highlightedLayout = serializedObject.FindProperty("highlightedLayout");
            var mainLayout = serializedObject.FindProperty("mainLayout");
            var mainFitter = serializedObject.FindProperty("mainFitter");
            var targetFitter = serializedObject.FindProperty("targetFitter");
            var targetRect = serializedObject.FindProperty("targetRect");

            var isInteractable = serializedObject.FindProperty("isInteractable");
            var enableIcon = serializedObject.FindProperty("enableIcon");
            var enableText = serializedObject.FindProperty("enableText");
            var useCustomIconSize = serializedObject.FindProperty("useCustomIconSize");
            var useCustomTextSize = serializedObject.FindProperty("useCustomTextSize");
            var useUINavigation = serializedObject.FindProperty("useUINavigation");
            var navigationMode = serializedObject.FindProperty("navigationMode");
            var wrapAround = serializedObject.FindProperty("wrapAround");
            var selectOnUp = serializedObject.FindProperty("selectOnUp");
            var selectOnDown = serializedObject.FindProperty("selectOnDown");
            var selectOnLeft = serializedObject.FindProperty("selectOnLeft");
            var selectOnRight = serializedObject.FindProperty("selectOnRight");
            var checkForDoubleClick = serializedObject.FindProperty("checkForDoubleClick");
            var useLocalization = serializedObject.FindProperty("useLocalization");
            var useSounds = serializedObject.FindProperty("useSounds");
            var doubleClickPeriod = serializedObject.FindProperty("doubleClickPeriod");
            var fadingMultiplier = serializedObject.FindProperty("fadingMultiplier");
            var useCustomContent = serializedObject.FindProperty("useCustomContent");

            var onClick = serializedObject.FindProperty("onClick");
            var onDoubleClick = serializedObject.FindProperty("onDoubleClick");
            var onHover = serializedObject.FindProperty("onHover");
            var onLeave = serializedObject.FindProperty("onLeave");

            switch (buttonTarget.latestTabIndex)
            {
                case 0:
                    ReachUIEditorHandler.DrawHeader(customSkin, "Header_Content", 6);

                    if (useCustomContent.boolValue == false)
                    {
                        if (buttonTarget.normalImageObj != null || buttonTarget.highlightImageObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableIcon.boolValue = ReachUIEditorHandler.DrawTogglePlain(enableIcon.boolValue, customSkin, "Enable Icon");

                            GUILayout.Space(4);

                            if (enableIcon.boolValue == true)
                            {
                                ReachUIEditorHandler.DrawPropertyCW(buttonIcon, customSkin, "Button Icon", 80);
                                ReachUIEditorHandler.DrawPropertyCW(iconScale, customSkin, "Icon Scale", 80);
                                if (enableText.boolValue == true) { ReachUIEditorHandler.DrawPropertyCW(spacing, customSkin, "Spacing", 80); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (buttonTarget.normalTextObj != null || buttonTarget.highlightTextObj != null)
                        {
                            GUILayout.BeginVertical(EditorStyles.helpBox);
                            GUILayout.Space(-3);

                            enableText.boolValue = ReachUIEditorHandler.DrawTogglePlain(enableText.boolValue, customSkin, "Enable Text");

                            GUILayout.Space(4);

                            if (enableText.boolValue == true)
                            {
                                ReachUIEditorHandler.DrawPropertyCW(buttonText, customSkin, "Button Text", 80);
                                if (useCustomTextSize.boolValue == false) { ReachUIEditorHandler.DrawPropertyCW(textSize, customSkin, "Text Size", 80); }
                            }

                            GUILayout.EndVertical();
                        }

                        if (Application.isPlaying == false) { buttonTarget.UpdateUI(); }
                    }

                    else { EditorGUILayout.HelpBox("'Use Custom Content' is enabled. Content is now managed manually.", MessageType.Info); }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    autoFitContent.boolValue = ReachUIEditorHandler.DrawTogglePlain(autoFitContent.boolValue, customSkin, "Auto-Fit Content", "Sets the width based on the button content.");

                    GUILayout.Space(4);

                    if (autoFitContent.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);
                        EditorGUI.indentLevel = 1;
                        EditorGUILayout.PropertyField(padding, new GUIContent(" Padding"), true);
                        EditorGUI.indentLevel = 0;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();

                    isInteractable.boolValue = ReachUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");

                    if (Application.isPlaying == true && GUILayout.Button("Update UI", customSkin.button)) { buttonTarget.UpdateUI(); }

                    ReachUIEditorHandler.DrawHeader(customSkin, "Header_Events", 10);
                    EditorGUILayout.PropertyField(onClick, new GUIContent("On Click"), true);
                    EditorGUILayout.PropertyField(onDoubleClick, new GUIContent("On Double Click"), true);
                    EditorGUILayout.PropertyField(onHover, new GUIContent("On Hover"), true);
                    EditorGUILayout.PropertyField(onLeave, new GUIContent("On Leave"), true);
                    break;

                case 1:
                    ReachUIEditorHandler.DrawHeader(customSkin, "Header_Resources", 6);
                    ReachUIEditorHandler.DrawProperty(normalCG, customSkin, "Normal CG");
                    ReachUIEditorHandler.DrawProperty(highlightCG, customSkin, "Highlight CG");
                    ReachUIEditorHandler.DrawProperty(disabledCG, customSkin, "Disabled CG");

                    if (enableText.boolValue == true)
                    {
                        ReachUIEditorHandler.DrawProperty(normalTextObj, customSkin, "Normal Text");
                        ReachUIEditorHandler.DrawProperty(highlightTextObj, customSkin, "Highlighted Text");
                        ReachUIEditorHandler.DrawProperty(disabledTextObj, customSkin, "Disabled Text");
                    }

                    if (enableIcon.boolValue == true)
                    {
                        ReachUIEditorHandler.DrawProperty(normalImageObj, customSkin, "Normal Icon");
                        ReachUIEditorHandler.DrawProperty(highlightImageObj, customSkin, "Highlight Icon");
                        ReachUIEditorHandler.DrawProperty(disabledImageObj, customSkin, "Disabled Icon");
                    }

                    ReachUIEditorHandler.DrawProperty(disabledLayout, customSkin, "Disabled Layout");
                    ReachUIEditorHandler.DrawProperty(normalLayout, customSkin, "Normal Layout");
                    ReachUIEditorHandler.DrawProperty(highlightedLayout, customSkin, "Highlighted Layout");
                    ReachUIEditorHandler.DrawProperty(mainLayout, customSkin, "Main Layout");

                    if (autoFitContent.boolValue == true)
                    {
                        ReachUIEditorHandler.DrawProperty(mainFitter, customSkin, "Main Fitter");
                        ReachUIEditorHandler.DrawProperty(targetFitter, customSkin, "mappingObject Fitter");
                        ReachUIEditorHandler.DrawProperty(targetRect, customSkin, "mappingObject Rect");
                    }
                    break;

                case 2:
                    ReachUIEditorHandler.DrawHeader(customSkin, "Header_Settings", 6);
                    ReachUIEditorHandler.DrawProperty(fadingMultiplier, customSkin, "Fading Multiplier", "Set the animation fade multiplier.");
                    ReachUIEditorHandler.DrawProperty(doubleClickPeriod, customSkin, "Double Click Period");
                    isInteractable.boolValue = ReachUIEditorHandler.DrawToggle(isInteractable.boolValue, customSkin, "Is Interactable");
                    useCustomContent.boolValue = ReachUIEditorHandler.DrawToggle(useCustomContent.boolValue, customSkin, "Use Custom Content", "Bypasses inspector values and allows manual editing.");
                    if (useCustomContent.boolValue == true || enableText.boolValue == false) { GUI.enabled = false; }
                    useCustomTextSize.boolValue = ReachUIEditorHandler.DrawToggle(useCustomTextSize.boolValue, customSkin, "Use Custom Text Size");
                    useLocalization.boolValue = ReachUIEditorHandler.DrawToggle(useLocalization.boolValue, customSkin, "Use Localization", "Bypasses localization functions when disabled.");
                    GUI.enabled = true;
                    checkForDoubleClick.boolValue = ReachUIEditorHandler.DrawToggle(checkForDoubleClick.boolValue, customSkin, "Check For Double Click");
                    useSounds.boolValue = ReachUIEditorHandler.DrawToggle(useSounds.boolValue, customSkin, "Use Button Sounds");
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);

                    useUINavigation.boolValue = ReachUIEditorHandler.DrawTogglePlain(useUINavigation.boolValue, customSkin, "Use UI Navigation", "Enables controller navigation.");

                    GUILayout.Space(4);

                    if (useUINavigation.boolValue == true)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        ReachUIEditorHandler.DrawPropertyPlain(navigationMode, customSkin, "Navigation Mode");

                        if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Horizontal)
                        {
                            EditorGUI.indentLevel = 1;
                            wrapAround.boolValue = ReachUIEditorHandler.DrawToggle(wrapAround.boolValue, customSkin, "Wrap Around");
                            EditorGUI.indentLevel = 0;
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Vertical)
                        {
                            wrapAround.boolValue = ReachUIEditorHandler.DrawTogglePlain(wrapAround.boolValue, customSkin, "Wrap Around");
                        }

                        else if (buttonTarget.navigationMode == UnityEngine.UI.Navigation.Mode.Explicit)
                        {
                            EditorGUI.indentLevel = 1;
                            ReachUIEditorHandler.DrawPropertyPlain(selectOnUp, customSkin, "Select On Up");
                            ReachUIEditorHandler.DrawPropertyPlain(selectOnDown, customSkin, "Select On Down");
                            ReachUIEditorHandler.DrawPropertyPlain(selectOnLeft, customSkin, "Select On Left");
                            ReachUIEditorHandler.DrawPropertyPlain(selectOnRight, customSkin, "Select On Right");
                            EditorGUI.indentLevel = 0;
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.EndVertical();
                    buttonTarget.UpdateUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
            if (Application.isPlaying == false) { Repaint(); }
        }
    }
}
#endif