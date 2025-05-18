using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光碰撞事件
/// </summary>
public class LaserRaycastHitEvent : HitEvent
{
    private LaserRaycast laserRaycast;
    private Action<LaserHit> hitAction;
    public LaserRaycastHitEvent(LaserActiveHits laserActiveHits, LaserRaycast laserRaycast) : base(laserActiveHits)
    {
        this.laserRaycast = laserRaycast;
    }

    public void AddEvent(Action<LaserHit> action)
    {
        hitAction += action;
    }

    public void RemoveEvent(Action<LaserHit> action)
    {
        hitAction -= action;
    }

    public override void OnHit(int hitCount)
    {
        hitAction?.Invoke(laserRaycast.Hits[hitCount - 1]);
    }

    /// <summary>
    /// 从外部对Action进行赋值
    /// </summary>
    /// <param name="raycastHitEvent"></param>
    public void InheritedEvent(LaserRaycastHitEvent raycastHitEvent)
    {
        hitAction = raycastHitEvent.hitAction;
    }
}
