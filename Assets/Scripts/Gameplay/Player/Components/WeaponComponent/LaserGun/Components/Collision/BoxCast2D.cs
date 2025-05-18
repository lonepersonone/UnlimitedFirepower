using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCast2D 
{
    private readonly Dictionary<Collider2D, List<RaycastHit2D>> castResult = new();
    private RaycastHit2D[] hits = new RaycastHit2D[15]; // ���������

    private BoxCastData boxCastData;

    /// <summary>
    /// ����ӿ�
    /// ��ÿһ��castData������ײ���
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
    /// ����ϰ���洢�ϰ�����ײ����Ϣ 
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
    /// �����޶����ڴ��������߼��
    /// </summary>
    /// <param name="data"></param>
    /// <param name="hits"></param>
    /// <returns></returns>
    public static int BoxCastNonAlloc(BoxCastData data, RaycastHit2D[] hits)
    {
        return Physics2D.BoxCastNonAlloc(data.Origin, data.Size, data.Angle, data.Direction, hits, data.Distance, data.Mask);
    }
}
