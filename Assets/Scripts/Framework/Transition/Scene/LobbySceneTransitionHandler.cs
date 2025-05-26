using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public class LobbySceneTransitionHandler : SceneTransitionHandlerBase
    {
        public override string SceneName => "LobbyScene";
        public string battleSceneName = "BattleScene";

        protected override IEnumerator OnInitializing()
        {
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentSceneName == battleSceneName)
            {
                // 如果当前在战斗场景，先卸载战斗场景
                DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
            }
            else
            {              
                // 直接加载大厅场景
                DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"开始加载大厅场景: {name}"),
                (name, id) => Debug.Log($"大厅场景加载完成: {name}")
                );
            }
            yield return null;
        }

        private void OnBattleSceneUnloadedForLobby(string sceneName, string instanceId)
        {
            Debug.Log($"战斗场景 {sceneName} (ID: {instanceId}) 卸载完成，返回大厅");

            // 加载大厅场景
            DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"开始加载大厅场景: {name}"),
                (name, id) => Debug.Log($"大厅场景加载完成: {name}")
                );
        }
    }

}

