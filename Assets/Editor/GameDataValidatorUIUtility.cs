using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GameDataValidator
{
    public static void RunValidation(List<Type> types, bool autoFix, bool checkIDUnique, List<string> ignoreFields)
    {
        foreach (var type in types)
        {
            string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");
            HashSet<string> ids = new();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, type);
                if (obj == null)
                {
                    Debug.LogWarning($"路径加载失败：{path}");
                    continue;
                }

                var so = new SerializedObject(obj);
                var prop = so.GetIterator();
                bool hasNull = false;

                while (prop.NextVisible(true))
                {
                    if (ignoreFields.Contains(prop.name))
                        continue;

                    if (prop.propertyType == SerializedPropertyType.ObjectReference && prop.objectReferenceValue == null)
                    {
                        Debug.LogWarning($"空引用字段: {prop.name} 在 {path}");
                        hasNull = true;

                        if (autoFix)
                        {
                            // 尝试自动修复
                            // 你可以添加更多自定义逻辑
                            // 目前只做提示
                        }
                    }

                    if (checkIDUnique && prop.name == "ID" && prop.propertyType == SerializedPropertyType.String)
                    {
                        string id = prop.stringValue;
                        if (!ids.Add(id))
                            Debug.LogError($"重复 ID: {id} 在 {path}");
                    }
                }

                if (autoFix && hasNull)
                {
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("数据校验完成。");
    }
}
