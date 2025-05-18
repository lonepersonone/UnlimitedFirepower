using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����״��ײ�壬���߶ηֲ�
/// </summary>
public class LaserCollision : IOutputUpdate<Dictionary<Collider2D, List<RaycastHit2D>>>
{
    private readonly ILaserKeyPointProvider laserKeyPoints;
    private readonly LaserLength laserLength;
    private readonly BoxCast2D boxCast2D;
    private readonly CollisionData collisionData;

    public LaserCollision(ILaserKeyPointProvider laserKeyPoints, LaserLength laserLength, CollisionData collisionData)
    {
        boxCast2D = new();
        this.laserKeyPoints = laserKeyPoints;
        this.laserLength = laserLength;
        this.collisionData = collisionData;
    }

    public Dictionary<Collider2D, List<RaycastHit2D>> Update()
    {
        int castCount = laserKeyPoints.Count == 0 ? 0 : laserKeyPoints.Count - 1; // ������ײ��Ͷ��������
        Span<BoxCastData> castData = stackalloc BoxCastData[castCount]; // ֱ����ջ�ڴ��п��ٴ洢�ռ�
        castCount = CalculateBoxCastData(ref castData);
        DrawGizmo(castData, castCount);
        return boxCast2D.Update(castData, castCount);       
    }

    /// <summary>
    /// ���ݷ�������boxcast����
    /// ע��box������laserʵ�ʳ�����ƥ��
    /// </summary>
    /// <param name="castData"></param>
    private int CalculateBoxCastData(ref Span<BoxCastData> castData)
    {
        float sumDistance = 0;
        int castCount = 0;
        for(int i = 0; i < castData.Length && sumDistance < laserLength.Current; i++, castCount++)
        {
            Vector2 startPoint = laserKeyPoints[i];
            Vector2 secondPoint = laserKeyPoints[i + 1];
            Vector2 direction = secondPoint - startPoint;
            float segmentDistance = direction.magnitude;
            sumDistance += segmentDistance;
            // ��laserĩ�˵�boxcast������Ч�ü�
            if (sumDistance > laserLength.Current)
                segmentDistance -= sumDistance - laserLength.Current;
            castData[i] = new BoxCastData(direction, segmentDistance, startPoint, collisionData.LayerMask, collisionData.Width);
        }
        return castCount;
    }


    public void DrawGizmo(in ReadOnlySpan<BoxCastData> data, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Vector2 normal = new Vector2(-data[i].Direction.y, data[i].Direction.x); 
            Vector2 leftBottom = data[i].Origin - normal * data[i].Width;
            Vector2 rightBottom = data[i].Origin + normal * data[i].Width;
            Vector2 leftTop = leftBottom + data[i].Direction * data[i].Distance;
            Vector2 rightTop = rightBottom + data[i].Direction * data[i].Distance;

            Debug.DrawLine(leftBottom, leftTop, Color.green);
            Debug.DrawLine(rightBottom, rightTop, Color.green);
            Debug.DrawLine(leftBottom, rightBottom, Color.green);
            Debug.DrawLine(leftTop, rightTop, Color.green);
            Debug.DrawLine(Vector3.zero, Vector3.one * 2);
        }
    }


}
