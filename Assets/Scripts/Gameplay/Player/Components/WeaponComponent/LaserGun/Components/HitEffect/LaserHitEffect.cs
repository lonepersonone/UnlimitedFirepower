using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ײ��Ч
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
    /// ʵʱ����effects�ĳ���
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
    /// ʵʱ�������е�Hit Effect
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
    /// Ԥ���ӿڣ����ݾ�������ʵʱ����effects������
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected abstract int CalculateRealtimeEffects(int value);
}
