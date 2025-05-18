using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LaserRectLine
{
    [SerializeField][Range(0, 15)] private int maxPoints = 10;
    [SerializeField][Range(0, 2)] private float width = 0.6f;
    public int MaxPoints => maxPoints;
    public float Width => width/2;
    public float Length { get; private set; } = 20;
    
    private List<Vector2> points = new();
    private LaserRectLineMesh laserMesh;
    private MeshFilter meshFilter;
    public Mesh SharedMesh => meshFilter.sharedMesh;

    public void Initialize(MeshFilter filter)
    {
        meshFilter = filter;
        laserMesh = new LaserRectLineMesh(points);
    }

    /// <summary>
    /// 将关键点由世界坐标转换为Mesh使用的本地坐标
    /// </summary>
    /// <param name="point"></param>
    public void AddMeshLocalPoint(Vector2 point)
    {
        if(points.Count < maxPoints)
        {
            Vector3 localPoint = meshFilter.transform.worldToLocalMatrix * new Vector4(point.x, point.y, 0, 1);
            points.Add(localPoint);
        }       
    }

    public void ClearPoints()
    {
        points.Clear();
    }

    /// <summary>
    /// 生成MeshFilter网格
    /// </summary>
    public void GenerateFullRectMesh()
    {
        if (points.Count < 1)
            points.Add(Vector2.zero);

        meshFilter.sharedMesh = laserMesh.CreateMesh(-Width, Length);
    }

    /// <summary>
    /// 计算射线有效长度
    /// </summary>
    /// <returns></returns>
    public float CalculateLength()
    {
        float length = 0;
        for(int i = 0; i < points.Count-1; i++)
        {
            length += (points[i+1] - points[i]).magnitude;
        }
        return length;
    }
}
