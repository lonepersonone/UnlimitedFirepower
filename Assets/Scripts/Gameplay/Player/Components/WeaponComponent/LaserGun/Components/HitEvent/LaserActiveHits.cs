using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光实时碰撞点
/// </summary>
public class LaserActiveHits : IUpdate
{
    private readonly float hitCarlibration = 0.1f; // 校准误差值
    private readonly ILaserKeyPointProvider keyPointProvider;
    private readonly LaserLength laserLength;
    public int Value { get; private set; } // 已激活hit的数量
    public LaserActiveHits(ILaserKeyPointProvider provider, LaserLength length)
    {
        keyPointProvider = provider;
        laserLength = length;
    }

    public void Update()
    {
        float sumDistance = 0;
        Value = 0;
        for(int i = 1; i < keyPointProvider.Count && laserLength.Current > sumDistance; i++)
        {
            float segmentDistance = (keyPointProvider[i] - keyPointProvider[i - 1]).magnitude;
            if (IsLaserReachedHitPoint(segmentDistance, sumDistance))
                Value++;
            sumDistance += segmentDistance;
        }
    }

    public bool IsLaserReachedHitPoint(float segmentDistance, float sumDistance)
    {
        return laserLength.Current > segmentDistance + sumDistance - hitCarlibration;
    }
}
