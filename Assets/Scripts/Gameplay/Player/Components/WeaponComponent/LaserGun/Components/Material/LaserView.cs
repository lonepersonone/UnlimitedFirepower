using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激光Material，显示Shader效果
/// </summary>
public class LaserView : IUpdate
{
    private LaserRectLine meshFilter;
    private LaserMaterial laserMaterial;
    private LaserLength laserLength;
    private LaserDissolve laserDissolve;
    private MeshRenderer meshRenderer;
    private ViewData viewData;

    public LaserView(LaserRectLine rectMeshFilter, LaserLength laserLength, LaserDissolve laserDissolve, MeshRenderer meshRenderer, ViewData viewData)
    {
        this.laserMaterial = new LaserMaterial();
        this.meshFilter = rectMeshFilter;
        this.laserLength = laserLength;
        this.laserDissolve = laserDissolve;
        this.meshRenderer = meshRenderer;
        this.viewData = viewData;
    }

    void IUpdate.Update()
    {
        //Debug.Log("LaserView Update");
        laserMaterial.Clear();
        laserMaterial.SetShape(meshFilter.Width, meshFilter.Length, meshFilter.SharedMesh.uv, laserLength.Fill, laserDissolve.Value);

        meshRenderer.SetPropertyBlock(laserMaterial.PropertyBlock);
        meshRenderer.sortingOrder = viewData.SortOrder;
    }
}
