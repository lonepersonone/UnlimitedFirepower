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

    [Header("场景配置")]
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

    // 初始化主场景
    public void InitializeMainScene()
    {
        Debug.Log($"开始更新{mainSceneName}场景对象数据");

        mainScenePersistentObjects.Clear();
        objectStates.Clear();
        sceneObjects.Clear();

        Scene mainScene = SceneManager.GetSceneByName(mainSceneName);
        if (!mainScene.isLoaded)
        {
            Debug.LogError($"主场景 {mainSceneName} 未加载！");
            return;
        }

        // 存储主场景中的所有对象
        GameObject[] rootObjects = mainScene.GetRootGameObjects();
        List<GameObject> mainSceneObjects = new List<GameObject>();

        foreach (GameObject obj in rootObjects)
        {
            // 检查是否为持久对象
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

        Debug.Log($"已更新{mainSceneName}场景对象数据");
    }

    // 隐藏主场景对象
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

        Debug.Log("主场景对象已隐藏");
    }

    // 显示主场景对象
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

        Debug.Log("主场景对象已显示");
    }

    // 添加持久对象（不会被隐藏的对象）
    public void AddPersistentObject(GameObject obj)
    {
        if (obj != null && !mainScenePersistentObjects.Contains(obj))
        {
            mainScenePersistentObjects.Add(obj);

            // 如果对象在主场景中，从隐藏列表中移除
            if (sceneObjects.ContainsKey(mainSceneName) && sceneObjects[mainSceneName].Contains(obj))
            {
                sceneObjects[mainSceneName].Remove(obj);
            }
        }
    }

    // 移除持久对象
    public void RemovePersistentObject(GameObject obj)
    {
        if (mainScenePersistentObjects.Contains(obj))
        {
            mainScenePersistentObjects.Remove(obj);

            // 如果对象在主场景中，添加到隐藏列表
            if (sceneObjects.ContainsKey(mainSceneName))
            {
                sceneObjects[mainSceneName].Add(obj);
            }
        }
    }

}
