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
                        Debug.LogError("EffectLibraryManager: δ����Ĭ����Ч�⣡");
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

        // ��̬������Ч��
        public static void SetEffectLibrary(EffectLibrary library)
        {
            effectLibrary = library;
        }

        // ֱ�ӻ�ȡ��Ч���õĿ�ݷ���
        public static EffectConfig GetEffect(string effectName)
        {
            return Library.GetEffectConfig(effectName);
        }
    }
}

