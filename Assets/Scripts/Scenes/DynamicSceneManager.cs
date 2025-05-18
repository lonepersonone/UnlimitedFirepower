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
    // �����������...
}

public class DynamicSceneManager : MonoBehaviour
{
    public static DynamicSceneManager Instance { get; private set; }

    [Header("��������")]
    [SerializeField] private float loadingScreenDelay = 0.5f;

    // �洢�Ѽ��صĳ���ʵ��
    private Dictionary<string, string> loadedScenes = new Dictionary<string, string>();
    // �������ؼ���������������ΨһID
    private Dictionary<string, int> sceneCounters = new Dictionary<string, int>();

    //�������ؽ����¼�
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

    #region ��ͨ�������أ��滻��ǰ������
    public void LoadScene(string sceneName, System.Action<string, string> onLoadBegin = null, System.Action<string, string> onLoadComplete = null)
    {
        string instanceId = GenerateSceneInstanceId(sceneName);

        // �������ؿ�ʼ�ص�
        onLoadBegin?.Invoke(sceneName, instanceId);

        // ��ʼ�첽���س���
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, instanceId, onLoadComplete));
    }
    #endregion

    #region Additive�������أ����ӵ���ǰ������
    public void LoadSceneAdditively(string sceneName, System.Action<string, string> onLoadBegin = null, System.Action<string, string> onLoadComplete = null)
    {
        string instanceId = GenerateSceneInstanceId(sceneName);

        // �������ؿ�ʼ�ص�
        onLoadBegin?.Invoke(sceneName, instanceId);

        // ��ʼ�첽���س���
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, instanceId, onLoadComplete));
    }
    #endregion

    #region ����ж��
    public void UnloadScene(string sceneName, System.Action<string, string> onUnloadComplete = null)
    {
        if (!loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"����ж��δ���صĳ���: {sceneName}");
            return;
        }

        string instanceId = loadedScenes[sceneName];

        // ��ʼ�첽ж�س���
        StartCoroutine(UnloadSceneAsync(sceneName, instanceId, onUnloadComplete));
    }
    #endregion

    #region �첽���س���Э��
    private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode, string instanceId, System.Action<string, string> onComplete)
    {
        // ��鳡���Ƿ����
        if (!SceneExists(sceneName))
        {
            Debug.LogError($"����������: {sceneName}");

            yield break;
        }

        // �����Additiveģʽ����鳡���Ƿ��Ѽ���
        if (mode == LoadSceneMode.Additive && loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"�����Ѽ���: {sceneName} (ID: {loadedScenes[sceneName]})");

            // ������ɻص�
            onComplete?.Invoke(sceneName, loadedScenes[sceneName]);

            yield break;
        }

        // ��ʼ���س���
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        //asyncLoad.allowSceneActivation = false;

        // �ȴ��������
        while (!asyncLoad.isDone)
        {
            OnLoadingProcess?.Invoke(asyncLoad.progress);

            // ���ؽ���Ϊ0.9ʱ��ʾ������ɣ�����δ����
            if (asyncLoad.progress >= 0.9f)
            {
                // �ȴ�������ӳ٣�ȷ����Դ��ȫ����
                yield return new WaitForSeconds(loadingScreenDelay);

                // �����
                asyncLoad.allowSceneActivation = true;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            }

            yield return null;
        }

        // �����Ѽ��س����б�
        if (mode == LoadSceneMode.Additive)
        {
            loadedScenes[sceneName] = instanceId;
        }
        else
        {
            // ������ͨ���أ��������Additive���صĳ�����¼
            loadedScenes.Clear();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // ������ɻص�
        onComplete?.Invoke(sceneName, instanceId);
    }
    #endregion

    #region �첽ж�س���Э��
    private IEnumerator UnloadSceneAsync(string sceneName, string instanceId, System.Action<string, string> onComplete)
    {
        // ��鳡���Ƿ��Ѽ���
        if (!loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogWarning($"����ж��δ���صĳ���: {sceneName}");

            yield break;
        }

        // ��ʼж�س���
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        // �ȴ�ж�����
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // ��������¼
        loadedScenes.Remove(sceneName);

        // ��Դ����
        yield return Resources.UnloadUnusedAssets();

        // ������ɻص�
        onComplete?.Invoke(sceneName, instanceId);
    }
    #endregion

    #region ��������
    // ���ɳ���ʵ��ID
    private string GenerateSceneInstanceId(string sceneName)
    {
        if (!sceneCounters.ContainsKey(sceneName))
        {
            sceneCounters[sceneName] = 0;
        }

        string instanceId = $"{sceneName}_{sceneCounters[sceneName]++}";
        return instanceId;
    }

    // ��鳡���Ƿ������Build Settings��
    private bool SceneExists(string sceneName)
    {
        // ��ȡ����ö�ٵ�����ֵ
        Array enumValues = System.Enum.GetValues(typeof(SceneBuildIndex));

        // ��ʽת��Ϊint����
        int[] indices = new int[enumValues.Length];
        for (int i = 0; i < enumValues.Length; i++)
        {
            indices[i] = (int)enumValues.GetValue(i);
        }

        // ��ȡ���г���·��
        string[] scenePaths = System.Array.ConvertAll(indices, index =>
            SceneUtility.GetScenePathByBuildIndex(index)
        );

        // ��鳡���Ƿ����
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

    // ��ȡ��ǰ���ص����г���
    public List<string> GetLoadedScenes()
    {
        return new List<string>(loadedScenes.Keys);
    }
    #endregion
}