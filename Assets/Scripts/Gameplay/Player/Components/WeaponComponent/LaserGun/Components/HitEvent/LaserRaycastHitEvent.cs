using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ײ�¼�
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
    /// ���ⲿ��Action���и�ֵ
    /// </summary>
    /// <param name="raycastHitEvent"></param>
    public void InheritedEvent(LaserRaycastHitEvent raycastHitEvent)
    {
        hitAction = raycastHitEvent.hitAction;
    }
}
