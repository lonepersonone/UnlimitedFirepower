using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Visuals
{
    public class GraphicsSettingsController : MonoBehaviour
    {
        public Dropdown ResolutionDropdown;
        public HorizontalSelector WindowModeHorizontalSelector;
        public HorizontalSelector FrameRateHorizontalSelector;
        public SwitchManager VsyncSwitchManager;

        private void Start()
        {
            InitializeUI();
        }

        // 初始化UI组件
        private void InitializeUI()
        {
            // 分辨率下拉菜单
            List<string> resolutionOptions = GraphicsSettingsManager.Instance.GetResolutionOptions();
            ResolutionDropdown.ClearOptions();
            ResolutionDropdown.AddOptions(resolutionOptions);
            ResolutionDropdown.SetDropdownIndex(GraphicsSettingsManager.Instance.GetCurrentResolutionIndex());
            ResolutionDropdown.UpdateItemLayout();
            ResolutionDropdown.onValueChanged.AddListener(GraphicsSettingsManager.Instance.SetResolution);

           
            WindowModeHorizontalSelector.defaultIndex = GraphicsSettingsManager.Instance.GetWindowMode();
            WindowModeHorizontalSelector.UpdateSelector();
            WindowModeHorizontalSelector.UpdateUI();
            WindowModeHorizontalSelector.onValueChanged.AddListener(GraphicsSettingsManager.Instance.SetWindowMode);
            WindowModeHorizontalSelector.onValueChanged.AddListener(UpdateVsyncSwitchState);


            FrameRateHorizontalSelector.onValueChanged.AddListener(GraphicsSettingsManager.Instance.SetTargetFrameRate);
            FrameRateHorizontalSelector.defaultIndex = GraphicsSettingsManager.Instance.GetCurrentFrameRate();
            FrameRateHorizontalSelector.UpdateSelector();


            VsyncSwitchManager.isOn = GraphicsSettingsManager.Instance.settings.vsyncEnabled;
            VsyncSwitchManager.UpdateUI();
            VsyncSwitchManager.onValueChanged.AddListener(GraphicsSettingsManager.Instance.SetVSync);
        }

        // 根据窗口更新垂直同步状态
        private void UpdateVsyncSwitchState(int index)
        {
            VsyncSwitchManager.isOn = GraphicsSettingsManager.Instance.settings.vsyncEnabled;
            VsyncSwitchManager.UpdateUI();
        }

    }

}


