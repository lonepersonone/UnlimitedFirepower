using MyGame.Framework.Event;
using MyGame.Gameplay.Level;
using MyGame.Scene.BattleRoom;
using MyGame.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public class BattleSceneTransitionHandler : SceneTransitionHandlerBase
    {
        public override string SceneName => "BattleScene";
        public float loadingDelay = 1.0f;

        protected override IEnumerator OnInitializing()
        {
            Debug.Log("开始加载战斗场景...");
            yield return null;

            DynamicSceneManager.Instance.LoadSceneAdditively(SceneName, OnBattleSceneLoadBegin, OnBattleSceneLoaded);

            Debug.Log("战斗场景初始化完成");
        }

        protected override void OnScalingCamera()
        {

            // 战斗场景特定的相机设置
        }

        protected override IEnumerator OnSpawningUI()
        {
            GameEventManager.TriggerEvent(GameEventType.BattleInitial);
            yield return new WaitForSeconds(1f);
        }

        protected override void OnReady()
        {
            base.OnReady();
                       
            // 触发战斗开始事件
            GameEventManager.TriggerEvent(GameEventType.BattleStarted);
        }

        private void OnBattleSceneLoadBegin(string sceneName, string instanceId)
        {
            Debug.Log($"战斗场景 {sceneName} (ID: {instanceId}) 准备加载");

            // 初始化战斗数据
            BattleDataBridge.Instance.InitializeData(
                MainDataManager.Instance.ScriptableManager,
                new LevelAttribute(MainDataManager.Instance.MapData.CurrentGalxy.CurrentPlanet.LevelDataSO, 3),
                MainDataManager.Instance.PlayerData,
                MainDataManager.Instance.UpgradeData,
                MainDataManager.Instance.WealthData); ;
            GameEventManager.TriggerEvent(GameEventType.SceneObjectInitial);
        }

        private void OnBattleSceneLoaded(string sceneName, string instanceId)
        {
            Debug.Log($"战斗场景 {sceneName} (ID: {instanceId}) 加载完成");
            GameEventManager.TriggerEvent(GameEventType.SceneObjectHide);
        }

    }

}

