using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRaycastHitEffect : LaserHitEffect
{
    private readonly LaserRaycast laserRaycast;
    private readonly ViewData viewData;

    public LaserRaycastHitEffect( LaserActiveHits activeHits, ParticleSystem hitEffectPrefab, LaserRaycast laserRaycast, ViewData viewData) 
        : base(activeHits, hitEffectPrefab, laserRaycast)
    {
        this.laserRaycast = laserRaycast;
        this.viewData = viewData;
    }

    protected override int CalculateRealtimeEffects(int value)
    {
        if(value > 0)
        {
            Collider2D hitObject = laserRaycast.Hits[value - 1].HitObject;
            bool isHitObject = hitObject == false;
            bool isNonHit = viewData.IsNonHitEffect == false;
            if (isHitObject && isNonHit)
                value--; //终点没有特效
        }
        return value;
    }
}
