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
        MySceneManager.Instance.LoadScene("LobbyScene", true, () =>
        {
            Debug.Log("已成功进入大厅场景");
        });
    }

    // 从主场景进入战斗场景
    public void EnterBattleScene()
    {
        MySceneManager.Instance.LoadScene("BattleScene", false, () => {
            Debug.Log("战斗场景已准备就绪，开始战斗");
        });
    }

    // 战斗胜利，返回主场景
    public void BattleVictory()
    {
        MySceneManager.Instance.LoadScene("MainSceneE", true, () => {
            Debug.Log("战斗结束，返回主场景");
        });
    }

    public void EnterNextGalaxy()
    {
        MySceneManager.Instance.LoadScene("GalaxySceneE", true, () => {
            Debug.Log("进入下一星系");
        });
    }

    #endregion

}
