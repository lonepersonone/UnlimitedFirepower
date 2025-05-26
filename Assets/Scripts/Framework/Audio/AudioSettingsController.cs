using Michsky.UI.Reach;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{
    public class AudioSettingsController : MonoBehaviour
    {
        [SerializeField] private SliderManager masterVolumeSlider;
        [SerializeField] private SliderManager musicVolumeSlider;
        [SerializeField] private SliderManager sfxVolumeSlider;
        [SerializeField] private SliderManager uiVolumeSlider;

        // 音量组名称常量（与AudioSystem保持一致）
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SFXVolume";
        private const string UI_VOLUME = "UIVolume";

        private void Start()
        {
            // 初始化滑块值
            masterVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(MASTER_VOLUME);
            musicVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(MUSIC_VOLUME);
            sfxVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(SFX_VOLUME);
            uiVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(UI_VOLUME);

            // 添加事件监听
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            uiVolumeSlider.onValueChanged.AddListener(SetUIVolume);
        }

        // 设置主音量
        private void SetMasterVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(MASTER_VOLUME, volume);
        }

        // 设置音乐音量
        private void SetMusicVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(MUSIC_VOLUME, volume);
        }

        // 设置音效音量
        private void SetSFXVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(SFX_VOLUME, volume);
        }

        // 设置UI音量
        private void SetUIVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(UI_VOLUME, volume);
        }

        // 重置所有音量为默认值
        public void ResetToDefaults()
        {
            AudioSystem.Instance.SetVolume(MASTER_VOLUME, 1f);
            AudioSystem.Instance.SetVolume(MUSIC_VOLUME, 0.7f);
            AudioSystem.Instance.SetVolume(SFX_VOLUME, 0.8f);
            AudioSystem.Instance.SetVolume(UI_VOLUME, 0.9f);

            // 更新滑块显示
            masterVolumeSlider.mainSlider.value = 1f;
            musicVolumeSlider.mainSlider.value = 0.7f;
            sfxVolumeSlider.mainSlider.value = 0.8f;
            uiVolumeSlider.mainSlider.value = 0.9f;
        }
    }
}


