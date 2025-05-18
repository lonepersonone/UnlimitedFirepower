using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace MyGame.Framework.Audio
{

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("音频配置")]
        [SerializeField] private AudioConfig audioConfig;

        [Header("音频混合器")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("音频源")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private AudioSource ambientSource;

        [Header("音量设置")]
        [Range(0f, 1f)][SerializeField] private float masterVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float sfxVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float uiVolume = 1f;
        [Range(0f, 1f)][SerializeField] private float ambientVolume = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // 初始化音频源
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
            if (uiSource == null) uiSource = gameObject.AddComponent<AudioSource>();
            if (ambientSource == null) ambientSource = gameObject.AddComponent<AudioSource>();

            // 配置音频源
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
            uiSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("UI")[0];
            ambientSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambient")[0];

            musicSource.spatialBlend = 0;
            sfxSource.spatialBlend = 0;
            uiSource.spatialBlend = 0;
            ambientSource.spatialBlend = 0;

            // 应用音量设置
            ApplyVolumeSettings();
        }

        private void ApplyVolumeSettings()
        {
            musicSource.volume = musicVolume;
            sfxSource.volume = sfxVolume;
            uiSource.volume = uiVolume;
            ambientSource.volume = ambientVolume;
        }

        #region 背景音乐控制

        public void PlayMainMenuMusic() => PlayMusic(audioConfig.mainMenuBGM);
        public void PlayGameplayMusic() => PlayMusic(audioConfig.gameplayBGM);
        public void PlayBossBattleMusic() => PlayMusic(audioConfig.bossBattleBGM);
        public void PlayVictoryMusic() => PlayMusic(audioConfig.victoryBGM);
        public void PlayDefeatMusic() => PlayMusic(audioConfig.defeatBGM);
        public void PlaySpaceAmbient() => PlayAmbient(audioConfig.spaceAmbient);

        private void PlayMusic(AudioClip clip, bool loop = true, float fadeDuration = 0.5f)
        {
            if (clip == null || musicSource.clip == clip) return;

            if (fadeDuration > 0 && musicSource.isPlaying)
            {
                StartCoroutine(FadeMusic(clip, loop, fadeDuration));
            }
            else
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.Play();
            }
        }

        private void PlayAmbient(AudioClip clip, bool loop = true)
        {
            if (clip == null || ambientSource.clip == clip) return;

            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.Play();
        }
        #endregion

        #region 玩家飞船音效
        public void PlayShipEngine() => PlaySFX(audioConfig.shipEngine, true);
        public void PlayShipThrust() => PlaySFX(audioConfig.shipThrust);
        public void PlayShipLaserFire() => PlaySFX(audioConfig.shipLaserFire);
        public void PlayShipMissileFire() => PlaySFX(audioConfig.shipMissileFire);
        public void PlayShipShieldUp() => PlaySFX(audioConfig.shipShieldUp);
        public void PlayShipShieldDown() => PlaySFX(audioConfig.shipShieldDown);
        public void PlayShipShieldHit() => PlaySFX(audioConfig.shipShieldHit);
        public void PlayShipHit() => PlaySFX(audioConfig.shipHit);
        public void PlayShipExplosion() => PlaySFX(audioConfig.shipExplosion);
        #endregion

        #region 敌人音效
        public void PlayEnemyBasicFire() => PlaySFX(audioConfig.enemyBasicFire);
        public void PlayEnemyBossFire() => PlaySFX(audioConfig.enemyBossFire);
        public void PlayEnemyHit() => PlaySFX(audioConfig.enemyHit);
        public void PlayEnemyExplosion() => PlaySFX(audioConfig.enemyExplosion);
        public void PlayEnemySpawn() => PlaySFX(audioConfig.enemySpawn);
        public void PlayBossSpawn() => PlaySFX(audioConfig.bossSpawn);
        public void PlayBossRoar() => PlaySFX(audioConfig.bossRoar);
        #endregion

        #region 道具与升级音效
        public void PlayPowerupCollect() => PlaySFX(audioConfig.powerupCollect);
        public void PlayWeaponUpgrade() => PlaySFX(audioConfig.weaponUpgrade);
        public void PlayShieldUpgrade() => PlaySFX(audioConfig.shieldUpgrade);
        public void PlayEngineUpgrade() => PlaySFX(audioConfig.engineUpgrade);
        public void PlayHealthRestore() => PlaySFX(audioConfig.healthRestore);
        #endregion

        #region UI音效
        public void PlayButtonClick() => uiSource.PlayOneShot(audioConfig.buttonClick);
        public void PlayButtonHover() => uiSource.PlayOneShot(audioConfig.buttonHover);
        public void PlayMenuOpen() => uiSource.PlayOneShot(audioConfig.menuOpen);
        public void PlayMenuClose() => uiSource.PlayOneShot(audioConfig.menuClose);
        public void PlayNotification() => uiSource.PlayOneShot(audioConfig.notification);
        public void PlayLevelComplete() => uiSource.PlayOneShot(audioConfig.levelComplete);
        #endregion

        #region 环境与特效音效
        public void PlayAsteroidImpact() => PlaySFX(audioConfig.asteroidImpact);
        public void PlayBlackHoleGravity() => PlaySFX(audioConfig.blackHoleGravity);
        public void PlayWormholeEnter() => PlaySFX(audioConfig.wormholeEnter);
        public void PlayWormholeExit() => PlaySFX(audioConfig.wormholeExit);
        public void PlayLargeExplosion() => PlaySFX(audioConfig.explosionLarge);
        public void PlayMediumExplosion() => PlaySFX(audioConfig.explosionMedium);
        public void PlaySmallExplosion() => PlaySFX(audioConfig.explosionSmall);
        #endregion

        #region 武器音效
        public void PlayRandomLaserSound()
        {
            if (audioConfig.laserSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, audioConfig.laserSounds.Length);
                PlaySFX(audioConfig.laserSounds[randomIndex]);
            }
        }

        public void PlayPlayerLaserSound() => PlaySFX(audioConfig.laserSounds[0]);

        public void PlayUAVLaserSound() => PlaySFX(audioConfig.laserSounds[1]);

        public void PlayRandomMissileSound()
        {
            if (audioConfig.missileSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, audioConfig.missileSounds.Length);
                PlaySFX(audioConfig.missileSounds[randomIndex]);
            }
        }

        public void PlayRailgunFire() => PlaySFX(audioConfig.railgunFire);
        public void PlayPlasmaFire() => PlaySFX(audioConfig.plasmaFire);
        public void PlayEMPEffect() => PlaySFX(audioConfig.EMPEffect);
        #endregion

        #region 辅助方法
        private void PlaySFX(AudioClip clip, bool loop = false)
        {
            if (clip == null) return;

            if (loop)
            {
                sfxSource.clip = clip;
                sfxSource.loop = true;
                sfxSource.Play();
            }
            else
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        private IEnumerator FadeMusic(AudioClip newClip, bool loop, float duration)
        {
            float startVolume = musicSource.volume;
            float targetVolume = 0f;
            float elapsed = 0f;

            // 淡出当前音乐
            while (elapsed < duration)
            {
                musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            musicSource.Stop();
            musicSource.clip = newClip;
            musicSource.loop = loop;
            musicSource.volume = startVolume;
            musicSource.Play();
        }


        #endregion
    }

}

