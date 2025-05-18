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
            Debug.Log("��ʼ����ս������...");
            yield return null;

            DynamicSceneManager.Instance.LoadSceneAdditively(SceneName, OnBattleSceneLoadBegin, OnBattleSceneLoaded);

            Debug.Log("ս��������ʼ�����");
        }

        protected override void OnScalingCamera()
        {

            // ս�������ض����������
        }

        protected override IEnumerator OnSpawningUI()
        {
            GameEventManager.TriggerEvent(GameEventType.BattleInitial);
            yield return new WaitForSeconds(1f);
        }

        protected override void OnReady()
        {
            base.OnReady();
                       
            // ����ս����ʼ�¼�
            GameEventManager.TriggerEvent(GameEventType.BattleStarted);
        }

        private void OnBattleSceneLoadBegin(string sceneName, string instanceId)
        {
            Debug.Log($"ս������ {sceneName} (ID: {instanceId}) ׼������");

            // ��ʼ��ս������
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
            Debug.Log($"ս������ {sceneName} (ID: {instanceId}) �������");
            GameEventManager.TriggerEvent(GameEventType.SceneObjectHide);
        }

    }

}

