using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public class SceneTransitionFactory : MonoBehaviour
    {
        private static Dictionary<string, System.Func<ISceneTransitionHandler>> handlerFactories = new Dictionary<string, System.Func<ISceneTransitionHandler>>();

        // ע�᳡����������������
        public static void RegisterHandlerFactory(string sceneName, System.Func<ISceneTransitionHandler> factoryMethod)
        {
            if (!handlerFactories.ContainsKey(sceneName))
            {
                handlerFactories[sceneName] = factoryMethod;
            }
        }

        // ��������������ʵ��
        public static ISceneTransitionHandler CreateHandler(string sceneName)
        {
            if (handlerFactories.TryGetValue(sceneName, out var factoryMethod))
            {
                return factoryMethod();
            }

            Debug.LogError($"δ�ҵ����� {sceneName} �Ĵ�������������");
            return null;
        }

        // ��ȡ������ע��ĳ�������
        public static List<string> GetRegisteredScenes()
        {
            return new List<string>(handlerFactories.Keys);
        }

    }

}

