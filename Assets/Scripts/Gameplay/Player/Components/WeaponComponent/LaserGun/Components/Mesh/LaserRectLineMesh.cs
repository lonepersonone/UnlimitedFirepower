using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRectLineMesh : MonoBehaviour
{
    private List<Vector2> points;
    private Vector3[] vertices = Array.Empty<Vector3>();
    private int[] triangles = Array.Empty<int>();
    private Vector2[] uv = Array.Empty<Vector2>();
    private readonly Mesh mesh;

    public LaserRectLineMesh(List<Vector2> points)
    {
        this.points = points;
        mesh = new Mesh();
    }

    public Mesh CreateMesh(float width, float length)
    {
        mesh.Clear();
        mesh.vertices = GenerateVertices(width);
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUV(length);
        return mesh;
    }

    private Vector3[] GenerateVertices(float width)
    {
        ResizeArray(ref vertices, (points.Count - 1) * 4);

        for (int i = 0, j = points.Count - 1; j > 0; j--, i++)
        {
            Vector3 end = points[j - 1];
            Vector3 start = points[j];
            Vector3 direction = end - start;
            Vector3 normal = new Vector3(-direction.y, direction.x, 0).normalized;

            vertices[i * 4 + 0] = start - normal * width;
            vertices[i * 4 + 1] = start + normal * width;
            vertices[i * 4 + 2] = end - normal * width;
            vertices[i * 4 + 3] = end + normal * width;
        }

        return vertices;
    }

    private int[] GenerateTriangles()
    {
        ResizeArray(ref triangles, (points.Count - 1) * 6);
        for (int i = 0; i < points.Count - 1; i++)
        {
            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 2;
            triangles[i * 6 + 2] = i * 4 + 1;

            triangles[i * 6 + 3] = i * 4 + 2;
            triangles[i * 6 + 4] = i * 4 + 3;
            triangles[i * 6 + 5] = i * 4 + 1;
        }
        return triangles;
    }

    private Vector2[] GenerateUV(float length)
    {
        ResizeArray(ref uv, (points.Count - 1) * 4);
        float current = 0;
        for (int i = 0, j = points.Count - 1; j > 0; ++i, --j)
        {
            float subQuadLength = (points[j - 1] - points[j]).magnitude;
            uv[i * 4 + 0] = new Vector2(0, current / length);
            uv[i * 4 + 1] = new Vector2(1, current / length);
            uv[i * 4 + 2] = new Vector2(0, (subQuadLength + current) / length);
            uv[i * 4 + 3] = new Vector2(1, (subQuadLength + current) / length);

            current += subQuadLength;
        }
        return uv;
    }

    private void ResizeArray<T>(ref T[] array, int length)
    {
        if(array != null)
        {
            if (array.Length != length)
                array = new T[length];
        }

    }
}
