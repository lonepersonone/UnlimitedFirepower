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
                // �����ǰ��ս����������ж��ս������
                DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
            }
            else
            {              
                // ֱ�Ӽ��ش�������
                DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"��ʼ���ش�������: {name}"),
                (name, id) => Debug.Log($"���������������: {name}")
                );
            }
            yield return null;
        }

        private void OnBattleSceneUnloadedForLobby(string sceneName, string instanceId)
        {
            Debug.Log($"ս������ {sceneName} (ID: {instanceId}) ж����ɣ����ش���");

            // ���ش�������
            DynamicSceneManager.Instance.LoadScene(SceneName,
                (name, id) => Debug.Log($"��ʼ���ش�������: {name}"),
                (name, id) => Debug.Log($"���������������: {name}")
                );
        }
    }

}

