using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ʵʱ����
/// </summary>
public class LaserLength : IEnable, IUpdate
{
    private RaycastData raycastData;
    private LaserRectLine rectMeshFilter;
    public float Current { get; protected set; } // ����ʵʱ����
    public float Fill { get; private set; } // ������ȱ���

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
