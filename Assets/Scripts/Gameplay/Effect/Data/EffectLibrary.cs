using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "EffectLibrary", menuName = "Effects/Effect Library")]
    public class EffectLibrary : ScriptableObject
    {
        [SerializeField] private List<EffectConfig> effects = new List<EffectConfig>();

        // 根据名称查找特效配置
        public EffectConfig GetEffectConfig(string effectName)
        {
            foreach (var effect in effects)
            {
                if (effect != null && effect.effectName == effectName)
                {
                    return effect;
                }
            }

            Debug.LogWarning($"EffectLibrary: 未找到名为 '{effectName}' 的特效配置");
            return null;
        }

        // 添加特效配置
        public void AddEffectConfig(EffectConfig config)
        {
            if (config != null && !effects.Contains(config))
            {
                effects.Add(config);
            }
        }

        // 获取所有特效配置
        public List<EffectConfig> GetAllEffects()
        {
            return effects;
        }
    }
}

