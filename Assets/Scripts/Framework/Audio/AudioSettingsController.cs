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

        // ���������Ƴ�������AudioSystem����һ�£�
        private const string MASTER_VOLUME = "MasterVolume";
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string SFX_VOLUME = "SFXVolume";
        private const string UI_VOLUME = "UIVolume";

        private void Start()
        {
            // ��ʼ������ֵ
            masterVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(MASTER_VOLUME);
            musicVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(MUSIC_VOLUME);
            sfxVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(SFX_VOLUME);
            uiVolumeSlider.mainSlider.value = AudioSystem.Instance.GetVolume(UI_VOLUME);

            // ����¼�����
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            uiVolumeSlider.onValueChanged.AddListener(SetUIVolume);
        }

        // ����������
        private void SetMasterVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(MASTER_VOLUME, volume);
        }

        // ������������
        private void SetMusicVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(MUSIC_VOLUME, volume);
        }

        // ������Ч����
        private void SetSFXVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(SFX_VOLUME, volume);
        }

        // ����UI����
        private void SetUIVolume(float volume)
        {
            AudioSystem.Instance.SetVolume(UI_VOLUME, volume);
        }

        // ������������ΪĬ��ֵ
        public void ResetToDefaults()
        {
            AudioSystem.Instance.SetVolume(MASTER_VOLUME, 1f);
            AudioSystem.Instance.SetVolume(MUSIC_VOLUME, 0.7f);
            AudioSystem.Instance.SetVolume(SFX_VOLUME, 0.8f);
            AudioSystem.Instance.SetVolume(UI_VOLUME, 0.9f);

            // ���»�����ʾ
            masterVolumeSlider.mainSlider.value = 1f;
            musicVolumeSlider.mainSlider.value = 0.7f;
            sfxVolumeSlider.mainSlider.value = 0.8f;
            uiVolumeSlider.mainSlider.value = 0.9f;
        }
    }
}


