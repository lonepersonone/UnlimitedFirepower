using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光碰撞特效
/// </summary>
public abstract class LaserHitEffect : IUpdate, IDisable
{
    private ILaserKeyPointProvider laserKeyPointProvider;
    private LaserActiveHits activeHits;
    private ParticleSystem hitEffectPrefab;
    private List<ParticleSystem> effects;

    public LaserHitEffect(LaserActiveHits activeHits, ParticleSystem hitEffectPrefab, ILaserKeyPointProvider laserKeyPointProvider)
    {
        this.laserKeyPointProvider = laserKeyPointProvider;
        this.activeHits = activeHits;
        this.hitEffectPrefab = hitEffectPrefab;
        this.effects = new();
    }

    public void Disable()
    {
        if(effects != null)
        {
            foreach(ParticleSystem p in effects)
            {
                p.Stop();
            }
        }
        effects.Clear();
    }

    public void Update()
    {
        if(hitEffectPrefab != null)
        {
            int count = CalculateRealtimeEffects(activeHits.Value);
            AlinSizeToHits(count);
            UpdateEffects();
        }
    }

    /// <summary>
    /// 实时更新effects的长度
    /// </summary>
    /// <param name="value"></param>
    public void AlinSizeToHits(int value)
    {
        while(effects.Count < value)
        {
            ParticleSystem p  = new ParticleSystem();
            effects.Add(p);
        }

        while(activeHits.Value > value)
        {
            effects[effects.Count-1].Stop();
            effects.RemoveAt(effects.Count-1);
        }
    }

    /// <summary>
    /// 实时运行所有的Hit Effect
    /// </summary>
    public void UpdateEffects()
    {
        if (effects != null)
        {
            for(int i = 0; i < laserKeyPointProvider.Count; i++)
            {
                if (effects[i].isPlaying == false)
                    effects[i].Play();
                effects[i].transform.position = laserKeyPointProvider[i + 1];
            }
        }
    }

    /// <summary>
    /// 预留接口，根据具体条件实时更新effects的数量
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract int CalculateRealtimeEffects(int value);
}
