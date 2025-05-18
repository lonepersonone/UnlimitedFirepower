using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCast2D 
{
    private readonly Dictionary<Collider2D, List<RaycastHit2D>> castResult = new();
    private RaycastHit2D[] hits = new RaycastHit2D[15]; // 最大检测数量

    private BoxCastData boxCastData;

    /// <summary>
    /// 对外接口
    /// 对每一段castData进行碰撞检测
    /// </summary>
    /// <param name="castData"></param>
    /// <param name="castCount"></param>
    /// <returns></returns>
    public Dictionary<Collider2D, List<RaycastHit2D>> Update(in ReadOnlySpan<BoxCastData> castData, int castCount)
    {
        castResult.Clear();
        for(int i=0; i<castCount; i++)
        {
            BoxCastObstacle(castData[i]);
        }
        return castResult;
    }

    /// <summary>
    /// 检测障碍物，存储障碍物碰撞体信息 
    /// </summary>
    public void BoxCastObstacle(BoxCastData castData)
    {
        int hitCounts = BoxCastNonAlloc(castData, hits);

        for(int i = 0; i < hitCounts; i++)
        {
            Collider2D castCollider = hits[i].collider;
            castResult.TryAdd(castCollider, new List<RaycastHit2D>());
            castResult[castCollider].Add(hits[i]);
        }
    }

    /// <summary>
    /// 进行无额外内存分配的射线检测
    /// </summary>
    /// <param name="data"></param>
    /// <param name="hits"></param>
    /// <returns></returns>
    public static int BoxCastNonAlloc(BoxCastData data, RaycastHit2D[] hits)
    {
        return Physics2D.BoxCastNonAlloc(data.Origin, data.Size, data.Angle, data.Direction, hits, data.Distance, data.Mask);
    }
}
