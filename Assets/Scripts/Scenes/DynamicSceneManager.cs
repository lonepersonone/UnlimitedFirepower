using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MyGame.UI.Transition;
using System;

public enum SceneBuildIndex
{
    MainMenu = 0,
    MainScene = 1,
    BattleScene = 2,
    // 添加其他场景...
}

public class DynamicSceneManager : MonoBehaviour
{
    public static DynamicSceneManager Instance { get; private set; }

    [Header("加载配置")]
    [SerializeField] private float loadingScreenDelay = 0.5f;

    // 存储已加载的场景实例
    private Dictionary<string, string> loadedScenes = new Dictionary<string, string>();
    // 场景加载计数器，用于生成唯一ID
    private Dictionary<string, int> sceneCounters = new Dictionary<string, int>();

    //场景加载进度事件
    public Action<float> OnLoadingProcess;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #region 普通场景加载（替换当前场景）
    public void LoadScene(string sceneName, System.Action<string, string> onLoadBegin = null, System.Action<string, string> onLoadComplete = null)
    {
        string instanceId = GenerateSceneInstanceId(sceneName);

        // 触发加载开始回调
        onLoadBegin?.Invoke(sceneName, instanceId);

        // 开始异步加载场景
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, instanceId, onLoadComplete));
    }
    #endregion

    #region Additive场景加载（叠加到当前场景）
    public void LoadSceneAdditively(string sceneName, System.Action<string, string> onLoadBegin = null, System.Action<string, string> onLoadComplete = null)
    {
        string instanceId = GenerateSceneInstanceId(sceneName);

        // 触发加载开始回调
        onLoadBegin?.Invoke(sceneName, instanceId);

        // 开始异步加载场景
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, instanceId, onLoadComplete));
    }
    #endregion

    #region 场景卸载
    public void UnloadScene(string sceneName, System.Action<string, string> onUnloadComplete = null)
    {
        if (!loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"尝试卸载未加载的场景: {sceneName}");
            return;
        }

        string instanceId = loadedScenes[sceneName];

        // 开始异步卸载场景
        StartCoroutine(UnloadSceneAsync(sceneName, instanceId, onUnloadComplete));
    }
    #endregion

    #region 异步加载场景协程
    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, string instanceId, System.Action<string, string> onComplete)
    {
        // 检查场景是否存在
        if (!SceneExists(sceneName))
        {
            Debug.LogError($"场景不存在: {sceneName}");

            yield break;
        }

        // 如果是Additive模式，检查场景是否已加载
        if (mode == LoadSceneMode.Additive && loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"场景已加载: {sceneName} (ID: {loadedScenes[sceneName]})");

            // 调用完成回调
            onComplete?.Invoke(sceneName, loadedScenes[sceneName]);

            yield break;
        }

        // 开始加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        //asyncLoad.allowSceneActivation = false;

        // 等待加载完成
        while (!asyncLoad.isDone)
        {
            OnLoadingProcess?.Invoke(asyncLoad.progress);

            // 加载进度为0.9时表示加载完成，但还未激活
            if (asyncLoad.progress >= 0.9f)
            {
                // 等待额外的延迟，确保资源完全加载
                yield return new WaitForSeconds(loadingScreenDelay);

                // 激活场景
                asyncLoad.allowSceneActivation = true;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            }

            yield return null;
        }

        // 更新已加载场景列表
        if (mode == LoadSceneMode.Additive)
        {
            loadedScenes[sceneName] = instanceId;
        }
        else
        {
            // 对于普通加载，清除所有Additive加载的场景记录
            loadedScenes.Clear();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // 调用完成回调
        onComplete?.Invoke(sceneName, instanceId);
    }
    #endregion

    #region 异步卸载场景协程
    private IEnumerator UnloadSceneAsync(string sceneName, string instanceId, System.Action<string, string> onComplete)
    {
        // 检查场景是否已加载
        if (!loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"尝试卸载未加载的场景: {sceneName}");

            yield break;
        }

        // 开始卸载场景
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        // 等待卸载完成
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // 清理场景记录
        loadedScenes.Remove(sceneName);

        // 资源回收
        yield return Resources.UnloadUnusedAssets();

        // 调用完成回调
        onComplete?.Invoke(sceneName, instanceId);
    }
    #endregion

    #region 辅助方法
    // 生成场景实例ID
    private string GenerateSceneInstanceId(string sceneName)
    {
        if (!sceneCounters.ContainsKey(sceneName))
        {
            sceneCounters[sceneName] = 0;
        }

        string instanceId = $"{sceneName}_{sceneCounters[sceneName]++}";
        return instanceId;
    }

    // 检查场景是否存在于Build Settings中
    private bool SceneExists(string sceneName)
    {
        // 获取场景枚举的所有值
        Array enumValues = System.Enum.GetValues(typeof(SceneBuildIndex));

        // 显式转换为int数组
        int[] indices = new int[enumValues.Length];
        for (int i = 0; i < enumValues.Length; i++)
        {
            indices[i] = (int)enumValues.GetValue(i);
        }

        // 获取所有场景路径
        string[] scenePaths = System.Array.ConvertAll(indices, index =>
            SceneUtility.GetScenePathByBuildIndex(index)
        );

        // 检查场景是否存在
        foreach (string path in scenePaths)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    // 获取当前加载的所有场景
    public List<string> GetLoadedScenes()
    {
        return new List<string>(loadedScenes.Keys);
    }
    #endregion
}