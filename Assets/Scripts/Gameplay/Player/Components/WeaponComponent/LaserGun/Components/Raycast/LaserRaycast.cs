using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光射线以收集所有碰撞点
/// </summary>
public class LaserRaycast : IUpdate, ILaserKeyPointProvider 
{
    private readonly List<LaserHit> hits = new();
    private int count;

    private Transform _transform;
    private RaycastData _raycastData;

    public LaserRaycast(Transform transform, RaycastData raycastData)
    {
        this._transform = transform;
        this._raycastData = raycastData;
    }

    public IReadOnlyList<LaserHit> Hits => hits;

    public int Count => count;

    public Vector2 this[int index] => index == 0 ? _transform.position : hits[index-1].HitPoint;

    public void Update()
    {
        //Debug.Log("LaserRaycast Update");
        //射线不会检测物体内部碰撞体
        Physics2D.queriesStartInColliders = false;
        count = 0;

        ShootRecursive();
    }

    public void ShootRecursive()
    {
        ShootRecursive(_transform.position, _transform.up, _raycastData.LayerMask, _raycastData.MaxPoints);
    }

    public void ShootRecursive(Vector2 startPoint, Vector2 direction, LayerMask layerMask, float maxPoints)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(startPoint, direction, _raycastData.MaxDistance, layerMask);

        if(hit2D.collider != null)
        {
            HandleHitPoint(startPoint, hit2D, direction);
            if(LaserManager.Instance.reflectingColliders.Contains(hit2D.collider) && count <= maxPoints)
            {
                ShootRecursive(hit2D.point, hits[count - 1].ReflectedDirection, layerMask, maxPoints);
            }

        }

        else
        {
            hit2D.point = startPoint + direction * _raycastData.MaxDistance;
            HandleHitPoint(hit2D.point, hit2D, direction);
        }
    }
    
    /// <summary>
    /// 将射线碰撞点添加进hits中
    /// </summary>
    public void HandleHitPoint(Vector2 startPoint, RaycastHit2D hit2D, Vector2 direction)
    {
        float distance = Vector2.Distance(startPoint, hit2D.point);
        
        LaserHit hit = new LaserHit();
        hit.SetValue(hit2D.collider, hit2D.normal, direction, hit2D.point, distance);

        hits.Add(hit);

        count = hits.Count;
    }

}
