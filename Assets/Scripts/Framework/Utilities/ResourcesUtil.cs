using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGame.Framework.Utilities
{
    public static class ResourcesUtil
    {
        public static Sprite LoadSprite(string str)
        {
            if (Resources.Load<Sprite>(str) == null)
            {
                Debug.LogError($"{str} is null");
                return null;
            }
            else
                return Resources.Load<Sprite>(str);
        }


        /// <summary>
        /// �첽����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folderPath"></param>
        /// <param name="onProgress"></param>
        /// <returns></returns>
        public static async Task<T[]> LoadAllAsync<T>(string folderPath, Action<float> onProgress = null) where T : ScriptableObject
        {
            // �첽�����ļ�����������Դ
            var request = Resources.LoadAsync(folderPath);
            while (!request.isDone)
            {
                onProgress?.Invoke(request.progress);
                await Task.Yield();
            }

            // ʵ�ʼ���������Դ
            var all = Resources.LoadAll<T>(folderPath);
            onProgress?.Invoke(1f);

            return all;
        }

    }
}


