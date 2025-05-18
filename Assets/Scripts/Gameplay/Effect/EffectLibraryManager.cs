using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Data.SO;

namespace MyGame.Gameplay.Effect
{
    public  class EffectLibraryManager : MonoBehaviour
    {
        private static EffectLibraryManager instance;
        private static EffectLibrary effectLibrary;

        [SerializeField] private EffectLibrary defaultLibrary;

        public static EffectLibrary Library
        {
            get
            {
                if (effectLibrary == null)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<EffectLibraryManager>();
                        if (instance == null)
                        {
                            GameObject obj = new GameObject("EffectLibraryManager");
                            instance = obj.AddComponent<EffectLibraryManager>();
                        }
                    }

                    effectLibrary = instance.defaultLibrary;

                    if (effectLibrary == null)
                    {
                        Debug.LogError("EffectLibraryManager: 未设置默认特效库！");
                    }
                }

                return effectLibrary;
            }
        }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // 动态设置特效库
        public static void SetEffectLibrary(EffectLibrary library)
        {
            effectLibrary = library;
        }

        // 直接获取特效配置的快捷方法
        public static EffectConfig GetEffect(string effectName)
        {
            return Library.GetEffectConfig(effectName);
        }
    }
}

