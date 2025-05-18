using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光实时长度
/// </summary>
public class LaserLength : IEnable, IUpdate
{
    private RaycastData raycastData;
    private LaserRectLine rectMeshFilter;
    public float Current { get; protected set; } // 激光实时长度
    public float Fill { get; private set; } // 激光进度比例

    public LaserLength(RaycastData raycastData, LaserRectLine laserMeshFilter)
    {
        this.raycastData = raycastData;
        this.rectMeshFilter = laserMeshFilter;
    }

    public void Enable()
    {
        //Debug.Log("LaserLength Enabled");
        SetLaserToZero();
    }

    public virtual void Update()
    {
        CalculateRealTimeLength();
    }

    public void SetLaserToZero()
    {
        Current = 0;
        Fill = 0;
    }

    public void CalculateRealTimeLength()
    {
        Current = Current + Mathf.Clamp(raycastData.ShootSpeed * Time.deltaTime, 0, rectMeshFilter.Length);
        Fill = Current / rectMeshFilter.Length;
    }


}
