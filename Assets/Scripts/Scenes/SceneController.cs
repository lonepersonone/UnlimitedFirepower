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
        MySceneManager.Instance.LoadScene("LobbyScene", true, () =>
        {
            Debug.Log("�ѳɹ������������");
        });
    }

    // ������������ս������
    public void EnterBattleScene()
    {
        MySceneManager.Instance.LoadScene("BattleScene", false, () => {
            Debug.Log("ս��������׼����������ʼս��");
        });
    }

    // ս��ʤ��������������
    public void BattleVictory()
    {
        MySceneManager.Instance.LoadScene("MainSceneE", true, () => {
            Debug.Log("ս������������������");
        });
    }

    public void EnterNextGalaxy()
    {
        MySceneManager.Instance.LoadScene("GalaxySceneE", true, () => {
            Debug.Log("������һ��ϵ");
        });
    }

    #endregion

}
