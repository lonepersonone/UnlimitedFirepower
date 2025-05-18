using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using MyGame.Data.SO;

public class GameDataImportChecker : AssetPostprocessor
{
    // 只对指定类型进行校验（白名单）
    private static readonly Type[] ValidTypes = new Type[]
    {
        typeof(WeaponData),
        typeof(UnityEditor.U2D.Animation.CharacterData),
        typeof(WaveData),
        typeof(DynamicTextData),

    };

    private static readonly List<string> IgnoredFields = new List<string> { "icon", "prefab" };

    // 在资源导入完成后自动触发
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var path in importedAssets)
        {
            UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(path);
            if (obj == null) continue;

            Type type = obj.GetType();
            if (!ValidTypes.Contains(type)) continue;

            Debug.Log($"🔍 自动校验资源：{path}");

            // 运行自动校验
            GameDataValidator.RunValidation(
                new List<Type> { type },
                autoFix: false,
                checkIDUnique: true,
                ignoreFields: IgnoredFields
            );
        }
    }
}
