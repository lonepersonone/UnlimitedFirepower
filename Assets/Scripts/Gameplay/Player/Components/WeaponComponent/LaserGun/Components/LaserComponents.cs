using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponents 
{
    public readonly LaserRaycast laserRaycast;
    public readonly TransformMapper transformMapper;
    public readonly LaserView laserView;
    public readonly LaserLength laserLength;
    public readonly LaserCollision laserCollision;
    public readonly LaserActiveHits laserActiveHits;
    public readonly LaserRaycastHitEvent laserRaycastHitEvent;
    public readonly LaserHitEffect laserHitEffect;
    public readonly LaserDissolve laserDissolve;
    public readonly LaserCoroutineShutdown laserCoroutineShutdown;
    public readonly LaserMeshRebuilding laserMeshRebuilding;
    public readonly LaserInteraction laserInteraction;

    public LaserComponents(LaserBase laserBase, LaserData laserData)
    {
        MeshRenderer mesh = laserBase.GetComponent<MeshRenderer>();

        laserRaycast = new LaserRaycast(laserBase.transform, laserData.RaycastData);
        transformMapper = new TransformMapper(laserBase.transform);
        laserLength = new LaserRaycastLength(laserData.RaycastData, laserData.LaserMeshFilter, laserRaycast);
        laserActiveHits = new LaserActiveHits(laserRaycast, laserLength);
        laserHitEffect = new LaserRaycastHitEffect(laserActiveHits, laserData.HitEffectPrefab, laserRaycast, laserData.ViewData);
        laserRaycastHitEvent = new LaserRaycastHitEvent(laserActiveHits, laserRaycast);
        laserCollision = new LaserCollision(laserRaycast, laserLength, laserData.CollisionData);
        laserDissolve = new LaserDissolve(laserData.ViewData);
        laserView = new LaserView(laserData.LaserMeshFilter, laserLength, laserDissolve, mesh, laserData.ViewData);
        laserCoroutineShutdown = new LaserCoroutineShutdown(laserBase, laserDissolve, laserLength);
        laserMeshRebuilding = new LaserMeshRebuilding(laserRaycast, laserData.LaserMeshFilter);
        laserInteraction = new LaserInteraction(laserBase);
    }
}
