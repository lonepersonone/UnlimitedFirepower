using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 集成所有Data模块
/// </summary>
[Serializable]
public class LaserData 
{
    [SerializeField] private RaycastData raycastData;
    [SerializeField] private CollisionData collisionData;
    [SerializeField] private ViewData viewData;
    [SerializeField] private LaserRectLine laserMeshFilter;
    [SerializeField] private AudioSource laserAudio;
    [SerializeField] private AudioSource hitAudio;

    public RaycastData RaycastData => raycastData;
    public CollisionData CollisionData => collisionData;
    public ViewData ViewData => viewData;
    public LaserRectLine LaserMeshFilter => laserMeshFilter;

    public ParticleSystem HitEffectPrefab => viewData.HitEffectPrefab;
}
