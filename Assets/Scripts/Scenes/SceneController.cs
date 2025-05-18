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

    [Header("������������")]
    public string lobbySceneName = "LobbyScene";
    public string mainSceneName = "MainScene";
    public string battleSceneName = "BattleScene";

    [Header("�������ز���")]
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

        // ��ʼ����ǰ����
        currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log($"��ǰ����: {currentSceneName}");
    }

    #region �����л�����
    // �Ӵ�������������
    public void EnterMainScene()
    {
        MySceneManager.Instance.LoadScene("MainScene", false,() => {
            Debug.Log("�ѳɹ�����������");
        });
    }

    // �����������ش���
    public void ReturnToLobby()
    {
        if (currentSceneName == battleSceneName)
        {
            // �����ǰ��ս����������ж��ս������
            DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
        }
        else
        {
            // ֱ�Ӽ��ش�������
            LoadScene(lobbySceneName);
        }
    }

    // ������������ս������
    public void EnterBattleScene()
    {
        MySceneManager.Instance.LoadScene("BattleScene", false, () => {
            Debug.Log("ս��������׼����������ʼս��");
        });
    }

    // ��ɫ��ս�������������ش���
    public void PlayerDiedInBattle()
    {
        // ж��ս�����������ش���
        DynamicSceneManager.Instance.UnloadScene(battleSceneName, OnBattleSceneUnloadedForLobby);
    }

    // ս��ʤ��������������
    public void BattleVictory()
    {
        MySceneManager.Instance.LoadScene("MainSceneE", true, () => {
            Debug.Log("ս������������������");
        });
    }

    private void OnBattleSceneUnloadedForLobby(string sceneName, string instanceId)
    {
        Debug.Log($"ս������ {sceneName} (ID: {instanceId}) ж����ɣ����ش���");

        // ���ش�������
        LoadScene(lobbySceneName);
    }

    public void EnterNextGalaxy()
    {
        MySceneManager.Instance.LoadScene("GalaxySceneE", true, () => {
            Debug.Log("������һ��ϵ");
        });
    }

    #endregion

    #region ��������
    private void LoadScene(string sceneName, System.Action callback = null)
    {
        DynamicSceneManager.Instance.LoadScene(sceneName,
            (name, id) => {
                Debug.Log($"���� {name} ��ʼ����");
            },
            (name, id) => {
                Debug.Log($"���� {name} �������");
                currentSceneName = name;

                if (callback != null)
                {
                    callback();
                }
            });
    }


    #endregion

}
