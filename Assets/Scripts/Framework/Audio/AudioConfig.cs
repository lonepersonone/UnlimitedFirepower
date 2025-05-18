using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Audio
{

    // 太空射击游戏音频配置
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Audio/Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        #region 背景音乐
        [Header("背景音乐")]
        public AudioClip mainMenuBGM;       // 主菜单音乐
        public AudioClip gameplayBGM;       // 普通游戏场景音乐
        public AudioClip bossBattleBGM;     // Boss战音乐
        public AudioClip victoryBGM;        // 胜利音乐
        public AudioClip defeatBGM;         // 失败音乐
        public AudioClip spaceAmbient;      // 太空环境音
        #endregion

        #region 玩家飞船音效
        [Header("玩家飞船音效")]
        public AudioClip shipEngine;        // 引擎音效
        public AudioClip shipThrust;        // 加速音效
        public AudioClip shipLaserFire;     // 激光射击
        public AudioClip shipMissileFire;   // 导弹发射
        public AudioClip shipShieldUp;      // 护盾开启
        public AudioClip shipShieldDown;    // 护盾关闭
        public AudioClip shipShieldHit;     // 护盾受击
        public AudioClip shipHit;           // 飞船受击
        public AudioClip shipExplosion;     // 飞船爆炸
        #endregion

        #region 敌人音效
        [Header("敌人音效")]
        public AudioClip enemyBasicFire;    // 普通敌人射击
        public AudioClip enemyBossFire;     // Boss射击
        public AudioClip enemyHit;          // 敌人受击
        public AudioClip enemyExplosion;    // 敌人爆炸
        public AudioClip enemySpawn;        // 敌人出现
        public AudioClip bossSpawn;         // Boss出现
        public AudioClip bossRoar;          // Boss咆哮
        #endregion

        #region 道具与升级音效
        [Header("道具与升级音效")]
        public AudioClip powerupCollect;    // 收集道具
        public AudioClip weaponUpgrade;     // 武器升级
        public AudioClip shieldUpgrade;     // 护盾升级
        public AudioClip engineUpgrade;     // 引擎升级
        public AudioClip healthRestore;     // 生命值恢复
        #endregion

        #region UI音效
        [Header("UI音效")]
        public AudioClip buttonClick;       // 按钮点击
        public AudioClip buttonHover;       // 按钮悬停
        public AudioClip menuOpen;          // 菜单打开
        public AudioClip menuClose;         // 菜单关闭
        public AudioClip notification;      // 通知提示
        public AudioClip levelComplete;     // 关卡完成
        #endregion

        #region 环境与特效音效
        [Header("环境与特效音效")]
        public AudioClip asteroidImpact;    // 小行星撞击
        public AudioClip blackHoleGravity;  // 黑洞引力
        public AudioClip wormholeEnter;     // 虫洞进入
        public AudioClip wormholeExit;      // 虫洞离开
        public AudioClip explosionLarge;    // 大型爆炸
        public AudioClip explosionMedium;   // 中型爆炸
        public AudioClip explosionSmall;    // 小型爆炸
        #endregion

        #region 武器音效
        [Header("武器音效")]
        public AudioClip[] laserSounds;     // 激光音效组
        public AudioClip[] missileSounds;   // 导弹音效组
        public AudioClip railgunFire;       // 轨道炮发射
        public AudioClip plasmaFire;        // 等离子炮发射
        public AudioClip EMPEffect;         // 电磁脉冲效果
        #endregion
    }
}

