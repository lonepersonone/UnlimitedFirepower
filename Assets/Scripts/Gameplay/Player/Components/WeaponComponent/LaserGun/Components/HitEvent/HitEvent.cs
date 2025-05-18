using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : IUpdate
{
    private readonly LaserActiveHits hits;
    private int hitCount;

    public HitEvent(LaserActiveHits laserActiveHits)
    {
        this.hits = laserActiveHits;
    }

    public void Update()
    {
        if(hitCount != hits.Value)
        {
            hitCount = hits.Value;
            if(hitCount > 0)
            {
                OnHit(hitCount);
            }    
        }
    }

    public virtual void OnHit(int hitCount) { }
}
