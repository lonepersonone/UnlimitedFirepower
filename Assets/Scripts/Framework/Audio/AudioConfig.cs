using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // ̫�������Ϸ��Ƶ����
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        #region ��������
        [Header("��������")]
        public AudioClip mainMenuBGM;       // ���˵�����
        public AudioClip gameplayBGM;       // ��ͨ��Ϸ��������
        public AudioClip bossBattleBGM;     // Bossս����
        public AudioClip victoryBGM;        // ʤ������
        public AudioClip defeatBGM;         // ʧ������
        public AudioClip spaceAmbient;      // ̫�ջ�����
        #endregion

        #region ��ҷɴ���Ч
        [Header("��ҷɴ���Ч")]
        public AudioClip shipEngine;        // ������Ч
        public AudioClip shipThrust;        // ������Ч
        public AudioClip shipLaserFire;     // �������
        public AudioClip shipMissileFire;   // ��������
        public AudioClip shipShieldUp;      // ���ܿ���
        public AudioClip shipShieldDown;    // ���ܹر�
        public AudioClip shipShieldHit;     // �����ܻ�
        public AudioClip shipHit;           // �ɴ��ܻ�
        public AudioClip shipExplosion;     // �ɴ���ը
        #endregion

        #region ������Ч
        [Header("������Ч")]
        public AudioClip enemyBasicFire;    // ��ͨ�������
        public AudioClip enemyBossFire;     // Boss���
        public AudioClip enemyHit;          // �����ܻ�
        public AudioClip enemyExplosion;    // ���˱�ը
        public AudioClip enemySpawn;        // ���˳���
        public AudioClip bossSpawn;         // Boss����
        public AudioClip bossRoar;          // Boss����
        #endregion

        #region ������������Ч
        [Header("������������Ч")]
        public AudioClip powerupCollect;    // �ռ�����
        public AudioClip weaponUpgrade;     // ��������
        public AudioClip shieldUpgrade;     // ��������
        public AudioClip engineUpgrade;     // ��������
        public AudioClip healthRestore;     // ����ֵ�ָ�
        #endregion

        #region UI��Ч
        [Header("UI��Ч")]
        public AudioClip buttonClick;       // ��ť���
        public AudioClip buttonHover;       // ��ť��ͣ
        public AudioClip menuOpen;          // �˵���
        public AudioClip menuClose;         // �˵��ر�
        public AudioClip notification;      // ֪ͨ��ʾ
        public AudioClip levelComplete;     // �ؿ����
        #endregion

        #region ��������Ч��Ч
        [Header("��������Ч��Ч")]
        public AudioClip asteroidImpact;    // С����ײ��
        public AudioClip blackHoleGravity;  // �ڶ�����
        public AudioClip wormholeEnter;     // �涴����
        public AudioClip wormholeExit;      // �涴�뿪
        public AudioClip explosionLarge;    // ���ͱ�ը
        public AudioClip explosionMedium;   // ���ͱ�ը
        public AudioClip explosionSmall;    // С�ͱ�ը
        #endregion

        #region ������Ч
        [Header("������Ч")]
        public AudioClip[] laserSounds;     // ������Ч��
        public AudioClip[] missileSounds;   // ������Ч��
        public AudioClip railgunFire;       // ����ڷ���
        public AudioClip plasmaFire;        // �������ڷ���
        public AudioClip EMPEffect;         // �������Ч��
        #endregion
    }
}

