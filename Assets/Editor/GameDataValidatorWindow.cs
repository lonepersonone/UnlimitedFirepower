using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using MyGame.Data.SO;

public class GameDataValidationWindow : EditorWindow
{
    private List<Type> typeSelections = new List<Type>();
    private Vector2 scrollPos;
    private bool autoFix = true;
    private bool checkIDUnique = true;
    private List<string> ignoredFields = new List<string>();

    // ✅ 白名单：只允许以下数据类型参与检查
    private static readonly Type[] AllowedGameDataTypes = new Type[]
    {
        typeof(WeaponData),
        typeof(UnityEditor.U2D.Animation.CharacterData),
        typeof(WaveData),
        typeof(DynamicTextData),
    };

    [MenuItem("Tools/Game data Validator")]
    public static void ShowWindow()
    {
        GetWindow<GameDataValidationWindow>("Game data Validator");
    }

    private void OnGUI()
    {
        GUILayout.Label("数据校验配置", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 显示白名单中的类型
        GUILayout.Label("添加资源类型:");
        foreach (var type in AllowedGameDataTypes)
        {
            if (!typeSelections.Contains(type))
            {
                if (GUILayout.Button($"添加 {type.Name}"))
                {
                    typeSelections.Add(type);
                }
            }
        }

        EditorGUILayout.Space();
        GUILayout.Label("已添加的类型:");
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100));
        foreach (var type in typeSelections.ToList())
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(type.Name);
            if (GUILayout.Button("移除", GUILayout.Width(60)))
                typeSelections.Remove(type);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        autoFix = EditorGUILayout.Toggle("自动修复空引用", autoFix);
        checkIDUnique = EditorGUILayout.Toggle("检查 ID 唯一性", checkIDUnique);

        GUILayout.Label("忽略字段（例如 prefab、icon）：");
        string ignored = string.Join(",", ignoredFields);
        ignored = EditorGUILayout.TextField(ignored);
        ignoredFields = ignored.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

        EditorGUILayout.Space();
        if (GUILayout.Button("开始校验"))
        {
            GameDataValidator.RunValidation(
                typeSelections,
                autoFix,
                checkIDUnique,
                ignoredFields
            );
        }
    }
}
