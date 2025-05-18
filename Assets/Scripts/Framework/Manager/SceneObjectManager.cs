using MyGame.Framework.Event;
using MyGame.Framework.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObjectManager : GameSystemBase
{
    public static SceneObjectManager Instance;

    [Header("��������")]
    [SerializeField] private string mainSceneName = "MainScene";
    [SerializeField] private List<string> persistentObjects = new List<string>();

    private Dictionary<string, List<GameObject>> sceneObjects = new Dictionary<string, List<GameObject>>();
    private Dictionary<GameObject, bool> objectStates = new Dictionary<GameObject, bool>();
    private HashSet<string> activeScenes = new HashSet<string>();
    private List<GameObject> mainScenePersistentObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public override async Task InitializeAsync(Action<float> onProgress = null)
    {
        InitializeMainScene();

        GameEventManager.RegisterListener(GameEventType.SceneObjectInitial, InitializeMainScene);
        GameEventManager.RegisterListener(GameEventType.SceneObjectShow, ShowMainSceneObjects);
        GameEventManager.RegisterListener(GameEventType.SceneObjectHide, HideMainSceneObjects);

        await Task.Delay(100);
    }

    private void OnDestroy()
    {
        GameEventManager.UnregisterListener(GameEventType.SceneObjectInitial, InitializeMainScene);
        GameEventManager.UnregisterListener(GameEventType.SceneObjectShow, ShowMainSceneObjects);
        GameEventManager.UnregisterListener(GameEventType.SceneObjectHide, HideMainSceneObjects);
    }

    // ��ʼ��������
    public void InitializeMainScene()
    {
        Debug.Log($"��ʼ����{mainSceneName}������������");

        mainScenePersistentObjects.Clear();
        objectStates.Clear();
        sceneObjects.Clear();

        Scene mainScene = SceneManager.GetSceneByName(mainSceneName);
        if (!mainScene.isLoaded)
        {
            Debug.LogError($"������ {mainSceneName} δ���أ�");
            return;
        }

        // �洢�������е����ж���
        GameObject[] rootObjects = mainScene.GetRootGameObjects();
        List<GameObject> mainSceneObjects = new List<GameObject>();

        foreach (GameObject obj in rootObjects)
        {
            // ����Ƿ�Ϊ�־ö���
            if (persistentObjects.Contains(obj.name))
            {
                mainScenePersistentObjects.Add(obj);
            }
            else
            {
                mainSceneObjects.Add(obj);

            }
        }

        sceneObjects[mainSceneName] = mainSceneObjects;
        activeScenes.Add(mainSceneName);

        foreach (GameObject obj in sceneObjects[mainSceneName])
        {
            objectStates[obj] = obj.activeSelf;
        }

        Debug.Log($"�Ѹ���{mainSceneName}������������");
    }

    // ��������������
    public void HideMainSceneObjects()
    {
        if (!sceneObjects.ContainsKey(mainSceneName)) return;

        foreach (GameObject obj in sceneObjects[mainSceneName])
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        Debug.Log("����������������");
    }

    // ��ʾ����������
    public void ShowMainSceneObjects()
    {
        if (!sceneObjects.ContainsKey(mainSceneName)) return;

        foreach (GameObject obj in sceneObjects[mainSceneName])
        {
            if (obj != null)
            {
                if(objectStates.ContainsKey(obj)) obj.SetActive(objectStates[obj]);
                else obj.SetActive(true);
            }
        }

        Debug.Log("��������������ʾ");
    }

    // ��ӳ־ö��󣨲��ᱻ���صĶ���
    public void AddPersistentObject(GameObject obj)
    {
        if (obj != null && !mainScenePersistentObjects.Contains(obj))
        {
            mainScenePersistentObjects.Add(obj);

            // ����������������У��������б����Ƴ�
            if (sceneObjects.ContainsKey(mainSceneName) && sceneObjects[mainSceneName].Contains(obj))
            {
                sceneObjects[mainSceneName].Remove(obj);
            }
        }
    }

    // �Ƴ��־ö���
    public void RemovePersistentObject(GameObject obj)
    {
        if (mainScenePersistentObjects.Contains(obj))
        {
            mainScenePersistentObjects.Remove(obj);

            // ����������������У���ӵ������б�
            if (sceneObjects.ContainsKey(mainSceneName))
            {
                sceneObjects[mainSceneName].Add(obj);
            }
        }
    }

}
