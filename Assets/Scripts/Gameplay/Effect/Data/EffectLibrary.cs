using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "EffectLibrary", menuName = "Effects/Effect Library")]
    public class EffectLibrary : ScriptableObject
    {
        [SerializeField] private List<EffectConfig> effects = new List<EffectConfig>();

        // �������Ʋ�����Ч����
        public EffectConfig GetEffectConfig(string effectName)
        {
            foreach (var effect in effects)
            {
                if (effect != null && effect.effectName == effectName)
                {
                    return effect;
                }
            }

            Debug.LogWarning($"EffectLibrary: δ�ҵ���Ϊ '{effectName}' ����Ч����");
            return null;
        }

        // �����Ч����
        public void AddEffectConfig(EffectConfig config)
        {
            if (config != null && !effects.Contains(config))
            {
                effects.Add(config);
            }
        }

        // ��ȡ������Ч����
        public List<EffectConfig> GetAllEffects()
        {
            return effects;
        }
    }
}

