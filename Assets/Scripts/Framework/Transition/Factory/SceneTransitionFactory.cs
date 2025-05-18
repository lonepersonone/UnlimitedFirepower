using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Transition
{
    public class SceneTransitionFactory : MonoBehaviour
    {
        private static Dictionary<string, System.Func<ISceneTransitionHandler>> handlerFactories = new Dictionary<string, System.Func<ISceneTransitionHandler>>();

        // 注册场景处理器工厂方法
        public static void RegisterHandlerFactory(string sceneName, System.Func<ISceneTransitionHandler> factoryMethod)
        {
            if (!handlerFactories.ContainsKey(sceneName))
            {
                handlerFactories[sceneName] = factoryMethod;
            }
        }

        // 创建场景处理器实例
        public static ISceneTransitionHandler CreateHandler(string sceneName)
        {
            if (handlerFactories.TryGetValue(sceneName, out var factoryMethod))
            {
                return factoryMethod();
            }

            Debug.LogError($"未找到场景 {sceneName} 的处理器工厂方法");
            return null;
        }

        // 获取所有已注册的场景名称
        public static List<string> GetRegisteredScenes()
        {
            return new List<string>(handlerFactories.Keys);
        }

    }

}

