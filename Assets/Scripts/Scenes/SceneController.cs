using MyGame.Framework.Audio;
using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using MyGame.Framework.Transition;
using MyGame.Gameplay.Level;
using MyGame.Scene.BattleRoom;
using MyGame.Scene.Main;
using MyGame.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;
using MySceneManager = MyGame.Framework.Transition.SceneManager;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [Header("场景名称配置")]
    public string lobbySceneName = "LobbyScene";
    public string mainSceneName = "MainScene";
    public string battleSceneName = "BattleScene";

    [Header("场景加载参数")]
    public bool showLoadingScreen = true;
    public float loadingDelay = 1.0f;

    private string currentSceneName;
    private string previousSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化当前场景
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log($"当前场景: {currentSceneName}");
    }

    #region 场景切换方法
    // 从大厅进入主场景
    public void EnterMainScene()
    {
        MySceneManager.Instance.LoadScene("MainScene", false,() => {
            Debug.Log("已成功进入主场景");
        });
    }

    // 从主场景返回大厅
    public void ReturnToLobby()
    {
        if (currentSceneName == battleSceneName)
        {
            // 如果当前在战斗场景，先卸载战斗场景
            DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
        }
        else
        {
            // 直接加载大厅场景
            LoadScene(lobbySceneName);
        }
    }

    // 从主场景进入战斗场景
    public void EnterBattleScene()
    {
        MySceneManager.Instance.LoadScene("BattleScene", false, () => {
            Debug.Log("战斗场景已准备就绪，开始战斗");
        });
    }

    // 角色在战斗中死亡，返回大厅
    public void PlayerDiedInBattle()
    {
        // 卸载战斗场景，返回大厅
        DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
    }

    // 战斗胜利，返回主场景
    public void BattleVictory()
    {
        MySceneManager.Instance.LoadScene("MainSceneE", true, () => {
            Debug.Log("战斗结束，返回主场景");
        });
    }

    private void OnBattleSceneUnloadedForLobby(string sceneName, string instanceId)
    {
        Debug.Log($"战斗场景 {sceneName} (ID: {instanceId}) 卸载完成，返回大厅");

        // 加载大厅场景
        LoadScene(lobbySceneName);
    }

    public void EnterNextGalaxy()
    {
        MySceneManager.Instance.LoadScene("GalaxySceneE", true, () => {
            Debug.Log("进入下一星系");
        });
    }

    #endregion

    #region 辅助方法
    private void LoadScene(string sceneName, System.Action callback = null)
    {
        DynamicSceneManager.Instance.LoadScene(sceneName,
            (name, id) => {
                Debug.Log($"场景 {name} 开始加载");
            },
            (name, id) => {
                Debug.Log($"场景 {name} 加载完成");
                currentSceneName = name;

                if (callback != null)
                {
                    callback();
                }
            });
    }


    #endregion

}
