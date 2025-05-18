using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ViewData 
{
    [SerializeField] [Min(0)] private float dissolveTime = 1;
    [SerializeField] private ParticleSystem hitEffectPrefab;
    [SerializeField] private bool isNonHitEffect = true;
    [SerializeField] private int sortOrder;

    public float DissolveTime => dissolveTime;
    public ParticleSystem HitEffectPrefab => hitEffectPrefab;
    public bool IsNonHitEffect => isNonHitEffect;
    public int SortOrder => sortOrder;
}
